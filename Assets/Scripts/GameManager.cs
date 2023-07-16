using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{

    public static string keyBestScore = "bestRecord";//static - не изменяется и нужен
                                                     // все время доступ

    public Text scoreText;  // текст с очками
    public GameObject panelPause; //панель паузы  // обращается к GameObject panel pause
    public List<GameObject> lifesItems;// ссылка на GameObject каринок с жизнями

    [Header("Sounds")]
    public AudioClip sndPauseActivate;//звук когда активируется пауза
    public AudioClip sndPauseDeactivate;//звук когда деактивируется пауза

    [HideInInspector] //прячет в инспектре 
    public bool pauseActive;

    public int lifes; //жизни

    int score;
    

    AudioManager audioManager; //находит объект со скриптом <AudioManager>

    private void Awake() // Вызывается раньше чем Start
    {
        GameManager[] gameManagers = FindObjectsOfType<GameManager>(); //находим массив всех
                                                                       //объекстов <GameManager> 
        for (int i = 0; i < gameManagers.Length; i++)  // цикл
        {
            if (gameManagers[i].gameObject != gameObject) // если новый не равен текущему, то
            {
                Destroy(gameObject);   // уничтожается
                break;  // прервать цикл досрочно               
                //или
                //if (gameManagers[i] != this)gameObject != gameObject
                //{

                //}
            }

        }
        //находит объект со скриптом <AudioManager>
        audioManager = FindObjectOfType<AudioManager>(); 
    }

    private void Start()
    {
        scoreText.text = "000";   // стартовое число очков

        DontDestroyOnLoad(gameObject); // очки уничтожаться не будут, // а будут переходить в некст сцену
        
        UpdateLifes();
    }
    public void AddScore(int addScore)
    {
        score += addScore;
        scoreText.text = score.ToString();  //если числовой текст то вызываем ToString

        SaveBestScore(); // cразу сохраняется
    }

    public void SaveBestScore()// cразу сохраняется
    {
        int oldBestScore = PlayerPrefs.GetInt(keyBestScore); //берем старый рекорд
        if(score > oldBestScore) // если текущий рекорд больше старого рекорда, то...
        {//сохраняем
            PlayerPrefs.SetInt(keyBestScore, score); //где храниться лучший рекорд
                                                     //а в скобках айди и рекордом
                                                     //равен текущим очкам
        }
    }

    public void AddLife() 
    {
        if (lifes >= lifesItems.Count) //если жизней больше чем картинок,
                                       //то не добавляем жизни
        {
            return;                    //то не добавляем жизни
        }

        lifes++; // +жизнь
        UpdateLifes(); //обновит оставшиеся жизни
    }
    public void LoseLife()
    {
        lifes--; // мяч упал на низ экрана и минус жизнь
        UpdateLifes(); //обновит оставшиеся жизни
        if (lifes <= 0) //
        {
            //restart
        }
    }


    // проийтись по каждому элементу зная его индех
    // начинаем с 0, индекс < колличества всех элементов, тогда +1 к индексу
    private void UpdateLifes()
    {
        for (int index  = 0; index < lifesItems.Count; index++) 
        {
            if (index < lifes)
            {
                lifesItems[index].SetActive(true); //прибавляется
            }
            else
            {
                lifesItems[index].SetActive(false); // отнимактся
            }

        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // нажатие кнопок обрабатывается в методе Update
        {
            if (pauseActive)
            {
                Time.timeScale = 1f; //пауза активна - вернуть время в 1
                pauseActive = false;
                audioManager.PlaySound(sndPauseDeactivate); //включить музыку
            }
            else
            {
                Time.timeScale = 0f;//маштаб игрового времени
                                    //включить паузу
                pauseActive = true;
                audioManager.PlaySound(sndPauseActivate); //включить музыку
            }
            panelPause.SetActive(pauseActive); //убирает текст с паузой с экрана 
                                               //выводит паузу текст
        }

        if (Input.GetKeyDown(KeyCode.Return))//нажимаю интер
        {
            AddLife(); // +1 жизнь
        }
    }
}
