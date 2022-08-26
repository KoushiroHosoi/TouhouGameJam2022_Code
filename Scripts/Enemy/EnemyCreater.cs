using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    public class EnemyCreater : MonoBehaviour
    {
        [SerializeField] private DistanceManager distanceManager;

        [SerializeField] private ObstacleCreater obstacleCreater;

        [SerializeField] private int remiriaPos;
        [SerializeField] private RemiriaManager remiriaPrefab;
        private bool isInstantiateRemiria;

        [SerializeField] private int huranPos;
        [SerializeField] private HuranManager huranPrefab;
        private bool isInstantiateHuran;

        [SerializeField] private int urumiPos;
        [SerializeField] private UrumiManager urumiPrefab;
        private bool isInstantiateUrumi;

        [SerializeField] private int seijaPos;
        [SerializeField] private SeijaManager seijaPrefab;
        private bool isInstantiateSeija;

        private void Awake()
        {
            isInstantiateRemiria = false;
            isInstantiateHuran = false;
            isInstantiateUrumi = false;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //それぞれのキャラを一回ずつ生成する
            if(!isInstantiateRemiria && distanceManager.Distance < remiriaPos)
            {
                RemiriaManager remiria = Instantiate(remiriaPrefab);
                remiria.onDeadCharacter += DeadCharacter;
                obstacleCreater.ChangeCanCreating(false);
                isInstantiateRemiria = true;
            }
            else if(!isInstantiateHuran && distanceManager.Distance < huranPos)
            {
                HuranManager huran = Instantiate(huranPrefab);
                huran.onDeadCharacter += DeadCharacter;
                obstacleCreater.ChangeCanCreating(false);
                isInstantiateHuran = true;
            }
            else if (!isInstantiateUrumi && distanceManager.Distance < urumiPos)
            {
                UrumiManager urumi = Instantiate(urumiPrefab);
                urumi.onDeadCharacter += DeadCharacter;
                obstacleCreater.ChangeCanCreating(false);
                isInstantiateUrumi = true;
            }
            else if(!isInstantiateSeija && distanceManager.Distance < seijaPos)
            {
                SeijaManager seija = Instantiate(seijaPrefab);
                seija.onDeadCharacter += DeadCharacter;
                obstacleCreater.ChangeCanCreating(false);
                isInstantiateSeija = true;
            }
        }

        //キャラクターがやられたときの処理
        private void DeadCharacter()
        {
            obstacleCreater.ChangeCanCreating(true);
        }
    }
}
