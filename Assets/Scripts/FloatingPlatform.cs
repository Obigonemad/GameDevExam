using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.up; // Direction of movement
    public float moveDistance = 2f; // How far it moves
    public float moveSpeed = 1f; // Speed of movement

    private Vector3 startPosition;
    private bool movingForward = true;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float moveStep = moveSpeed * Time.deltaTime;
        if (movingForward)
        {
            transform.position += moveDirection.normalized * moveStep;
            if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
            {
                movingForward = false;
            }
        }
        else
        {
            transform.position -= moveDirection.normalized * moveStep;
            if (Vector3.Distance(startPosition, transform.position) <= 0.1f)
            {
                movingForward = true;
            }
        }
    }
}