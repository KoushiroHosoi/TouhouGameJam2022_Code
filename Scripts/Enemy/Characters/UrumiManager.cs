using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;

namespace HamuGame
{
    public class UrumiManager : EnemyBase
    {
        [SerializeField] private StraightBulletManager straightBulletPrefab;
        [SerializeField] private BombManager bombPrefab;
        [SerializeField] private FollowBulletManager followBulletPrefab;

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

            //上からボムを落とす
            for (int i = 0; i < 5; i++)
            {
                Vector3 createPos = new Vector3(3f - 3.5f * i, 6, 0);
                BombManager bombObject = Instantiate(bombPrefab, createPos, Quaternion.identity);
                bombObject.SetData(0.8f, 5);
            }

            yield return new WaitForSeconds(3.5f);

            //プレイヤーに向かって速い球を扇状に打つ
            for (int i = 0; i < 5; i++)
            {
                for (int k = 0; k < 7; k++)
                {
                    Vector3 playerVec = (this.transform.position - player.transform.position) * -1;
                    float rot = Mathf.Atan2(playerVec.y, playerVec.x) * Mathf.Rad2Deg;
                    StraightBulletManager bulletObject = Instantiate(straightBulletPrefab, this.transform.position, Quaternion.Euler(0, 0, rot - 45 + 15 * k));
                    bulletObject.SetData(14, 10);
                }

                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(1.2f);

            //追跡弾とボムを撃つ
            for (int i = 0; i < 5; i++)
            {
                FollowBulletManager bulletObject = Instantiate(followBulletPrefab, this.transform.position, Quaternion.identity);
                bulletObject.SetData(8, 10);

                Vector3 createPos = new Vector3(3f - 3.5f * i, 6, 0);
                BombManager bombObject = Instantiate(bombPrefab, createPos, Quaternion.identity);
                bombObject.SetData(1, 5);

                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(2.2f);

            onDeadCharacter();
            Destroy(this.gameObject);
        }
    }
}
