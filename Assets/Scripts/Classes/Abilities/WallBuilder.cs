using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class WallBuilderAbility : IAbility
{
    public string Description => "Builds a wall that blocks the path of the other player.";

    public bool Execute(Context context)
    {
        return true;
    }
}