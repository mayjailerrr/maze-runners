using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class WallDestroyerAbility : IAbility
{
    public string Description => "Destroys a wall that blocks the path of the player.";

    public bool Execute(Context context)
    {
        return true;
    }
}