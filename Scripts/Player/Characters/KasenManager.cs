using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;

namespace HamuGame
{
    public class KasenManager : CharacterBase
    {
        //返り値ありなのでFunc
        public event Func<IEnumerator> playDoubleJump;

        //特殊能力を上書き
        public override void ActivateSpecialAbilities()
        {
            StartCoroutine(playDoubleJump());
        }
    }
}
