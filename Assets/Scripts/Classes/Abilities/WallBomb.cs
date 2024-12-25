using System;
using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public class BombAbility : IAbility
{
    public string Description => "Explodes, destroying all walls in a 3x3 area around the piece.";

    public bool Execute(Context context)
    {
        return true;
    }
}