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
    public BT_WORKINGSTATUS workingStatus = BT_WORKINGSTATUS.idle;
    public BT_ROOT root;
    NavMeshAgent Agent;
    BT_STATUS TREESTATUS = BT_STATUS.RUNNING;
    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        root = new BT_ROOT("ROOT");
        BT_SEQE workingloop = new BT_SEQE("WORKINGLOOP");
        BT_SELE EnergyController = new BT_SELE("ENERGYCONTROLLER");
        BT_SEQE GoSleep = new BT_SEQE("GOSLEEP");
        BT_LEAF GoToWork = new BT_LEAF("GOTOWORK", GoToWork);
        BT_LEAF GetTheOrder = new BT_LEAF("GETTHEORDER", GetTheOrder);
        BT_LEAF GetMat = new BT_LEAF("GETMAT", GetMat);
        BT_LEAF Crafting = new BT_LEAF("CRAFTING", Crafting);
        BT_LEAF Expedition = new BT_LEAF("EXPEDITION", Expedition);
        BT_LEAF EnergyCheck = new BT_LEAF("ENERGYCHECK", EnergyCheck);
        BT_LEAF GoToSleepFR = new BT_LEAF("GOTOSLEEP", GoToSleepFR);
        BT_LEAF Recharging = new BT_LEAF("RECHARGING", Recharging);
        root.ADDCHILD(workingloop);
        workingloop.ADDCHILD(GoToWork);
        workingloop.ADDCHILD(GetTheOrder);
        workingloop.ADDCHILD(GetMat);
        workingloop.ADDCHILD(Crafting);
        workingloop.ADDCHILD(Expedition);
        workingloop.ADDCHILD(EnergyController);
            EnergyController.ADDCHILD(EnergyCheck);    
            EnergyController.ADDCHILD(GoSleep);
                GoSleep.ADDCHILD(GoToSleepFR);   
                GoSleep.ADDCHILD(Recharging);
        root.printTree();         
    }

}
