using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //îwåiÇìÆÇ©Ç∑èàóù
    public class BackImageManager : MonoBehaviour
    {
        [SerializeField] private float scrollSpeed; 
        [SerializeField] private float startLine;
        [SerializeField] private float deadLine;

        private Vector3 startPos;

        private void Awake()
        {
            startPos = this.transform.position;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(scrollSpeed * Time.deltaTime, 0, 0);

            if (transform.position.x < deadLine)
            {
                transform.position = startPos;
            }
        }
    }
}
