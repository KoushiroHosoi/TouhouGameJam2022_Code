using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    public class ObstacleParentManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float speed;
        private bool canMove;

        // Start is called before the first frame update
        void Awake()
        {
            canMove = false;
        }

        //移動可能なら左に一定の速度で動かす
        void FixedUpdate()
        {
            if (canMove)
            {
                rb.velocity = new Vector2(-1, 0) * speed;
            }

            if(this.transform.position.x < -25)
            {
                Destroy(this.gameObject);
            }
        }

        public void ChangeCanMove()
        {
            canMove = true;
        }
    }
}
