using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    public float padSpeed = 1f;

    public bool keyboard;
    public bool autoPlay; // ��������
    public float maxX;

    float yPosition;
    Ball ball; // ��������

    GameManager gameManager;
   
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        ball = FindObjectOfType<Ball>(); // ������ � ������ ������ ������
                                         // ( ������ ������ �� ������������������)
        yPosition = transform.position.y;

        Cursor.visible= false; // ��������� ������
    }

    // Update is called once per frame
    void Update()
    {

        if (gameManager.pauseActive)
        {
            //���� true ������ ������ �� �����, ����� �������
            return; // ������� �� ������� void Update()
        }

        Movement();

    }

    private void Movement()
    {
        Vector3 padNewPosition;
        if (autoPlay)// ��������
        {
            Vector3 ballPos = ball.transform.position;
            padNewPosition = new Vector3(ballPos.x, yPosition, 0);

            //padNewPos.x = Mathf.Clamp(padNewPos.x, -maxX, maxX);
            //transform.position = padNewPos;
        }
        if (keyboard)// �� �������� ����������
        {
            padNewPosition = transform.position;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                //����������� ������� ��� ����������� �� ���
                padNewPosition.x += padSpeed * Time.deltaTime; 
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                //����������� ������� ��� ����������� �� ���
                padNewPosition.x -= padSpeed * Time.deltaTime;
            }

        }
        else// �� �������� �����
        {

            Vector3 mousePixelPosition = Input.mousePosition; // ������� ���� � ����������� ������
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePixelPosition);
            // ������� ���� � ����������� �������� ����

            // mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            padNewPosition = new Vector3(mouseWorldPosition.x, yPosition, 0);
            //Vector3 padNewPosition = mouseWorldPosition;  //�������� ���������
            //padNewPosition.z = 0;
            //padNewPosition.y = yPosition;
        }

        padNewPosition.x = Mathf.Clamp(padNewPosition.x, -maxX, maxX); // (���� �����, ���, ����)
        transform.position = padNewPosition;// ��������� �� �������� �� ����
        //if (padNewPosition.x > maxX)       
        //{
        //    padNewPosition.x = maxX;
        //}
        //if (padNewPosition.x < -maxX)
        //{
        //    padNewPosition.x = -maxX;
        //}
    }
}
