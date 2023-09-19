using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FrenzyStatusEffects : StatusEffects
{
    private void Start() {
        SetDuration(5f);
        SetSpeedModifier(2f);
        SetColor(Color.red);
    }
}
