using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //public TextMesh[] OrderText;
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
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
