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

        //ˆÚ“®‰Â”\‚È‚ç¶‚Éˆê’è‚Ì‘¬“x‚Å“®‚©‚·
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
