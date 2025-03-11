using UnityEngine;

public class PlatformUpDownMovement : MonoBehaviour
{
    [SerializeField] private float verticalSpeed = 1.0f;
    [SerializeField] private float verticalDistance = 2.0f;

    private Vector3 initialPosition;
    private float movementDirection = 1.0f;
    private float verticalOffset = 0f;

    void Start()
    {
        // Gem platformens startposition
        initialPosition = transform.position;
    }

    void Update()
    {
        // Opdater den aktuelle vertikale offset
        verticalOffset += verticalSpeed * movementDirection * Time.deltaTime;

        // Tjek om vi har nået grænsen for vertikal bevægelse
        if (Mathf.Abs(verticalOffset) >= verticalDistance)
        {
            // Begræns offset til maksimal distance
            verticalOffset = verticalDistance * Mathf.Sign(verticalOffset);

            // Skift retning
            movementDirection *= -1;
        }

        // Opdater positionen baseret på startpositionen og den aktuelle vertikale offset
        transform.position = new Vector3(
            transform.position.x,
            initialPosition.y + verticalOffset,
            transform.position.z
        );
    }
}