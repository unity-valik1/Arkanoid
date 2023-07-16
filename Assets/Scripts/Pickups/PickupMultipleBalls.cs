using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMultipleBalls : MonoBehaviour
{
    private void ApplyEffect()
    {   //находит объект со скриптом <GameManager>
        Ball ball = FindObjectOfType<Ball>(); //находим скрипт м€ча

        ball.Duplicate(); // создает еще м€ч 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pad")) //столкновение с платформой
        {
            // применение эффекта
            ApplyEffect(); // примен€ем эффект
            Destroy(gameObject); // удал€ем иконку бонуса
        }

    }
}
