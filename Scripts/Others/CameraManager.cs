using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //カメラにくっつけるクラス
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager player;
        [SerializeField] private float offsetX;

        private Vector3 cameraPos;

        //Playerがvelocityで動かしてた時の残骸
        private void LateUpdate()
        {
            /*
            if (player != null)
            {
                cameraPos = this.transform.position;
                cameraPos.x = player.transform.position.x - offsetX;
                this.transform.position = cameraPos;
            }
            */
        }
    }
}
