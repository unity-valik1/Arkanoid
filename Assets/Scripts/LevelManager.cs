using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public int blocksCount;

    //private void Start()
    //{
    //    Block[] allBlocks = FindObjectsOfType<Block>(); // находит все объекты с типом <Block>
    //    blocksCount= allBlocks.Length; //взять все найденные блоки (и)= длинну этого массива
    //}

    public void BlockCreated()
    {
        blocksCount++;
    }

    public void BlockDestroyed()
    {
        blocksCount--;
        if (blocksCount <= 0)
        {
            //Уровень пройдень
            int index = SceneManager.GetActiveScene().buildIndex; //SceneManager дай пж
                                                                  //активную сцену с текущим
                                                                  //идексом
            SceneManager.LoadScene(index + 1); // +1 к текущему индексу сцены и
                                               // переходим на некст сцену

            GameManager gameManager = FindObjectOfType<GameManager>(); //находит объект со скриптом <GameManager>
            gameManager.SaveBestScore(); //Сохранить рекорд

        }
    }
}
