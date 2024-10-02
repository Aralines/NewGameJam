using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 50;           // Максимальное здоровье врага
    private int currentHealth;           // Текущее здоровье

    public Slider healthSlider;          // Ссылка на слайдер здоровья

    void Start()
    {
        currentHealth = maxHealth;       // Устанавливаем здоровье на максимальное при старте
        healthSlider.maxValue = maxHealth;  // Устанавливаем максимальное значение для слайдера
        healthSlider.value = currentHealth; // Обновляем текущее значение слайдера
    }

    public void TakeDamage(int damage)
    {
        // Уменьшаем здоровье врага
        currentHealth -= damage;
        Debug.Log("Враг получил урон! Текущее здоровье: " + currentHealth);

        // Обновляем слайдер здоровья
        healthSlider.value = currentHealth;

        // Если здоровье врага меньше или равно 0, он умирает
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Логика смерти врага (например, уничтожение объекта)
        Debug.Log("Враг погиб!");

        // Уничтожаем слайдер здоровья
        Destroy(healthSlider.gameObject);

        // Уничтожаем объект врага
        Destroy(gameObject);
    }
}
