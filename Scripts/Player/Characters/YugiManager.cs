using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;

namespace HamuGame
{
    public class YugiManager : CharacterBase
    {
        public event Func<IEnumerator> startMonsterPower;

        public override void ActivateSpecialAbilities()
        {
            StartCoroutine(startMonsterPower());
        }
    }
}
