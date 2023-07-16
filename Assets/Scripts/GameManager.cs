using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public static string keyBestScore = "bestRecord";//static - �� ���������� � �����
                                                     // ��� ����� ������

    public Text scoreText;  // ����� � ������
    public GameObject panelPause; //������ �����  // ���������� � GameObject panel pause
    public List<GameObject> lifesItems;// ������ �� GameObject ������� � �������

    [Header("Sounds")]
    public AudioClip sndPauseActivate;//���� ����� ������������ �����
    public AudioClip sndPauseDeactivate;//���� ����� �������������� �����

    [HideInInspector] //������ � ��������� 
    public bool pauseActive;

    public int lifes; //�����

    int score;
    

    AudioManager audioManager; //������� ������ �� �������� <AudioManager>

    private void Awake() // ���������� ������ ��� Start
    {
        GameManager[] gameManagers = FindObjectsOfType<GameManager>(); //������� ������ ����
                                                                       //��������� <GameManager> 
        for (int i = 0; i < gameManagers.Length; i++)  // ����
        {
            if (gameManagers[i].gameObject != gameObject) // ���� ����� �� ����� ��������, ��
            {
                Destroy(gameObject);   // ������������
                break;  // �������� ���� ��������               
                //���
                //if (gameManagers[i] != this)gameObject != gameObject
                //{

                //}
            }

        }
        //������� ������ �� �������� <AudioManager>
        audioManager = FindObjectOfType<AudioManager>(); 
    }

    private void Start()
    {
        scoreText.text = "000";   // ��������� ����� �����

        DontDestroyOnLoad(gameObject); // ���� ������������ �� �����, // � ����� ���������� � ����� �����
        
        UpdateLifes();
    }
    public void AddScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();  //���� �������� ����� �� �������� ToString

        SaveBestScore(); // c���� �����������
    }

    public void SaveBestScore()// c���� �����������
    {
        int oldBestScore = PlayerPrefs.GetInt(keyBestScore); //����� ������ ������
        if(score > oldBestScore) // ���� ������� ������ ������ ������� �������, ��...
        {//���������
            PlayerPrefs.SetInt(keyBestScore, score); //��� ��������� ������ ������
                                                     //� � ������� ���� � ��������
                                                     //����� ������� �����
        }
    }

    public void AddLife() 
    {
        if (lifes >= lifesItems.Count) //���� ������ ������ ��� ��������,
                                       //�� �� ��������� �����
        {
            return;                    //�� �� ��������� �����
        }

        lifes++; // +�����
        UpdateLifes(); //������� ���������� �����
    }
    public void LoseLife()
    {
        lifes--; // ��� ���� �� ��� ������ � ����� �����
        UpdateLifes(); //������� ���������� �����
        if (lifes <= 0) //
        {
            //restart
        }
    }


    // ��������� �� ������� �������� ���� ��� �����
    // �������� � 0, ������ < ����������� ���� ���������, ����� +1 � �������
    private void UpdateLifes()
    {
        for (int index  = 0; index < lifesItems.Count; index++) 
        {
            if (index < lifes)
            {
                lifesItems[index].SetActive(true); //������������
            }
            else
            {
                lifesItems[index].SetActive(false); // ����������
            }

        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // ������� ������ �������������� � ������ Update
        {
            if (pauseActive)
            {
                Time.timeScale = 1f; //����� ������� - ������� ����� � 1
                pauseActive = false;
                audioManager.PlaySound(sndPauseDeactivate); //�������� ������
            }
            else
            {
                Time.timeScale = 0f;//������ �������� �������
                                    //�������� �����
                pauseActive = true;
                audioManager.PlaySound(sndPauseActivate); //�������� ������
            }
            panelPause.SetActive(pauseActive); //������� ����� � ������ � ������ 
                                               //������� ����� �����
        }

        if (Input.GetKeyDown(KeyCode.Return))//������� �����
        {
            AddLife(); // +1 �����
        }
    }
}
