using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    //‚Ü‚Á‚·‚®i‚Þ’e
    public class StraightBulletManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        private Vector3 localVelocity;

        private float speed;
        private float limitTime;
        private float timer;

        private void Start()
        {
            SePlayer.Instance.Play(6);
        }

        // Update is called once per frame
        void Update()
        {
            localVelocity = transform.TransformDirection(Vector3.right);

            timer += Time.deltaTime;
            if(timer > limitTime)
            {
                Destroy(this.gameObject);
            }
        }

        private void FixedUpdate()
        {
            rb.velocity = localVelocity * speed;
        }

        public void SetData(float s,float t)
        {
            speed = s;
            limitTime = t;
            timer = 0;
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
