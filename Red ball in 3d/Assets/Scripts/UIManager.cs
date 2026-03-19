using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public Image[] heartImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private int currentHeartIndex;

    void Start()
    {
        currentHeartIndex = heartImages.Length - 1;
    }

    public void UpdateHeartsOnDamage()
    {
        if (currentHeartIndex >= 0)
        {
            // Ѕерем текущее сердце по индексу и запускаем мигание
            StartCoroutine(BlinkHeartRoutine(heartImages[currentHeartIndex]));
            // —двигаем индекс на следующее сердце дл€ следующего вызова
            currentHeartIndex--;
        }
    }

    private IEnumerator BlinkHeartRoutine(Image heartImage)
    {
        float duration = 1.0f; // —делал мигание чуть быстрее, чтобы вписатьс€ в секунду смерти
        float interval = 0.1f;
        float elapsed = 0;

        while (elapsed < duration)
        {
            heartImage.sprite = (heartImage.sprite == fullHeart) ? emptyHeart : fullHeart;
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }

        heartImage.sprite = emptyHeart;
    }
}