using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //ぶつかった障害物と背景オブジェクトを消すクラス
    public class ObjectCollecter : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<ObstacleManager>(out var obstacle))
            {
                Destroy(obstacle.gameObject);
            }

            if (collision.TryGetComponent<BackObjectManager>(out var backObject))
            {
                Destroy(backObject.gameObject);
            }
        }
    }
}
