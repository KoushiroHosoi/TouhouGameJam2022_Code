using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    //�v���C���[��ǔ�����e
    public class FollowBulletManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        private Vector3 localVelocity;

        private float timer;
        //��0�ɂȂ��������������
        private float limitTime;
        private float speed;

        private PlayerManager player;
        private Vector3 playerVec;
        //�Ǐ]�\���ǂ���
        private bool canFollowing;

        private void Awake()
        {
            player = GameObject.FindObjectOfType<PlayerManager>();
        }

        private void Start()
        {
            SePlayer.Instance.Play(6);
        }


        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;

            //�Ǐ]������
            if (canFollowing)
            {
                playerVec = (this.transform.position - player.transform.position) * -1;
            }

            //��v�̋����ɂȂ�����Ǐ]����߂�����
            if(canFollowing && Vector3.Distance(player.transform.position, this.transform.position) < 5f)
            {
                canFollowing = false;
            }

            if (timer > limitTime)
            {
                Destroy(this.gameObject);
            }
        }

        private void FixedUpdate()
        {
            rb.velocity = playerVec.normalized * speed;
        }

        //�ړ��X�s�[�h�Ƒ�����ݒ肷��
        public void SetData(float s,float lt)
        {
            speed = s;
            limitTime = lt;

            canFollowing = true;
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
