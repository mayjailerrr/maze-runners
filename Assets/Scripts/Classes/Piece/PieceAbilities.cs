
using System.Collections.Generic;
using MazeRunners;
using System;
public static class PieceAbilities
{
    private static readonly Dictionary<AbilityName, IAbility> Abilities = new Dictionary<AbilityName, IAbility>
    {
        { AbilityName.Teleport, new TeleportAbility() }, 
        { AbilityName.SpeedBoost, new SpeedBoostAbility() }, 
        // { AbilityName.EtherealShield, ReducesDamageFromDamageTrapsForTwoTurns },
        // { AbilityName.SwiftWind, AugmentSpeedForOneTurn }, 
        // { AbilityName.IllusoryEcho, CreateCloneAndDistractEnemyOneTurn }
    };

    public static IAbility GetAbility(AbilityName abilityName)
    {
        if (Abilities.ContainsKey(abilityName))
        {
            return Abilities.TryGetValue(abilityName, out var ability) ? ability : null;
        }

        return null;
    }

}