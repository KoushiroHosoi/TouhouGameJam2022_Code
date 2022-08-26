using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using DG.Tweening;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    //�ォ�痎���Ă��Ĕ�������e
    public class BombManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private SpriteRenderer sprite;
        //�������鎞��
        private float explosionTime;
        private float timer;
        //�����͈�
        private float explosionRange;
        //�Փ˂̊Ǘ�
        private bool isGroundCollision;
        private bool canDamage;

        // Start is called before the first frame update
        void Awake()
        {
            timer = 0;
        }


        // Update is called once per frame
        void Update()
        {
            timer += Time.deltaTime;
        }

        //�n�ʂƐڐG����܂ŉ��ɓ�����
        private void FixedUpdate()
        {
            if (!isGroundCollision)
            {
                rb.velocity = new Vector2(0, -2) * timer * 3f;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //�������ɓ���������_���[�W
            if (canDamage && collision.TryGetComponent<ICollisionEnemyAttack>(out var player))
            {
                player.CollisionEnemyAttack();
            }

            //�n�ʂɓ��������甚���J�n
            if (!isGroundCollision && collision.TryGetComponent<GroundManager>(out var ground))
            {
                isGroundCollision = true;
                StartCoroutine(StartExplosion());
            }
        }

        //�������ԁE�͈͂�ݒ肷��
        public void SetData(float eTime,float eRange)
        {
            explosionTime = eTime;
            explosionRange = eRange;
            isGroundCollision = false;
            canDamage = false;
        }

        private IEnumerator StartExplosion()
        {
            //���������� & �ϐF������
            Color defaltColor = sprite.color;

            Color newColor;
            newColor = Color.gray;
            sprite.color = newColor;
            yield return this.transform.DOScale(0, 1f).SetLink(gameObject).WaitForCompletion();

            //�F�����ɖ߂��Ċg�傷��
            SePlayer.Instance.Play(13);
            canDamage = true;
            sprite.color = defaltColor;
            yield return this.transform.DOScale(explosionRange, explosionTime).SetLink(gameObject).WaitForCompletion();

            Destroy(this.gameObject);
            yield break;
        }
    }
}
