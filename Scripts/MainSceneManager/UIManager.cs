using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using UnityEngine.UI;

namespace HamuGame
{
    //�c�苗�����o���N���X
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private DistanceManager distanceManager;
        [SerializeField] private Text distanceText;

        // Start is called before the first frame update
        void Start()
        {
            distanceText.text = "�c�� " + distanceManager.Distance.ToString("f2").PadLeft(6,'0') + "���[�g���I";
        }

        // Update is called once per frame
        void Update()
        {
            distanceText.text = "�c�� " + distanceManager.Distance.ToString("f2").PadLeft(6,'0') + "���[�g���I";
        }
    }
}
