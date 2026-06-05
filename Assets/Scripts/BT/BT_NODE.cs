using System.Collections.Generic;
using UnityEngine;
public enum BT_STATUS
{ //enum dei tre stati che può avere il BT_CONTROLLER
    SUCCESS, //success va al prossimo
    FAILURE, //salta il nodo attuale e/o il resto della seqe
    RUNNING //continua ad andare
}
public class BT_NODE
{
    public BT_STATUS status;
    public List<BT_NODE> children = new List<BT_NODE>();
    public int CURRENTCHILD = 0;
    public string NODENAME;
    public BT_NODE()//costruttore vuoto che deve stare vuoto e che viene riempito dai nodi quando vengono usati
    {
        
    }
    public void ADDCHILD(BT_NODE N) //prende il nodo attuale e lo copia su in BT_NODE
    {
        children.Add(N);
    }
    public virtual BT_STATUS PROCESS()//fa funzionare gli script sele seqe e leaf
    {
        return children[CURRENTCHILD].PROCESS();
    }
    public void printName()//debug
    {
        Debug.Log(NODENAME);
    }
}
