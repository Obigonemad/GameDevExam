using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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

    // Reference til checkpoints - kan tilpasses dit checkpoint system
    [Header("Checkpoint System")]
    [SerializeField] private Transform lastCheckpoint;

    private string currentSceneName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentSceneName = scene.name;

        // Find pauseMenuUI i den nye scene hvis den ikke allerede er sat
        if (pauseMenuUI == null)
        {
            Transform canvas = GameObject.FindObjectOfType<Canvas>()?.transform;
            if (canvas != null)
            {
                Transform panel = canvas.Find("PauseMenuPanel");
                if (panel != null)
                    pauseMenuUI = panel.gameObject;
            }
        }

        // Sørg for at pausemenuen er deaktiveret ved start
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        GameIsPaused = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        // Ignorer ESC i MainMenu
        if (currentSceneName == mainMenuSceneName)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
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
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Stop menumusik
        if (menuMusic != null && menuMusic.isPlaying)
            menuMusic.Stop();

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Stop menumusik
        if (menuMusic != null && menuMusic.isPlaying)
            menuMusic.Stop();

        SceneManager.LoadScene(currentSceneName);
    }

    public void RestartCheckpoint()
    {
        // Implementer logik til at vende tilbage til sidste checkpoint
        Time.timeScale = 1f;
        GameIsPaused = false;

        // Stop menumusik
        if (menuMusic != null && menuMusic.isPlaying)
            menuMusic.Stop();

        // Find spilleren
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Hvis du har et checkpoint system, skal dette tilpasses
        if (player != null && lastCheckpoint != null)
        {
            player.transform.position = lastCheckpoint.position;
            Debug.Log("Player respawned at checkpoint");
        }
        else
        {
            Debug.LogWarning("No checkpoint found. Consider implementing a checkpoint system.");
        }

        Resume();
    }

    public void ExitGame()
    {
        Debug.Log("Exiting game...");

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
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Dette er en hjælpefunktion til at sætte det sidste checkpoint
    public void SetLastCheckpoint(Transform checkpoint)
    {
        lastCheckpoint = checkpoint;
        Debug.Log("Checkpoint set at: " + checkpoint.position);
    }
}