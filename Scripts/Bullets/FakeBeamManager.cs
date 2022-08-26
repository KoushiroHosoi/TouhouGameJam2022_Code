using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using DG.Tweening;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    //偽の位置に発射するビーム（正邪用）
    public class FakeBeamManager : MonoBehaviour
    {
        [SerializeField] SpriteRenderer sprite;

        private bool canDamage;

        private Vector2 beamSize;
        //予備のビームを見せる時間
        private float preparationTime;
        //ビームを出す時間
        private float shotTime;
        private float createPosY;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<ICollisionEnemyAttack>(out var player) && canDamage)
            {
                player.CollisionEnemyAttack();
            }
        }

        //必要な時間を設定させ.コルーチン開始させる
        public void SetData(float cPosY, Vector2 bs, float pt, float st)
        {
            createPosY = cPosY;
            beamSize = bs;
            preparationTime = pt;
            shotTime = st;
            canDamage = false;
            this.transform.localScale = new Vector3(beamSize.x, beamSize.y, 0);
            StartCoroutine(StartShootingBeam());
        }

        public IEnumerator StartShootingBeam()
        {
            //偽の発射位置をプレイヤーに見せる
            //発射する場所の上方向に移動する
            this.transform.position = new Vector3(0, createPosY + 7.5f, 0);

            //半透明にする
            Color defaltColor = sprite.color;

            Color newColor;
            newColor = Color.gray;
            newColor.a = 0.5f;
            sprite.color = newColor;

            //一旦消す
            SePlayer.Instance.Play(8);
            yield return this.transform.DOScale(new Vector3(beamSize.x, createPosY + 7.5f, 0), preparationTime).SetLink(gameObject).WaitForCompletion();
            this.transform.localScale = new Vector3(0, beamSize.y, 0);

            //透明化を解除する
            sprite.color = defaltColor;
            canDamage = true;

            //発射する
            SePlayer.Instance.Play(9);
            //ここで位置を発射位置に戻す
            this.gameObject.transform.position = new Vector3(0, createPosY, 0);
            this.transform.DOMoveX(beamSize.x / 3 * -1, shotTime).SetLink(gameObject);
            yield return this.transform.DOScale(new Vector3(beamSize.x, beamSize.y, 0), shotTime * (2f / 3f)).SetLink(gameObject).WaitForCompletion();

            //消す
            yield return this.transform.DOScale(new Vector3(beamSize.x, 0, 0), shotTime * (1f / 3f)).SetLink(gameObject).WaitForCompletion();

            Destroy(this.gameObject);

            yield break;
        }
    }
}
