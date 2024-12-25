using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class AbsorbAbilitiesAbility : IAbility
{
    public string Description => "Absorbs the ability of the piece it attacks.";

    public bool Execute(Context context)
    {
       return true;
    }
}