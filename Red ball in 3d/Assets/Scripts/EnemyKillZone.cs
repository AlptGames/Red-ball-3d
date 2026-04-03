using UnityEngine;

public class EnemyKillZone : MonoBehaviour
{
    [Header("Настройки прыжка")]
    public float bounceForce = 10f; // Сила отскока игрока
    public GameObject deathEffect;   // (Опционально) Префаб взрыва/частиц

    // Этот метод срабатывает, когда что-то входит в наш верхний триггер
    private void OnTriggerEnter(Collider other)
    {
        // 1. Проверяем тег игрока
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRb = other.GetComponent<Rigidbody>();
            if (playerRb != null)
            {
                // 2. Подкидываем игрока
                Vector3 vel = playerRb.velocity;
                vel.y = bounceForce;
                playerRb.velocity = vel;

                // 3. УНИЧТОЖАЕМ КУБ (родителя)
                // Важно: Destroy(transform.parent.gameObject) удалит весь объект врага
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
