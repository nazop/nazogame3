using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class start : MonoBehaviour
{

    private float buttonX;
    private float buttonY;
    private GameObject player;
    public GameObject prefabButton; // インスペクターから入れる
    private int yoko = 5;
    private int tate = 7;
    private GameObject[,] buttonArray; // 2次元配列
    private Vector3 board;
    private Vector3 hidarihasi;
    private int necessaryTap; // 何個タップでOKにするか
    private GameObject[] tapButton; // タップしたボタンを入れる
    private int clickNumber; // 現在タップされている値
    private int numberMax;
    private int score;
    private GameObject scoreText;
    private GameObject timeText;
    private int time;
    private int maxTime; // 1ゲーム何秒か
    private float leftTime; // 前回タイムが減ってからの経過時間

    private int syokitiue; // ボタン生成時の初期値の上限
    private int syokitisita; // 初期値の下限

    private int coin; // 今回得たコイン
    private GameObject coinObj;

    private int nokori; // 残り時間

    private void Awake()
    {
        // 他のstartより前に呼び出したい物
        // ボタンの初期値とか

        score = 0;

        leftTime = 0; // 前回タイムが減ってからの経過時間
        maxTime = 30;
        time = 0; // 今のタイム
        nokori = maxTime; // 残りタイム

        necessaryTap = 2;

        clickNumber = 0;

        coin = 0;
        // コインの表示テキストのゲームオブジェクト
        coinObj = GameObject.FindGameObjectWithTag("Coin");

        numberMax = PlayerPrefs.GetInt("maxNumber", 50); // 数値の最大値
        syokitiue = PlayerPrefs.GetInt("syokitiue", 1); // ボタン生成時の初期値の上限
        syokitisita = PlayerPrefs.GetInt("syokitisita", 1); // 初期値の下限

        // スコアとタイムのテキストオブジェクトを取ってくる
        scoreText = GameObject.FindGameObjectWithTag("Score");
        timeText = GameObject.FindGameObjectWithTag("Time");
        // 文字を初期化
        scoreText.GetComponent<Text>().text = " score:" + score.ToString();
        timeText.GetComponent<Text>().text = " time:" + nokori.ToString();
        coinObj.GetComponent<Text>().text = " coin:" + coin.ToString();
    }

    // Use this for initialization
    void Start()
    {


        // 画面にボタンを並べつつそのボタンをbuttonArrayに突っ込む

        // 並べるplayerを取ってくる
        //player = GameObject.FindGameObjectWithTag("canvas"); //canvasタグが2個あったら詰む書き方
        player = GameObject.FindGameObjectWithTag("Player");
        buttonArray = new GameObject[yoko, tate];

        for (int i = 0; i < yoko; i++)
        {
            for (int j = 0; j < tate; j++)
            {
                buttonArray[i, j] = CreateButton();

            }
        }

        tapButton = new GameObject[necessaryTap];

        //Debug.Log(hidarihasi);
        //Debug.Log(board.x);

    }

    // Update is called once per frame
    void Update()
    {
        /*
        // タッチされているかチェック(スマホ)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {

                //TapTouch(touch.position.x, touch.position.y);
                TapTouch(touch.position);
            }

        }

        // マウスを離した時
        if (Input.GetMouseButtonUp(0))
        {
            //TapTouch(Input.mousePosition.x, Input.mousePosition.y);
            TapTouch(Input.mousePosition);

        }
        */

        timeCheck();

    }

    GameObject CreateButton()
    {
        // ボタンを作る

        GameObject ret = Instantiate(prefabButton);

        ret.transform.SetParent(player.transform, false); // playerを親にする

        return ret;
    }

    //void TapTouch(float x, float y)
    private void TapTouch(Vector3 pos)
    {
        // そういえばこれcolliderアタッチしてないと動かないよな……。
        // そしてボタンだからonclickで良いというオチ(=これは使う必要性が無いので使っておりません)

        Vector2 wpos = Camera.main.ScreenToWorldPoint(pos);
        RaycastHit2D hit = Physics2D.Raycast(wpos, Vector2.zero);

        if (hit.collider != null)
        {
            Debug.Log(hit.collider.name);
        }

        return;

    }

    public void clickAfter(GameObject clickButton)
    {
        // クリック処理、基本的にボタンプレハブのonclickから呼び出される
        //Debug.Log("クリック");


        // クリックした奴と今までクリックした奴を比較していく
        // 同じ物をクリックした事があると初期化
        for (int i = 0; i < tapButton.Length; i++)
        {
            if (tapButton[i] == clickButton)
            {
                // 同じ物をクリックした場合、初期化
                //tapButton = new GameObject[necessaryTap];
                tapButtonSyokika();
                tapButton[0] = clickButton;
                tapButtontennmetu(clickButton);
                // そういえばclickNumberは同じの押したら変わらないな……

                break;
            }

            // 最後まで来た時
            if (tapButton[i] == null)
            {

                if (i == 0)
                {
                    // 1個目の時ナンバーをセットする
                    clickNumber = clickButton.GetComponent<buttonTap>().getNumber();
                    tapButton[i] = clickButton;
                    tapButtontennmetu(clickButton);

                }
                else
                {
                    // 2個目以降の時

                    if (clickNumber != clickButton.GetComponent<buttonTap>().getNumber())
                    {
                        // 別のナンバーである場合初期化

                        //tapButton = new GameObject[necessaryTap];
                        tapButtonSyokika();
                        tapButton[0] = clickButton;
                        clickNumber = clickButton.GetComponent<buttonTap>().getNumber();
                        tapButtontennmetu(clickButton);

                    }
                    else
                    {

                        // 同じナンバーの場合でオブジェクトがどれとも被っていない時追加
                        // ここまで来てるって事は同じのが無かったという事
                        tapButton[i] = clickButton;
                        tapButtontennmetu(clickButton);

                    }
                }

                break;
            }
        }

        // 必要数タップしてたらここを起動
        if (tapButton[tapButton.Length - 1] != null)
        {
            for (int i = 0; i < tapButton.Length; i++)
            {


                if (i == tapButton.Length - 1)
                {
                    // 最後に押した奴はナンバーを上げる
                    tapButton[i].GetComponent<buttonTap>().numberUp();
                }
                else
                {
                    // 最後でない奴は初期化
                    tapButton[i].GetComponent<buttonTap>().numberStart();
                }
            }
            //tapButton = new GameObject[necessaryTap];
            tapButtonSyokika();
            clickNumber = 0;
        }

        return;
    }

    private void tapButtonSyokika()
    {
        // 点滅処理の為に生えてきた初期化メソッド
        // 点滅を消す

        for (int i = 0; i < tapButton.Length; i++)
        {
            if (tapButton[i] == null)
            {
                // 何もない時
                break;

            } else
            {
                // 何か入ってる時
                tapButton[i].GetComponent<buttonTap>().settennmetu(0); // 点滅を消す

            }
        }


        tapButton = new GameObject[necessaryTap];

        return;
    }

    private void tapButtontennmetu(GameObject tapB)
    {
        // 渡されたボタンを点滅させる
        // 点滅処理の為に生えてきたメソッド
        tapB.GetComponent<buttonTap>().settennmetu(1); // 点滅させる


        return;
    }

    public void scorePlus(int plus)
    {
        // オーバーフロー処理
        // ……する事は想定してない
        try
        {
            checked
            {
                score += plus;
            }
        }

        catch (OverflowException)
        {
            score = 2147483647;
        }

        scoreText.GetComponent<Text>().text = " score:" + score.ToString();

        return;

    }

    private void timeCheck()
    {
        float interval = 1;
        leftTime += Time.deltaTime;
        if (leftTime >= interval)
        {
            time++;
            leftTime--;
            nokori = maxTime - time;
            timeText.GetComponent<Text>().text = " time:" + nokori.ToString();
        }

        if (nokori < 0)
        {
            gameOver();
        }

        return;
    }

    private void gameOver()
    {
        int maxscore = PlayerPrefs.GetInt("maxScore", 0);
        if (score > PlayerPrefs.GetInt("maxScore", 0))
        {
            PlayerPrefs.SetInt("oldScore", maxscore); // 前回までのmax

            PlayerPrefs.SetInt("maxScore", score); // 最高スコア更新
        }
        PlayerPrefs.SetInt("score", score); // 今回の得点も保存


        int coinmoto = PlayerPrefs.GetInt("Coin", 0);
        PlayerPrefs.SetInt("Coin", coin + coinmoto);
        PlayerPrefs.SetInt("newCoin", coin); // 今回のコイン

        //SceneManager.LoadScene("start");
        SceneManager.LoadScene("tokutenn");
    }

    public int MaxNumber()
    {
        return numberMax;
    }

    public int syokitiUe()
    {
        return syokitiue;
    }

    public int syokitiSita()
    {
        return syokitisita;
    }

    public void coinUp(int Plus)
    {

        //coin = 2147483647; // デバッグ用

        // オーバーフロー処理
        // ……する事は想定してない
        try
        {
            checked
            {
                coin += Plus;
            }
        }

        catch (OverflowException)
        {
            coin = 2147483647;
        }

        coinObj.GetComponent<Text>().text = " coin:" + coin.ToString();

        return;
    }


    public void timeUp()
    {
        maxTime++;
        nokori = maxTime - time;
        timeText.GetComponent<Text>().text = " time:" + nokori.ToString();
        //Debug.Log(maxTime);
        return;
    }

    public void gomi()
    {
        // ゴミ箱押したら一番最後に押した奴初期化するよ
        // タップ状況も初期化するよ

        for (int i = tapButton.Length - 1; i >= 0; i--)
        {
            if (tapButton[i] != null)
            {
                tapButton[i].GetComponent<buttonTap>().numberStart();
                break;
            }

        }

        //tapButton = new GameObject[necessaryTap];
        tapButtonSyokika();
        clickNumber = 0;

        return;

    }
}
