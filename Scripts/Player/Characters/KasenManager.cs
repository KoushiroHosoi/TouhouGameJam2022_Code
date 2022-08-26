using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;

namespace HamuGame
{
    public class KasenManager : CharacterBase
    {
        //�Ԃ�l����Ȃ̂�Func
        public event Func<IEnumerator> playDoubleJump;

        //����\�͂��㏑��
        public override void ActivateSpecialAbilities()
        {
            StartCoroutine(playDoubleJump());
        }
    }
}
