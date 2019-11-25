using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class end : MonoBehaviour {

    private int score;
    private GameObject scoreText;
    private int maxscore;

    private int newcoin; // 今回得たコイン
    private GameObject coinObj;
    private int coin; // 合計コイン

    // Use this for initialization
    private void Start () {

        // スコアのテキストオブジェクトを取ってくる
        scoreText = GameObject.FindGameObjectWithTag("Score");

        // 前回までの最高得点と今回の得点を取って来る       
        maxscore = PlayerPrefs.GetInt("maxScore", 0);

        score = PlayerPrefs.GetInt("score", 0); // セーブされた値、orセーブが無い時は0
        //oldscore = PlayerPrefs.GetInt("oldScore", 0); // セーブされた値、orセーブが無い時は0

        if (maxscore == score)
        {
            // 最高得点が今回更新されていた場合ここに来る
            // 一個前の最高得点を取って来る
            maxscore = PlayerPrefs.GetInt("oldScore", 0); // セーブされた値、orセーブが無い時は0        }

        }
        // 文字を初期化
        scoreText.GetComponent<Text>().text = 
            " MaxScore:" + maxscore.ToString() + "\n" +
            " Score:" + score.ToString();

        // コイン表示
        newcoin = PlayerPrefs.GetInt("newCoin", 0);
        coin = PlayerPrefs.GetInt("Coin", 0);
        // コインの表示テキストのゲームオブジェクト
        coinObj = GameObject.FindGameObjectWithTag("Coin");

        coinObj.GetComponent<Text>().text =
            " new coin:" + newcoin.ToString() + "\n" +
            " coin:" + coin.ToString();

        // 直接この画面に飛ぶと前回の得点のままなんだけどまぁ通常そんなことないからね……

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void onClick()
    {
        SceneManager.LoadScene("start");

    }
}
