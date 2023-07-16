using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [Tooltip("ќчки за уничтожени€ блока")]
    public int points;  //очки за блок
    public bool invisible; //невидимый блок

    [Header("Explosive")]
    public bool explosive; //взрывающийс€ блок
    public float explosionRadius; // радиус взрыва

    [Header("Prefabs")]
    public GameObject pickupPrefab;  //сюда ложим какой-то прифаб (монетка)
    public GameObject particleEffectPrefab;  //сюда ложим какой-то прифаб (салют)

    [Header("Sounds")]
    public AudioClip sndDestroyBlock; //сюда ложим какой-то звук 


    GameManager gameManager; //ссылка на скрипт (объект) GameManager (объ€вл€ем переменную)
    LevelManager levelManager;//ссылка на скрипт (объект) LevelManager (объ€вл€ем переменную) 
    AudioManager audioManager;//ссылка на скрипт (объект) AudioManager (объ€вл€ем переменную)

    SpriteRenderer spriteRenderer;//ссылка на скрипт (объект) SpriteRenderer (объ€вл€ем переменную)


    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>(); //находит объект со скриптом <GameManager>
        levelManager = FindObjectOfType<LevelManager>(); //находит объект со скриптом <LevelManager>
        audioManager = FindObjectOfType<AudioManager>(); //находит объект со скриптом <AudioManager>

        spriteRenderer = GetComponent<SpriteRenderer>(); // находим ссылку на SpriteRenderer

        levelManager.BlockCreated();

        if (invisible)
        {
            spriteRenderer.enabled = false;  //enabled выключает конкретный компонент
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (invisible)
        {
            spriteRenderer.enabled = true; //при касании блок включаетс€
            invisible= false;
            return; // return - дальше выполнение функции не идет
        }

        DestroyBlock();  //уничтожение блока
    }

    public void DestroyBlock()
    {
        gameManager.AddScore(points); //добавл€ет очки за разрушение блока
        levelManager.BlockDestroyed(); // сообщает что блок  был уничтожен
        Destroy(gameObject); //уничтожение блока
        //print("Block Collision");
        Instantiate(pickupPrefab, transform.position, Quaternion.identity); // создать монетку на основе прифаба
                                                                            // (т.е. блок разрушаетс€ и выпадает монетка)
                                                                            //Quaternion.identity - вращение объект при по€влении хз
        Instantiate(particleEffectPrefab, transform.position, Quaternion.identity); // создать салют на основе прифаба
         
        audioManager.PlaySound(sndDestroyBlock); //вкл музыку 
        if (explosive)
        {
            //блок взрывной - логика взрыва
            Explode();
        }
    
    }
    private void Explode()//блок взрывной - логика взрыва
    {
        //Ќаходит коллайдеры все в радиусе окружности cо слоем Block
        int layerMask = LayerMask.GetMask("Block");
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, layerMask);

        //for(int i = 0; i < colliders.Length; i++)    //тоже самое снизу   
        //{
        //    print(colliders[i].name);
        //}

        foreach (Collider2D col in colliders)//дл€ каждого коллайдера в массиве
                                                  //коллайдеров сделать действие какое-то в скобках
        {
            Block block = col.GetComponent<Block>();// ищим у коллайдера скрипт Block
            if (block == null)
            {
                // объект без скрипта Block - просто уничтожаетс€
                Destroy(col.gameObject);
            }
            else
            {
                // ќбъект со скриптом Block
                block.DestroyBlock();
            }
        }
    }


    private void OnDrawGizmos()  //рисуем окружность с радиусом взрыва
    {
        Gizmos.color = Color.red; //цвет окружности красный
        Gizmos.DrawWireSphere(transform.position, explosionRadius);//рисуем окружность с радиусом взрыва
    }
}
