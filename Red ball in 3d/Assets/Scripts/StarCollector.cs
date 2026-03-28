using UnityEngine;
using UnityEngine.UI;

public class StarCollector : MonoBehaviour
{
    [Header("Настройки уровня")]
    public int totalStarsInLevel = 10;

    [Header("UI Элементы")]
    public Slider progressBar;
    public Image progressBarFill;

    [Header("Объекты медалей")]
    public GameObject bronzeMedalUI; // UI объект для бронзовой медали
    public GameObject silverMedalUI; // UI объект для серебряной медали
    public GameObject goldMedalUI;   // UI объект для золотой медали

    private int collectedStars = 0;

    void Start()
    {
        if (progressBar != null)
        {
            progressBar.maxValue = totalStarsInLevel;
            progressBar.value = 0;
        }
        else
        {
            Debug.LogError("Slider не назначен!");
        }

        // Скрываем все медали при старте
        HideAllMedals();
        UpdateProgressBarColor(); // Устанавливаем начальный цвет шкалы
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Star"))
        {
            collectedStars++;
            Destroy(other.gameObject);
            UpdateProgressBar();
            UpdateMedals(); // Обновляем отображение медалей
            UpdateProgressBarColor(); // Обновляем цвет шкалы после получения медали
        }
    }

    void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.value = collectedStars;
        }
    }

    // Обновляет отображение медалей
    void UpdateMedals()
    {
        HideAllMedals(); // Скрываем предыдущую медаль

        float progressPercentage = (float)collectedStars / totalStarsInLevel;

        if (progressPercentage >= 1.0f)
        {
            if (goldMedalUI != null) goldMedalUI.SetActive(true);
            Debug.Log("Золотая медаль!");
        }
        else if (progressPercentage >= 0.7f)
        {
            if (silverMedalUI != null) silverMedalUI.SetActive(true);
            Debug.Log("Серебряная медаль!");
        }
        else if (progressPercentage >= 0.4f)
        {
            if (bronzeMedalUI != null) bronzeMedalUI.SetActive(true);
            Debug.Log("Бронзовая медаль!");
        }
        else
        {
            Debug.Log("Без медали.");
        }
    }

    // Обновляет цвет заливки шкалы в зависимости от текущей медали
    void UpdateProgressBarColor()
    {
        float progressPercentage = (float)collectedStars / totalStarsInLevel;

        if (progressBarFill != null)
        {
            if (progressPercentage >= 1.0f)
            {
                progressBarFill.color = Color.yellow; // Золотой
            }
            else if (progressPercentage >= 0.7f)
            {
                progressBarFill.color = Color.gray; // Серебряный
            }
            else if (progressPercentage >= 0.4f)
            {
                progressBarFill.color = Color.red; // Бронзовый
            }
            else
            {
                // Если нет медали, то установим начальный цвет.
                // Можно добавить отдельный цвет для этого случая, если нужно.
                // Пример: progressBarFill.color = Color.white; 
            }
        }
    }

    // Вспомогательный метод для скрытия всех объектов медалей
    void HideAllMedals()
    {
        if (bronzeMedalUI != null) bronzeMedalUI.SetActive(false);
        if (silverMedalUI != null) silverMedalUI.SetActive(false);
        if (goldMedalUI != null) goldMedalUI.SetActive(false);
    }
}
