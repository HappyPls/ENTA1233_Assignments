using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levelLayoutPrefabs;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject agentPrefab;

    private GameObject currentPlayer;
    private List<GameObject> currentAgents = new List<GameObject>();
    private GameObject currentLayoutInstance;

    [SerializeField] private GameObject gameOverUI;
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadLevel(string levelName, LoadSceneMode mode)
    {
        SceneManager.LoadScene(levelName, mode);
    }

    public void LoadLevelAdditively(string levelName)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ensure we only spawn once per additive load
        if (mode == LoadSceneMode.Additive)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.SetActiveScene(scene);
            SceneManager.UnloadSceneAsync("MainMenu");

            if (this.gameObject.activeInHierarchy)
            {
                StartCoroutine(DelayedSpawn());
            }
            else
            {
                Debug.LogWarning("LevelManager is inactive. Cannot start DelayedSpawn.");
            }
        }
    }

    private IEnumerator DelayedSpawn()
    {
        yield return null;

        Scene activeScene = SceneManager.GetActiveScene();
        SpawnLayouts(activeScene);

        yield return null;

        RespawnPlayer(activeScene);
        SpawnAgents();
    }

    private void SpawnLayouts(Scene targetScene)
    {
        if (currentLayoutInstance != null)
            Destroy(currentLayoutInstance);

        int randomIndex = Random.Range(0, levelLayoutPrefabs.Length);
        int[] rotationAngles = { 0, 90, 180, 270 };
        int randomYRotation = rotationAngles[Random.Range(0, rotationAngles.Length)];
        Quaternion rotation = Quaternion.Euler(0, randomYRotation, 0);

        currentLayoutInstance = Instantiate(levelLayoutPrefabs[randomIndex], Vector3.zero, rotation);
        SceneManager.MoveGameObjectToScene(currentLayoutInstance, targetScene);
    }

    private void RespawnPlayer(Scene targetScene)
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (currentPlayer != null)
            Destroy(currentPlayer);

        currentPlayer = Instantiate(characterPrefab, Vector3.zero, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(currentPlayer, targetScene);
        currentPlayer.GetComponent<PlayerStats>()?.InitializeStats();

    }

    private void SpawnAgents()
    {
        foreach (GameObject agent in currentAgents)
        {
            if (agent != null)
                Destroy(agent);
        }

        currentAgents.Clear();

        if (currentLayoutInstance == null)
        {
            Debug.LogWarning("No layout to spawn agents in.");
            return;
        }

        Renderer[] renderers = currentLayoutInstance.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return;

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer rend in renderers)
        {
            bounds.Encapsulate(rend.bounds);
        }

        int numAgents = Random.Range(10, 20);
        float minDistanceFromPlayer = 10f;

        for (int i = 0; i < numAgents; i++)
        {
            bool foundPosition = false;
            for (int attempt = 0; attempt < 10 && !foundPosition; attempt++)
            {
                Vector3 localRandomPoint = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    bounds.center.y,
                    Random.Range(bounds.min.z, bounds.max.z)
                );


                if (NavMesh.SamplePosition(localRandomPoint, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
                {
                    if (currentPlayer != null)
                    {
                        float distance = Vector3.Distance(hit.position, currentPlayer.transform.position);
                        if (distance < minDistanceFromPlayer)
                            continue;
                    }
                    Quaternion randomizeRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
                    GameObject agent = Instantiate(agentPrefab, hit.position, randomizeRotation);
                    currentAgents.Add(agent);
                    Debug.Log($"Agent {i + 1} spawned at {hit.position}!");
                    foundPosition = true;
                }
            }

        if (!foundPosition)
            {
                Debug.LogWarning($"Ägent {i + 1} failed to find valid NavMesh in layout bounds.");
            }
        }
    }
    public void ClearState()
    {
        if (currentPlayer != null) Destroy(currentPlayer);
        if (currentLayoutInstance != null) Destroy(currentLayoutInstance);

        foreach (var agent in currentAgents)
        {
            if (agent != null) Destroy(agent);
        }

        currentAgents.Clear();
    }
    public void Respawn()
    {
        Time.timeScale = 1f;
        StartCoroutine(DelayedSpawn());
    }
    public void ShowGameOverScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }
    public void OnRestartGame()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.LoadLevelAdditively("SimpleLevel");
        }
    }
}
