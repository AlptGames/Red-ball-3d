using UnityEngine;

public class HeroMovingForCamera3 : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 direction;

    [Header("Settings")]
    public float speedWalk = 3f;
    public float rotationSpeed = 10f;
    public float jump = 10f;

    [Header("Links")]
    public GameObject directionPoint;
    public bool onGround = true;

    private HeroFront frontObject;
    public Transform frontObjectTransform;

    public float sensorOffset = 0.7f;

    [Header("Smoothness")]
    public float acceleration = 15f;
    public float deceleration = 10f;

    [Header("Air Control")]
    [Range(0.1f, 1f)]
    public float airControlModifier = 0.3f; // 0.3 значит, что в воздухе управление на 70% слабее

    public int lives = 3;

    public Transform playerPos;

    public static Transform lastCheckpointPos;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 50f;
        frontObject = FindFirstObjectByType<HeroFront>();
    }

    void Update()
    {
        direction = Vector3.zero;

        if (frontObject != null && !frontObject.onTheLadder)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            Vector3 forward = directionPoint.transform.forward;
            Vector3 right = directionPoint.transform.right;
            forward.y = 0;
            right.y = 0;
            direction = (forward * v + right * h).normalized;

            if (Input.GetKeyDown(KeyCode.Space) && onGround)
            {
                rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
                onGround = false;
            }
        }
        else if (frontObject != null && frontObject.onTheLadder)
        {
            HandleLadder();
        }
    }

    void LateUpdate()
    {
        if (frontObjectTransform != null)
        {
            Vector3 pushDirection = direction != Vector3.zero ? direction.normalized : frontObjectTransform.forward;
            Vector3 targetPosition = transform.position + (pushDirection * sensorOffset);
            targetPosition.y = transform.position.y;

            frontObjectTransform.position = targetPosition;

            if (direction != Vector3.zero)
            {
                frontObjectTransform.forward = direction;
            }
        }
    }

    void HandleLadder()
    {
        rb.useGravity = false;
        Vector3 ladderDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) ladderDir = Vector3.up;
        if (Input.GetKey(KeyCode.S)) ladderDir = Vector3.down;

        rb.velocity = ladderDir * speedWalk;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            frontObject.SetLadderStatus(false);
            rb.useGravity = true;
        }
    }

    void FixedUpdate()
    {
        bool canMove = (frontObject == null || !frontObject.onTheLadder);

        if (direction != Vector3.zero && canMove)
        {
            // 1. Определяем текущее ускорение: полное на земле или уменьшенное в воздухе
            float currentAcceleration = onGround ? acceleration : (acceleration * airControlModifier);

            // 2. Рассчитываем изменение скорости
            Vector3 targetVelocity = direction * speedWalk;
            Vector3 velocityChange = targetVelocity - new Vector3(rb.velocity.x, 0, rb.velocity.z);

            // Прикладываем силу (с учетом того, на земле мы или нет)
            rb.AddForce(velocityChange * currentAcceleration, ForceMode.Acceleration);

            // 3. Вращение шара (визуальное) замедляем в воздухе аналогично
            Vector3 torqueAxis = Vector3.Cross(Vector3.up, direction);
            float rotationStep = onGround ? acceleration : (acceleration * airControlModifier);
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, torqueAxis * (speedWalk * 2f), Time.fixedDeltaTime * rotationStep);
        }
        else if (onGround && canMove)
        {
            // Торможение работает только на земле
            Vector3 horizontalVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(-horizontalVel * deceleration, ForceMode.Acceleration);
            rb.angularVelocity = Vector3.Lerp(rb.angularVelocity, Vector3.zero, Time.fixedDeltaTime * deceleration);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) onGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) onGround = false;
    }
}