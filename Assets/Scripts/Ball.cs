using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Ball : MonoBehaviour
{
    public float explosionRadius;
    public float speed;

    public GameObject explosionEffect; //сюда ложим какой-то прифаб (взрыв мяча)
    public AudioClip explosionModeSound; //звук когда активируется режим взрыва мяча

    Rigidbody2D rb;
    AudioSource audioSource; //переменная аудио

    Pad pad;

    bool isStarted;
    bool isMagnetActive; // включен магнит к платформе или нет
    bool isExplosive; // включен взрыв мяча или нет

    float yPosition; //позиция мяча по отношению у
    float xDelta;  //позиция мяча по отношению х




    private void Awake()
    {
        //Поиск компанентов на этом же GameObject лучше делать в Awake
        rb =GetComponent<Rigidbody2D>(); // находим ссылку на Rigidbody2D
        audioSource = GetComponent<AudioSource>(); // находим ссылку на AudioSource
    }
    public void ActivateMagnet()
    {
        //включает магнит к платформе
        isMagnetActive = true;
    }

    public void ActivateExplosion()
    {
        //включает взрыв мяча
        isExplosive = true;
        explosionEffect.SetActive(true); // включает эффект взрыва мяча(огонь)
        audioSource.clip = explosionModeSound; //меняем звуки через .clip


        // TODO Поменять цвет траила и мяча
    }

    public void MultiplySpeed(float speedKoef) // функция, которая умножает скорость мяча
                                               // на какой-то коэффициент
                                               // т.е. увеличивает скорость мяча
    {
        speed *= speedKoef; //Скорость мяча умножить на какой-то кф.
                            //*= умножить на само себя
                            // если скорость < 1, то уменьшится
                            //если скорость > 1, то увеличится
        rb.velocity = rb.velocity.normalized * speed; //новая скорость = старая скорость * на новый кф
    }

    void Start()
    {
        pad = FindObjectOfType<Pad>();  //ссылка на платформу

        yPosition = transform.position.y; //позиция мяча по у

        //позиция мяча по х минус позиция платформы по х
        xDelta = transform.position.x - pad.transform.position.x;

        if (pad.autoPlay )
        {
            StartBall();
        }
    }

    public void Restart()
    {
        isStarted = false; //возвращает мячь на платформу после подения на низ экрана

        rb.velocity = Vector2.zero; // или new Vector2( 0, 0 ); сбрасывает силу мяча
    }

    public void Duplicate()
    {
        Ball originalBall = this;//this - использовать себя в качестве дубликата (мяч) 
        Ball newBall = Instantiate(originalBall); // newBall - ссылка на новый объект
        // или Instantiate(this);

        newBall.speed = speed; //Скорость нового мяча равна скорости текущего мяча 
        newBall.StartBall(); //запускаем сразу новый мяч

        if(isMagnetActive) // если у текущего мяча включен магнит, то..
        { 
            newBall.ActivateMagnet(); // новому мячу тоже вкл магнит
        }
        if (isExplosive) // если у текущего мяча включен взрыв, то..
        {
            newBall.ActivateExplosion(); // новому мячу тоже вкл взрыв
        }
    }

    private void Update()
    {
        if (isStarted)
        {
             //Мяч запущен - ничего не делаем
        }
        else
        {
            //Мяч не запущен

            //двигаться вместе с платформой
            Vector3 padPosition = pad.transform.position;
            //новая позиция мяча
            Vector3 ballNewPosition = new Vector3(padPosition.x + xDelta, yPosition, 0); 
            transform.position = ballNewPosition;

            // проверить левую кнопку мыши
            if (Input.GetMouseButtonDown(0)) // 0 левая клавиша мишы
            {
                StartBall();
            }
        }
        //print(rb.velocity);
        //print(rb.velocity.magnitude);  // magnitude - длина вектора
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
        if (Application.isPlayer)//проверка (находится ли игра в режиме игры)
        {
            Gizmos.DrawRay(transform.position, rb.velocity);
        }

        Gizmos.color = Color.magenta; //цвет окружности синий
                                   //(цвет задаем перед строчкой, для которой нужно задать)
        Gizmos.DrawWireSphere(transform.position, explosionRadius);//рисуем окружность с радиусом взрыва
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioSource.Play();//вкл музыку       // audioSource.Stop() //выкл музыку
        if (isMagnetActive && collision.gameObject.CompareTag("Pad")) //если магнетизм активен и
                                                                         //если произошла коллизия с 
                                                                         //геймобжектом у которого
                                                                         //таг = pad, то...
            {
                //позиция мяча по у
                yPosition = transform.position.y;

                //позиция мяча по х минус позиция платформы по х
                xDelta = transform.position.x - pad.transform.position.x;

                Restart(); // мяч прилипает к платформе
            }

        if (isExplosive && collision.gameObject.CompareTag("Block"))
        //если мяч взрывной или он коснулся объекта с тагом "Block" то ...
        {
            Explode(); //мяч взрывается
        }
    }

    private void Explode()//мяч взрывной - логика взрыва
    {
        //Находит коллайдеры все в радиусе окружности cо слоем Block
        int layerMask = LayerMask.GetMask("Block");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position, 
            explosionRadius,
            layerMask);

        //for(int i = 0; i < colliders.Length; i++)    //тоже самое снизу   
        //{
        //    print(colliders[i].name);
        //}

        foreach (Collider2D col in colliders)//для каждого коллайдера в массиве
                                             //коллайдеров сделать действие какое-то в скобках
        {
            Block block = col.GetComponent<Block>();// ищим у коллайдера скрипт Block
            if (block == null)
            {
                // объект без скрипта Block - просто уничтожается
                Destroy(col.gameObject);
            }
            else
            {
                // Объект со скриптом Block
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
