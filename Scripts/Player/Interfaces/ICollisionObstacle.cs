using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    public interface ICollisionObstacle
    {
        public void CollisionObstacle(ObstacleManager obstacle);
    }
}
