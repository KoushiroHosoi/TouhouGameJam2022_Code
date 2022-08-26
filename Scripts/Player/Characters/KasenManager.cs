using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HamuGame;
using System;

namespace HamuGame
{
    public class KasenManager : CharacterBase
    {
        //•Ô‚è’l‚ ‚è‚È‚Ì‚ÅFunc
        public event Func<IEnumerator> playDoubleJump;

        //“Áê”\—Í‚ğã‘‚«
        public override void ActivateSpecialAbilities()
        {
            StartCoroutine(playDoubleJump());
        }
    }
}
