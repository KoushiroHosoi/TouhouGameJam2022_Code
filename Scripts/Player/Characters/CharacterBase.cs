using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;

namespace HamuGame
{
    //キャラクターの基本データ(親クラス)
    public class CharacterBase : MonoBehaviour
    {
        [SerializeField] protected float normalSpeed;
        [SerializeField] protected float specialSpeed;
        [SerializeField] protected float normalJumpPower;
        [SerializeField] protected float specialJumpPower;
        [SerializeField] protected float normalGaugeAcceleration;
        [SerializeField] protected float specialnormalGaugeAcceleration;


        public float NormalSpeed { get => normalSpeed; }
        public float SpecialSpeed { get => specialSpeed; }
        public float NormalJumpPower { get => normalJumpPower; }
        public float SpecialJumpPower { get => specialJumpPower; }
        public float NormalGaugeAcceleration { get => normalGaugeAcceleration; }
        public float SpecialnormalGaugeAcceleration { get => specialnormalGaugeAcceleration; }

        public virtual void ActivateSpecialAbilities()
        {
            Debug.Log("上書きされてないよ！");
        }
    }
}
