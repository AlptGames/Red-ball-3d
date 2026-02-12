using UnityEngine;

public class HeroMovingForCamera3 : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 direction;

    [Header("Settings")]
    public float speedWalk = 3f;
    public float speedRun = 6f;
    public float torqueForce = 50f;   // Сила кручения увеличена
    public float rotationSpeed = 10f;
    public float jump = 10f;

    [Header("Links")]
    public GameObject directionPoint; // Камера или точка направления 
    public bool onGround = true;

    private HeroFront frontObject;
    public Transform frontObjectTransform;
    private float currentSpeed;

    public float sensorOffset = 0.7f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Чтобы шар не буксовал, увеличиваем лимит скорости вращения
        rb.maxAngularVelocity = 50f;

        frontObject = FindFirstObjectByType<HeroFront>();
    }

    void Update()
    {
        direction = Vector3.zero;

        if (frontObject != null && !frontObject.onTheLadder)
        {
            // Сбор ввода
            float h = 0;
            float v = 0;

            if (Input.GetKey(KeyCode.W)) v = 1;
            if (Input.GetKey(KeyCode.S)) v = -1;
            if (Input.GetKey(KeyCode.D)) h = 1;
            if (Input.GetKey(KeyCode.A)) h = -1;

            // Расчет направления относительно directionPoint
            Vector3 forward = directionPoint.transform.forward;
            Vector3 right = directionPoint.transform.right;
            forward.y = 0;
            right.y = 0;
            direction = (forward * v + right * h).normalized;

            // Прыжок
            if (Input.GetKeyDown(KeyCode.Space) && onGround)
            {
                rb.AddForce(Vector3.up * jump, ForceMode.Impulse);
                onGround = false;
            }

            currentSpeed = Input.GetKey(KeyCode.LeftShift) ? speedRun : speedWalk;

            // Поворот визуальной модели (не самого шара!)
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
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
            // 1. ПРОВЕРКА: Если мы движемся, обновляем направление "выноса"
            // Если стоим (direction == zero), используем текущий forward датчика
            Vector3 pushDirection = direction != Vector3.zero ? direction.normalized : frontObjectTransform.forward;

            // 2. Рассчитываем позицию (теперь она никогда не будет в центре)
            Vector3 targetPosition = transform.position + (pushDirection * sensorOffset);
            targetPosition.y = transform.position.y;

            frontObjectTransform.position = targetPosition;

            // 3. Поворачиваем датчик, только если есть ввод
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
        if (direction != Vector3.zero && (frontObject == null || !frontObject.onTheLadder))
        {
            // Главное исправление: крутим шар перпендикулярно направлению движения
            Vector3 torqueAxis = Vector3.Cross(Vector3.up, direction);
            rb.AddTorque(torqueAxis * torqueForce * currentSpeed, ForceMode.Force);

            // Дополнительная сила для отзывчивости
            rb.AddForce(direction * currentSpeed * 2f, ForceMode.Acceleration);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false;
        }
    }
}