using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform player;            // ������ �� ������
    public float moveSpeed = 2f;        // �������� �������� �����
    public float attackRange = 1.5f;    // ������ ��� �����
    public int attackDamage = 5;        // ����, ������� ���� ������� ��� �����
    public float attackCooldown = 1.5f; // ����� �������� ����� �������
    public float guardRange = 5f;       // ������ ���������� ����

    private Vector2 startPosition;      // ��������� ������� ����� (����� ���������� ����)
    private float nextAttackTime = 0f;  // �����, ����� ���� ����� ����� ���������

    private Collider2D enemyCollider;   // ��������� �����
    private Collider2D playerCollider;  // ��������� ������

    private float fixedZPosition;       // ������������� �������� Z ��� ���� ��������

    void Start()
    {
        // ���������� ��������� ������� ����� ��� ����� ��� ���������� ����
        startPosition = transform.position;

        // �������� ���������� ����� � ������ ��� ����� �� ��������
        enemyCollider = GetComponent<Collider2D>();
        playerCollider = player.GetComponent<Collider2D>();

        // ��������� Z ����������
        fixedZPosition = transform.position.z;

        // �������� ������� �����������
        if (enemyCollider == null)
        {
            Debug.LogError("����������� ��������� � �����!");
        }
        if (playerCollider == null)
        {
            Debug.LogError("����������� ��������� � ������!");
        }
    }

    void Update()
    {
        // ���������, ��������� �� ����� � ���� ������
        if (Vector2.Distance(transform.position, player.position) <= guardRange)
        {
            // ���� ����� � ���� ������, ��������� � ���� � �������
            MoveTowardsPlayer();
            if (Time.time >= nextAttackTime && IsWithinAttackRange())
            {
                AttackPlayer();
                nextAttackTime = Time.time + attackCooldown;  // ������������� �������� ��� ��������� �����
            }
        }
        else
        {
            // ���� ����� ����� �� ������� ���� ������, ������������ �� �������� �������
            ReturnToStartPosition();
        }
    }

    // ����� ��� �������� � ������
    void MoveTowardsPlayer()
    {
        // ���� ���� ��� �� � ������� �����, ���������� ��������
        if (!IsWithinAttackRange())
        {
            // ��������� ����������� � ������
            Vector2 direction = (player.position - transform.position).normalized;

            // ������� ����� � ������ � ������ �������
            Vector3 newPosition = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // ������������ ������������� �������� Z
            newPosition.z = fixedZPosition;

            // ��������� ������� �����
            transform.position = newPosition;
        }
    }

    // ����� ��� ��������, ��������� �� ���� �� ���������� ����� � ������ �����������
    bool IsWithinAttackRange()
    {
        // ��������� ����������� ���������� ����� �������� ��������
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ��������� ������� (�������) ����������� ����� � ������
        float enemyColliderRadius = enemyCollider.bounds.extents.magnitude;  // ������ ���������� �����
        float playerColliderRadius = playerCollider.bounds.extents.magnitude;  // ������ ���������� ������

        // ���������� �������� ����� �� �������, ������� ��������� ������ ����� � ������� �����������
        float stopDistance = attackRange + enemyColliderRadius + playerColliderRadius;

        // ���� ���������� �� ������ ������, ��� ���������� ��� ���������, ���������� ��������
        return distanceToPlayer <= stopDistance;
    }

    // ����� ��� ����������� �� ��������� �������
    void ReturnToStartPosition()
    {
        // ���� ���� �� �� ����� ��������� �������, ���������� ��� �����
        Vector3 newPosition = Vector2.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);

        // ������������ ������������� �������� Z
        newPosition.z = fixedZPosition;

        // ��������� ������� �����
        transform.position = newPosition;
    }

    // ����� ��� ����� ������
    void AttackPlayer()
    {
        // ����� ��������� ������ � ������ ����
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }

    // ����� ��� ������������ ���� ������ (������� ������) � ������� �����
    void OnDrawGizmosSelected()
    {
        // ������ ���������� ����
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, guardRange);

        // ���������� ������ ����� �����
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
