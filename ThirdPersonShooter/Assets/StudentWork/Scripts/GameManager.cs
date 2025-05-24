using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //singleton pattern
    public static GameManager Instance { get; private set;  }

    [SerializeField] private CharacterManager characterManager;
    [SerializeField] private LevelManager levelManager;


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
        levelManager.LoadLevelAdditively("SimpleLevel");
        characterManager.SpawnCharacter();
    }
    // Update is called once per frame
    void Update()
    { 
        TogglePause();
        RestartGame();
    }

    public void TogglePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
            //restart game
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);

            Debug.Log("Game Restarted!");
        }
    }

}
