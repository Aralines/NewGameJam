using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthSlider;      // ������ �� ������� ��������
    public Transform enemyTransform; // ������ �� �����
    public Vector3 offset;           // �������� �������� ������������ �����
    private Camera mainCamera;       // ������, ������� ����� �������������� ��� ��������

    void Start()
    {
        mainCamera = Camera.main;    // �������� ������ �� �������� ������
    }

    void Update()
    {
        // ����������� ������� ������� ����� � �������� ���������� � ������������� ������� ��������
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(enemyTransform.position + offset);
        healthSlider.transform.position = screenPosition;
    }
}
