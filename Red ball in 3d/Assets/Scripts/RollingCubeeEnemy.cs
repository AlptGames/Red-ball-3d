using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingCubeeEnemy : MonoBehaviour
{
    [Header("Настройки движения")]
    public Transform[] waypoints; // Точки маршрута
    public float rollSpeed = 5f;  // Скорость переката
    public float delayBetweenSteps = 0.2f; // Пауза между шагами

    private int _currentWaypointIndex = 0;
    private bool _isRolling = false;
    private float _cubeSize = 1f; // Размер стороны кубa

    void Start()
    {
        _cubeSize = transform.localScale.y;
        if (waypoints.Length > 0)
        {
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        while (true)
        {
            Transform target = waypoints[_currentWaypointIndex];

            // Определяем направление к следующей точке
            Vector3 direction = (target.position - transform.position).normalized;

            // Выбираем доминирующую ось (вперед, назад, влево или вправо)
            Vector3 rollAxis = Vector3.zero;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.z))
                rollAxis = direction.x > 0 ? Vector3.right : Vector3.left;
            else
                rollAxis = direction.z > 0 ? Vector3.forward : Vector3.back;

            // Выполняем один перекат
            yield return StartCoroutine(Roll(rollAxis));

            // Если достигли точки (примерно), переходим к следующей
            if (Vector3.Distance(transform.position, target.position) < _cubeSize)
            {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
            }

            yield return new WaitForSeconds(delayBetweenSteps);
        }
    }

    IEnumerator Roll(Vector3 direction)
    {
        _isRolling = true;

        float halfSize = _cubeSize / 2f;
        // Точка опоры берется от ТЕКУЩЕГО положения куба (даже если его толкнули)
        Vector3 anchor = transform.position + (Vector3.down * halfSize) + (direction * halfSize);
        Vector3 axis = Vector3.Cross(Vector3.up, direction);

        Quaternion startRot = transform.rotation;
        // Целевой поворот — строго кратный 90 градусам относительно текущего
        Vector3 currentEuler = startRot.eulerAngles;
        Quaternion targetRot = Quaternion.AngleAxis(90, axis) * startRot;

        float elapsed = 0;
        float duration = 1f / rollSpeed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            // Используем SmoothStep для мягкого начала и конца (убирает дергание)
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);

            // Вращаем вокруг ребра динамически
            float currentAngle = Mathf.Lerp(0, 90, t);

            // Математически вычисляем позицию центра относительно точки опоры
            // Это позволяет кубу "катиться" из любой точки, где он оказался
            Vector3 relativePos = Quaternion.AngleAxis(currentAngle, axis) * (Vector3.up * halfSize + direction * -halfSize);
            transform.position = anchor + relativePos;
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);

            yield return null;
        }

        // В конце ПЛАВНО выравниваем только углы, чтобы куб лежал ровно на грани
        Vector3 finalEuler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(
            Mathf.Round(finalEuler.x / 90) * 90,
            Mathf.Round(finalEuler.y / 90) * 90,
            Mathf.Round(finalEuler.z / 90) * 90
        );

        _isRolling = false;
    }
}