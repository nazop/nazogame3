using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class buttonTap : MonoBehaviour {

    private GameObject gamesystem; // ゲームシステムオブジェクトのこと
    private start startCS; // Main関数的な動きしてる奴を入れる
    private int number; // このボタンの値
    private GameObject childText; // このボタンの値の表示GUI

    // 0:通常 1:縦横の数値増加 2:タイム増加 3:コイン増加
    // 現在:緑、赤、白、黄
    // やっぱ1止めて2と3で緑赤黄
    private int bomb;
    private int bombsyurui = 2;
    public Material[] bombMaterial; // インスペクターでボムの色を設定

    private int tennmetu; // 1なら点滅
    private IEnumerator Coroutine; // 点滅コルーチン

    private Color normalcol;

    // Use this for initialization
    void Start() {
        // gamesystemタグのゲームオブジェクトを探索
        gamesystem = GameObject.FindGameObjectWithTag("gamesystem");
        startCS = gamesystem.GetComponent<start>();

        // なぜか親子関係はtransformオブジェクトが担ってるんだよなぁ
        childText = this.gameObject.transform.Find("Text").gameObject;
        //number = 2;

        bomb = 0;

        normalcol = this.gameObject.GetComponent<Button>().colors.normalColor;
        Coroutine = ColorCoroutine();

        numberStart();

    }

    // Update is called once per frame
    void Update() {



    }

    /// ボタンをクリックした時の処理
    public void OnClick()
    {
        // ボタンプレハブにこれをアタッチしてonclickの+を押してボタンプレハブを選択、このスクリプトを選択する
        // そうしないと使えません!


        // gamesystemタグのゲームオブジェクトにアタッチされているstartスクリプトのclickafterメソッドを呼び出している
        // このゲームオブジェクトを相手に投げている
        startCS.clickAfter(this.gameObject);
    }

    public void numberUp()
    {
        // 数値が増える時これが呼ばれる

        startCS.scorePlus(number);
        startCS.coinUp(number);

        bombCheck();
        bombset();

        number++;
        if(number > startCS.MaxNumber())
        {
            //Debug.Log(startCS.MaxNumber());
            number--;
        }

        childText.GetComponent<Text>().text = number.ToString();





    }

    public void numberStart()
    {
        // 初期値設定
 
        bombCheck();

        bombset(); // ボムをセット

        number = (int)Random.Range(startCS.syokitiSita(), startCS.syokitiUe() + 1);

        childText.GetComponent<Text>().text = number.ToString();

        return;
    }

    public void bombCheck()
    {
        if (bomb == 1)
        {
            // タイム増加
            startCS.timeUp();
        }
        else if (bomb == 2)
        {
            // コイン増加
            startCS.coinUp(number);
        }

    }

    private void bombset()
    {
        // ボムかどうかをセット

        bomb = 0;

        // 2割の確率でボム
        if ((int)Random.Range(0, 100) >= 80)
        {
            bomb = (int)Random.Range(1, bombsyurui + 1);
            //Debug.Log(this.gameObject + " " + this);
            //Debug.Log(bomb);

        }

        this.gameObject.GetComponent<Image>().material = bombMaterial[bomb];
        //this.gameObject.GetComponent<Renderer>().material = bombMaterial[bomb];
    }

    public int getNumber()
    {
        return number;
    }

    private IEnumerator ColorCoroutine()
    {
        // 起動している間色を点滅させるコルーチン

        while (true)
        {
            yield return new WaitForSeconds(0.2f);

 

            // ボタンで設定してる色の中で色変更しようぜ企画が採用

            ColorBlock _color = this.gameObject.GetComponent<Button>().colors;

            if (_color.normalColor == _color.highlightedColor)
            {
                _color.normalColor = normalcol;


            } else
            {
                _color.normalColor = _color.highlightedColor;
            }

            this.gameObject.GetComponent<Button>().colors = _color;

        }
    }

    public void settennmetu(int flag)
    {
        // 0だと点滅しない、1だと点滅する
        // 現状わざわざ変数に入れて記録しなくても良いんだけども

        tennmetu = flag;

        if(tennmetu == 0)
        {
            StopCoroutine(Coroutine);

            // 色元に戻す

            // this.gameObject.GetComponent<Button>().colors.normalColorには代入出来ない
            ColorBlock _color = this.gameObject.GetComponent<Button>().colors;
            // でも取り出すとこう出来る、何だその仕様
            _color.normalColor = normalcol;
            // ただし取り出した物は元のオブジェクトと切り離された別個の存在、複製品
            // よって代入しても元のは変わらない
            // だからもう一度取って来て代入し直さないといけない
            this.gameObject.GetComponent<Button>().colors = _color;

        } else if(tennmetu == 1)
        {
            StartCoroutine(Coroutine);
        }

        return;
    }

}
