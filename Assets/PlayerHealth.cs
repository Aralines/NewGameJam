using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;    // Максимальное здоровье игрока
    private int currentHealth;     // Текущее здоровье

    void Start()
    {
        currentHealth = maxHealth;  // Устанавливаем здоровье на максимум в начале
    }

    // Метод для получения урона
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Игрок получил урон. Текущее здоровье: " + currentHealth);

        // Проверяем, не умер ли игрок
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Логика смерти игрока (например, завершение игры или респаун)
        Debug.Log("Игрок погиб!");
        // Временно можно просто отключить объект
        gameObject.SetActive(false);
    }
}
