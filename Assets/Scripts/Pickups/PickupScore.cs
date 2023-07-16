using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScore : MonoBehaviour
{
    public int points;
    private void ApplyEffect()
    {   //находит объект со скриптом <GameManager>
        GameManager gameManager = FindObjectOfType<GameManager>();
        
        gameManager.AddScore(points); //добавляем очки
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision. gameObject.CompareTag("Pad")) //столкновение с платформой
        {
            // применение эффекта
            ApplyEffect(); // применяем эффект
            Destroy(gameObject); // удаляем иконку бонуса
        }
      
    }

}
