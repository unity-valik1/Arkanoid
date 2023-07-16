using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseGame : MonoBehaviour
{
    GameManager gameManager;
    Ball ball;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // ссылки
        ball = FindObjectOfType<Ball>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball") //если √ќ у которого таг Ball, то делаем ниже
            //(collision.gameObject.CompareTag("Ball"))  - лучше в производительности ( одно и тоже)
        {
            // если м€ч
            gameManager.LoseLife();  //игрок тер€ет жизнь

            ball.Restart(); // м€ч возвращаетс€ на платформу
        }
        else
        {
            Destroy(collision.gameObject); // если не м€ч - уничтожаем объект
        }
    }
}
