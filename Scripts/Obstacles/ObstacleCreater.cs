using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //��Q���𐶐�����N���X
    public class ObstacleCreater : MonoBehaviour
    {
        //��Q���Ƃ�����܂Ƃ߂�e��Prefab[
        [SerializeField] private ObstacleManager obstaclePrefab;
        [SerializeField] private ObstacleParentManager obstacleParentPrefab;
        
        //��Q����Y���W�Ə�Q�����m�̋������i�[
        
        [SerializeField] private float obstacleYPos;
        [SerializeField] private float obstacleXDistance;

        //��Q���𐶐����鎞�Ԃ̊Ԋu
        [SerializeField] private float minCreateWaitTime = 3f;
        [SerializeField] private float maxCreateWaitTime = 5f;

        //��Q���̐�����������i�[
        [SerializeField] private int minObstacleVolume;
        [SerializeField] private int maxObstacleVolume;

        [SerializeField] private PlayerManager player;
        
        private float waitTime;
        private float timer;
        //false�̂Ƃ��������Ȃ��悤�ɂ���
        private bool canCreating;

        private void Awake()
        {
            timer = 0;
            canCreating = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            waitTime =  Random.Range(minCreateWaitTime, maxCreateWaitTime);
        }

        // Update is called once per frame
        void Update()
        {
            if (canCreating)
            {
                timer += Time.deltaTime;
            }

            if (timer > waitTime)
            {
                //���������������
                int createVolume = Random.Range(minObstacleVolume, maxObstacleVolume + 1);
                //��ڂ̐�������ꏊ������
                Vector3 standartPos = new Vector3(player.gameObject.transform.position.x + 28, obstacleYPos, 0);
                //��Q���̐e�𐶐�
                ObstacleParentManager obstacleParent = Instantiate(obstacleParentPrefab, standartPos, Quaternion.identity);
                obstacleParent.transform.SetParent(this.transform);
                
                //�s�U�~�b�h��ɐ�������
                int median = Mathf.CeilToInt(createVolume / 2);

                for (int i = 0; i < median; i++)
                {
                    for (int k = 0; k <= i; k++)
                    {
                        Vector3 addPos = new Vector3(i * obstacleXDistance, k, 0);
                        Vector3 createPos = standartPos + addPos;
                        ObstacleManager obstacleObject = Instantiate(obstaclePrefab, createPos, Quaternion.identity);
                        obstacleObject.transform.SetParent(obstacleParent.transform);
                    }
                }
                for (int i = median; i < createVolume; i++)
                {
                    //���s�񐔂����炵�Ă�������
                    //ex)createVolume  = 6 �Ȃ�3,2,1(createVolume - i)�ŃX�g�b�v������
                    for (int k = (createVolume - i); k > 0; k--)
                    {
                        Vector3 addPos = new Vector3(i * obstacleXDistance, k - 1, 0);
                        Vector3 createPos = standartPos + addPos;
                        ObstacleManager obstacleObject = Instantiate(obstaclePrefab, createPos, Quaternion.identity);
                        obstacleObject.transform.SetParent(obstacleParent.transform);
                    }
                }

                //��Q���̐e�𓮂��悤�ɂ���
                obstacleParent.ChangeCanMove();

                waitTime = timer + Random.Range(minCreateWaitTime, maxCreateWaitTime);
            }
        }

        //��Q���𐶐��\�E�s�\��ς��鏈��
        public void ChangeCanCreating(bool b)
        {
            waitTime += 1;
            canCreating = b;
        }
    }
}
