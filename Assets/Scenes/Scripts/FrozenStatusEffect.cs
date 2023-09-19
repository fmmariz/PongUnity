using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FrozenStatusEffects : StatusEffects
{
    private void Start() {
        SetDuration(2f);
        SetSpeedModifier(0f);
        SetColor(Color.blue);
    }
}
