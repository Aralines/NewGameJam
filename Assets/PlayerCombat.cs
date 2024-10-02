using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 0.5f;          // ������ �����
    public Transform attackZone;              // ���� ����� ���������
    public int attackDamage = 10;             // ���� �� �����
    public LayerMask enemyLayers;             // ����, �� ������� ��������� �����

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformAttack();  // ��������� ����� ��� ������� �� ������
        }
    }

    void PerformAttack()
    {
        // ������� ���� ������ � ������� �����
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayers);

        // ���� ����� �������, ���������� ����������� ����� � ������� ���������� �����
        if (hitEnemies.Length > 0)
        {
            // ������� ���������� �����
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

            // ���������� ����������� ����� � ������� ���������� �����
            Vector2 direction = (closestEnemy.position - transform.position).normalized;

            // ������� � ����������� ���������� �����
            AttackInDirection(direction);
        }
    }

    // ������� � �������� �����������
    void AttackInDirection(Vector2 direction)
    {
        // ���������� ���� ����� � ����������� �����
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // ���� ����� �� ����������� (����� ��� ������)
            if (direction.x > 0)
            {
                // ����� ������
                attackZone.localPosition = new Vector3(0.5f, 0, 0);
                Debug.Log("����� ������");
            }
            else
            {
                // ����� �����
                attackZone.localPosition = new Vector3(-0.5f, 0, 0);
                Debug.Log("����� �����");
            }
        }
        else
        {
            // ���� ����� �� ��������� (����� ��� ����)
            if (direction.y > 0)
            {
                // ����� �����
                attackZone.localPosition = new Vector3(0, 0.5f, 0);
                Debug.Log("����� �����");
            }
            else
            {
                // ����� ����
                attackZone.localPosition = new Vector3(0, -0.5f, 0);
                Debug.Log("����� ����");
            }
        }

        // ��������� ����� �� ������ � ���� ����
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackZone.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    // ��� ������������ ���� ����� � ���������
    void OnDrawGizmosSelected()
    {
        if (attackZone == null)
            return;

        // ������ ������ ����� ������ ������
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // ������ ������� ������� ���� �����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackZone.position, attackRange);
    }
}
