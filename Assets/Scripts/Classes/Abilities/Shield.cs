using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class ShieldAbility : IAbility
{
    public string Description => "Shield the piece from damage for x turns.";

    public bool Execute(Context context)
    {
        return true;
    }
}