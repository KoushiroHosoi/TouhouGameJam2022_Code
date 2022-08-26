using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //áŠQ•¨‚É‚­‚Á‚Â‚¯‚éƒNƒ‰ƒX
    public class ObstacleManager : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<ICollisionObstacle>(out var player))
            {
                player.CollisionObstacle(this);
            }
        }
    }
}
