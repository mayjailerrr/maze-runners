using System;
using System.Collections.Generic;
using System.Linq;

public static class TrapFactory
{
   private static readonly List<ITrapEffect> predefinedTraps = new List<ITrapEffect>
    {
        new SlowTrap(2, 3),  
        new SlowTrap(1, 2),    
        // new DamageTrap(10),    
        // new FreezeTrap(1),    
        // new FreezeTrap(2),     
        // new HealTrap(15),  
        // new DamageTrap(5),     
        // new SwapTrap(),        
        // new DamageTrap(8),    
        new SlowTrap(3, 1)     
    };

    private static readonly Random random = new Random();

    public static ITrapEffect CreateRandomTrap()
    {
        int index = random.Next(predefinedTraps.Count);
        return predefinedTraps[index];
    }

    public static IEnumerable<ITrapEffect> GetPredefinedTraps(int count)
    {
        return predefinedTraps
            .OrderBy(_ => random.Next())
            .Take(count);
    }
}
