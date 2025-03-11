using UnityEngine;

public class SkullSidewaysMovement : MonoBehaviour
{
    [SerializeField] private float sidewaysSpeed = 2.0f;
    [SerializeField] private float sidewaysDistance = 3.0f;

    private Vector3 initialPosition;
    private float movementDirection = 1.0f;
    private float sidewaysOffset = 0f;

    void Start()
    {
        // Gem objektets startposition
        initialPosition = transform.position;
    }

    void Update()
    {
        // Opdater den aktuelle sidelæns offset
        sidewaysOffset += sidewaysSpeed * movementDirection * Time.deltaTime;

        // Tjek om vi har nået grænsen for sidelæns bevægelse
        if (Mathf.Abs(sidewaysOffset) >= sidewaysDistance)
        {
            // Begræns offset til maksimal distance
            sidewaysOffset = sidewaysDistance * Mathf.Sign(sidewaysOffset);

            // Skift retning
            movementDirection *= -1;
        }

        // Opdater positionen baseret på startpositionen og den aktuelle sidelæns offset
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            initialPosition.z + sidewaysOffset
        );
    }
}