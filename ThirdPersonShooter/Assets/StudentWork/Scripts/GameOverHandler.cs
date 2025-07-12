using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverHandler : MonoBehaviour
{
    private void Update()
    {
        // Allow restart with space key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnRestartButtonPressed();
        }
    }
    public void OnRestartButtonPressed()
    {
        if (LevelManager.Instance != null)
        {
            Time.timeScale = 1f;
            LevelManager.Instance.OnRestartGame();
        }
        else
        {
            Debug.LogWarning("LevelManager not found.");
        }
    }

    public void OnQuitButtonPressed()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.QuitGame();
        }
        else
        {
            Debug.LogWarning("LevelManager not found.");
        }
    }
}
