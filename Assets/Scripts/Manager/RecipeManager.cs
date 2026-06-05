using System;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public GameObject[] Recipe;
    public GameObject[] Material;
    public int[] MatNeeded;
    public int[] MatOwned;
    public int ChestMaxVolume;
    public int currentRecipe;
    public Transform WorkBench;
    public Transform ExpeditionBench;

    [Header("Orders")]
    public int[] orderMat1 = new int[3];
    public int[] orderMat2 = new int[3];
    public int[] orderMat3 = new int[3];

    public float timer;
    public bool placed = false;

    public int currentMat;
    public static event Action MaterialOwned;
    public static event Action OrderTV;
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
    void Start() //setta al massimo i materiali che hanno le casse
    {
        MatOwned[0] = ChestMaxVolume;
        MatOwned[1] = ChestMaxVolume;
        MatOwned[2] = ChestMaxVolume;
        MaterialOwned?.Invoke();
    }
    void OnEnable()
    {
        BT_CONTROLLER.OnGetTheOrder += GetTheOrder;
        BT_CONTROLLER.OnCrafting += Crafting;
        BT_CONTROLLER.OnExpedition += Expedition;
    }
                                    //i vari eventi
    void OnDisable()
    {
        BT_CONTROLLER.OnGetTheOrder -= GetTheOrder;
        BT_CONTROLLER.OnCrafting -= Crafting;
        BT_CONTROLLER.OnExpedition -= Expedition;
    }
    void Update() //aggiorna il timer per la gestione della posizione dei lavori e dopo tot lo setta a false
    {
        if (placed)
        {
            timer += Time.deltaTime;
        }
        if (!placed)
        {
            timer = 0;
        }
        if(timer >= 3f)
        {
            Recipe[currentRecipe].SetActive(false);
            timer = 0;
            placed = false;
        }
    }
    private void GetTheOrder() //funzione che con random range prende un ordine e dice i materiali che servono per quel ordine
    {
        currentRecipe = UnityEngine.Random.Range(0, Recipe.Length);
        Debug.Log("New Order: " + Recipe[currentRecipe].name);
        OrderTV?.Invoke();
        if(currentRecipe == 0)
        {
            MatNeeded[0] = orderMat1[0];
            MatNeeded[1] = orderMat1[1];
            MatNeeded[2] = orderMat1[2];
            return;
        }
        if(currentRecipe == 1)
        {
            MatNeeded[0] = orderMat2[0];
            MatNeeded[1] = orderMat2[1];
            MatNeeded[2] = orderMat2[2];
            return;
        }
        if(currentRecipe == 2)
        {
            MatNeeded[0] = orderMat3[0];
            MatNeeded[1] = orderMat3[1];
            MatNeeded[2] = orderMat3[2];
            return;
        }
    }
    private void Crafting() //funzione che setta a true l'ordine selezionato e lo sposta sulla working bench
    {
        GameObject Crafted = Recipe[currentRecipe];
        Crafted.transform.position = WorkBench.position;
        Crafted.SetActive(true);
    }
    private void Expedition() //funzione che attiva il bool per la funzione del spostare e settare a false dopo tot e sposta l'oggetto sulla ExpeditionBench
    {
        GameObject Crafted = Recipe[currentRecipe];
        placed = true;
        if(timer >= 2f)
        {
            Crafted.transform.position = ExpeditionBench.position;
        }
    }
}
