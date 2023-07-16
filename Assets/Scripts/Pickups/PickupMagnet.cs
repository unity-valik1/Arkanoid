using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMagnet : MonoBehaviour
{
    private void ApplyEffect()
    {   //������� ������ �� �������� <GameManager>
        Ball[] balls = FindObjectsOfType<Ball>(); //������� ��� ������� � �����
        foreach(Ball ball in balls)
        {
            ball.ActivateMagnet();//� � ���� ����� �������� ������� ��������� �������

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
