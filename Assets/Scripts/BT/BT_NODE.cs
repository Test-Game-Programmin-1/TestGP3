using System.Collections.Generic;
using UnityEngine;
public enum BT_STATUS
{
    SUCCESS,
    FAILURE,
    RUNNING
}
public class BT_NODE : MonoBehaviour
{
    public BT_STATUS status;
    public List<BT_NODE> children = new List<BT_NODE>();
    public int CURRENTCHILD = 0;
    public string NODENAME;
    public BT_NODE()
    {
        
    }
    public void ADDCHILD(BT_NODE N)
    {
        children.Add(N);
    }
    public virtual BT_STATUS PROCESS()
    {
        return children[CURRENTCHILD].PROCESS();
    }
    public void printName()
    {
        Debug.Log(NODENAME);
    }
}
