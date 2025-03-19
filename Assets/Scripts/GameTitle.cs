using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameTitle : MonoBehaviour
{
    [Header("Text Settings")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private Color mainColor = Color.white;
    [SerializeField] private Color glowColor = new Color(1f, 1f, 1f, 0.5f);

    [Header("Animation Settings")]
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float pulseAmount = 0.2f;
    [SerializeField] private float hoverSpeed = 0.5f;
    [SerializeField] private float hoverAmount = 10f;

    private float initialFontSize;
    private Vector3 initialPosition;

    void Start()
    {
        if (titleText == null)
            titleText = GetComponent<TextMeshProUGUI>();

        initialFontSize = titleText.fontSize;
        initialPosition = transform.position;

        // Sæt grundlæggende text styling
        titleText.color = mainColor;
        titleText.enableVertexGradient = true;
        titleText.fontStyle = FontStyles.Bold;

        // Tilføj glow effect
        titleText.materialForRendering.EnableKeyword("GLOW_ON");
        titleText.materialForRendering.SetColor("_GlowColor", glowColor);
    }

    void Update()
    {
        // Pulserende størrelse
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        titleText.fontSize = initialFontSize * pulse;

        // Svævende bevægelse
        float hover = Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
        transform.position = initialPosition + new Vector3(0f, hover, 0f);
    }
}