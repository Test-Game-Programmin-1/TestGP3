using UnityEngine;
public enum GameState
{
    Playing,
    Paused,
    X2
}
public class GameManager : MonoBehaviour
{
    public GameState state;
    public static GameManager instance;
    void Awake()
    {
        if(instance != null)
        {
            Destroy(instance);
            return;
        }
            instance = this;
        state = GameState.Playing;
    }
    void OnEnable()
    {
        if(state == GameState.Paused)
        {
            Time.timeScale = 0;
        }
        else if(state == GameState.Playing)
        {
            Time.timeScale = 1;
        }
        else if(state == GameState.X2)
        {
            Time.timeScale = 2;
        }
    }
}
