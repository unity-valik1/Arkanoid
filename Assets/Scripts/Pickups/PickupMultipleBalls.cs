using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupMultipleBalls : MonoBehaviour
{
    private void ApplyEffect()
    {   //������� ������ �� �������� <GameManager>
        Ball ball = FindObjectOfType<Ball>(); //������� ������ ����

        ball.Duplicate(); // ������� ��� ��� 
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
