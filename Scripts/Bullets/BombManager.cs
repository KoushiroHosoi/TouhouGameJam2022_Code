using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using DG.Tweening;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    //上から落ちてきて爆発する弾
    public class BombManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private SpriteRenderer sprite;
        //爆発する時間
        private float explosionTime;
        private float timer;
        //爆発範囲
        private float explosionRange;
        //衝突の管理
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

        //地面と接触するまで下に動かす
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
            //爆発中に当たったらダメージ
            if (canDamage && collision.TryGetComponent<ICollisionEnemyAttack>(out var player))
            {
                player.CollisionEnemyAttack();
            }

            //地面に当たったら爆発開始
            if (!isGroundCollision && collision.TryGetComponent<GroundManager>(out var ground))
            {
                isGroundCollision = true;
                StartCoroutine(StartExplosion());
            }
        }

        //爆発時間・範囲を設定する
        public void SetData(float eTime,float eRange)
        {
            explosionTime = eTime;
            explosionRange = eRange;
            isGroundCollision = false;
            canDamage = false;
        }

        private IEnumerator StartExplosion()
        {
            //小さくする & 変色させる
            Color defaltColor = sprite.color;

            Color newColor;
            newColor = Color.gray;
            sprite.color = newColor;
            yield return this.transform.DOScale(0, 1f).SetLink(gameObject).WaitForCompletion();

            //色を元に戻して拡大する
            SePlayer.Instance.Play(13);
            canDamage = true;
            sprite.color = defaltColor;
            yield return this.transform.DOScale(explosionRange, explosionTime).SetLink(gameObject).WaitForCompletion();

            Destroy(this.gameObject);
            yield break;
        }
    }
}
