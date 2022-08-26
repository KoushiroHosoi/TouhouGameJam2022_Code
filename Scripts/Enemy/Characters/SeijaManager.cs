using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;

namespace HamuGame
{
    public class SeijaManager : EnemyBase
    {
        [SerializeField] private GameObject reverseObject;
        [SerializeField] private FollowBulletManager followBulletPrefab;
        [SerializeField] private StraightBulletManager straightBulletPrefab;
        [SerializeField] private FakeBeamManager beamPrefab;

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
            reverseObject.transform.position = new Vector3(this.gameObject.transform.position.x * -1f, this.gameObject.transform.position.y * -1f, 0);
        }

        private IEnumerator ShotBullet()
        {
            yield return new WaitForSeconds(3f);

            //追跡弾を打つ
            for (int i = 0; i < 3; i++)
            {
                FollowBulletManager bulletObject = Instantiate(followBulletPrefab, this.transform.position, Quaternion.identity);
                bulletObject.SetData(8, 10);

                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(2f);

            //プレイヤーに向かって速い球と遅い球を扇状に打つ
            for (int i = 0; i < 7; i++)
            {
                float speed = 0;
                float rotSize = 0;
                if (i % 2 == 0)
                {
                    speed = 9;
                    rotSize = 12.5f;
                }
                else
                {
                    speed = 7.2f;
                    rotSize = 17.5f;
                }

                for (int k = -2; k < 3; k++)
                {
                    Vector3 playerVec = (this.transform.position - player.transform.position) * -1;
                    float rot = Mathf.Atan2(playerVec.y, playerVec.x) * Mathf.Rad2Deg;
                    StraightBulletManager bulletObject = Instantiate(straightBulletPrefab, this.transform.position, Quaternion.Euler(0, 0, rot + rotSize * k));
                    bulletObject.SetData(speed, 10);
                }

                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(2.7f);

            //前後からプレイヤーに向かって打つ
            for (int i = 0; i < 5; i++)
            {
                for(int k = 0; k < 2; k++)
                {
                    if (k == 0)
                    {
                        Vector3 playerVec = (this.transform.position - player.transform.position) * -1;
                        float rot = Mathf.Atan2(playerVec.y, playerVec.x) * Mathf.Rad2Deg;
                        StraightBulletManager bulletObject = Instantiate(straightBulletPrefab, this.transform.position, Quaternion.Euler(0, 0, rot));
                        bulletObject.SetData(12, 5);
                    }
                    else
                    {
                        Vector3 playerVec = (reverseObject.transform.position - player.transform.position) * -1;
                        float rot = Mathf.Atan2(playerVec.y, playerVec.x) * Mathf.Rad2Deg;
                        StraightBulletManager bulletObject = Instantiate(straightBulletPrefab, reverseObject.transform.position, Quaternion.Euler(0, 0, rot));
                        bulletObject.SetData(12, 5);
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(2.5f);

            //ダマシの位置にビームを打つ
            Vector3 createPos = new Vector3(0, player.transform.position.y, 0);
            FakeBeamManager beam = Instantiate(beamPrefab, createPos, Quaternion.identity);
            beam.SetData(player.transform.position.y, new Vector2(64, 2), 1, 1.4f);

            yield return new WaitForSeconds(2.5f);


            onDeadCharacter();
            Destroy(this.gameObject);

            yield break;
        }
    }
}
