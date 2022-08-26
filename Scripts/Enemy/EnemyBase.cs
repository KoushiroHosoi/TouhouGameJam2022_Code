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

        //�q�I�u�W�F�N�g���������ꂽ�Ƃ��ɂ��
        protected void BaseStart()
        {
            FindPlayer();
            StayBeforePlayerPos();
            timer = 0;
            startPos = this.transform.position;
        }

        //�㉺�Ɉړ�����
        protected void UpDownMove()
        {
            timer += Time.deltaTime;
            this.gameObject.transform.position = new Vector3(startPos.x, startPos.y * Mathf.Sin(timer), 0);
        }

        //Player��������
        protected void FindPlayer()
        {
            player = FindObjectOfType<PlayerManager>();
        }

        //Player�̑O���ɃL�����N�^�[��u��
        protected void StayBeforePlayerPos()
        {
            this.gameObject.transform.position = offsetPos;
        }
    }
}
