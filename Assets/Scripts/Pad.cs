using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pad : MonoBehaviour
{
    public float padSpeed = 1f;

    public bool keyboard;
    public bool autoPlay; // автоплей
    public float maxX;

    float yPosition;
    Ball ball; // автоплей

    GameManager gameManager;
   
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        ball = FindObjectOfType<Ball>(); // только в старте искать ссылку
                                         // ( сильно влияет на производительность)
        yPosition = transform.position.y;

        Cursor.visible= false; // выключает курсор
    }

    // Update is called once per frame
    void Update()
    {

        if (gameManager.pauseActive)
        {
            //если true ничего делать не нужно, пауза активга
            return; // выходим из функуии void Update()
        }

        Movement();

    }

    private void Movement()
    {
        Vector3 padNewPosition;
        if (autoPlay)// автоплей
        {
            Vector3 ballPos = ball.transform.position;
            padNewPosition = new Vector3(ballPos.x, yPosition, 0);

            //padNewPos.x = Mathf.Clamp(padNewPos.x, -maxX, maxX);
            //transform.position = padNewPos;
        }
        if (keyboard)// не автоплей клавиатура
        {
            padNewPosition = transform.position;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                //перемещение плавное вне зависимости от фпс
                padNewPosition.x += padSpeed * Time.deltaTime; 
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                //перемещение плавное вне зависимости от фпс
                padNewPosition.x -= padSpeed * Time.deltaTime;
            }

        }
        else// не автоплей мышка
        {

            Vector3 mousePixelPosition = Input.mousePosition; // позиция мыши в координатах экрана
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePixelPosition);
            // позиция мыши в координатах игрового мира

            // mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            padNewPosition = new Vector3(mouseWorldPosition.x, yPosition, 0);
            //Vector3 padNewPosition = mouseWorldPosition;  //движение платформы
            //padNewPosition.z = 0;
            //padNewPosition.y = yPosition;
        }

        padNewPosition.x = Mathf.Clamp(padNewPosition.x, -maxX, maxX); // (Само число, мин, макс)
        transform.position = padNewPosition;// платформа не выезжает за игру
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
