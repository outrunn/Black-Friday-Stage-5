using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private bool rotates = false;
    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private GameObject viewArea;

    [Header("Movement Settings")]
    [SerializeField] private bool moves = false;
    [SerializeField] private float moveDistance = 5f;
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private Vector2 waitTimeRange = new Vector2(1f, 3f);
    [SerializeField, Tooltip("0 up, 1 right, 2 down, 3 left")] private int moveDirection = 0;
    int currentDirection = 0;

    [Header("Animation Settings")]
    [SerializeField] private Animator animator;

    private float startAngle;
    private bool isMoving = false;

    void Awake()
    {
        currentDirection = moveDirection;
        animator.SetInteger("direction", currentDirection);

        //currentDirection = (currentDirection + 3) % 4; // equivalent to -1 mod 4
        switch (currentDirection)
        {
            case 0: //up
                viewArea.transform.rotation = Quaternion.Euler(0f, 0f, 0);
                break;
            case 1: //right
                viewArea.transform.rotation = Quaternion.Euler(0f, 0f, 270);
                break;
            case 2: //down
                viewArea.transform.rotation = Quaternion.Euler(0f, 0f, 180);
                break;
            case 3: //left
                viewArea.transform.rotation = Quaternion.Euler(0f, 0f, 90);
                break;
        }
        
    }

    void Start()
    {
        startAngle = transform.eulerAngles.z;
        
        if (rotates && moves)
        {
            Debug.Log("Turning off both rotates and moves -- both were set to true");
            rotates = false;
            moves = false;
        }

        if (moves)
        {
            StartCoroutine(PatrolRoutine());
        }
            
    }

    void Update()
    {
        if (rotates)
        {
            float oscillation = Mathf.PingPong(Time.time * rotationSpeed, 1f) * 2f - 1f;
            float currentAngle = startAngle + oscillation * rotationAngle;

            int direction = GetDirectionFromAngle(currentAngle);
            float snappedAngle = GetSnappedAngle(direction);

            if (viewArea != null)
                viewArea.transform.rotation = Quaternion.Euler(0f, 0f, snappedAngle);

            if (animator != null)
                animator.SetInteger("direction", direction);
        }
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            // Walk forward
            yield return MoveInDirection(currentDirection);

            // Wait at the end
            float wait = Random.Range(waitTimeRange.x, waitTimeRange.y);
            yield return new WaitForSeconds(wait);

            // Turn 90° LEFT twice (up→left→down or down→left→up)
            yield return TurnLeftSmoothly();
            yield return TurnLeftSmoothly();

            // Walk forward again (now opposite direction)
            yield return MoveInDirection(currentDirection);

            // Wait again
            wait = Random.Range(waitTimeRange.x, waitTimeRange.y);
            yield return new WaitForSeconds(wait);

            // Turn back (two left turns again)
            yield return TurnLeftSmoothly();
            yield return TurnLeftSmoothly();
        }
    }

    private IEnumerator TurnLeftSmoothly()
    {
        // Turn left (counter-clockwise)
        currentDirection = (currentDirection + 3) % 4; // equivalent to -1 mod 4
        float snappedAngle = GetSnappedAngle(currentDirection);

        if (viewArea != null)
            viewArea.transform.rotation = Quaternion.Euler(0f, 0f, snappedAngle);
        if (animator != null)
            animator.SetInteger("direction", currentDirection);

        // Tiny delay to simulate turn
        yield return new WaitForSeconds(0.2f);
    }

    private IEnumerator MoveInDirection(int dir)
    {
        isMoving = true;
        Vector3 start = transform.position;
        Vector3 offset = DirectionToVector(dir) * moveDistance;
        Vector3 end = start + offset;

        while (Vector3.Distance(transform.position, end) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, end, movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = end;
        isMoving = false;
    }

    private Vector3 DirectionToVector(int dir)
    {
        switch (dir)
        {
            case 0: return Vector3.up;
            case 1: return Vector3.right;
            case 2: return Vector3.down;
            case 3: return Vector3.left;
            default: return Vector3.zero;
        }
    }

    private int GetDirectionFromAngle(float angle)
    {
        angle = (angle + 360f) % 360f;
        if (angle >= 45f && angle < 135f) return 1;  // Right
        if (angle >= 135f && angle < 225f) return 2; // Down
        if (angle >= 225f && angle < 315f) return 3; // Left
        return 0;                                   // Up
    }

    private float GetSnappedAngle(int direction)
    {
        switch (direction)
        {
            case 0: return 0f;    // Up
            case 1: return -90f;  // Right
            case 2: return 180f;  // Down
            case 3: return 90f;   // Left
            default: return 0f;
        }
    }
}
