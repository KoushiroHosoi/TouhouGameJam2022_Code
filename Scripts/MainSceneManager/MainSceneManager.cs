using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using naichilab.EasySoundPlayer.Scripts;
using UnityEngine.SceneManagement;

namespace HamuGame
{
    public class MainSceneManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager player;
        [SerializeField] private DistanceManager distanceManager;
        private bool isGamePlaying;

        private void Awake()
        {
            isGamePlaying = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            //BGM���ł����̂ł�����Ə���������
            float volume = BgmPlayer.Instance.Volume;
            volume *= 0.75f;
            BgmPlayer.Instance.Volume = volume;
            BgmPlayer.Instance.Play(1);
        }

        // Update is called once per frame
        void Update()
        {
            //�|�[�Y�@�\�̏���
            if (Input.GetKeyDown(KeyCode.E) && isGamePlaying)
            {
                isGamePlaying = false;
                Time.timeScale = 0;
            }
            else if(Input.GetKeyDown(KeyCode.E) && !isGamePlaying)
            {
                isGamePlaying = true;
                Time.timeScale = 1;
            }

            //�N���A�������̏���
            if (distanceManager.Distance <= 0)
            {
                SceneManager.LoadScene("EndScene");
            }
        }
    }
}
