using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.Monetization;

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

    private string _gameId = "3375725";
    private bool testMode = false; // true=test

    public GameObject koukoku_ok; // インスペクターから入れた方が画面使いやすい
    public GameObject koukoku_no; // インスペクターから入れた方が画面使いやすい

    public GameObject adbutton; // インスペクターから入れた方が画面使いやすい

    private void Awake()
    {

        if (Application.platform == RuntimePlatform.Android)
        {
            adbutton.SetActive(true);
        }
    }

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

        // 広告の設定
        Monetization.Initialize(_gameId, testMode);

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

    public void ad()
    {
        if(suta.sutaminamax() == false)
        {
            addia(0);
            return;
        }

        adkakuninn();
        return;

    }

    //広告結果イベント
    private void HandleShowResult(UnityEngine.Monetization.ShowResult result)
    //private void HandleShowResult(UnityEngine.Advertisements.ShowResult result)
    {
        switch (result)
        {
            //最後まで終了
            case UnityEngine.Monetization.ShowResult.Finished:
                suta.sutaminaplus();
                addia(1);
                break;
            //途中スキップ
            case UnityEngine.Monetization.ShowResult.Skipped:
                addia(2);
                break;
            //表示失敗
            case UnityEngine.Monetization.ShowResult.Failed:
                addia(3);
                break;
        }
    }

    private void addia(int syurui)
    {
        if(syurui == 1)
        {
            errorText.text =
            "スタミナが\n" +
            "1回復しました";

        } else if(syurui == 2)
        {
            errorText.text =
            "広告が\n" +
            "途中終了しました";
        } else if(syurui == 3)
        {
            errorText.text =
            "表示失敗";
        } else if (syurui == 0)
        {
            errorText.text =
            "プレイ数上限値です";
        }


        errordialog.SetActive(true);

        Invoke("errorDelay", 1.0f);
        return;
    }

    private void adkakuninn()
    {
        errorText.text =
        "本当に\n" +
        "広告を見ますか?";

        errordialog.SetActive(true);

        koukoku_ok.SetActive(true);
        koukoku_no.SetActive(true);

        return;
    }

    public void koukoku_ok_click()
    {
        koukoku_no_click();
        adstart();
    }

    public void koukoku_no_click()
    {
        errordialog.SetActive(false);

        koukoku_ok.SetActive(false);
        koukoku_no.SetActive(false);
        return;
    }

    private void adstart()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            // ビデオ見せる

            // 失敗の数々、結局備え付けのadは古いとかで使えなくてアセットダウンロードした
            //ShowAdCallbacks options = new ShowAdCallbacks();
            //ShowOptions options = new ShowOptions();
            //options.finishCallback = HandleShowResult;
            //options.resultCallback = HandleShowResult;
            //ShowAdPlacementContent pc = Monetization.GetPlacementContent("rewardedVideo");
            ShowAdPlacementContent pc = Monetization.GetPlacementContent("rewardedVideo") as ShowAdPlacementContent;
            //Advertisement.Show("rewardedVideo", options);
            if (pc != null)
            {
                pc.Show(HandleShowResult);
            }


        }
    }

}
