using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseGame : MonoBehaviour
{
    GameManager gameManager;
    Ball ball;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); // ������
        ball = FindObjectOfType<Ball>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ball") //���� �� � �������� ��� Ball, �� ������ ����
            //(collision.gameObject.CompareTag("Ball"))  - ����� � ������������������ ( ���� � ����)
        {
            // ���� ���
            gameManager.LoseLife();  //����� ������ �����

            ball.Restart(); // ��� ������������ �� ���������
        }
        else
        {
            Destroy(collision.gameObject); // ���� �� ��� - ���������� ������
        }
    }
}
