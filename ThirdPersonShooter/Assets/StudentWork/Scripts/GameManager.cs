using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //singleton pattern
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        InitializeGame();
    }

    private void InitializeGame()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
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
            Debug.Log("Ctrl+R detected!");
            Time.timeScale = 1f;
            LevelManager levelManager = FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                levelManager.ClearState();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            Debug.Log("Game Restarted!");
        }
    }

}
