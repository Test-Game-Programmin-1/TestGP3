using UnityEngine;
using UnityEngine.AI;
public enum BT_WORKINGSTATUS
{
    idle,
    working
}
public class BT_CONTROLLER : MonoBehaviour
{
    [SerializeField] Transform start;
    [SerializeField] Transform work;
    [SerializeField] Transform EXPEDITION;
    [SerializeField] Transform sleep;
    BT_WORKINGSTATUS workingStatus = BT_WORKINGSTATUS.idle;
    BT_ROOT root;
    NavMeshAgent Agent;
    BT_STATUS TREESTATUS = BT_STATUS.RUNNING;
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        root = new BT_ROOT("ROOT");
        BT_SEQE workingloop = new BT_SEQE("WORKINGLOOP");
        BT_SELE EnergyController = new BT_SELE("ENERGYCONTROLLER");
        BT_SEQE GoSleep = new BT_SEQE("GOSLEEP");
        BT_LEAF _GoToWork = new BT_LEAF("GOTOWORK", GoToWork);
        BT_LEAF _GetTheOrder = new BT_LEAF("GETTHEORDER", GetTheOrder);
        BT_LEAF _GetMat = new BT_LEAF("GETMAT", GetMat);
        BT_LEAF _Crafting = new BT_LEAF("CRAFTING", Crafting);
        BT_LEAF _Expedition = new BT_LEAF("EXPEDITION", Expedition);
        BT_LEAF _EnergyCheck = new BT_LEAF("ENERGYCHECK", EnergyCheck);
        BT_LEAF _GoToSleepFR = new BT_LEAF("GOTOSLEEP", GoToSleepFR);
        BT_LEAF _Recharging = new BT_LEAF("RECHARGING", Recharging);
        root.ADDCHILD(workingloop);
        workingloop.ADDCHILD(_GoToWork);
        workingloop.ADDCHILD(_GetTheOrder);
        workingloop.ADDCHILD(_GetMat);
        workingloop.ADDCHILD(_Crafting);
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
        if(TREESTATUS != BT_STATUS.RUNNING)
        {
            root.CURRENTCHILD = 0;
            TREESTATUS = BT_STATUS.RUNNING;
        }
    }
    private BT_STATUS MoveTo(Vector3 Destination)
    {
        if(workingStatus == BT_WORKINGSTATUS.idle)
        {
            Agent.SetDestination(Destination);
            workingStatus = BT_WORKINGSTATUS.working;
        }
        else if(Vector3.SqrMagnitude(Agent.pathEndPosition - Destination) >= 3)
        {
            workingStatus = BT_WORKINGSTATUS.idle;
            return BT_STATUS.FAILURE;
        }
        else if(Vector3.SqrMagnitude(Destination - transform.position) < 3)
        {
            workingStatus = BT_WORKINGSTATUS.idle;
            return BT_STATUS.SUCCESS;
        }
        return BT_STATUS.RUNNING;
    }
    private BT_STATUS GoToWork()
    {
        return MoveTo(start.position);
    }
    private BT_STATUS GetTheOrder()
    {
        return BT_STATUS.SUCCESS;
    }
    private BT_STATUS GetMat()
    {
        return BT_STATUS.SUCCESS;
    }
    private BT_STATUS Crafting()
    {
        return MoveTo(work.position);
    }
    private BT_STATUS Expedition()
    {
        return MoveTo(EXPEDITION.position);
    }
    private BT_STATUS EnergyCheck()
    {
        return BT_STATUS.SUCCESS;
    }
    private BT_STATUS GoToSleepFR()
    {
        return MoveTo(sleep.position);
    }
    private BT_STATUS Recharging()
    {
        return BT_STATUS.SUCCESS;
    }

}
