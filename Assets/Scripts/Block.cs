using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Tooltip("���� �� ����������� �����")]
    public int points;  //���� �� ����
    public bool invisible; //��������� ����

    [Header("Explosive")]
    public bool explosive; //������������ ����
    public float explosionRadius; // ������ ������

    [Header("Prefabs")]
    public GameObject pickupPrefab;  //���� ����� �����-�� ������ (�������)
    public GameObject particleEffectPrefab;  //���� ����� �����-�� ������ (�����)

    [Header("Sounds")]
    public AudioClip sndDestroyBlock; //���� ����� �����-�� ���� 


    GameManager gameManager; //������ �� ������ (������) GameManager (��������� ����������)
    LevelManager levelManager;//������ �� ������ (������) LevelManager (��������� ����������) 
    AudioManager audioManager;//������ �� ������ (������) AudioManager (��������� ����������)

    SpriteRenderer spriteRenderer;//������ �� ������ (������) SpriteRenderer (��������� ����������)


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); //������� ������ �� �������� <GameManager>
        levelManager = FindObjectOfType<LevelManager>(); //������� ������ �� �������� <LevelManager>
        audioManager = FindObjectOfType<AudioManager>(); //������� ������ �� �������� <AudioManager>

        spriteRenderer = GetComponent<SpriteRenderer>(); // ������� ������ �� SpriteRenderer

        levelManager.BlockCreated();

        if (invisible)
        {
            spriteRenderer.enabled = false;  //enabled ��������� ���������� ���������
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (invisible)
        {
            spriteRenderer.enabled = true; //��� ������� ���� ����������
            invisible= false;
            return; // return - ������ ���������� ������� �� ����
        }

        DestroyBlock();  //����������� �����
    }

    public void DestroyBlock()
    {
        gameManager.AddScore(points); //��������� ���� �� ���������� �����
        levelManager.BlockDestroyed(); // �������� ��� ����  ��� ���������
        Destroy(gameObject); //����������� �����
        //print("Block Collision");
        Instantiate(pickupPrefab, transform.position, Quaternion.identity); // ������� ������� �� ������ �������
                                                                            // (�.�. ���� ����������� � �������� �������)
                                                                            //Quaternion.identity - �������� ������ ��� ��������� ��
        Instantiate(particleEffectPrefab, transform.position, Quaternion.identity); // ������� ����� �� ������ �������
         
        audioManager.PlaySound(sndDestroyBlock); //��� ������ 
        if (explosive)
        {
            //���� �������� - ������ ������
            Explode();
        }
    
    }
    private void Explode()//���� �������� - ������ ������
    {
        //������� ���������� ��� � ������� ���������� c� ����� Block
        int layerMask = LayerMask.GetMask("Block");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layerMask);

        //for(int i = 0; i < colliders.Length; i++)    //���� ����� �����   
        //{
        //    print(colliders[i].name);
        //}

        foreach (Collider2D col in colliders)//��� ������� ���������� � �������
                                                  //����������� ������� �������� �����-�� � �������
        {
            Block block = col.GetComponent<Block>();// ���� � ���������� ������ Block
            if (block == null)
            {
                // ������ ��� ������� Block - ������ ������������
                Destroy(col.gameObject);
            }
            else
            {
                // ������ �� �������� Block
                block.DestroyBlock();
            }
        }
    }


    private void OnDrawGizmos()  //������ ���������� � �������� ������
    {
        Gizmos.color = Color.red; //���� ���������� �������
        Gizmos.DrawWireSphere(transform.position, explosionRadius);//������ ���������� � �������� ������
    }
}
