using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using DG.Tweening;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    //�U�̈ʒu�ɔ��˂���r�[���i���חp�j
    public class FakeBeamManager : MonoBehaviour
    {
        [SerializeField] SpriteRenderer sprite;

        private bool canDamage;

        private Vector2 beamSize;
        //�\���̃r�[���������鎞��
        private float preparationTime;
        //�r�[�����o������
        private float shotTime;
        private float createPosY;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<ICollisionEnemyAttack>(out var player) && canDamage)
            {
                player.CollisionEnemyAttack();
            }
        }

        //�K�v�Ȏ��Ԃ�ݒ肳��.�R���[�`���J�n������
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
            //�U�̔��ˈʒu���v���C���[�Ɍ�����
            //���˂���ꏊ�̏�����Ɉړ�����
            this.transform.position = new Vector3(0, createPosY + 7.5f, 0);

            //�������ɂ���
            Color defaltColor = sprite.color;

            Color newColor;
            newColor = Color.gray;
            newColor.a = 0.5f;
            sprite.color = newColor;

            //��U����
            SePlayer.Instance.Play(8);
            yield return this.transform.DOScale(new Vector3(beamSize.x, createPosY + 7.5f, 0), preparationTime).SetLink(gameObject).WaitForCompletion();
            this.transform.localScale = new Vector3(0, beamSize.y, 0);

            //����������������
            sprite.color = defaltColor;
            canDamage = true;

            //���˂���
            SePlayer.Instance.Play(9);
            //�����ňʒu�𔭎ˈʒu�ɖ߂�
            this.gameObject.transform.position = new Vector3(0, createPosY, 0);
            this.transform.DOMoveX(beamSize.x / 3 * -1, shotTime).SetLink(gameObject);
            yield return this.transform.DOScale(new Vector3(beamSize.x, beamSize.y, 0), shotTime * (2f / 3f)).SetLink(gameObject).WaitForCompletion();

            //����
            yield return this.transform.DOScale(new Vector3(beamSize.x, 0, 0), shotTime * (1f / 3f)).SetLink(gameObject).WaitForCompletion();

            Destroy(this.gameObject);

            yield break;
        }
    }
}
