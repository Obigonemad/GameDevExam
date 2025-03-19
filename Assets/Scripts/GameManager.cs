using UnityEngine;
using TMPro; // TextMeshPro namespace
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private TextMeshProUGUI coinText; // �ndret fra Text til TextMeshProUGUI
    [SerializeField] private int totalCoins = 59; // Totalt antal m�nter i spillet
    private int collectedCoins = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Tilf�j event listener for scene loading
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI(); // Opdater UI ved start
    }

    // N�r en ny scene loades, find coin text UI'en
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find coin text i den nye scene
        GameObject coinTextObject = GameObject.FindGameObjectWithTag("CoinText");
        if (coinTextObject != null)
        {
            coinText = coinTextObject.GetComponent<TextMeshProUGUI>(); // �ndret til TextMeshProUGUI
            UpdateUI(); // Opdater UI i den nye scene
        }
    }

    public void AddCoins(int amount)
    {
        collectedCoins += amount;
        UpdateUI();
        Debug.Log("Coins collected: " + collectedCoins + "/" + totalCoins);
    }

    private void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = collectedCoins + "/" + totalCoins + " Coins";
        }
    }

    // N�r objektet �del�gges, fjern event listeneren
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}