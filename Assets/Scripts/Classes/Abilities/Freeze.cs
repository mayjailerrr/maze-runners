using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class FreezeAbility : IAbility
{
    public string Description => "Freeze one or more pieces for x turns.";

    public bool Execute(Context context)
    {
        return true;
    }
}