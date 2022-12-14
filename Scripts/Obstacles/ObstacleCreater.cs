using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //障害物を生成するクラス
    public class ObstacleCreater : MonoBehaviour
    {
        //障害物とせれをまとめる親のPrefab[
        [SerializeField] private ObstacleManager obstaclePrefab;
        [SerializeField] private ObstacleParentManager obstacleParentPrefab;
        
        //障害物のY座標と障害物同士の距離を格納
        
        [SerializeField] private float obstacleYPos;
        [SerializeField] private float obstacleXDistance;

        //障害物を生成する時間の間隔
        [SerializeField] private float minCreateWaitTime = 3f;
        [SerializeField] private float maxCreateWaitTime = 5f;

        //障害物の生成する個数を格納
        [SerializeField] private int minObstacleVolume;
        [SerializeField] private int maxObstacleVolume;

        [SerializeField] private PlayerManager player;
        
        private float waitTime;
        private float timer;
        //falseのとき生成しないようにする
        private bool canCreating;

        private void Awake()
        {
            timer = 0;
            canCreating = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            waitTime =  Random.Range(minCreateWaitTime, maxCreateWaitTime);
        }

        // Update is called once per frame
        void Update()
        {
            if (canCreating)
            {
                timer += Time.deltaTime;
            }

            if (timer > waitTime)
            {
                //生成する個数を決定
                int createVolume = Random.Range(minObstacleVolume, maxObstacleVolume + 1);
                //一個目の生成する場所を決定
                Vector3 standartPos = new Vector3(player.gameObject.transform.position.x + 28, obstacleYPos, 0);
                //障害物の親を生成
                ObstacleParentManager obstacleParent = Instantiate(obstacleParentPrefab, standartPos, Quaternion.identity);
                obstacleParent.transform.SetParent(this.transform);
                
                //ピザミッド上に生成する
                int median = Mathf.CeilToInt(createVolume / 2);

                for (int i = 0; i < median; i++)
                {
                    for (int k = 0; k <= i; k++)
                    {
                        Vector3 addPos = new Vector3(i * obstacleXDistance, k, 0);
                        Vector3 createPos = standartPos + addPos;
                        ObstacleManager obstacleObject = Instantiate(obstaclePrefab, createPos, Quaternion.identity);
                        obstacleObject.transform.SetParent(obstacleParent.transform);
                    }
                }
                for (int i = median; i < createVolume; i++)
                {
                    //実行回数を減らしていきたい
                    //ex)createVolume  = 6 なら3,2,1(createVolume - i)でストップさせる
                    for (int k = (createVolume - i); k > 0; k--)
                    {
                        Vector3 addPos = new Vector3(i * obstacleXDistance, k - 1, 0);
                        Vector3 createPos = standartPos + addPos;
                        ObstacleManager obstacleObject = Instantiate(obstaclePrefab, createPos, Quaternion.identity);
                        obstacleObject.transform.SetParent(obstacleParent.transform);
                    }
                }

                //障害物の親を動くようにする
                obstacleParent.ChangeCanMove();

                waitTime = timer + Random.Range(minCreateWaitTime, maxCreateWaitTime);
            }
        }

        //障害物を生成可能・不可能を変える処理
        public void ChangeCanCreating(bool b)
        {
            waitTime += 1;
            canCreating = b;
        }
    }
}
