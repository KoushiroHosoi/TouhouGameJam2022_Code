using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //EndScene�ŉ��ɐ������ꂽ�L�����N�^�[�ɃA�^�b�`����N���X
    public class EndCharacterMoveLeft : MonoBehaviour
    {
        private float timer;

        private void Start()
        {
            timer = 0;
        }
        void Update()
        {
            transform.position -= new Vector3(1, 0, 0) * Time.deltaTime * 5f;
            timer += Time.deltaTime;
            if(timer > 15)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
