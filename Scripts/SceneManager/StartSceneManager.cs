using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using UnityEngine.SceneManagement;
using naichilab.EasySoundPlayer.Scripts;
using UnityEngine.UI;

namespace HamuGame
{
    //Start�V�[���̐_�N���X
    public class StartSceneManager : MonoBehaviour
    {
        //�\���������p�l�����i�[
        [SerializeField] private GameObject explanationPanel;
        [SerializeField] private GameObject soundPanel;
        [SerializeField] private GameObject creditPanel;

        //On�EOff�p�̃{�^�����i�[
        [SerializeField] private Button startButton;

        [SerializeField] private Button explanationButton;
        [SerializeField] private Button explanationCancelButton;

        [SerializeField] private Button soundButton;
        [SerializeField] private Button soundCancelButton;

        [SerializeField] private Button creditButton;
        [SerializeField] private Button creditCancelButton;

        private void Awake()
        {
            creditPanel.SetActive(false);
            soundPanel.SetActive(false);
            explanationPanel.SetActive(false);
        }

        // Start is called before the first frame update
        void Start()
        {
            //BGM�炷
            BgmPlayer.Instance.Play(0);

            //���ꂼ��̃{�^���ɃC�x���g��ݒ�
            startButton.onClick.AddListener(StartGame);

            explanationButton.onClick.AddListener(ChangeExplanationPanel);
            explanationCancelButton.onClick.AddListener(ChangeExplanationPanel);

            soundButton.onClick.AddListener(ChangeSoundPanel);
            soundCancelButton.onClick.AddListener(ChangeSoundPanel);

            creditButton.onClick.AddListener(ChangeCreditPanel);
            creditCancelButton.onClick.AddListener(ChangeCreditPanel);
        }

        public void StartGame()
        {
            StartCoroutine(GameStartCoroutine());
        }

        private IEnumerator GameStartCoroutine()
        {
            SePlayer.Instance.Play(0);
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("MainRunScene");
        }

        public void ChangeExplanationPanel()
        {
            if (explanationPanel.activeSelf)
            {
                SePlayer.Instance.Play(1);
                explanationPanel.SetActive(false);
            }
            else
            {
                SePlayer.Instance.Play(0);
                explanationPanel.SetActive(true);
            }
        }

        public void ChangeSoundPanel()
        {
            if (soundPanel.activeSelf)
            {
                SePlayer.Instance.Play(1);
                soundPanel.SetActive(false);
            }
            else
            {
                SePlayer.Instance.Play(0);
                soundPanel.SetActive(true);
            }
        }

        public void ChangeCreditPanel()
        {
            if (creditPanel.activeSelf)
            {
                SePlayer.Instance.Play(1);
                creditPanel.SetActive(false);
            }
            else
            {
                SePlayer.Instance.Play(0);
                creditPanel.SetActive(true);
            }
        }
    }
}
