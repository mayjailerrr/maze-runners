using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class CloneAbility : IAbility
{
    public string Description => "Clones the piece and creates a new piece with the same abilities.";

    public bool Execute(Context context)
    {
        return true;
    }
}