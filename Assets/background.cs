using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    // 背景を横スクロールさせる物
    // spriteにつけて使う
    // 背景画像を二つ横にぴったりくっつけて並べて二つともにつける

    private Vector3 startpos;
    private float speed;
    private float haba;
    private float count;

    // Start is called before the first frame update
    void Start()
    {
        startpos = this.gameObject.transform.position;
        speed = -0.01f;
        haba = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        count = 0;

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(speed, 0, 0);
        count += Mathf.Abs(speed);
        if (count > haba)
        {
            transform.position = startpos;
            count = 0;
        }

    }
}
