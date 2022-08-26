using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;
using UnityEngine.SceneManagement;
using naichilab.EasySoundPlayer.Scripts;

namespace HamuGame
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerManager : MonoBehaviour,ICollisionObstacle,ICollisionEnemyAttack
    {
        //デバッグ用（死ななくなるよ）
        [SerializeField] private bool isDebug;

        [SerializeField] private Rigidbody2D rb;

        //それぞれのキャラクターを格納
        [SerializeField] private KasenManager kasen;
        [SerializeField] private SuikaManager suika;
        [SerializeField] private YugiManager yugi;

        //子オブジェクトを格納（Spriteの切り替えに必要）
        private List<GameObject> characterList = new List<GameObject>();

        //現在操作中のキャラクターを格納
        private CharacterEnum nowPlayer;

        //特殊能力ゲージ
        [SerializeField] private float specialPower;
        [SerializeField] private float maxSpecialPower;

        //ダッシュ用のアニメーション
        [SerializeField] private GameObject[] dashAnimetion;

        //キャラ特有のデータ
        private float moveSpeed;
        private float jumpPower;
        private float gaugeAcceleration;

        //ライフ関連
        private int lifeCount;
        private bool isDamaging;

        //ボタンの押しっぱなしを防止するよう
        private bool jumpChecker;
        //ジャンプ用のタイマー
        private float jumpTimer;
        //接地してるかどうか
        private bool isOnGround;
        //特殊能力を発動中か(交代可能かも見る)
        private bool isActivatingSpecialAbilities;
        //二段ジャンプが可能か
        private bool canDoubleJump;
        //二段ジャンプしてたかどうか
        private bool isDouble;
        //障害物を無視できるか
        private bool ignoreObstacle;
        //敵からの攻撃を無視できるか
        private bool ignoreEnemyAttack;
        
        //UI変更する用の委譲
        public event Action<CharacterEnum> changeCharacter;
        public event Action onTakeDamaged;

        public float SpecialPower
        {
            get
            {
                return specialPower;
            }
            set
            {
                if (specialPower >= maxSpecialPower)
                {
                    specialPower = maxSpecialPower;
                }
                else
                {
                    specialPower = value;
                }
            }
        }

        public float MaxSpecialPower { get => maxSpecialPower; }
        public float MoveSpeed { get => moveSpeed; }
        public Rigidbody2D Rb { get => rb; }
        public bool IsOnGround { get => isOnGround; }

        private void Awake()
        {
            characterList.Add(kasen.gameObject);
            characterList.Add(suika.gameObject);
            characterList.Add(yugi.gameObject);

            kasen.playDoubleJump += CanDoubleJump;
            suika.changeSize += ChangeSize;
            yugi.startMonsterPower += StartMonsterPower;

            //ダッシュアニメーション用
            dashAnimetion[0].SetActive(false);
            dashAnimetion[1].SetActive(false);

            //スピード関連
            jumpPower = 0;
            moveSpeed = 0;
            gaugeAcceleration = 0;

            jumpTimer = 0;

            lifeCount = 2;
            isDamaging = false;

            jumpChecker = true;

            isOnGround = false;
            isActivatingSpecialAbilities = false;
            canDoubleJump = false;
            isDouble = false;
            ignoreObstacle = false;
            ignoreEnemyAttack = false;
        }

        // Start is called before the first frame update
        void Start()
        {
            //華仙のみを表示させる
            for(int i = 1; i < 3; i++)
            {
                characterList[i].SetActive(false);
            }

            //データを設定する
            SetCharacterData(kasen);
            nowPlayer = CharacterEnum.Kasen;
        }

        // Update is called once per frame
        void Update()
        {
            //スペシャルゲージの増加
            SpecialPower += Time.deltaTime * gaugeAcceleration;

            //キャラクター移動の処理
            if (Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.RightArrow))
            {
                IncreaseSpeed();
            }
            else if (Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.LeftArrow))
            {
                DecreaseSpeed();
            }

            //キャラクターの変更の処理
            if (Input.GetKeyDown(KeyCode.A) && SpecialPower >= 12 && !isActivatingSpecialAbilities　&& !isDamaging)
            {
                specialPower -= 12;
                ChangePlayer();
            }

            //ジャンプ関連の処理
            if (Input.GetKeyDown(KeyCode.Space))
            {
                jumpTimer = 0;
                jumpChecker = true;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                jumpTimer += Time.deltaTime * 5f;
                if(jumpTimer > 1)
                {
                    if (isOnGround)
                    {
                        Jump(jumpTimer);
                    }
                }
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                //通常のジャンプ
                if(!canDoubleJump)
                {
                    if (isOnGround)
                    {
                        Jump(jumpTimer);
                    }
                }
                //二段ジャンプ専用の処理
                else
                {
                    if (isOnGround)
                    {
                        Jump(jumpTimer);
                        isDouble = false;
                    }
                    else
                    {
                        if (!isDouble)
                        {
                            //Debug.Log("二段ジャンプ");
                            DoubleJump(jumpTimer);
                            isDouble = true;
                        }
                    }
                }                
            }

            //Sを押したら特殊能力発動
            if (Input.GetKeyDown(KeyCode.S) && SpecialPower >= 80 && !isActivatingSpecialAbilities)
            {
                specialPower -= 80;
                switch (nowPlayer)
                {
                    case CharacterEnum.Kasen:
                        kasen.ActivateSpecialAbilities();
                        break;

                    case CharacterEnum.Suika:
                        suika.ActivateSpecialAbilities();
                        break;

                    case CharacterEnum.Yugi:
                        yugi.ActivateSpecialAbilities();
                        break;
                }
            }
        }

        //操作するキャラクターを変更するメソッド
        private void ChangePlayer()
        {
            switch (nowPlayer)
            {
                case CharacterEnum.Kasen:
                    //表示するキャラの入れ替え
                    characterList[0].SetActive(false);
                    characterList[1].SetActive(true);
                    //データの入れ替え
                    SetCharacterData(suika);
                    nowPlayer = CharacterEnum.Suika;
                    break;

                case CharacterEnum.Suika:
                    characterList[1].SetActive(false);
                    characterList[2].SetActive(true);
                    SetCharacterData(yugi);
                    nowPlayer = CharacterEnum.Yugi;
                    break;

                case CharacterEnum.Yugi:
                    characterList[2].SetActive(false);
                    characterList[0].SetActive(true);
                    SetCharacterData(kasen);
                    nowPlayer = CharacterEnum.Kasen;
                    break;
            }
            //UIを変更する
            changeCharacter(nowPlayer);
        }

        //キャラクターをジャンプさせる
        private void Jump(float touchTime)
        {
            if (!jumpChecker) return;

            if(touchTime > 1)
            {
                touchTime = 1;
            }
            else if(touchTime < 0.7)
            {
                touchTime = 0.7f;
            }
            rb.AddForce(new Vector2(0, jumpPower) * touchTime, ForceMode2D.Impulse);
            SePlayer.Instance.Play(3);

            jumpTimer = 0;
            jumpChecker = false;
        }

        //二段ジャンプ専用の処理
        private void DoubleJump(float touchTime)
        {
            /*
            if (touchTime > 1)
            {
                touchTime = 1;
            }
            else if (touchTime < 0.7)
            {
                touchTime = 0.7f;
            }
            */
            rb.AddForce(new Vector2(0, jumpPower/1.5f), ForceMode2D.Impulse);
            SePlayer.Instance.Play(3);
        }

        //キャラクターのデータを入れる（スタート・キャラ変更時・特殊能力発動時に使用）
        private void SetCharacterData(CharacterBase charaData)
        {
            if (!isActivatingSpecialAbilities)
            {
                moveSpeed = charaData.NormalSpeed;
                jumpPower = charaData.NormalJumpPower * 2.5f;
                gaugeAcceleration = charaData.NormalGaugeAcceleration * 1.5f / 5f;
                dashAnimetion[0].SetActive(true);
                dashAnimetion[1].SetActive(false);
            }
            else
            {
                moveSpeed = charaData.SpecialSpeed;
                jumpPower = charaData.SpecialJumpPower * 2.5f;
                gaugeAcceleration = charaData.SpecialnormalGaugeAcceleration * 1.5f / 5f;
                dashAnimetion[0].SetActive(false);
                dashAnimetion[1].SetActive(true);
            }
        }

        //キャラを前方に勧める処理
        private void IncreaseSpeed()
        {
            //キャラによって移動範囲を変える
            float defaltX = 3f + moveSpeed * 0.5f;
            if (transform.position.x > defaltX || !isOnGround) return;
            transform.position += new Vector3(1, 0, 0) * 5f * Time.deltaTime;
        }

        //目的の速度まで減速させる
        private void DecreaseSpeed()
        {
            float defaltX = -5f + moveSpeed * 0.5f * -1;
            if (transform.position.x < defaltX || !isOnGround) return;
            transform.position -= new Vector3(1, 0, 0) * 5f * Time.deltaTime;
        }

        //死んだときの処理
        private IEnumerator OnRetry()
        {
            Debug.Log("死んだよ");
            if (isDebug) yield break;

            SePlayer.Instance.Play(5);

            yield return new WaitForSeconds(0.25f);
            SceneManager.LoadScene("ReStartScene");
        }

        //特殊能力を発動させるときの処理
        private void StartSpecialAbilities(CharacterBase charaData)
        {
            isActivatingSpecialAbilities = true;
            SetCharacterData(charaData);
        }

        //特殊能力を終了させるときの処理
        private void EndSpecialAbilities(CharacterBase charaData)
        {
            isActivatingSpecialAbilities = false;
            SetCharacterData(charaData);
        }

        //華仙の特殊能力
        private IEnumerator CanDoubleJump()
        {
            SePlayer.Instance.Play(11);
            StartSpecialAbilities(kasen);

            canDoubleJump = true;

            yield return new WaitForSeconds(7);

            SePlayer.Instance.Play(10);
            yield return new WaitForSeconds(1);

            SePlayer.Instance.Play(10);
            yield return new WaitForSeconds(1);

            SePlayer.Instance.Play(10);
            yield return new WaitForSeconds(1);

            canDoubleJump = false;

            SePlayer.Instance.Play(12);
            EndSpecialAbilities(kasen);

            yield break;
        }

        //萃香の特殊能力
        private IEnumerator ChangeSize()
        {
            int random = UnityEngine.Random.Range(0, 100);
            float changeScale = 0;

            if (random < suika.ChangeSmallPercent)
            {
                suika.ChangeSpecialParameter(false);
                changeScale = 1f / 2f;
                this.gameObject.transform.localScale *= changeScale;
            }
            else
            {
                suika.ChangeSpecialParameter(true);
                changeScale = 2.4f;
                this.gameObject.transform.localScale *= changeScale;
                ignoreObstacle = true;
                ignoreEnemyAttack = true;
            }

            SePlayer.Instance.Play(11);
            //ChangeSpecialParameter()の後に実行させる
            StartSpecialAbilities(suika);

            yield return new WaitForSeconds(4);

            SePlayer.Instance.Play(10);
            yield return new WaitForSeconds(1);
            SePlayer.Instance.Play(10);
            yield return new WaitForSeconds(1);
            SePlayer.Instance.Play(10);
            yield return new WaitForSeconds(1);

            this.gameObject.transform.localScale /= changeScale;
            ignoreObstacle = false;
            ignoreEnemyAttack = false;

            SePlayer.Instance.Play(12);
            EndSpecialAbilities(suika);
            
            yield break;
        }

        //勇儀の特殊能力
        private IEnumerator StartMonsterPower()
        {
            SePlayer.Instance.Play(11);
            StartSpecialAbilities(yugi);

            SetCharacterData(yugi);

            ignoreObstacle = true;

            yield return new WaitForSeconds(5);

            SePlayer.Instance.Play(10);
            yield return new WaitForSeconds(1);
            SePlayer.Instance.Play(10);
            yield return new WaitForSeconds(1);
            SePlayer.Instance.Play(10);
            yield return new WaitForSeconds(1);

            ignoreObstacle = false;

            SePlayer.Instance.Play(12);
            EndSpecialAbilities(yugi);

            yield break;
        }

        //ダメージを受けたときの処理
        private IEnumerator TakeDamagedCoroutine()
        {
            Debug.Log("ダメージ受けたよ");

            //Spriteを点滅させる前準備
            List<SpriteRenderer> renderers = new List<SpriteRenderer>();
            foreach(var character in characterList)
            {
                SpriteRenderer renderer = character.GetComponent<SpriteRenderer>();
                renderers.Add(renderer);
            }

            lifeCount--;
            onTakeDamaged();
            isDamaging = true;
            SePlayer.Instance.Play(4);

            //Spriteを点灯させる
            for (int i = 0; i < 20; i++)
            {
                //flashInterval待ってから
                yield return new WaitForSeconds(0.075f);
                //spriteRendererをオフ
                foreach(var renderer in renderers)
                {
                    renderer.enabled = false;
                }

                //flashInterval待ってから
                yield return new WaitForSeconds(0.075f);
                //spriteRendererをオン
                foreach (var renderer in renderers)
                {
                    renderer.enabled = true;
                }
            }

            isDamaging = false;

            yield break;
        }

        //接地したときの処理
        public void ChangeIsOnGround(PlayerRayManager player,bool result)
        {
            if (player)
            {
                isOnGround = result;
            }
        }

        //攻撃されたor障害物と当たったときの処理
        public void CollisionObstacle(ObstacleManager obstacle)
        {
            if (isDamaging) return;

            if (!ignoreObstacle)
            {
                //OnRetry();
                if(lifeCount > 0)
                {
                    StartCoroutine(TakeDamagedCoroutine());
                }
                else
                {
                    StartCoroutine(OnRetry());
                }
            }
            else
            {
                Destroy(obstacle.gameObject);
            }
        }

        public void CollisionEnemyAttack()
        {
            if (isDamaging) return;

            if (!ignoreEnemyAttack)
            {
                if (lifeCount > 0)
                {
                    StartCoroutine(TakeDamagedCoroutine());
                }
                else
                {
                    StartCoroutine(OnRetry());
                }
            }
        }
    }
}
