using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    //一定時間を過ぎるとプレイヤーのいる方向に向きを変える弾
    public class RefractiveBulletManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        private Vector3 localVelocity;

        private float speed;
        private float limitTime;
        private float timer;

        private PlayerManager player;

        private bool canChangeDirection;

        // Start is called before the first frame update
        void Start()
        {
            SePlayer.Instance.Play(6);
        }

        // Update is called once per frame
        void Update()
        {
            localVelocity = transform.TransformDirection(Vector3.right);

            timer += Time.deltaTime;

            //弾の生存する半分の時間になったらプレイヤーの方向に向く
            if(canChangeDirection && timer >= (limitTime/2))
            {
                canChangeDirection = false;
                Vector3 playerVec = (this.transform.position - player.transform.position);
                //ベクトルを角度に直す
                float rot = Mathf.Atan2(playerVec.y, playerVec.x) * Mathf.Rad2Deg;
                this.gameObject.transform.Rotate(0, 0, rot);
                speed *= 1.5f;
            }


            if (timer > limitTime)
            {
                Destroy(this.gameObject);
            }
        }

        private void FixedUpdate()
        {
            rb.velocity = localVelocity * speed;
        }

        public void SetData(float s, float t)
        {
            speed = s;
            limitTime = t;

            timer = 0;
            player = GameObject.FindObjectOfType<PlayerManager>();
            canChangeDirection = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<ICollisionEnemyAttack>(out var player))
            {
                player.CollisionEnemyAttack();
                Destroy(this.gameObject);
            }
        }
    }
}
