using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sutamina : MonoBehaviour
{
    private GameObject sutaminaText;
    private int playsuu; // スタミナ
    private int nokorizikann; // 回復までの時間

    private DateTime lasttime; // 前回の回復時間
    private TimeSpan span; // lasttimeからの経過時間
    private double spansecond;

    private int maxplay; // MAXスタミナ
    private int kaihukutime; //回復に必要な秒数 
    private int kaihukuminutes;

    // Start is called before the first frame update
    void Start()
    {

        maxplay = 5;
        kaihukuminutes = 5; // 5分
        kaihukutime = kaihukuminutes * 60; 

        playsuu = PlayerPrefs.GetInt("sutamina", maxplay);
        // string型で保存しておいた前回の時間を取得
        lasttime = DateTime.Parse(PlayerPrefs.GetString("lasttime", DateTime.Now.ToString()));
        //Debug.Log(lasttime);

        sutaminaText = GameObject.FindGameObjectWithTag("sutamina");

        updatetime();
    }

    // Update is called once per frame
    void Update()
    {
        updatetime();
    }

    private void Textkousinn()
    {
        int spp;
        int minu;
        String secondtext;
        String minutestext;

        nokorizikann = kaihukutime - (int)spansecond;

        if (playsuu < maxplay)
        {
            spp = nokorizikann % 60;

            if (nokorizikann > 60)
            {
                minu = nokorizikann / 60;
            } else
            {
                minu = 0;
            }

        } else {
            spp = 0;
            minu = 0;
        }
        secondtext = spp.ToString("00");
 
        minutestext = minu.ToString("00");

        sutaminaText.GetComponent<Text>().text = 
            " プレイ回数:" + playsuu.ToString() + "\n"+
            " 回復まで" + minutestext + ":" + secondtext;
    }

    private void updatetime()
    {
        span = DateTime.Now - lasttime; // 何かオーバーフローしそうな式だなぁ……
        spansecond = span.TotalSeconds;
          

        Textkousinn();

        if (playsuu >= maxplay)
        {            
            return;
        }

        for (int i = playsuu; i < maxplay; i++)
        {
            if (spansecond >= kaihukutime)
            {
                spansecond -= kaihukutime;
                playsuu++;
                PlayerPrefs.SetInt("sutamina", playsuu);

                lasttime = DateTime.Now;
                PlayerPrefs.SetString("lasttime", lasttime.ToString());
                Textkousinn();

            }
            else
            {
                break;
            }
        }       

    }

    public bool sutaminadec()
    {
        if (playsuu > 0)
        {
            if (playsuu == maxplay)
            {
                lasttime = DateTime.Now;
                PlayerPrefs.SetString("lasttime", lasttime.ToString());
            }

            playsuu--;
            PlayerPrefs.SetInt("sutamina", playsuu);



            return true;
        }

        return false;
    }

    public void sutaminaplus()
    {

        playsuu++;
        PlayerPrefs.SetInt("sutamina", playsuu);

    }

    public bool sutaminamax()
    {
        if (playsuu < 100)
        {
            return true;

        }
        else
        {
            return false;
        }
    }
}
