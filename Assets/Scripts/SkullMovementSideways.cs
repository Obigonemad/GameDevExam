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
        // Opdater den aktuelle sidel�ns offset
        sidewaysOffset += sidewaysSpeed * movementDirection * Time.deltaTime;

        // Tjek om vi har n�et gr�nsen for sidel�ns bev�gelse
        if (Mathf.Abs(sidewaysOffset) >= sidewaysDistance)
        {
            // Begr�ns offset til maksimal distance
            sidewaysOffset = sidewaysDistance * Mathf.Sign(sidewaysOffset);

            // Skift retning
            movementDirection *= -1;
        }

        // Opdater positionen baseret p� startpositionen og den aktuelle sidel�ns offset
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            initialPosition.z + sidewaysOffset
        );
    }
}