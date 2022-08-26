using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using naichilab.EasySoundPlayer.Scripts;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HamuGame
{
    public class EndSceneManager : MonoBehaviour
    {
        [SerializeField] private Button backButton;
        //égÇ¡ÇƒÇ»Ç¢ÉLÉÉÉâÇÃSpriteì¸ÇÍÇÈ
        [SerializeField] private GameObject[] allCharacters;
        //ÇªÇÍÇºÇÍÇÃê∂ê¨èÍèäÇì¸ÇÍÇÈ
        [SerializeField] private GameObject upCreater;
        [SerializeField] private GameObject downCreater;



        private void Start()
        {
            BgmPlayer.Instance.Play(2);
            backButton.onClick.AddListener(BackStartScene);
            StartCoroutine(CreateCharacters());
        }

        public void BackStartScene()
        {
            StartCoroutine(BackSceneCoroutine());
        }

        private IEnumerator BackSceneCoroutine()
        {
            SePlayer.Instance.Play(0);
            yield return new WaitForSeconds(0.6f);
            SceneManager.LoadScene("StartScene");
        }

        private IEnumerator CreateCharacters()
        {
            while (true)
            {
                int randomUp = UnityEngine.Random.Range(0, allCharacters.Length);
                GameObject upObj = Instantiate(allCharacters[randomUp], upCreater.transform.position, Quaternion.identity);
                upObj.AddComponent<EndCharacterMoveRight>();

                yield return new WaitForSeconds(0.5f);

                int randomDown = UnityEngine.Random.Range(0, allCharacters.Length);
                GameObject downObj = Instantiate(allCharacters[randomDown], downCreater.transform.position, Quaternion.identity);
                downObj.AddComponent<EndCharacterMoveLeft>();

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
