using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupExplosivBall : MonoBehaviour
{
    private void ApplyEffect()
    {   //находит объект со скриптом <GameManager>
        Ball[] balls = FindObjectsOfType<Ball>(); //находим все скрипты с м€чом
        foreach (Ball ball in balls)
        {
            ball.ActivateExplosion();//и у этих м€чей вызываем функцию активации взрыва

        }
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
