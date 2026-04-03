using UnityEngine;

public class KeepTopPosition : MonoBehaviour
{
    // Скрипт просто сбрасывает вращение до мирового "нуля" в каждом кадре
    void Update()
    {
        // Оставляем позицию как у родителя (куба), но вращение всегда (0, 0, 0)
        transform.rotation = Quaternion.identity;
    }
}