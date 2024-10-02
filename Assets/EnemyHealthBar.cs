using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthSlider;      // —сылка на слайдер здоровь€
    public Transform enemyTransform; // —сылка на врага
    public Vector3 offset;           // —мещение слайдера относительно врага
    private Camera mainCamera;       //  амера, котора€ будет использоватьс€ дл€ прив€зки

    void Start()
    {
        mainCamera = Camera.main;    // ѕолучаем ссылку на основную камеру
    }

    void Update()
    {
        // ѕреобразуем мировую позицию врага в экранные координаты и устанавливаем позицию слайдера
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(enemyTransform.position + offset);
        healthSlider.transform.position = screenPosition;
    }
}
