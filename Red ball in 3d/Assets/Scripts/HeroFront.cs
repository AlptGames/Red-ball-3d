using UnityEngine;

public class HeroFront : MonoBehaviour
{
    public bool onTheLadder = false;
    private GameObject hero;

    // Даем переменной понятное имя со строчной буквы
    private HeroMovingForCamera3 heroScript;

    void Start()
    {
        onTheLadder = false;

        // Находим скрипт на сцене
        heroScript = FindFirstObjectByType<HeroMovingForCamera3>();

        if (heroScript != null)
        {
            hero = heroScript.gameObject;
        }
        else
        {
            Debug.LogError("HeroMovingForCamera3 не найден на сцене!");
        }
    }

    void OnTriggerEnter(Collider col)
    {
        // Проверяем, что коснулись лестницы И что скрипт героя найден
        if (col.gameObject.GetComponent<Ladder>() != null && heroScript != null)
        {
            SetLadderStatus(true);
            heroScript.onGround = true; // Используем правильное имя переменной
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.GetComponent<Ladder>() != null && heroScript != null)
        {
            SetLadderStatus(false);
            heroScript.onGround = false; // Используем правильное имя переменной
        }
    }

    public void SetLadderStatus(bool status)
    {
        onTheLadder = status;
        if (hero != null)
        {
            hero.GetComponent<Rigidbody>().useGravity = !status;
        }
    }
}