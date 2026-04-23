using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
     public Vector3 direction = Vector3.right; // Направление движения платформы (вверх/вниз, влево/вправо)
    public float distance = 5f; // Дистанция, которую пройдет платформа
    public float moveSpeed = 2f; // Скорость движения платформы

    private Vector3 startPosition; // Начальная позиция платформы
    private Vector3 endPosition;   // Конечная позиция платформы
    private bool movingToEnd = true; // Флаг, указывающий, движется ли платформа к конечной точке
    

    void Start()
    {
        startPosition = transform.position; // Запоминаем начальную позицию
        endPosition = startPosition + direction.normalized * distance; // Вычисляем конечную позицию
    }

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        Vector3 targetPosition;

        // Определяем, к какой точке движемся
        if (movingToEnd)
        {
            targetPosition = endPosition;
        }
        else
        {
            targetPosition = startPosition;
        }

        // Плавное перемещение к целевой позиции
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Проверяем, достигли ли мы целевой позиции
        if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
        {
            movingToEnd = !movingToEnd; // Меняем направление движения
        }
    }

    // Обработка столкновения с игроком
   /* private void OnCollisionEnter(Collision collision)
    {
        // Проверяем, является ли объект, столкнувшийся с платформой, игроком
        if (collision.gameObject.CompareTag("Player"))
        {
            // Делаем игрока дочерним объектом платформы, чтобы он двигался вместе с ней
            collision.transform.parent = transform;
        }
    }*/

    // Обработка прекращения столкновения с игроком
    private void OnCollisionExit(Collision collision)
    {
        // Проверяем, является ли объект, отделившийся от платформы, игроком
        if (collision.gameObject.CompareTag("Player"))
        {
            // Отсоединяем игрока от платформы
            collision.transform.parent = null;
        }
    }
}
