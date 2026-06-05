using UnityEngine;

public class BT_LEAF : BT_NODE //azioni dell'albero che fanno avvenire il pensare, tick = stato
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
    public override BT_STATUS PROCESS()
    {
        if(processMetod != null)
        {
            return processMetod();
        }
        return BT_STATUS.FAILURE;
    }
}
