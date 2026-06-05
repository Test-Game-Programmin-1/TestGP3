using UnityEngine;

public class BT_ROOT : BT_NODE //Fa partire la logica
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
