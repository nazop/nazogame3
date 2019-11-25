using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    private int coin;
    private GameObject coinText;

    private int syokitiue;
    private GameObject ueText;
    private int uecoin;
    private int uecoinPlus; // コインの上昇率

    private int syokitisita;
    private GameObject sitaText;
    private int sitacoin;
    private int sitacoinPlus;

    private int max;
    private GameObject maxText;
    private int maxcoin;
    private int maxcoinPlus;

    private GameObject dialog; // 表示するダイアログのような物
    //private GameObject errordialog; // エラー用 onにしといてね
    public GameObject errordialog; // インスペクターから入れた方が画面使いやすい
    private Text errorText; // Textコンポーネントなので.text="エラーテキスト"で変更

    private int max_max; // 数値上限の上限



    // Start is called before the first frame update
    void Start()
    {
        dialog = GameObject.FindGameObjectWithTag("Dialog");
        /*
        errordialog = GameObject.FindGameObjectWithTag("error");
        errordialog.SetActive(false);
        */

        // コインのテキストを書いてるオブジェクトを取って来る
        coinText = GameObject.FindGameObjectWithTag("Dia_Coin");
        coin = PlayerPrefs.GetInt("Coin", 0); // セーブされた値、セーブが無い時は0
        // 文字を修正
        coinText.GetComponent<Text>().text = " coin:" + coin.ToString();

        // 上限アップボタン
        syokitiue = PlayerPrefs.GetInt("syokitiue", 1); // ボタン生成時の初期値の上限
        ueText = GameObject.FindGameObjectWithTag("Dia_ue");
        uecoinPlus = 100;
        //uecoin = syokitiue * uecoinPlus;
        uecoin = keisann(syokitiue, uecoinPlus);
        uehyouzi();


        // 下限アップボタン
        syokitisita = PlayerPrefs.GetInt("syokitisita", 1); // 初期値の下限
        sitaText = GameObject.FindGameObjectWithTag("Dia_sita");
        sitacoinPlus = 200;
        //sitacoin = syokitisita * sitacoinPlus;
        sitacoin = keisann(syokitisita, sitacoinPlus);
        sitahyouzi();

        // 最大値アップボタン
        max = PlayerPrefs.GetInt("maxNumber", 50); // 数値の最大値
        maxText = GameObject.FindGameObjectWithTag("Dia_max");
        maxcoinPlus = 200;
        //maxcoin = max * maxcoinPlus;
        maxcoin = keisann(max, maxcoinPlus);
        maxhyouzi();

        max_max = 10000; // あの幅ではここまで、99999でも可

        errorText = errordialog.transform.Find("Text").gameObject.GetComponent<Text>();

        // デバッグ用
        /*
        coin = 10000000;
        max = 1;
        max_max = 1;
        */

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void coinchange(int change)
    {
        coin -= change;

        // 文字を修正
        coinText.GetComponent<Text>().text = " coin:" + coin.ToString();

        PlayerPrefs.SetInt("Coin", coin);

        return;
    }

    private void uehyouzi()
    {
        // 文字を修正
        ueText.GetComponent<Text>().text = 
            "初期値上限アップ\n" +
            "現在値:" + syokitiue + "\n" +
            "coin:" + uecoin.ToString();
    }

    private void sitahyouzi()
    {
        // 文字を修正
        sitaText.GetComponent<Text>().text =
            "初期値下限アップ\n" +
            "現在値:" + syokitisita + "\n" +
            "coin:" + sitacoin.ToString();
    }

    private void maxhyouzi()
    {
        // 文字を修正
        maxText.GetComponent<Text>().text =
            "数値上限アップ\n" +
            "現在値:" + max + "\n" +
            "coin:" + maxcoin.ToString();
    }


    public void ueClick()
    {
        if (syokitiue >= max)
        {
            // もし数値上限値以上だったらエラー
            // 上になって貰っては困るが
            errorText.text =
            "上限値です\n" +
            "数値上限を\n" +
            "上げて下さい";
            errordia();
            return;
        }

        if (coin >= uecoin)
        {
            coinchange(uecoin);
            syokitiue++;
            PlayerPrefs.SetInt("syokitiue", syokitiue);
            //uecoin = syokitiue * uecoinPlus;
            uecoin = keisann(syokitiue, uecoinPlus);
            uehyouzi();
        }

        return;
    }

    public void sitaClick()
    {
        if (syokitisita >= syokitiue)
        {
            errorText.text =
             "上限値です\n" +
             "初期値上限を\n" +
             "上げて下さい";

            // もし数値上限値以上だったらエラー
            // 上になって貰っては困るが
            errordia();
            return;
        }

        if (coin >= sitacoin)
        {
            coinchange(sitacoin);
            syokitisita++;
            PlayerPrefs.SetInt("syokitisita", syokitisita);
            //sitacoin = syokitisita * sitacoinPlus;
            sitacoin = keisann(syokitisita, sitacoinPlus);
            sitahyouzi();
        }

        return;
    }

    public void modoruClick()
    {
        GameObject.FindGameObjectWithTag("gamesystem").GetComponent<startgamenn>().coinChange();
        dialog.SetActive(false);
        return;
    }

    public void maxClick()
    {
        if (coin >= maxcoin)
        {
            coinchange(maxcoin);

            if (max == max_max)
            {
                errorText.text =
                "上限値です";

                errordia();

                return;

            } else
            {
                max++;
            }


            PlayerPrefs.SetInt("maxNumber", max);
            //maxcoin = max * maxcoinPlus;
            maxcoin = keisann(max, maxcoinPlus);
            maxhyouzi();
        }

        return;
    }

    private void errordia()
    {
        errordialog.SetActive(true);

        Invoke("errorDelay", 1.0f);
        return;
    }

    private void errorDelay()
    {
        errordialog.SetActive(false);
        return;
    }

    private int keisann(int atai, int plus)
    {
        // 必要な強化コインの量を計算

        //int co = plus + atai * 100;
        int co = 0;

        // 一応エラー処理
        try
        {
            checked
            {
                //co = atai * 100;
                co = atai * plus;
            }
        }


        catch (OverflowException)
        {
            co = 2147483647;
        }

        return co;
    }

}
