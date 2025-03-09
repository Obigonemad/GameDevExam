using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI timerText;
    private float currentTime;
    private bool isTimerRunning;

    void Awake()
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
        // Find timer text i den nye scene
        timerText = GameObject.FindGameObjectWithTag("TimerText")?.GetComponent<TextMeshProUGUI>();

        if (timerText == null)
        {
            Debug.LogWarning("TimerText not found in scene: " + scene.name);
        }

        // Start timeren automatisk hvis det ikke er MainMenu
        if (scene.name != "MainMenu")
        {
            StartTimer();
        }
        else
        {
            ResetTimer();
        }
    }

    void Start()
    {
        // Initialiser display
        ResetTimer();

        // Hvis vi ikke starter fra MainMenu, start timeren med det samme
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            StartTimer();
        }
    }

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            float minutes = Mathf.FloorToInt(currentTime / 60);
            float seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            Debug.LogWarning("TimerText reference is missing!");
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
        Debug.Log("Timer Started");
    }

    public void StopTimer()
    {
        isTimerRunning = false;
        Debug.Log("Timer Stopped - Final Time: " + GetTimeString());
    }

    public void ResetTimer()
    {
        currentTime = 0;
        isTimerRunning = false;
        UpdateTimerDisplay();
        Debug.Log("Timer Reset");
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public string GetTimeString()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}