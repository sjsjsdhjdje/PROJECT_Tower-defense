using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints;

    public float speed = 0.5f;

    private int currentPoint = 0;

    void Update()
    {
        // Проверяем наличие маршрута
        if (waypoints == null || waypoints.Length == 0)
            return;

        // Получаем следующую точку
        Transform target = waypoints[currentPoint];

        // Двигаем врага к точке
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Проверяем, достиг ли враг точки
        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            currentPoint++;

            // Проверяем, закончился ли маршрут
            if (currentPoint >= waypoints.Length)
            {
                Destroy(gameObject);
            }
        }
    }
}