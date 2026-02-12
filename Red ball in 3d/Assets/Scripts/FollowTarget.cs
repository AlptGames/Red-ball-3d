using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target; // Сюда перетащите объект шара

    void Update()
    {
        if (target != null)
        {
            // Копируем только позицию, игнорируем вращение шара
            transform.position = target.position;
        }
    }
}