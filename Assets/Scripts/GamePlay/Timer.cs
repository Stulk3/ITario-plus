using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    float fpsTime = 0;//количесво кадров 
    public int nowTime;//время
    Text timeText;
    PlayerMove player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMove>();
        timeText = gameObject.GetComponent<Text>();
        timeText.text = "TIME\n" + Zeropad(nowTime, 3);//пишем тест с помощью функции добавления 0
    }
    void Update()
    {
        if(player.isActive) fpsTime = fpsTime + Time.deltaTime;//добавляем  долю секунд
        if (fpsTime >= 1)//если прошла секунда
        {
            fpsTime = fpsTime - 1;// то отнимаем секунду от долей
            nowTime=nowTime-1;
            timeText.text = "TIME\n" +Zeropad(nowTime , 3);//пишем тест с помощью функции добавления 0
        }
        if (nowTime <= 0)
        {
            PlayerMove pl = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
            pl.Deatch();
        }
    }
    public string Zeropad(int number, int count)//функция добавления 0 (число; количество обязательных символов)
    {
        string answer = "";
        for (int i = count; i > 0; i = i - 1)//цикл с количества символов до 1
        {
            if (number / Mathf.Pow(10, i - 1) >= 1)//если число поделив на 10 в степени обязательных цифр больше 1, то возвращаем это число
            {
                return answer + number;
            }
            else
            {
                answer = answer + "0"; //иначе в ответ добавляем 0
            }
        }
        return answer;
    }
}

