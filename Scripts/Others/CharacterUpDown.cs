using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //StartSceneでキャラクターを上下に動かすクラス
    public class CharacterUpDown : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(UpDownCoroutine());
        }

        private IEnumerator UpDownCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                this.transform.position += new Vector3(0, 0.3f, 0);
                yield return new WaitForSeconds(1);
                this.transform.position -= new Vector3(0, 0.3f, 0);
            }
        }
    }
}
