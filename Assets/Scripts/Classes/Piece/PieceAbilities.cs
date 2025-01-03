
using System.Collections.Generic;
using MazeRunners;
using System;
public static class PieceAbilities
{
    private static readonly Dictionary<AbilityName, IAbility> Abilities = new Dictionary<AbilityName, IAbility>
    {
        { AbilityName.Teleport, new TeleportAbility() }, 
        { AbilityName.SpeedBoost, new SpeedBoostAbility() }, 
        { AbilityName.Bomb, new BombAbility() }, 
        { AbilityName.Freeze, new FreezeAbility() }, 
        { AbilityName.WallBuilder, new WallBuilderAbility() }, 
        { AbilityName.WallDestroyer, new WallDestroyerAbility() }, 
        { AbilityName.Shield, new ShieldAbility() }, 
        { AbilityName.Clone, new CloneAbility() }, 
        { AbilityName.Invisibility, new InvisibilityAbility() }, 
        { AbilityName.Absorb, new AbsorbAbilitiesAbility() },
        { AbilityName.HealthDamage, new HealthDamageAbility() }
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