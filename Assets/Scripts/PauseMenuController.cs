using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PauseMenuController : MonoBehaviour
{
    public static PauseMenuController Instance { get; private set; }
    public static bool GameIsPaused = false;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    // Audio
    [Header("Audio")]
    [SerializeField] private AudioSource menuMusic;
    [SerializeField] private AudioSource buttonSound;

    // Checkpoints håndtering med scene-specifik lagring
    [System.Serializable]
    public class CheckpointData
    {
        public string sceneName;
        public Vector3 position;
    }

    // Reference til checkpoints
    [Header("Checkpoint System")]
    [SerializeField] private Transform lastCheckpoint;
    [SerializeField] private List<CheckpointData> checkpointsByScene = new List<CheckpointData>();
    private bool hasCheckpoint = false;

    private string currentSceneName;

    private void Awake()
    {
        Debug.Log("PauseMenuController Awake called");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            Debug.Log("PauseMenuController instance created and set to DontDestroyOnLoad");
        }
        else
        {
            Debug.Log("Duplicate PauseMenuController found, destroying this one");
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;
        Debug.Log($"Scene loaded: {currentSceneName}");

        // Find pauseMenuUI i den nye scene hvis den ikke allerede er sat
        if (pauseMenuUI == null)
        {
            Transform canvas = GameObject.FindObjectOfType<Canvas>()?.transform;
            if (canvas != null)
            {
                Transform panel = canvas.Find("PauseMenuPanel");
                if (panel != null)
                {
                    pauseMenuUI = panel.gameObject;
                    Debug.Log($"Found PauseMenuPanel in scene {currentSceneName}");
                }
                else
                {
                    Debug.LogWarning($"Could not find 'PauseMenuPanel' under Canvas in scene {currentSceneName}");
                }
            }
            else
            {
                Debug.LogWarning($"Could not find Canvas in scene {currentSceneName}");
            }
        }

        // Sørg for at pausemenuen er deaktiveret ved start
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            Debug.Log("PauseMenuPanel set to inactive");
        }

        GameIsPaused = false;
        Time.timeScale = 1f;

        // Vis alle gemte checkpoints til debugging
        Debug.Log($"Checkpoints by scene count: {checkpointsByScene.Count}");
        foreach (var checkpoint in checkpointsByScene)
        {
            Debug.Log($"Stored checkpoint for scene {checkpoint.sceneName} at position {checkpoint.position}");
        }
    }

    void Update()
    {
        // Ignorer ESC i MainMenu
        if (currentSceneName == mainMenuSceneName)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("ESC key pressed");
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Debug.Log("Resume called");
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        GameIsPaused = false;

        // Lås og skjul cursor når spillet genoptages
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Stop menumusik
        if (menuMusic != null && menuMusic.isPlaying)
            menuMusic.Stop();
    }

    public void Pause()
    {
        Debug.Log("Pause called");
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        GameIsPaused = true;

        // Aktiver cursor i pausemenuen
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Start menumusik
        if (menuMusic != null && !menuMusic.isPlaying)
            menuMusic.Play();
    }

    public void LoadMainMenu()
    {
        Debug.Log("LoadMainMenu called");
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Stop menumusik
        if (menuMusic != null && menuMusic.isPlaying)
            menuMusic.Stop();

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RestartLevel()
    {
        Debug.Log($"RestartLevel called for scene {currentSceneName}");
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Stop menumusik
        if (menuMusic != null && menuMusic.isPlaying)
            menuMusic.Stop();

        SceneManager.LoadScene(currentSceneName);
    }

    public void RestartCheckpoint()
    {
        Debug.Log($"RESTART CHECKPOINT called in scene {currentSceneName}");

        Time.timeScale = 1f;
        GameIsPaused = false;

        if (menuMusic != null && menuMusic.isPlaying)
            menuMusic.Stop();

        // Find spilleren med tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Debug.Log($"Found player: {player.name}");

            // Få fat i respawner komponenten
            PlayerRespawner respawner = player.GetComponent<PlayerRespawner>();

            if (respawner != null)
            {
                // Brug den eksisterende respawn funktion
                respawner.StartRespawn();
                Debug.Log("Called PlayerRespawner.StartRespawn()");
            }
            else
            {
                Debug.LogError("Player does not have PlayerRespawner component!");
            }
        }
        else
        {
            Debug.LogError("Could not find player with tag 'Player'!");
        }

        Resume();
    }

    public void ExitGame()
    {
        Debug.Log("ExitGame called");

#if UNITY_EDITOR
        // Stopper play mode i Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Afslutter spillet ved build
        Application.Quit();
#endif
    }

    public void PlayButtonSound()
    {
        if (buttonSound != null)
            buttonSound.Play();
    }

    void OnDestroy()
    {
        Debug.Log("PauseMenuController OnDestroy called");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Dette er en hjælpefunktion til at sætte det sidste checkpoint
    public void SetLastCheckpoint(Transform checkpoint)
    {
        if (checkpoint != null)
        {
            // Gem i legacy system
            lastCheckpoint = checkpoint;
            hasCheckpoint = true; // Dette bliver sat, men bruges ikke korrekt i RestartCheckpoint
            Debug.Log($"Legacy checkpoint set at: {checkpoint.position} in scene: {currentSceneName}");

            // Gem også i scene-specifik system
            string sceneName = SceneManager.GetActiveScene().name;

            // Find eksisterende eller tilføj ny
            CheckpointData data = checkpointsByScene.Find(c => c.sceneName == sceneName);

            if (data != null)
            {
                data.position = checkpoint.position;
                Debug.Log($"Updated scene-specific checkpoint in {sceneName} to {checkpoint.position}");
            }
            else
            {
                checkpointsByScene.Add(new CheckpointData
                {
                    sceneName = sceneName,
                    position = checkpoint.position
                });
                Debug.Log($"Added new scene-specific checkpoint in {sceneName} at {checkpoint.position}");
            }
        }
        else
        {
            Debug.LogError("Attempted to set null checkpoint!");
        }
    }

    // Hjælpe metode til at nulstille checkpoint data (brug i editor)
    public void ResetCheckpointData()
    {
        lastCheckpoint = null;
        hasCheckpoint = false;
        checkpointsByScene.Clear();
        Debug.Log("All checkpoint data has been reset");
    }

    // Editor menu til at nulstille
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Game/Reset PauseMenuController")]
    private static void ResetInstance()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance.gameObject);
            Debug.Log("PauseMenuController instance reset");
        }
    }
#endif
}