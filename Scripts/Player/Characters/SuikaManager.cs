using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;

namespace HamuGame
{
    public class SuikaManager : CharacterBase
    {
        [SerializeField] private int changeSmallPercent;

        [SerializeField] private float bigSpeed;
        [SerializeField] private float bigJumpPower;
        [SerializeField] private float bigSpecialnormalGaugeAcceleration;

        [SerializeField] private float smallSpeed;
        [SerializeField] private float smallJumpPower;
        [SerializeField] private float smallSpecialnormalGaugeAcceleration;

        public int ChangeSmallPercent { get => changeSmallPercent; }

        public event Func<IEnumerator> changeSize;

        public override void ActivateSpecialAbilities()
        {
            StartCoroutine(changeSize());
        }

        public void ChangeSpecialParameter(bool isBig)
        {
            if (isBig)
            {
                specialSpeed = bigSpeed;
                specialJumpPower = bigJumpPower;
                specialnormalGaugeAcceleration = bigSpecialnormalGaugeAcceleration;
            }
            else
            {
                specialSpeed = smallSpeed;
                specialJumpPower = smallJumpPower;
                specialnormalGaugeAcceleration = smallSpecialnormalGaugeAcceleration;
            }
        }
    }
}
