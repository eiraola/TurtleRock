using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [Header("System")]
    [SerializeField]
    private PlayerInput _playerInput;
    [Header("Movement values")]
    [Tooltip("The movement speed per seconds/metters of the player")]
    [SerializeField]
    private float movementSpeed = 10.0f;
    [Tooltip("The collision layers that will be ignored for the player when doing calculations")]
    [SerializeField]
    private LayerMask layersToIgnore = new LayerMask();
    [Tooltip("How many lectures will the player do against the ground. The less you have the cheaper it is, but the calculations would be less acurate.")]
    [SerializeField]
    private int numberOfGroundLectures = 10;
    [SerializeField]
    private float skinSize = 0.3f;
    [Tooltip("The force of the gravity")]
    [SerializeField]
    private float gravityForce = 0.0f;
    [Tooltip("How high can the player jump")]
    [SerializeField]
    private float maxJumpHeigth = 0.0f;
    private bool isGrounded = true;
    private Vector2 currentDirection = Vector2.zero;
    private Vector3 currentVerticalDirection = Vector3.zero;
    private Rigidbody ownerRB;
    private CapsuleCollider ownerCollider;
    private float _playerSize = 1.0f;
    private float _playerRadius = 1.0f;


    private void OnEnable()
    {
        ownerRB = GetComponent<Rigidbody>();
        ownerCollider = GetComponent<CapsuleCollider>();
        _playerSize = ownerCollider.bounds.size.y;
        _playerRadius = ownerCollider.radius;
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
        isGrounded = CheckIfGrounded();
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
        ownerRB.velocity = GetExternalAgentSpeed() + (finalDirection * movementSpeed + CurrentVerticalSpeed());
    }
    /// <summary>
    /// Rotates the player to the current direction we are going.
    /// </summary>
    /// <param name="direction"></param>
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
        return (CameraToGroundVector(Camera.main.transform.right).normalized * currentDirection.x + CameraToGroundVector(Camera.main.transform.forward).normalized * currentDirection.y);
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
        if (Physics.Raycast(transform.position - Vector3.up * _playerSize / 2f, Vector3.down, out hit, 0.1f, ~layersToIgnore, QueryTriggerInteraction.Ignore))
        {
            groundNormalVector = hit.normal;
        }
        Debug.DrawLine(transform.position - Vector3.up * _playerSize / 2f,(transform.position - Vector3.up * _playerSize / 2f) + Vector3.down * 0.1f );
        return Vector3.ProjectOnPlane(direction, groundNormalVector);
    }
    /// <summary>
    /// Returns if the player is grounded
    /// </summary>
    /// <returns></returns>
    private bool CheckIfGrounded()
    {
        float angleBetweenLectures = 360.0f / (float)numberOfGroundLectures;
        RaycastHit hit;
        bool result = false;
        for (int i = 0; i < numberOfGroundLectures; i++)
        {
            Debug.DrawLine(PlayerBoundsPositionByAngle(i * angleBetweenLectures), PlayerBoundsPositionByAngle(i * angleBetweenLectures) + Vector3.down * 0.5f);
            if (Physics.Raycast(PlayerBoundsPositionByAngle(i * angleBetweenLectures), Vector3.down, out hit, 0.5f, ~layersToIgnore, QueryTriggerInteraction.Ignore))
            {
                result = true;
            } 
        }
        return result;
    }
    /// <summary>
    /// Returns a point around the player bounds definded by the angle
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    private Vector3 PlayerBoundsPositionByAngle(float angle)
    {
        return transform.position - Vector3.up * _playerSize / 2f + _playerRadius * (Vector3.right * Mathf.Cos(angle * Mathf.Deg2Rad) + Vector3.forward * Mathf.Sin(angle * Mathf.Deg2Rad));
    }
    /// <summary>
    /// Updates current vertical speed
    /// </summary>
    /// <returns></returns>
    private Vector3 CurrentVerticalSpeed()
    {
        currentVerticalDirection += (-Vector3.up * gravityForce)* Time.fixedDeltaTime;
        if (currentVerticalDirection.y <= 0.0f && isGrounded)
        {
            currentVerticalDirection = Vector3.zero;
        }
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
    /// <summary>
    /// In case we are above an enemy, we will sum his speed to us so we move mith him
    /// </summary>
    /// <returns></returns>
    private Vector3 GetExternalAgentSpeed()
    {
        Vector3 groundNormalVector = Vector3.up;
        RaycastHit hit;
        if (Physics.Raycast(transform.position - Vector3.up * _playerSize / 2f, Vector3.down, out hit, 0.6f, ~layersToIgnore, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.gameObject.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
            {
                return agent.velocity;
            }
        }
        return Vector3.zero;
    }
}
