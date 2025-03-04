// using System;
// using System.Collections.Generic;
// using System.Linq;

// public static class TrapFactory
// {
//    private static readonly List<ITrapEffect> predefinedTraps = new List<ITrapEffect>
//     {
//         new SlowTrap(2, 3),  
//         new SlowTrap(1, 2),
//         new SlowTrap(3, 1),  

//          new SlowTrap(2, 3),  
//         new SlowTrap(1, 2),
//         new SlowTrap(3, 1), 

//         // new DamageTrap(1), 
//         // new DamageTrap(2),     
//         // new DamageTrap(2), 

//         new CodeLockTrap(),  

//         new FreezeTrap(1),    
//         new FreezeTrap(2),  

//         new FreezeTrap(1),    
//         new FreezeTrap(2),      
//     };

//     private static readonly Random random = new Random();

//     public static ITrapEffect CreateRandomTrap()
//     {
//         int index = random.Next(predefinedTraps.Count);
//         return predefinedTraps[index];
//     }

//     public static IEnumerable<ITrapEffect> GetPredefinedTraps(int count)
//     {
//         return predefinedTraps
//             .OrderBy(_ => random.Next())
//             .Take(count);
//     }
// }
using System;
using System.Collections.Generic;
using System.Linq;

public static class TrapFactory
{
    private static readonly Random random = new Random();

    public static ITrapEffect CreateTrap(TrapType type)
    {
        switch (type)
        {
            case TrapType.Freeze:
                return new FreezeTrap(2);
            case TrapType.Slow:
                return new SlowTrap(2, 3);
            case TrapType.CodeLock:
                return new CodeLockTrap();
            case TrapType.Memory:
                return new MemoryTrap();
            default:
                throw new ArgumentException("Unknown trap type.");
        }
    }

    public static ITrapEffect CreateRandomTrap()
    {
        TrapType[] trapTypes = { TrapType.Freeze, TrapType.Slow, TrapType.CodeLock, TrapType.Memory };
        TrapType selectedType = trapTypes[random.Next(trapTypes.Length)];
        return CreateTrap(selectedType);
    }
}
