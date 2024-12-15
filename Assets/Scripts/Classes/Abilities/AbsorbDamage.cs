using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class AbsorbDamageAbility : IAbility
{
    public string Description => "Absorbs the damage of the own player pieces";

    public bool Execute(Context context)
    {
        return true;
    }
}