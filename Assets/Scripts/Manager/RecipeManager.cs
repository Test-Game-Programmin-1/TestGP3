using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public GameObject[] Recipe;
    public GameObject[] Material;
    public int[] MatNeeded;
    public int[] MatOwned;
    public int ChestMaxVolume;
    public int currentRecipe;

    [Header("Orders")]
    public int[] orderMat1 = new int[3];
    public int[] orderMat2 = new int[3];
    public int[] orderMat3 = new int[3];

    public int currentMat;
    public static RecipeManager instance;
    void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
            return;
        }
            instance = this;
    }
    void Start()
    {
        MatOwned[0] = ChestMaxVolume;
        MatOwned[1] = ChestMaxVolume;
        MatOwned[2] = ChestMaxVolume;
        //aggiungere evento UI
    }
    void OnEnable()
    {
        BT_CONTROLLER.OnGetTheOrder += GetTheOrder;
    }
    void OnDisable()
    {
        BT_CONTROLLER.OnGetTheOrder -= GetTheOrder;
    }
    private void GetTheOrder()
    {
        currentRecipe = Random.Range(0, Recipe.Length);
        Debug.Log("New Order: " + Recipe[currentRecipe].name);
        if(currentRecipe == 0)
        {
            MatNeeded[0] = 1;//orderMat1[0];
            MatNeeded[1] = 1;//orderMat1[1];
            MatNeeded[2] = 0;//orderMat1[2];
            return;
        }
        if(currentRecipe == 1)
        {
            MatNeeded[0] = 1;//orderMat2[0];
            MatNeeded[1] = 0;//orderMat2[1];
            MatNeeded[2] = 1;//orderMat2[2];
            return;
        }
        if(currentRecipe == 2)
        {
            MatNeeded[0] = 0;//orderMat3[0];
            MatNeeded[1] = 1;//orderMat3[1];
            MatNeeded[2] = 1;//orderMat3[2];
            return;

        }

    }
}
