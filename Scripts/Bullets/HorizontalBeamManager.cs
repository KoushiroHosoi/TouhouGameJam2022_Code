using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using DG.Tweening;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    //真横にビームを出す
    public class HorizontalBeamManager : MonoBehaviour
    {
        [SerializeField] SpriteRenderer sprite;

        private bool isCollision;

        private Vector2 beamSize;
        //発射の準備時間
        private float preparationTime;
        //発射している時間
        private float shotTime;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<ICollisionEnemyAttack>(out var player) && isCollision)
            {
                player.CollisionEnemyAttack();
            }
        }

        public void SetData(Vector2 bs, float pt,float st)
        {
            beamSize = bs;
            preparationTime = pt;
            shotTime = st;
            this.transform.localScale = new Vector3(beamSize.x, beamSize.y, 0);
            isCollision = false;
            StartCoroutine(StartShootingBeam());
        }

        public IEnumerator StartShootingBeam()
        {
            //半透明にする
            Color nowColor = sprite.color;
            nowColor.a = 0.5f;
            sprite.color = nowColor;

            //一旦消す
            SePlayer.Instance.Play(8);
            yield return this.transform.DOScale(new Vector3(beamSize.x, 0, 0), preparationTime).SetLink(gameObject).WaitForCompletion();
            this.transform.localScale = new Vector3(0, beamSize.y, 0);

            //透明化を解除する
            nowColor.a = 1;
            sprite.color = nowColor;
            isCollision = true;

            //発射する
            SePlayer.Instance.Play(9);
            this.transform.DOMoveX(beamSize.x / 3 * -1, shotTime).SetLink(gameObject);
            yield return this.transform.DOScale(new Vector3(beamSize.x, beamSize.y, 0), shotTime * (2f / 3f)).SetLink(gameObject).WaitForCompletion();

            //消す
            yield return this.transform.DOScale(new Vector3(beamSize.x, 0, 0), shotTime * (1f / 3f)).SetLink(gameObject).WaitForCompletion();

            Destroy(this.gameObject);

            yield break;
        }
    }
}
