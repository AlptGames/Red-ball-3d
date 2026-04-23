using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections; // Нужен для использования Coroutines

public class JatPack : MonoBehaviour
{
    public float flyForce = 10f;
    public GameObject jetpackPrefab;
    public Transform playerBackBone;
    public GameObject flyEffectPrefab;

    public float jetpackDuration = 10f; // Время, в течение которого джет-пак активен (в секундах)

    private Rigidbody rb;
    private bool isFlying = false;
    private GameObject currentJetpack;
    private GameObject currentFlyEffect;
    private Coroutine jetpackTimerCoroutine; // Для управления таймером

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("JetpackController требует наличия Rigidbody на игроке.");
        }
    }

    void Update()
    {
        // Активация джет-пака при нажатии пробела, если он еще не aktif
        if (Input.GetKeyDown(KeyCode.Space) && currentJetpack == null && jetpackTimerCoroutine == null)
        {
            SpawnJetpack();
            if (jetpackTimerCoroutine != null)
            {
                StopCoroutine(jetpackTimerCoroutine); // Останавливаем предыдущий таймер, если он был
            }
            jetpackTimerCoroutine = StartCoroutine(JetpackTimer()); // Запускаем новый таймер
        }

        // Управление полётом, пока джет-пак активен
        if (Input.GetKey(KeyCode.Space) && currentJetpack != null)
        {
            Fly();
        }
        // Остановка полёта при отпускании пробела
        else if (Input.GetKeyUp(KeyCode.Space) && currentJetpack != null)
        {
            StopFlying();
        }
    }

    void SpawnJetpack()
    {
        if (jetpackPrefab != null && playerBackBone != null)
        {
            currentJetpack = Instantiate(jetpackPrefab, playerBackBone.position, playerBackBone.rotation, playerBackBone);
            // Можно добавить визуальный индикатор оставшегося времени, если нужно
        }
    }

    void Fly()
    {
        if (rb != null)
        {
            isFlying = true;
            rb.AddForce(transform.forward * flyForce, ForceMode.Acceleration);

            // Запуск спецэффекта, если он еще не запущен
            if (currentFlyEffect == null && flyEffectPrefab != null)
            {
                currentFlyEffect = Instantiate(flyEffectPrefab, currentJetpack.transform.position, Quaternion.identity);
                // currentFlyEffect.transform.SetParent(currentJetpack.transform); // Прикрепляем к джет-паку
            }
        }
    }

    void StopFlying()
    {
        isFlying = false;
        if (currentFlyEffect != null)
        {
            Destroy(currentFlyEffect);
            currentFlyEffect = null;
        }
    }

    // Корутина для отсчета времени джет-пака
    IEnumerator JetpackTimer()
    {
        yield return new WaitForSeconds(jetpackDuration); // Ждем указанное время

        Debug.Log("Время джет-пака истекло. Удаляем.");
        DestroyJetpack(); // Уничтожаем джет-пак и останавливаем полёт
    }

    void DestroyJetpack()
    {
        if (currentJetpack != null)
        {
            Destroy(currentJetpack);
            currentJetpack = null;
        }
        StopFlying(); // Убедимся, что полёт и эффект остановлены
        jetpackTimerCoroutine = null; // Сбрасываем корутину
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("JetpackPickup"))
        {
            Debug.Log("Джет-пак подобран!");
            // Логика для активации джет-пака при подборе (если необходимо)
            // В этом примере, джет-пак активируется по нажатию пробела.
            // Если хотите, чтобы он появлялся сразу, эту логику надо будет изменить.
        }
    }

    void OnDestroy()
    {
        // Очистка при уничтожении объекта игрока
        if (currentJetpack != null)
        {
            Destroy(currentJetpack);
        }
        if (currentFlyEffect != null)
        {
            Destroy(currentFlyEffect);
        }
        if (jetpackTimerCoroutine != null)
        {
            StopCoroutine(jetpackTimerCoroutine);
        }
    }
}
