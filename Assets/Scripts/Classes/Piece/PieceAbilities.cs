
using System.Collections.Generic;
using MazeRunners;
using System;
public static class PieceAbilities
{
    private static readonly Dictionary<AbilityName, IAbility> Abilities = new Dictionary<AbilityName, IAbility>
    {
        { AbilityName.Teleport, new TeleportAbility() }, 
        { AbilityName.SpeedBoost, new SpeedBoostAbility() }, 
        { AbilityName.AbsorbDamage, new AbsorbDamageAbility() }, 
        { AbilityName.WallBomb, new WallBombAbility() },
        { AbilityName.Freeze, new FreezeAbility() }, 
        { AbilityName.WallBuilder, new WallBuilderAbility() }, 
        { AbilityName.WallDestroyer, new WallDestroyerAbility() }, 
        { AbilityName.Shield, new ShieldAbility() }, 
        { AbilityName.Clone, new CloneAbility() }, 
        { AbilityName.Invisibility, new InvisibilityAbility() }, 
        { AbilityName.AbsorbAbility, new AbsorbAbilitiesAbility() },
        { AbilityName.HealthDamage, new HealthDamageAbility() },
        { AbilityName.RampartBuilder, new RampartBuilderAbility() }
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