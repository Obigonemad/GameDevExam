using UnityEngine;

public class EagleFlyInCircles : MonoBehaviour
{
    public float radius = 10f; // Radius of the circular path
    public float speed = 2f;   // Speed of rotation
    public float height = 5f;  // Height offset

    private float angle = 0f;
    private Vector3 startPosition;
    
    void Start()
    {
        startPosition = transform.position; // Store initial position
    }

    void Update()
    {
        angle += speed * Time.deltaTime; // Increase the angle over time

        // Calculate new position
        float x = startPosition.x + Mathf.Cos(angle) * radius;
        float z = startPosition.z + Mathf.Sin(angle) * radius;
        float y = startPosition.y + height; // Maintain height

        Vector3 newPosition = new Vector3(x, y, z);

        // Rotate eagle to face the direction it's moving
        Vector3 direction = newPosition - transform.position; // Calculate movement direction
        if (direction != Vector3.zero)
        {
            transform.forward = direction; // Make eagle face its movement direction
        }

        // Apply new position
        transform.position = newPosition;
    }
}