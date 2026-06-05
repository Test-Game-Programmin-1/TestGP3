using UnityEngine;

public class BT_SELE : BT_NODE //il selector è comparabile alla logica del "or" perché se fallisce uno non influenza gli altri, quindi se avviene un Failure il resto rimane intatto
{
    public BT_SELE(string n)
    {
        NODENAME = n;
    }
    public override BT_STATUS PROCESS()
    {
        BT_STATUS childstatus = children[CURRENTCHILD].PROCESS();
        if(childstatus == BT_STATUS.RUNNING)return childstatus;
        if(childstatus == BT_STATUS.SUCCESS)
        {
            CURRENTCHILD = 0;
            return BT_STATUS.SUCCESS;
        }
        CURRENTCHILD++;
        if(CURRENTCHILD >= children.Count)
        {
            CURRENTCHILD = 0;
            return BT_STATUS.FAILURE;
        }
        return BT_STATUS.RUNNING;
    }
}
