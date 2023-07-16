using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ball : MonoBehaviour
{
    public float explosionRadius;
    public float speed;

    public GameObject explosionEffect; //���� ����� �����-�� ������ (����� ����)
    public AudioClip explosionModeSound; //���� ����� ������������ ����� ������ ����

    Rigidbody2D rb;
    AudioSource audioSource; //���������� �����

    Pad pad;

    bool isStarted;
    bool isMagnetActive; // ������� ������ � ��������� ��� ���
    bool isExplosive; // ������� ����� ���� ��� ���

    float yPosition; //������� ���� �� ��������� �
    float xDelta;  //������� ���� �� ��������� �




    private void Awake()
    {
        //����� ����������� �� ���� �� GameObject ����� ������ � Awake
        rb =GetComponent<Rigidbody2D>(); // ������� ������ �� Rigidbody2D
        audioSource = GetComponent<AudioSource>(); // ������� ������ �� AudioSource
    }
    public void ActivateMagnet()
    {
        //�������� ������ � ���������
        isMagnetActive = true;
    }

    public void ActivateExplosion()
    {
        //�������� ����� ����
        isExplosive = true;
        explosionEffect.SetActive(true); // �������� ������ ������ ����(�����)
        audioSource.clip = explosionModeSound; //������ ����� ����� .clip


        // TODO �������� ���� ������ � ����
    }

    public void MultiplySpeed(float speedKoef) // �������, ������� �������� �������� ����
                                               // �� �����-�� �����������
                                               // �.�. ����������� �������� ����
    {
        speed *= speedKoef; //�������� ���� �������� �� �����-�� ��.
                            //*= �������� �� ���� ����
                            // ���� �������� < 1, �� ����������
                            //���� �������� > 1, �� ����������
        rb.velocity = rb.velocity.normalized * speed; //����� �������� = ������ �������� * �� ����� ��
    }

    void Start()
    {
        pad = FindObjectOfType<Pad>();  //������ �� ���������

        yPosition = transform.position.y; //������� ���� �� �

        //������� ���� �� � ����� ������� ��������� �� �
        xDelta = transform.position.x - pad.transform.position.x;

        if (pad.autoPlay )
        {
            StartBall();
        }
    }

    public void Restart()
    {
        isStarted = false; //���������� ���� �� ��������� ����� ������� �� ��� ������

        rb.velocity = Vector2.zero; // ��� new Vector2( 0, 0 ); ���������� ���� ����
    }

    public void Duplicate()
    {
        Ball originalBall = this;//this - ������������ ���� � �������� ��������� (���) 
        Ball newBall = Instantiate(originalBall); // newBall - ������ �� ����� ������
        // ��� Instantiate(this);

        newBall.speed = speed; //�������� ������ ���� ����� �������� �������� ���� 
        newBall.StartBall(); //��������� ����� ����� ���

        if(isMagnetActive) // ���� � �������� ���� ������� ������, ��..
        { 
            newBall.ActivateMagnet(); // ������ ���� ���� ��� ������
        }
        if (isExplosive) // ���� � �������� ���� ������� �����, ��..
        {
            newBall.ActivateExplosion(); // ������ ���� ���� ��� �����
        }
    }

    private void Update()
    {
        if (isStarted)
        {
             //��� ������� - ������ �� ������
        }
        else
        {
            //��� �� �������

            //��������� ������ � ����������
            Vector3 padPosition = pad.transform.position;
            //����� ������� ����
            Vector3 ballNewPosition = new Vector3(padPosition.x + xDelta, yPosition, 0); 
            transform.position = ballNewPosition;

            // ��������� ����� ������ ����
            if (Input.GetMouseButtonDown(0)) // 0 ����� ������� ����
            {
                StartBall();
            }
        }
        //print(rb.velocity);
        //print(rb.velocity.magnitude);  // magnitude - ����� �������
    }

    private void StartBall()
    {
        float randomX = Random.Range(0, 0);
        Vector2 direction = new Vector2(randomX, 1);
        Vector2 force = direction.normalized * speed;

        rb.velocity = force;

        isStarted = true;
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlayer)//�������� (��������� �� ���� � ������ ����)
        {
            Gizmos.DrawRay(transform.position, rb.velocity);
        }

        Gizmos.color = Color.magenta; //���� ���������� �����
                                   //(���� ������ ����� ��������, ��� ������� ����� ������)
        Gizmos.DrawWireSphere(transform.position, explosionRadius);//������ ���������� � �������� ������
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioSource.Play();//��� ������       // audioSource.Stop() //���� ������
        if (isMagnetActive && collision.gameObject.CompareTag("Pad")) //���� ��������� ������� �
                                                                         //���� ��������� �������� � 
                                                                         //������������ � ��������
                                                                         //��� = pad, ��...
            {
                //������� ���� �� �
                yPosition = transform.position.y;

                //������� ���� �� � ����� ������� ��������� �� �
                xDelta = transform.position.x - pad.transform.position.x;

                Restart(); // ��� ��������� � ���������
            }

        if (isExplosive && collision.gameObject.CompareTag("Block"))
        //���� ��� �������� ��� �� �������� ������� � ����� "Block" �� ...
        {
            Explode(); //��� ����������
        }
    }

    private void Explode()//��� �������� - ������ ������
    {
        //������� ���������� ��� � ������� ���������� c� ����� Block
        int layerMask = LayerMask.GetMask("Block");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position, 
            explosionRadius,
            layerMask);

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

    private void OnCollisionExit2D(Collision2D collision)
    {
        //print("Collision Exit!");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //print("Trigger!");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //print("Trigger Exit!");
    }
}
