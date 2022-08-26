using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    public class PlayerRayManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager player;
        [SerializeField] private float distance;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private BoxCollider2D boxCollider;
        private Vector2[] vertices;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector2 lowerRightVertex = GetBoxCollide2DVertices(3);
            Vector2 lowerLeftVertex = GetBoxCollide2DVertices(2);

            Debug.DrawLine(lowerRightVertex - new Vector2(-1f * distance * Mathf.Sin(this.transform.localEulerAngles.z * Mathf.Deg2Rad), distance * Mathf.Cos(this.transform.localEulerAngles.z * Mathf.Deg2Rad)),
               lowerLeftVertex - new Vector2(-1f * distance * Mathf.Sin(this.transform.localEulerAngles.z * Mathf.Deg2Rad), distance * Mathf.Cos(this.transform.localEulerAngles.z * Mathf.Deg2Rad))
               , Color.red);

            if (OnCollisionStayLayer(lowerRightVertex, lowerLeftVertex, groundLayer))
            {
                player.ChangeIsOnGround(this, true);
            }
            else
            {
                player.ChangeIsOnGround(this, false);
            }
        }

        //Rayを発射して指定されたLayerとの当たり判定をおこなう。
        private bool OnCollisionStayLayer(Vector2 startPos, Vector2 endPos, LayerMask layer)
        {
            RaycastHit2D raycastHit2D = Physics2D.Linecast(startPos - new Vector2(-1f * distance * Mathf.Sin(this.transform.localEulerAngles.z * Mathf.Deg2Rad), distance * Mathf.Cos(this.transform.localEulerAngles.z * Mathf.Deg2Rad)),
                                                           endPos - new Vector2(-1f * distance * Mathf.Sin(this.transform.localEulerAngles.z * Mathf.Deg2Rad), distance * Mathf.Cos(this.transform.localEulerAngles.z * Mathf.Deg2Rad)),
                                                           layer);

            return raycastHit2D.collider != null;
        }

        //BoxColliderの頂点座標を返す
        //左上から順に0,1…となってます
        private Vector2 GetBoxCollide2DVertices(int num)
        {
            Vector2[] boxColliderPosition = CalculateBoxCollide2DVertices(boxCollider);
            Vector2 lowerRightPosition = boxColliderPosition[num];
            return lowerRightPosition;
        }

        //BoxColliderの各頂点を算出する
        private Vector2[] CalculateBoxCollide2DVertices(BoxCollider2D collider)
        {
            Transform colliderTransform = collider.transform;
            Vector2 colliderLossyScale = colliderTransform.lossyScale;

            float boxColliderXSize = collider.size.x * colliderLossyScale.x;
            float boxColliderYSize = collider.size.y * colliderLossyScale.y;

            colliderLossyScale = new Vector2(boxColliderXSize, boxColliderYSize);

            colliderLossyScale *= 0.5f;

            Vector2 colliderWorldTransform = colliderTransform.TransformPoint(collider.offset);

            Vector2 vx = colliderTransform.right * colliderLossyScale.x;
            Vector2 vy = colliderTransform.up * colliderLossyScale.y;

            Vector2 p1 = -vx + vy;
            Vector2 p2 = vx + vy;
            Vector2 p3 = vx + -vy;
            Vector2 p4 = -vx + -vy;

            vertices = new Vector2[4];

            vertices[0] = colliderWorldTransform + p1;
            vertices[1] = colliderWorldTransform + p2;
            vertices[2] = colliderWorldTransform + p3;
            vertices[3] = colliderWorldTransform + p4;

            return vertices;
        }
    }
}
