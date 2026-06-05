using UnityEngine;
public enum GameState
{ //enum che viene usato per impostare lo stato del GamePlay
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
    void Update() //gestisce la time scale del gioco
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
            Time.timeScale = 5;
        }
    }
    public void X2() //funzioni per i bottoni in game
    {
        state = GameState.X2;
    }
    public void Pause()
    {
        state = GameState.Paused;
    }
    public void Playing()
    {
        state = GameState.Playing;
    }
}
