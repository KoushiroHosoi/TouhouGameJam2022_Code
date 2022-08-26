using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    public class HuranManager : EnemyBase
    {
        [SerializeField] private RefractiveBulletManager refBulletPrefab;
        [SerializeField] private StraightBulletManager strBulletPrefab;
        [SerializeField] private HorizontalBeamManager beamPrefab;
        
        public event Action onDeadCharacter;

        private void Awake()
        {
            BaseStart();
        }

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(ShotBullet());
        }

        // Update is called once per frame
        void Update()
        {
            UpDownMove();
        }

        private IEnumerator ShotBullet()
        {
            yield return new WaitForSeconds(3f);

            //一回曲がる球をプレイヤーに向かって打つ
            for (int i = 0; i < 4; i++)
            {
                Vector3 playerVec = (this.transform.position - player.transform.position) * -1;
                float rot = Mathf.Atan2(playerVec.y, playerVec.x) * Mathf.Rad2Deg;
                RefractiveBulletManager bulletObject = Instantiate(refBulletPrefab, this.transform.position, Quaternion.Euler(0, 0, rot));
                bulletObject.SetData(8, 5);

                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(2.5f);

            //プレイヤーに向かって速い球と遅い球を扇状に打つ
            for (int i = 0; i < 7; i++)
            {
                int speed = 0;
                float rotSize = 0;
                if (i % 2 == 0)
                {
                    speed = 7;
                    rotSize = 12.5f;
                }
                else
                {
                    speed = 5;
                    rotSize = 17.5f;
                }

                for (int k = -2; k < 3; k++)
                {
                    Vector3 playerVec = (this.transform.position - player.transform.position) * -1;
                    float rot = Mathf.Atan2(playerVec.y, playerVec.x) * Mathf.Rad2Deg;
                    StraightBulletManager bulletObject = Instantiate(strBulletPrefab, this.transform.position, Quaternion.Euler(0, 0, rot + rotSize * k));
                    bulletObject.SetData(speed, 10);
                }

                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(2.5f);

            //ビームを発射
            Vector3 createPos = new Vector3(0, player.transform.position.y, 0);
            HorizontalBeamManager beam = Instantiate(beamPrefab, createPos, Quaternion.identity);
            beam.SetData(new Vector2(64, 2), 1, 1.4f);

            yield return new WaitForSeconds(2.5f);

            onDeadCharacter();
            Destroy(this.gameObject);
        }
    }
}
