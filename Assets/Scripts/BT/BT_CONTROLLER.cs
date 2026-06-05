using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public enum BT_WORKINGSTATUS
{
    idle, //enum dei vari stati che può assumere quando lavora
    working
}
public class BT_CONTROLLER : MonoBehaviour
{
    [Header("Cheker")]
    public bool hasMat = false; //bool per capire se ha materiali o no
    [Header("Points")]
    [SerializeField] Transform start;
    [SerializeField] Transform work;
    [SerializeField] Transform EXPEDITION; //vari waypoint dove può andare Noir
    [SerializeField] Transform sleep;
    [SerializeField] Transform[] Chest;
    [SerializeField] Transform[] Warehouse;

    [Header("sleep")]
    [SerializeField] bool isSleeping = true;
    public float Energy; //sezione per la logica riguardo il dormire
    public float MaxEnergy;

    BT_WORKINGSTATUS workingStatus = BT_WORKINGSTATUS.idle; //il workingstatus è impostato di default in idle
    BT_ROOT root;
    NavMeshAgent Agent;
    BT_STATUS TREESTATUS = BT_STATUS.RUNNING; //il BT_STATUS è impostato di default a running

    //[Header("eventi")]
    public static event Action OnGetTheOrder;
    public static event Action OnCrafting;
    public static event Action OnExpedition;
    public static event Action OnEnergyUsage;
    public static event Action OnQuantityLeft;
    public static event Action ResetOrder;

    public static BT_CONTROLLER instance;

    void Awake()
    {
        if(instance != null)
        {
            Destroy(instance); //single tone
            return;
        }
            instance = this;
    }
    void Start()
    {
        Energy = MaxEnergy;

        Agent = GetComponent<NavMeshAgent>();

        root = new BT_ROOT("ROOT");
        BT_SEQE workingloop = new BT_SEQE("WORKINGLOOP");
        BT_SELE EnergyController = new BT_SELE("ENERGYCONTROLLER");
        BT_SEQE GoSleep = new BT_SEQE("GOSLEEP");
        BT_LEAF _GoToWork = new BT_LEAF("GOTOWORK", GoToWork);
        BT_LEAF _GetTheOrder = new BT_LEAF("GETTHEORDER", GetTheOrder);
        BT_LEAF _GetMat = new BT_LEAF("GETMAT", GetMat);
        //BT_LEAF _Crafting = new BT_LEAF("CRAFTING", Crafting);
        BT_LEAF _Expedition = new BT_LEAF("EXPEDITION", Expedition);  //Behaviour tree
        BT_LEAF _EnergyCheck = new BT_LEAF("ENERGYCHECK", EnergyCheck);
        BT_LEAF _GoToSleepFR = new BT_LEAF("GOTOSLEEP", GoToSleepFR);
        BT_LEAF _Recharging = new BT_LEAF("RECHARGING", Recharging);
        root.ADDCHILD(workingloop);
        workingloop.ADDCHILD(_GoToWork);
        workingloop.ADDCHILD(_GetTheOrder);
        workingloop.ADDCHILD(_GetMat);
        //workingloop.ADDCHILD(_Crafting);
        workingloop.ADDCHILD(_Expedition);
        workingloop.ADDCHILD(EnergyController);
            EnergyController.ADDCHILD(_EnergyCheck);    
            EnergyController.ADDCHILD(GoSleep);
                GoSleep.ADDCHILD(_GoToSleepFR);   
                GoSleep.ADDCHILD(_Recharging);
        root.printTree();         
    }
    void Update()
    {
        TREESTATUS = root.PROCESS();
        if(TREESTATUS != BT_STATUS.RUNNING) //fa ripartire il tree
        {
            root.CURRENTCHILD = 0;
            TREESTATUS = BT_STATUS.RUNNING;
        }
        if(isSleeping && Energy < MaxEnergy) //if ed else if che vanno con lo status del recharging/sleeping
        {
            Energy += Time.deltaTime;
            OnEnergyUsage?.Invoke();
        }
        else if(!isSleeping && Energy > 0)
        {
            Energy -= Time.deltaTime;
            OnEnergyUsage?.Invoke();
        }
    }
    private BT_STATUS MoveTo(Vector3 Destination) //status che sposta Noir da un WayPoint ad un altro
    {
        if(workingStatus == BT_WORKINGSTATUS.idle)
        {
            Agent.SetDestination(Destination);
            workingStatus = BT_WORKINGSTATUS.working;
        }
        else if(Vector3.SqrMagnitude(Agent.pathEndPosition - Destination) >= 0.1f)
        {
            workingStatus = BT_WORKINGSTATUS.idle;
            return BT_STATUS.FAILURE;
        }
        else if(Vector3.SqrMagnitude(Destination - transform.position) < 0.1f)
        {
            workingStatus = BT_WORKINGSTATUS.idle;
            return BT_STATUS.SUCCESS;
        }
        return BT_STATUS.RUNNING;
    }
    private BT_STATUS MaterialCheck() //check che se non hai materiali nella chest li vai a prendere dalla warehouse sennò lo prendi dalla chest e vai a fare l'ordine
    {
        if(RecipeManager.instance.currentMat >= 3){return BT_STATUS.SUCCESS;}
        if(RecipeManager.instance.MatNeeded[RecipeManager.instance.currentMat] == 0)
        {
            RecipeManager.instance.currentMat++;
            return BT_STATUS.RUNNING;
        }
        if(!hasMat)
        {
            if(RecipeManager.instance.MatOwned[RecipeManager.instance.currentMat] < RecipeManager.instance.MatNeeded[RecipeManager.instance.currentMat])
            {
                int RefillMat = RecipeManager.instance.ChestMaxVolume - RecipeManager.instance.MatOwned[RecipeManager.instance.currentMat];
                BT_STATUS _Warehouse = MoveTo(Warehouse[RecipeManager.instance.currentMat].position);
                if (_Warehouse == BT_STATUS.SUCCESS)
                {
                    RecipeManager.instance.MatOwned[RecipeManager.instance.currentMat] += RefillMat;
                    RecipeManager.instance.Material[RecipeManager.instance.currentMat].SetActive(true); 
                }
                OnQuantityLeft?.Invoke();
                return BT_STATUS.RUNNING;                  
            }
            else
            {
                BT_STATUS _Chest = MoveTo(Chest[RecipeManager.instance.currentMat].position);
                if(_Chest == BT_STATUS.SUCCESS)
                {
                    RecipeManager.instance.MatOwned[RecipeManager.instance.currentMat] -= RecipeManager.instance.MatNeeded[RecipeManager.instance.currentMat];
                    if(RecipeManager.instance.MatOwned[RecipeManager.instance.currentMat] <= 0)
                    {
                        RecipeManager.instance.Material[RecipeManager.instance.currentMat].SetActive(false); 
                    }
                    //aggiungere evento UI
                    OnQuantityLeft?.Invoke();
                    hasMat = true;
                }
                return BT_STATUS.RUNNING;
            }   
        }
        else
        {

            BT_STATUS _GoToWork = MoveTo(work.position);
            if(_GoToWork == BT_STATUS.SUCCESS)           
            {
                OnCrafting?.Invoke();
                hasMat = false;
                RecipeManager.instance.currentMat++;
            }
        }
        
        return BT_STATUS.RUNNING;
    }
    private BT_STATUS GoToWork() //status che manda alla posizione di lavoro
    {
        return MoveTo(start.position);
    }
    private BT_STATUS GetTheOrder() //stato che una volta invocato fa prendere l'ordine a Noir
    {
        OnGetTheOrder?.Invoke();
        isSleeping = false;
        RecipeManager.instance.currentMat = 0;
        return BT_STATUS.SUCCESS;
    }
    private BT_STATUS GetMat()
    {
        return MaterialCheck(); //stato che attiva la funzione del check dei materiali
    }
    private BT_STATUS Expedition()
    {
        OnExpedition?.Invoke(); //stato che fa portare il prodotto alla consegna
        return MoveTo(EXPEDITION.position);
    }
    private BT_STATUS EnergyCheck()
    {
        ResetOrder?.Invoke(); //check che viene invocato quando prendi un nuovo ordine
        if (Energy <= 2f){return BT_STATUS.FAILURE;}
        else{return BT_STATUS.SUCCESS;}
    }
    private BT_STATUS GoToSleepFR()
    {
        return MoveTo(sleep.position); //stato che se richiesto ti porta alla posizione di ricarica
    }
    private BT_STATUS Recharging()
    {
        isSleeping = true; //stato che permette la ricarica di Noir una volta raggiunta la posizione apposita
        if(Energy < MaxEnergy) return BT_STATUS.RUNNING;
        else return BT_STATUS.SUCCESS;
    }
}
