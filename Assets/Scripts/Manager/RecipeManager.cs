using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    public GameObject[] Recipe;
    public GameObject[] Material;
    public int currentRecipe;
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
    }
}
