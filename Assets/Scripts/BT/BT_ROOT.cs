using UnityEngine;

public class BT_ROOT : BT_NODE
{
    public BT_ROOT(string n)
    {
        NODENAME = n;
    }
    public void printTree()
    {
        Debug.Log(NODENAME);
    }
}
