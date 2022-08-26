using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;

namespace HamuGame
{
    public class RemiriaManager : EnemyBase
    {
        [SerializeField] private StraightBulletManager bulletPrefab;
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

            //扇状に打つ
            for (int i = 0; i < 7; i++)
            {
                for (int k = 0; k < 5; k++)
                {
                    StraightBulletManager bulletObject = Instantiate(bulletPrefab, this.transform.position, Quaternion.Euler(0, 0, 200 + 15 * k));
                    bulletObject.SetData(7, 10);
                }

                yield return new WaitForSeconds(1f);
            }

            //プレイヤーに向かって高速で打つ
            for (int i = 0; i < 5; i++)
            {
                Vector3 playerVec = (this.transform.position - player.transform.position) * -1;
                float rot = Mathf.Atan2(playerVec.y, playerVec.x) * Mathf.Rad2Deg;
                StraightBulletManager bulletObject = Instantiate(bulletPrefab, this.transform.position, Quaternion.Euler(0, 0, rot));
                bulletObject.SetData(12, 5);

                yield return new WaitForSeconds(0.5f);
            }

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
                    StraightBulletManager bulletObject = Instantiate(bulletPrefab, this.transform.position, Quaternion.Euler(0, 0, rot + rotSize * k));
                    bulletObject.SetData(speed, 10);
                }

                yield return new WaitForSeconds(0.3f);
            }

            onDeadCharacter();
            Destroy(this.gameObject);
        }
    }
}
