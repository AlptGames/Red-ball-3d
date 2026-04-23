using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public int lives = 3;
    public float invulDuration = 3f;
    public float restartDelay = 1f; // Задержка перед рестартом

    [Header("References")]
    public UIManager UIManager;
    private Renderer playerRenderer;
    private Material playerMaterial;
    private Color originalColor;

    private static Vector3 lastCheckpointPos;
    private static bool hasCheckpoint = false;
    private bool isInvulnerable = false;
    private bool isDead = false; // Чтобы не вызывать смерть дважды


    void Start()
    {
        playerRenderer = GetComponent<Renderer>();
        playerMaterial = playerRenderer.material;
        originalColor = playerMaterial.color;

        if (hasCheckpoint)
        {
            transform.position = lastCheckpointPos;
        }
    }

    public void TakeDamage(int damageAmount) // Теперь передаем сколько урона нанести
    {
        if (isInvulnerable || isDead) return;

        // Наносим урон по одному сердечку за раз для анимации
        for (int i = 0; i < damageAmount; i++)
        {
            if (lives > 0)
            {
                lives--;
                UIManager.UpdateHeartsOnDamage(); // Сообщаем UI, что ушло ОДНО седце
            }
        }

        if (lives <= 0)
        {
            StartCoroutine(DeathRoutine());
        }
        else
        {
            StartCoroutine(InvulnerabilityRoutine());
        }
    }

    private IEnumerator DeathRoutine()
    {
        isDead = true;
        // Здесь можно отключить управление игроком, если нужно:
        // GetComponent<PlayerMovement>().enabled = false;

        yield return new WaitForSeconds(restartDelay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator InvulnerabilityRoutine()
    {
        isInvulnerable = true;
        float elapsed = 0;
        float blinkInterval = 0.15f;

        while (elapsed < invulDuration)
        {
            playerMaterial.color = (playerMaterial.color == originalColor) ? Color.white : originalColor;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        playerMaterial.color = originalColor;
        isInvulnerable = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            lastCheckpointPos = other.transform.position;
            hasCheckpoint = true;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Spike")) { TakeDamage(1); }
        if(collision.gameObject.CompareTag("Water")) { TakeDamage(3); }
        if (collision.gameObject.CompareTag("Enemy")) { TakeDamage(1); }
    }
}