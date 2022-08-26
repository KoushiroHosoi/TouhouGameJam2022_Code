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
        //�f�o�b�O�p�i���ȂȂ��Ȃ��j
        [SerializeField] private bool isDebug;

        [SerializeField] private Rigidbody2D rb;

        //���ꂼ��̃L�����N�^�[���i�[
        [SerializeField] private KasenManager kasen;
        [SerializeField] private SuikaManager suika;
        [SerializeField] private YugiManager yugi;

        //�q�I�u�W�F�N�g���i�[�iSprite�̐؂�ւ��ɕK�v�j
        private List<GameObject> characterList = new List<GameObject>();

        //���ݑ��쒆�̃L�����N�^�[���i�[
        private CharacterEnum nowPlayer;

        //����\�̓Q�[�W
        [SerializeField] private float specialPower;
        [SerializeField] private float maxSpecialPower;

        //�_�b�V���p�̃A�j���[�V����
        [SerializeField] private GameObject[] dashAnimetion;

        //�L�������L�̃f�[�^
        private float moveSpeed;
        private float jumpPower;
        private float gaugeAcceleration;

        //���C�t�֘A
        private int lifeCount;
        private bool isDamaging;

        //�{�^���̉������ςȂ���h�~����悤
        private bool jumpChecker;
        //�W�����v�p�̃^�C�}�[
        private float jumpTimer;
        //�ڒn���Ă邩�ǂ���
        private bool isOnGround;
        //����\�͂𔭓�����(���\��������)
        private bool isActivatingSpecialAbilities;
        //��i�W�����v���\��
        private bool canDoubleJump;
        //��i�W�����v���Ă����ǂ���
        private bool isDouble;
        //��Q���𖳎��ł��邩
        private bool ignoreObstacle;
        //�G����̍U���𖳎��ł��邩
        private bool ignoreEnemyAttack;
        
        //UI�ύX����p�̈Ϗ�
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

            //�_�b�V���A�j���[�V�����p
            dashAnimetion[0].SetActive(false);
            dashAnimetion[1].SetActive(false);

            //�X�s�[�h�֘A
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
            //�ؐ�݂̂�\��������
            for(int i = 1; i < 3; i++)
            {
                characterList[i].SetActive(false);
            }

            //�f�[�^��ݒ肷��
            SetCharacterData(kasen);
            nowPlayer = CharacterEnum.Kasen;
        }

        // Update is called once per frame
        void Update()
        {
            //�X�y�V�����Q�[�W�̑���
            SpecialPower += Time.deltaTime * gaugeAcceleration;

            //�L�����N�^�[�ړ��̏���
            if (Input.GetKey(KeyCode.L) || Input.GetKey(KeyCode.RightArrow))
            {
                IncreaseSpeed();
            }
            else if (Input.GetKey(KeyCode.K) || Input.GetKey(KeyCode.LeftArrow))
            {
                DecreaseSpeed();
            }

            //�L�����N�^�[�̕ύX�̏���
            if (Input.GetKeyDown(KeyCode.A) && SpecialPower >= 12 && !isActivatingSpecialAbilities�@&& !isDamaging)
            {
                specialPower -= 12;
                ChangePlayer();
            }

            //�W�����v�֘A�̏���
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
                //�ʏ�̃W�����v
                if(!canDoubleJump)
                {
                    if (isOnGround)
                    {
                        Jump(jumpTimer);
                    }
                }
                //��i�W�����v��p�̏���
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
                            //Debug.Log("��i�W�����v");
                            DoubleJump(jumpTimer);
                            isDouble = true;
                        }
                    }
                }                
            }

            //S�������������\�͔���
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

        //���삷��L�����N�^�[��ύX���郁�\�b�h
        private void ChangePlayer()
        {
            switch (nowPlayer)
            {
                case CharacterEnum.Kasen:
                    //�\������L�����̓���ւ�
                    characterList[0].SetActive(false);
                    characterList[1].SetActive(true);
                    //�f�[�^�̓���ւ�
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
            //UI��ύX����
            changeCharacter(nowPlayer);
        }

        //�L�����N�^�[���W�����v������
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

        //��i�W�����v��p�̏���
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

        //�L�����N�^�[�̃f�[�^������i�X�^�[�g�E�L�����ύX���E����\�͔������Ɏg�p�j
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

        //�L������O���Ɋ��߂鏈��
        private void IncreaseSpeed()
        {
            //�L�����ɂ���Ĉړ��͈͂�ς���
            float defaltX = 3f + moveSpeed * 0.5f;
            if (transform.position.x > defaltX || !isOnGround) return;
            transform.position += new Vector3(1, 0, 0) * 5f * Time.deltaTime;
        }

        //�ړI�̑��x�܂Ō���������
        private void DecreaseSpeed()
        {
            float defaltX = -5f + moveSpeed * 0.5f * -1;
            if (transform.position.x < defaltX || !isOnGround) return;
            transform.position -= new Vector3(1, 0, 0) * 5f * Time.deltaTime;
        }

        //���񂾂Ƃ��̏���
        private IEnumerator OnRetry()
        {
            Debug.Log("���񂾂�");
            if (isDebug) yield break;

            SePlayer.Instance.Play(5);

            yield return new WaitForSeconds(0.25f);
            SceneManager.LoadScene("ReStartScene");
        }

        //����\�͂𔭓�������Ƃ��̏���
        private void StartSpecialAbilities(CharacterBase charaData)
        {
            isActivatingSpecialAbilities = true;
            SetCharacterData(charaData);
        }

        //����\�͂��I��������Ƃ��̏���
        private void EndSpecialAbilities(CharacterBase charaData)
        {
            isActivatingSpecialAbilities = false;
            SetCharacterData(charaData);
        }

        //�ؐ�̓���\��
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

        //���̓���\��
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
            //ChangeSpecialParameter()�̌�Ɏ��s������
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

        //�E�V�̓���\��
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

        //�_���[�W���󂯂��Ƃ��̏���
        private IEnumerator TakeDamagedCoroutine()
        {
            Debug.Log("�_���[�W�󂯂���");

            //Sprite��_�ł�����O����
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

            //Sprite��_��������
            for (int i = 0; i < 20; i++)
            {
                //flashInterval�҂��Ă���
                yield return new WaitForSeconds(0.075f);
                //spriteRenderer���I�t
                foreach(var renderer in renderers)
                {
                    renderer.enabled = false;
                }

                //flashInterval�҂��Ă���
                yield return new WaitForSeconds(0.075f);
                //spriteRenderer���I��
                foreach (var renderer in renderers)
                {
                    renderer.enabled = true;
                }
            }

            isDamaging = false;

            yield break;
        }

        //�ڒn�����Ƃ��̏���
        public void ChangeIsOnGround(PlayerRayManager player,bool result)
        {
            if (player)
            {
                isOnGround = result;
            }
        }

        //�U�����ꂽor��Q���Ɠ��������Ƃ��̏���
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
