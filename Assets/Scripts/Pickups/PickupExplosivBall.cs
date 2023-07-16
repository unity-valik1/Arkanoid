using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupExplosivBall : MonoBehaviour
{
    private void ApplyEffect()
    {   //������� ������ �� �������� <GameManager>
        Ball[] balls = FindObjectsOfType<Ball>(); //������� ��� ������� � �����
        foreach (Ball ball in balls)
        {
            ball.ActivateExplosion();//� � ���� ����� �������� ������� ��������� ������

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pad")) //������������ � ����������
        {
            // ���������� �������
            ApplyEffect(); // ��������� ������
            Destroy(gameObject); // ������� ������ ������
        }

    }
}
