using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //singleton pattern
    public static GameManager Instance { get; private set;  }

    [SerializeField] private LevelManager LevelManager;
    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeGame();
    }
    private void InitializeGame()
    {
        LevelManager.LoadLevelAdditively("SimpleLevel");
    }
    // Update is called once per frame
    void Update()
    { 
        TogglePause();
        RestartGame();
    }

    public void TogglePause()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.P))
        {
            bool isPaused = Time.timeScale == 0;
            Time.timeScale = isPaused ? 1 : 0;
            Debug.Log("Game " + (isPaused ? "resumed" : "paused"));
        }
    }

    public void RestartGame()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            LevelManager.Respawn();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            Debug.Log("Game Restarted!");
        }
    }

}
