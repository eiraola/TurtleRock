using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 10.0f;
    [SerializeField] 
    private PlayerInput _playerInput;
    [SerializeField]
    private LayerMask layersToIgnore = new LayerMask();
    [SerializeField]
    private float gravityForce = 0.0f;
    [SerializeField]
    private float maxJumpHeigth = 0.0f;
    private bool isGrounded = true;
    private Vector2 currentDirection = Vector2.zero;
    private Vector3 currentVerticalDirection = Vector3.zero;
    private Rigidbody ownerRB;
    private CapsuleCollider ownerCollider;
    private float playerSize = 1.0f;
    
    private void OnEnable()
    {
        ownerRB = GetComponent<Rigidbody>();
        ownerCollider = GetComponent<CapsuleCollider>();
        playerSize = ownerCollider.bounds.size.y;
        _playerInput.MovementEvent += SetDesiredDirection;
        _playerInput.JumpEvent += Jump;
    }
    private void OnDisable()
    {
        _playerInput.MovementEvent -= SetDesiredDirection;
        _playerInput.JumpEvent -= Jump;
    }
    private void FixedUpdate()
    {
        SetOwnerDirection();
    }

    /// <summary>
    /// Sets the owner desired direction.
    /// </summary>
    /// <param name="direction"></param>
    private void SetDesiredDirection(Vector2 direction)
    {
        currentDirection = direction;
    }

    /// <summary>
    /// Sets the rigidbody component direction so it will follow the ground
    /// </summary>
    private void SetOwnerDirection()
    {
        Vector3 finalDirection = DirectionToCameraVector();
        RotatePlayerTowardsMovement(finalDirection);
         ownerRB.velocity = (finalDirection * movementSpeed + CurrentVerticalSpeed());
    }
    private void RotatePlayerTowardsMovement(Vector3 direction)
    {
        transform.LookAt(transform.position +direction);
    }
    /// <summary>
    /// Returns the direction vector in the coordinates of the camera.
    /// </summary>
    /// <returns>The direction vector</returns>
    private Vector3 DirectionToCameraVector()
    {
        return (CameraToGroundVector(Camera.main.transform.right) * currentDirection.x + CameraToGroundVector(Camera.main.transform.forward) * currentDirection.y);
    }

    /// <summary>
    /// Projects a vector to the ground
    /// </summary>
    /// <param name="direction"></param>
    /// <returns>The projected vector</returns>
    private Vector3 CameraToGroundVector(Vector3 direction)
    {
        Vector3 groundNormalVector = Vector3.up;
        RaycastHit hit;
        isGrounded = false;
        if (Physics.Raycast(transform.position - Vector3.up * playerSize / 2f, Vector3.down, out hit, 0.1f, ~layersToIgnore, QueryTriggerInteraction.Ignore))
        {
            groundNormalVector = hit.normal;
            isGrounded = true;
            if (currentVerticalDirection.y <= 0.0f)
            {
                currentVerticalDirection = Vector3.zero;
            }
           
        }
        Debug.DrawLine(transform.position - Vector3.up * playerSize / 2f,(transform.position - Vector3.up * playerSize / 2f) + Vector3.down * 0.1f );
        return Vector3.ProjectOnPlane(direction, groundNormalVector);
    }
    /// <summary>
    /// Updates current vertical speed
    /// </summary>
    /// <returns></returns>
    private Vector3 CurrentVerticalSpeed()
    {
        currentVerticalDirection += (-Vector3.up * gravityForce * (!isGrounded ? 1 : 0))* Time.fixedDeltaTime;
        return currentVerticalDirection;
    }
    /// <summary>
    /// Changes the player vertical speed so it will jump to certain direction
    /// </summary>
    private void Jump()
    {
        if (!isGrounded)
        {
            return;
        }
        currentVerticalDirection = Vector3.up * Mathf.Sqrt(2.0f * gravityForce * maxJumpHeigth);
    }
}
