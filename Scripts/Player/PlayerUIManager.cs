using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using UnityEngine.UI;

namespace HamuGame
{
    public class PlayerUIManager : MonoBehaviour
    {
        [SerializeField] private PlayerManager player;

        [SerializeField] private Image greenCircleImage;
        [SerializeField] private Image characterImage;

        [SerializeField] private Sprite[] characterSprites;

        [SerializeField] private Image[] lifeImage;


        private void Awake()
        {
            characterImage.sprite = characterSprites[0];
        }

        // Start is called before the first frame update
        void Start()
        {
            greenCircleImage.fillAmount = player.SpecialPower / player.MaxSpecialPower;

            player.changeCharacter += ChangeCharacterIcon;
            player.onTakeDamaged += DecreaseLifeImage;
        }

        // Update is called once per frame
        void Update()
        {
            greenCircleImage.fillAmount = player.SpecialPower / player.MaxSpecialPower;
        }

        private void ChangeCharacterIcon(CharacterEnum character)
        {
            switch (character)
            {
                case CharacterEnum.Kasen:
                    characterImage.sprite = characterSprites[0];
                    break;

                case CharacterEnum.Suika:
                    characterImage.sprite = characterSprites[1];
                    break;

                case CharacterEnum.Yugi:
                    characterImage.sprite = characterSprites[2];
                    break;
            }
        }

        private void DecreaseLifeImage()
        {
            foreach(var icon in lifeImage)
            {
                if (icon.IsActive())
                {
                    icon.gameObject.SetActive(false);
                    break;
                }
            }
        }
    }
}
