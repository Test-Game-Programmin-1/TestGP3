using UnityEngine;

public class BT_LEAF : BT_NODE
{
    public delegate BT_STATUS tick();
    public tick processMetod;
    private string v;
    private BT_LEAF goToWork;

    public BT_LEAF(string n,tick pm)
    {
        NODENAME = n;
        processMetod = pm;
    }

    public BT_LEAF(string v, BT_LEAF goToWork)
    {
        this.v = v;
        this.goToWork = goToWork;
    }

    public override BT_STATUS PROCESS()
    {
        if(processMetod != null)
        {
            return processMetod();
        }
        return BT_STATUS.FAILURE;
    }
}
