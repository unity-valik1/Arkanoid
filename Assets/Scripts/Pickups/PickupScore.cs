using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScore : MonoBehaviour
{
    public int points;
    private void ApplyEffect()
    {   //������� ������ �� �������� <GameManager>
        GameManager gameManager = FindObjectOfType<GameManager>();
        
        gameManager.AddScore(points); //��������� ����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision. gameObject.CompareTag("Pad")) //������������ � ����������
        {
            // ���������� �������
            ApplyEffect(); // ��������� ������
            Destroy(gameObject); // ������� ������ ������
        }
      
    }

}
