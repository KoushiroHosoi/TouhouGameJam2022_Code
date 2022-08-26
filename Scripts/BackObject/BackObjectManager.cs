using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //背景のオブジェクトにつけるクラス
    public class BackObjectManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float speed;

        // Update is called once per frame
        void FixedUpdate()
        {
            //横に動かす
            rb.velocity = new Vector2(-1, 0) * speed;
        }
    }
}
