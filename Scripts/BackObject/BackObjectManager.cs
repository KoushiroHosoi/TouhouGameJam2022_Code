using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //�w�i�̃I�u�W�F�N�g�ɂ���N���X
    public class BackObjectManager : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float speed;

        // Update is called once per frame
        void FixedUpdate()
        {
            //���ɓ�����
            rb.velocity = new Vector2(-1, 0) * speed;
        }
    }
}
