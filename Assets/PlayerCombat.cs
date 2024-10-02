using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 0.5f;          // Радиус атаки
    public Transform attackZone;              // Зона атаки персонажа
    public int attackDamage = 10;             // Урон от атаки
    public LayerMask enemyLayers;             // Слои, на которых находятся враги

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformAttack();  // Выполняем атаку при нажатии на пробел
        }
    }

    void PerformAttack()
    {
        // Находим всех врагов в радиусе атаки
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

        // Если враги найдены, определяем направление атаки в сторону ближайшего врага
        if (hitEnemies.Length > 0)
        {
            // Находим ближайшего врага
            Transform closestEnemy = hitEnemies[0].transform;
            float minDistance = Vector2.Distance(transform.position, closestEnemy.position);

            foreach (Collider2D enemy in hitEnemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    closestEnemy = enemy.transform;
                    minDistance = distance;
                }
            }

            // Определяем направление атаки в сторону ближайшего врага
            Vector2 direction = (closestEnemy.position - transform.position).normalized;

            // Атакуем в направлении ближайшего врага
            AttackInDirection(direction);
        }
    }

    // Атакуем в заданном направлении
    void AttackInDirection(Vector2 direction)
    {
        // Перемещаем зону атаки в направлении врага
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Если атака по горизонтали (влево или вправо)
            if (direction.x > 0)
            {
                // Атака вправо
                attackZone.localPosition = new Vector3(0.5f, 0, 0);
                Debug.Log("Атака вправо");
            }
            else
            {
                // Атака влево
                attackZone.localPosition = new Vector3(-0.5f, 0, 0);
                Debug.Log("Атака влево");
            }
        }
        else
        {
            // Если атака по вертикали (вверх или вниз)
            if (direction.y > 0)
            {
                // Атака вверх
                attackZone.localPosition = new Vector3(0, 0.5f, 0);
                Debug.Log("Атака вверх");
            }
            else
            {
                // Атака вниз
                attackZone.localPosition = new Vector3(0, -0.5f, 0);
                Debug.Log("Атака вниз");
            }
        }

        // Выполняем атаку по врагам в этой зоне
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackZone.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    // Для визуализации зоны атаки в редакторе
    void OnDrawGizmosSelected()
    {
        if (attackZone == null)
            return;

        // Рисуем радиус атаки вокруг игрока
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Рисуем текущую позицию зоны атаки
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackZone.position, attackRange);
    }
}
