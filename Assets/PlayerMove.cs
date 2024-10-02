using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;  // Скорость игрока
    private Rigidbody2D rb;       // Ссылка на Rigidbody2D персонажа
    private Vector2 movement;     // Вектор направления движения
    private Animator animator;    // Ссылка на Animator
    private bool isAttacking = false;  // Флаг атаки

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();      // Получаем Rigidbody2D
        animator = GetComponent<Animator>();   // Получаем Animator
    }

    void Update()
    {
        // Если атака активна, отключаем управление
        if (!isAttacking)
        {
            // Получение ввода от игрока
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");

            // Обновляем параметры в Animator для правильных переходов
            UpdateAnimationParameters();

            // Логика приоритета анимации на основе направления
            if (movement.x != 0 || movement.y != 0)
            {
                // Если персонаж движется по диагонали, приоритет по вертикали
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
                // Если персонаж не двигается, возвращаемся в Idle
                animator.Play("Idle");
            }
        }

        // Запускаем атаку по нажатию на пробел
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            PerformAttack();  // Атака
        }
    }

    void FixedUpdate()
    {
        // Перемещение персонажа, если он не атакует
        if (!isAttacking)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void PerformAttack()
    {
        // Запускаем анимацию атаки
        animator.SetTrigger("AttackTrigger");

        // Отключаем управление движением во время атаки
        isAttacking = true;
    }

    // Метод для обновления параметров анимации
    private void UpdateAnimationParameters()
    {
        // Передаём значения осей в Animator
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);

        // Устанавливаем скорость для переключения между Idle и движением
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    // Метод, который вызывается по завершению анимации атаки
    public void EndAttack()
    {
        // Завершаем атаку и возвращаем управление движением
        isAttacking = false;

        // Сбрасываем триггер атаки, чтобы он не мешал другим анимациям
        animator.ResetTrigger("AttackTrigger");
    }
}
