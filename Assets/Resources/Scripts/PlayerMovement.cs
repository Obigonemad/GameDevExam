using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
     private const float gravity = -9.82f;
    [SerializeField] private float WalkSpeed = 1.5f;
    [SerializeField] private float SprintSpeed = 3.0f;
    [SerializeField] private float RotateSpeed = 55;
    [SerializeField] private float m_jumpHeight = 3.0f;
    [SerializeField] private float DistToGround = 0.9f;

    [SerializeField] private CharacterController m_characterController;
    [SerializeField] private Camera m_playerCamera;

    [SerializeField] Transform m_modelTransform;

    [SerializeField] private bool m_useBlendTree; 

    public event Action Jumping;  
    public event Action Landing; 
    private bool is2DMode = false; // Set to true when in the cave
    public bool IsSprinting() => m_sprinting;
    private bool m_sprinting;

    private float yaw;

    private Vector3 drawDir;
    private Vector3 velocity;

    private bool wasGrounded = false;

    private bool m_isFalling;

    public bool isFalling() => m_isFalling;

    private void Update() 
    {
        SetSprinting();
        AxesMovement();
        YOrientation();
        
        if (m_characterController.velocity.y > .25f)
        {
            Debug.Log("Rising");
        }
        else if (m_characterController.velocity.y < -.25f)
        {
            Debug.Log("Falling");
            m_isFalling = true;
        }

        if (!IsGrounded() && wasGrounded)
        {
            Debug.Log("LIFT OFF!");
        }
        else if (IsGrounded() && !wasGrounded)
        {
            m_isFalling = false;
            Landing?.Invoke();  
            Debug.Log("Landing");
        }

        wasGrounded = IsGrounded();

        if (IsGrounded())
            m_isFalling = false;
    }


    private void SetSprinting() => m_sprinting = Input.GetKey(KeyCode.LeftShift) ? true : false;

    private void YOrientation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            Rotate(true);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            Rotate(false);
        }
        else 
        {
            yaw += RotateSpeed * 0.035f * Input.GetAxis("Mouse X");
            transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        }
    }

    private void AxesMovement()
    {
        float h = Input.GetAxis("Horizontal"); // A/D for left/right
        float v = is2DMode ? 0 : Input.GetAxis("Vertical"); // Disable W/S in 2D mode

        Vector3 moveDir;
        var speed = m_sprinting ? SprintSpeed : WalkSpeed;

        if (is2DMode)
        {
            // Move only along the Z-axis (A = -Z, D = +Z)
            moveDir = new Vector3(0, 0, h);
        }
        else
        {
            // Normal 3D movement
            Vector3 _forward = new Vector3(transform.forward.x, 0, transform.forward.z);
            Vector3 _right = new Vector3(transform.right.x, 0, transform.right.z);

            Vector3 forward = _forward * v;
            Vector3 right = _right * h;
            moveDir = forward + right;
        }

        if (moveDir.sqrMagnitude > 1)
            moveDir.Normalize();

        SetModelRotation(moveDir);
        m_characterController.Move(moveDir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jumping?.Invoke();
            Invoke("SetJumpVelocity", 0.3f);
        }

        velocity.y += gravity * Time.deltaTime;
        m_characterController.Move(velocity * Time.deltaTime);
    }


    private void SetJumpVelocity()
    {
        velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * gravity);
    }

    private void Rotate(bool right)
    {
        int dir = right ? -1 : 1;

        yaw += RotateSpeed * 0.035f * dir;
        transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
    }
    
    private void SetModelRotation(Vector3 v)
    {
        v = new Vector3(v.x, 0.0f, v.z);
        drawDir = v * 5; // used for OnDrawGizmos (debugging)

        if (m_useBlendTree)
        {
            if (m_sprinting) // This is legacy for when blendtrees were only used for walking but not sprinting. 
            {
                if (v != Vector3.zero) {
                    Quaternion q = Quaternion.LookRotation(v, Vector3.up);
                    m_modelTransform.rotation = q;
                }
            }
            else
            {
                m_modelTransform.localRotation = Quaternion.identity; 
            }
        }
        else 
        {
            if (v != Vector3.zero) 
            {
                Quaternion q = Quaternion.LookRotation(v, Vector3.up);
                m_modelTransform.rotation = q;
            }
        }
    }

    public bool IsGrounded() => Physics.Raycast(transform.position, -Vector3.up, DistToGround + 0.01f);
    
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, drawDir);
    }
}
