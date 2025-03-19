using UnityEngine;
using TMPro;

public class TitleEffect : MonoBehaviour
{
    private TextMeshProUGUI titleText;
    [Header("Animation Settings")]
    [SerializeField] private float hoverSpeed = 1f;    // Hvor hurtigt titlen svæver
    [SerializeField] private float hoverAmount = 20f;  // Hvor meget titlen bevæger sig
    [SerializeField] private float rotateAmount = 2f;  // Hvor meget titlen roterer

    private Vector3 startPosition;
    private float timeOffset;

    void Start()
    {
        titleText = GetComponent<TextMeshProUGUI>();
        startPosition = transform.position;
        timeOffset = Random.Range(0f, 2f * Mathf.PI); // Random startposition
    }

    void Update()
    {
        // Svævende bevægelse
        float newY = startPosition.y + Mathf.Sin((Time.time + timeOffset) * hoverSpeed) * hoverAmount;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        // Let rotation
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * hoverSpeed) * rotateAmount);
    }
}