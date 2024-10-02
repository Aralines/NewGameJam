using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 50;           // ������������ �������� �����
    private int currentHealth;           // ������� ��������

    public Slider healthSlider;          // ������ �� ������� ��������

    void Start()
    {
        currentHealth = maxHealth;       // ������������� �������� �� ������������ ��� ������
        healthSlider.maxValue = maxHealth;  // ������������� ������������ �������� ��� ��������
        healthSlider.value = currentHealth; // ��������� ������� �������� ��������
    }

    public void TakeDamage(int damage)
    {
        // ��������� �������� �����
        currentHealth -= damage;
        Debug.Log("���� ������� ����! ������� ��������: " + currentHealth);

        // ��������� ������� ��������
        healthSlider.value = currentHealth;

        // ���� �������� ����� ������ ��� ����� 0, �� �������
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // ������ ������ ����� (��������, ����������� �������)
        Debug.Log("���� �����!");

        // ���������� ������� ��������
        Destroy(healthSlider.gameObject);

        // ���������� ������ �����
        Destroy(gameObject);
    }
}
