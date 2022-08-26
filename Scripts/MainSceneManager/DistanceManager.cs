using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //プレイヤーとゴールの位置関係を扱う
    public class DistanceManager : MonoBehaviour
    {
        //こいつが0になったらクリアになる
        [SerializeField] private float distance;
        [SerializeField] private PlayerManager player;

        public float Distance { get => distance; }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (player.IsOnGround)
            {
                distance -= Time.deltaTime * 2f;
            }
            else
            {
                distance -= Time.deltaTime;
            }
        }
    }
}
