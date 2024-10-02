using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;            // Ссылка на игрока
    public float moveSpeed = 2f;        // Скорость движения врага
    public float attackRange = 1.5f;    // Радиус для атаки
    public int attackDamage = 5;        // Урон, который враг наносит при атаке
    public float attackCooldown = 1.5f; // Время задержки между атаками
    public float guardRange = 5f;       // Радиус охраняемой зоны

    private Vector2 startPosition;      // Стартовая позиция врага (центр охраняемой зоны)
    private float nextAttackTime = 0f;  // Время, когда враг может снова атаковать

    private Collider2D enemyCollider;   // Коллайдер врага
    private Collider2D playerCollider;  // Коллайдер игрока

    private float fixedZPosition;       // Фиксированное значение Z для всех движений

    void Start()
    {
        // Запоминаем стартовую позицию врага как центр его охраняемой зоны
        startPosition = transform.position;

        // Получаем коллайдеры врага и игрока для учёта их размеров
        enemyCollider = GetComponent<Collider2D>();
        playerCollider = player.GetComponent<Collider2D>();

        // Фиксируем Z координату
        fixedZPosition = transform.position.z;

        // Проверка наличия коллайдеров
        if (enemyCollider == null)
        {
            Debug.LogError("Отсутствует коллайдер у врага!");
        }
        if (playerCollider == null)
        {
            Debug.LogError("Отсутствует коллайдер у игрока!");
        }
    }

    void Update()
    {
        // Проверяем, находится ли игрок в зоне охраны
        if (Vector2.Distance(transform.position, player.position) <= guardRange)
        {
            // Если игрок в зоне охраны, двигаемся к нему и атакуем
            MoveTowardsPlayer();
            if (Time.time >= nextAttackTime && IsWithinAttackRange())
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackCooldown;  // Устанавливаем задержку для следующей атаки
            }
        }
        else
        {
            // Если игрок вышел за пределы зоны охраны, возвращаемся на исходную позицию
            ReturnToStartPosition();
        }
    }

    // Метод для движения к игроку
    void MoveTowardsPlayer()
    {
        // Если враг ещё не в радиусе атаки, продолжаем движение
        if (!IsWithinAttackRange())
        {
            // Вычисляем направление к игроку
            Vector2 direction = (player.position - transform.position).normalized;

            // Двигаем врага к игроку с учётом времени
            Vector3 newPosition = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Обеспечиваем фиксированное значение Z
            newPosition.z = fixedZPosition;

            // Обновляем позицию врага
            transform.position = newPosition;
        }
    }

    // Метод для проверки, находится ли враг на расстоянии атаки с учётом коллайдеров
    bool IsWithinAttackRange()
    {
        // Вычисляем фактическое расстояние между центрами объектов
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Вычисляем размеры (радиусы) коллайдеров врага и игрока
        float enemyColliderRadius = enemyCollider.bounds.extents.magnitude;  // Радиус коллайдера врага
        float playerColliderRadius = playerCollider.bounds.extents.magnitude;  // Радиус коллайдера игрока

        // Остановить движение врага на границе, которая учитывает радиус атаки и размеры коллайдеров
        float stopDistance = attackRange + enemyColliderRadius + playerColliderRadius;

        // Если расстояние до игрока больше, чем расстояние для остановки, продолжаем движение
        return distanceToPlayer <= stopDistance;
    }

    // Метод для возвращения на стартовую позицию
    void ReturnToStartPosition()
    {
        // Если враг не на своей стартовой позиции, возвращаем его назад
        Vector3 newPosition = Vector2.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);

        // Обеспечиваем фиксированное значение Z
        newPosition.z = fixedZPosition;

        // Обновляем позицию врага
        transform.position = newPosition;
    }

    // Метод для атаки игрока
    void AttackPlayer()
    {
        // Найдём компонент игрока и нанесём урон
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    // Метод для визуализации зоны охраны (радиуса охраны) и радиуса атаки
    void OnDrawGizmosSelected()
    {
        // Рисуем охраняемую зону
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, guardRange);

        // Отображаем радиус атаки врага
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
