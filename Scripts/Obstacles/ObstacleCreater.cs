using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //áŠQ•¨‚ğ¶¬‚·‚éƒNƒ‰ƒX
    public class ObstacleCreater : MonoBehaviour
    {
        //áŠQ•¨‚Æ‚¹‚ê‚ğ‚Ü‚Æ‚ß‚ée‚ÌPrefab[
        [SerializeField] private ObstacleManager obstaclePrefab;
        [SerializeField] private ObstacleParentManager obstacleParentPrefab;
        
        //áŠQ•¨‚ÌYÀ•W‚ÆáŠQ•¨“¯m‚Ì‹——£‚ğŠi”[
        
        [SerializeField] private float obstacleYPos;
        [SerializeField] private float obstacleXDistance;

        //áŠQ•¨‚ğ¶¬‚·‚éŠÔ‚ÌŠÔŠu
        [SerializeField] private float minCreateWaitTime = 3f;
        [SerializeField] private float maxCreateWaitTime = 5f;

        //áŠQ•¨‚Ì¶¬‚·‚éŒÂ”‚ğŠi”[
        [SerializeField] private int minObstacleVolume;
        [SerializeField] private int maxObstacleVolume;

        [SerializeField] private PlayerManager player;
        
        private float waitTime;
        private float timer;
        //false‚Ì‚Æ‚«¶¬‚µ‚È‚¢‚æ‚¤‚É‚·‚é
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
                //¶¬‚·‚éŒÂ”‚ğŒˆ’è
                int createVolume = Random.Range(minObstacleVolume, maxObstacleVolume + 1);
                //ˆêŒÂ–Ú‚Ì¶¬‚·‚éêŠ‚ğŒˆ’è
                Vector3 standartPos = new Vector3(player.gameObject.transform.position.x + 28, obstacleYPos, 0);
                //áŠQ•¨‚Ìe‚ğ¶¬
                ObstacleParentManager obstacleParent = Instantiate(obstacleParentPrefab, standartPos, Quaternion.identity);
                obstacleParent.transform.SetParent(this.transform);
                
                //ƒsƒUƒ~ƒbƒhã‚É¶¬‚·‚é
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
                    //Às‰ñ”‚ğŒ¸‚ç‚µ‚Ä‚¢‚«‚½‚¢
                    //ex)createVolume  = 6 ‚È‚ç3,2,1(createVolume - i)‚ÅƒXƒgƒbƒv‚³‚¹‚é
                    for (int k = (createVolume - i); k > 0; k--)
                    {
                        Vector3 addPos = new Vector3(i * obstacleXDistance, k - 1, 0);
                        Vector3 createPos = standartPos + addPos;
                        ObstacleManager obstacleObject = Instantiate(obstaclePrefab, createPos, Quaternion.identity);
                        obstacleObject.transform.SetParent(obstacleParent.transform);
                    }
                }

                //áŠQ•¨‚Ìe‚ğ“®‚­‚æ‚¤‚É‚·‚é
                obstacleParent.ChangeCanMove();

                waitTime = timer + Random.Range(minCreateWaitTime, maxCreateWaitTime);
            }
        }

        //áŠQ•¨‚ğ¶¬‰Â”\E•s‰Â”\‚ğ•Ï‚¦‚éˆ—
        public void ChangeCanCreating(bool b)
        {
            waitTime += 1;
            canCreating = b;
        }
    }
}
