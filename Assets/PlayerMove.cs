using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;  // �������� ������
    private Rigidbody2D rb;       // ������ �� Rigidbody2D ���������
    private Vector2 movement;     // ������ ����������� ��������
    private Animator animator;    // ������ �� Animator
    private bool isAttacking = false;  // ���� �����

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();      // �������� Rigidbody2D
        animator = GetComponent<Animator>();   // �������� Animator
    }

    void Update()
    {
        // ���� ����� �������, ��������� ����������
        if (!isAttacking)
        {
            // ��������� ����� �� ������
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // ��������� ��������� � Animator ��� ���������� ���������
            UpdateAnimationParameters();

            // ������ ���������� �������� �� ������ �����������
            if (movement.x != 0 || movement.y != 0)
            {
                // ���� �������� �������� �� ���������, ��������� �� ���������
                if (Mathf.Abs(movement.y) > Mathf.Abs(movement.x))
                {
                    if (movement.y > 0)
                    {
                        animator.Play("WalkUp");
                    }
                    else
                    {
                        animator.Play("WalkDown");
                    }
                }
                else
                {
                    if (movement.x > 0)
                    {
                        animator.Play("WalkRight");
                    }
                    else
                    {
                        animator.Play("WalkLeft");
                    }
                }
            }
            else
            {
                // ���� �������� �� ���������, ������������ � Idle
                animator.Play("Idle");
            }
        }

        // ��������� ����� �� ������� �� ������
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            PerformAttack();  // �����
        }
    }

    void FixedUpdate()
    {
        // ����������� ���������, ���� �� �� �������
        if (!isAttacking)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void PerformAttack()
    {
        // ��������� �������� �����
        animator.SetTrigger("AttackTrigger");

        // ��������� ���������� ��������� �� ����� �����
        isAttacking = true;
    }

    // ����� ��� ���������� ���������� ��������
    private void UpdateAnimationParameters()
    {
        // ������� �������� ���� � Animator
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        // ������������� �������� ��� ������������ ����� Idle � ���������
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    // �����, ������� ���������� �� ���������� �������� �����
    public void EndAttack()
    {
        // ��������� ����� � ���������� ���������� ���������
        isAttacking = false;

        // ���������� ������� �����, ����� �� �� ����� ������ ���������
        animator.ResetTrigger("AttackTrigger");
    }
}
