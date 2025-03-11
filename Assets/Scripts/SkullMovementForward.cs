using UnityEngine;

public class SkullMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float moveDistance = 3.0f;

    private Vector3 startPosition;
    private float direction = 1.0f;
    private float currentOffset = 0f;

    void Start()
    {
        // Gem skraniets startposition
        startPosition = transform.position;
    }

    void Update()
    {
        // Opdater den aktuelle offset
        currentOffset += moveSpeed * direction * Time.deltaTime;

        // Tjek om vi har nået grænsen for bevægelse
        if (Mathf.Abs(currentOffset) >= moveDistance)
        {
            // Begræns offset til maksimal distance
            currentOffset = moveDistance * Mathf.Sign(currentOffset);

            // Skift retning
            direction *= -1;
        }

        // Opdater positionen baseret på startpositionen og den aktuelle offset
        transform.position = new Vector3(
            startPosition.x + currentOffset,
            transform.position.y,
            transform.position.z
        );
    }
}