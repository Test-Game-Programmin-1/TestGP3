using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //public TextMesh[] OrderText;
    [SerializeField] TMP_Text[] OrderMaterialText;
    [SerializeField] TMP_Text[] MatOwnedText;
    [SerializeField] GameObject[] Order;
    [SerializeField] Image EnergyBar;
    public static UIManager instance;
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
        BT_CONTROLLER.OnEnergyUsage += UpdateEnergyBar;
        BT_CONTROLLER.OnQuantityLeft += UpdateMatText;
        RecipeManager.MaterialOwned += UpdateMatText;
        RecipeManager.OrderTV += OrderTV;
        BT_CONTROLLER.ResetOrder += OrderReset;
    }
    void OnDisable()
    {
        BT_CONTROLLER.OnEnergyUsage -= UpdateEnergyBar;
        BT_CONTROLLER.OnQuantityLeft -= UpdateMatText;
        RecipeManager.MaterialOwned -= UpdateMatText;
        RecipeManager.OrderTV -= OrderTV;
        BT_CONTROLLER.ResetOrder -= OrderReset;
    }

    private void UpdateEnergyBar()
    {
        EnergyBar.fillAmount = BT_CONTROLLER.instance.Energy / BT_CONTROLLER.instance.MaxEnergy;
    }
    private void UpdateMatText()
    {
        MatOwnedText[0].text = RecipeManager.instance.MatOwned[0].ToString();
        MatOwnedText[1].text = RecipeManager.instance.MatOwned[1].ToString();
        MatOwnedText[2].text = RecipeManager.instance.MatOwned[2].ToString();
    }
    private void OrderTV()
    {
        if(RecipeManager.instance.currentRecipe == 0)
        {
            OrderMaterialText[0].text = RecipeManager.instance.orderMat1[0].ToString();
            OrderMaterialText[1].text = RecipeManager.instance.orderMat1[1].ToString();
            OrderMaterialText[2].text = RecipeManager.instance.orderMat1[2].ToString();
            Order[0].SetActive(true);
            return;
        }
        if(RecipeManager.instance.currentRecipe == 1)
        {
            OrderMaterialText[0].text = RecipeManager.instance.orderMat2[0].ToString();
            OrderMaterialText[1].text = RecipeManager.instance.orderMat2[1].ToString();
            OrderMaterialText[2].text = RecipeManager.instance.orderMat2[2].ToString();
            Order[1].SetActive(true);
            return;
        }
        if(RecipeManager.instance.currentRecipe == 2)
        {
            OrderMaterialText[0].text = RecipeManager.instance.orderMat3[0].ToString();
            OrderMaterialText[1].text = RecipeManager.instance.orderMat3[1].ToString();
            OrderMaterialText[2].text = RecipeManager.instance.orderMat3[2].ToString();
            Order[2].SetActive(true);
            return;
        }
    }
    private void OrderReset()
    {
        Order[0].SetActive(false);
        Order[1].SetActive(false);
        Order[2].SetActive(false);
        OrderMaterialText[0].text = "0";
        OrderMaterialText[1].text = "0";
        OrderMaterialText[2].text = "0";
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}