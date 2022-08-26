using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;

namespace HamuGame
{
    public class EnemyBase : MonoBehaviour
    {
        [SerializeField] private Vector3 offsetPos;
        protected PlayerManager player;

        protected float timer;
        protected Vector3 startPos;

        //子オブジェクトが生成されたときにやる
        protected void BaseStart()
        {
            FindPlayer();
            StayBeforePlayerPos();
            timer = 0;
            startPos = this.transform.position;
        }

        //上下に移動する
        protected void UpDownMove()
        {
            timer += Time.deltaTime;
            this.gameObject.transform.position = new Vector3(startPos.x, startPos.y * Mathf.Sin(timer), 0);
        }

        //Playerを見つける
        protected void FindPlayer()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        //Playerの前方にキャラクターを置く
        protected void StayBeforePlayerPos()
        {
            this.gameObject.transform.position = offsetPos;
        }
    }
}
