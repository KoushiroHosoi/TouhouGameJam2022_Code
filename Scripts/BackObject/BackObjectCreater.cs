using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    public class BackObjectCreater : MonoBehaviour
    {
        [SerializeField] private BackObjectManager backObjectPrefab;
        [SerializeField] private float backObjectYPos;
        private float waitTime;
        private float timer;

        [SerializeField] private PlayerManager player;

        // Start is called before the first frame update
        void Awake()
        {
            timer = 0;
            waitTime = 5f;
        }

        // Update is called once per frame
        void Update()
        {
            //一定時間ごとにオブジェクトを生成
            timer += Time.deltaTime;

            if(timer > waitTime)
            {
                Vector3 createPos = new Vector3(player.gameObject.transform.position.x + 20, backObjectYPos, 0);
                BackObjectManager backObject = Instantiate(backObjectPrefab, createPos, Quaternion.identity);
                backObject.transform.SetParent(this.transform);

                waitTime = timer + 5f;
            }
        }
    }
}
