using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using UnityEngine.UI;

namespace HamuGame
{
    //残り距離を出すクラス
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private DistanceManager distanceManager;
        [SerializeField] private Text distanceText;

        // Start is called before the first frame update
        void Start()
        {
            distanceText.text = "残り " + distanceManager.Distance.ToString("f2").PadLeft(6,'0') + "メートル！";
        }

        // Update is called once per frame
        void Update()
        {
            distanceText.text = "残り " + distanceManager.Distance.ToString("f2").PadLeft(6,'0') + "メートル！";
        }
    }
}
