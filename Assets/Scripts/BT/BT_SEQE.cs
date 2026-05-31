using UnityEngine;

public class BT_SEQE : BT_NODE
{
    public BT_SEQE(string n)
    {
        NODENAME = n;
    }
    public override BT_STATUS PROCESS()
    {
        BT_STATUS childstatus = children[CURRENTCHILD].PROCESS();
        if(childstatus == BT_STATUS.RUNNING)return childstatus;
        if(childstatus == BT_STATUS.FAILURE) return childstatus;
        CURRENTCHILD++;
        if(CURRENTCHILD >= children.Count)
        {
            CURRENTCHILD = 0;
            return BT_STATUS.SUCCESS;
        }
        return BT_STATUS.RUNNING;
    }
}
