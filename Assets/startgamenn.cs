using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class startgamenn : MonoBehaviour {

    private int score;
    private GameObject scoreText;

    private int coin;
    private GameObject coinText;

    // dialog用
    //private GameObject dialog; // onにしといてね
    // インスペクターから入れた方が画面使いやすいのでpublicに修正
    // transform.Findは親オブジェクトから消してるから駄目
    // もう一つ親として何も入ってないゲームオブジェクト作れば出来そうだけどまぁいいか
    public GameObject dialog; 

    //private GameObject canvas;

    // 遊び方画面用
    public GameObject asobikata; // インスペクターから入れる
    private bool asobikata_active; // 現在の状態

    private sutamina suta; // スタミナ処理用

    public GameObject errordialog; // インスペクターから入れた方が画面使いやすい
    private Text errorText; // Textコンポーネントなので.text="エラーテキスト"で中身変更

    // Use this for initialization
    void Start () {

        // スコアのテキストオブジェクトを取ってくる
        scoreText = GameObject.FindGameObjectWithTag("Score");

        score = PlayerPrefs.GetInt("maxScore", 0); // セーブされた値、セーブが無い時は0
        // 文字を初期化
        scoreText.GetComponent<Text>().text = " MaxScore:" + score.ToString();

        // コインのテキストを書いてるオブジェクトを取って来る
        coinText = GameObject.FindGameObjectWithTag("Coin");

        coinChange();

        /*
        dialog = GameObject.FindGameObjectWithTag("Dialog");
        dialog.SetActive(false);
        */

        //canvas = GameObject.FindGameObjectWithTag("canvas");

        //asobikata_active = false;

        suta = this.gameObject.GetComponent<sutamina>(); // スタミナ処理用

        // エラー処理用
        errorText = errordialog.transform.Find("Text").gameObject.GetComponent<Text>();


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClick()
    {
        
        if (suta.sutaminadec() == true)
        {
            SceneManager.LoadScene("index");

        }
        else
        {
            errordia();
            return;
        }
    }

    public void onClick_kyouka()
    {

        dialog.SetActive(true);

    }

    public void coinChange()
    {
        coin = PlayerPrefs.GetInt("Coin", 0); // セーブされた値、セーブが無い時は0
        // 文字を修正
        coinText.GetComponent<Text>().text = " Coin:" + coin.ToString();
        return;
    }

    public void onClick_asobikata()
    {
        asobikata_active = !asobikata_active;
        asobikata.SetActive(asobikata_active);
    }

    private void errordia()
    {
        errorText.text =
        "スタミナが\n" +
        "不足しています";

        errordialog.SetActive(true);

        Invoke("errorDelay", 1.0f);
        return;
    }

    private void errorDelay()
    {
        errordialog.SetActive(false);
        return;
    }
    
}
