using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class InvisibilityAbility : IAbility
{
    public string Description => "Makes the piece invisible for x turns.";

    public bool Execute(Context context)
    {
        return true;
    }
}