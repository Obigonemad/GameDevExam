using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Text coinText;
    [SerializeField] private int totalCoins = 45; // Totalt antal mønter i spillet
    private int collectedCoins = 0;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Tilføj event listener for scene loading
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

    // Når en ny scene loades, find coin text UI'en
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find coin text i den nye scene
        GameObject coinTextObject = GameObject.FindGameObjectWithTag("CoinText");
        if (coinTextObject != null)
        {
            coinText = coinTextObject.GetComponent<Text>();
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

    // Når objektet ødelægges, fjern event listeneren
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}