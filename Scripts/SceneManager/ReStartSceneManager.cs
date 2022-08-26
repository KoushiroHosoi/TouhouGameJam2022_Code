using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using naichilab.EasySoundPlayer.Scripts;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HamuGame
{
    public class ReStartSceneManager : MonoBehaviour
    {
        [SerializeField] private Button reStartButton;

        private void Start()
        {
            BgmPlayer.Instance.Play(3);
            reStartButton.onClick.AddListener(ReStartGame);
        }

        //MainRunScene‚É‚à‚Ç‚·
        public void ReStartGame()
        {
            StartCoroutine(ReStartCoroutine());
        }

        private IEnumerator ReStartCoroutine()
        {
            SePlayer.Instance.Play(0);
            yield return new WaitForSeconds(0.8f);
            SceneManager.LoadScene("MainRunScene");
        }
    }
}
