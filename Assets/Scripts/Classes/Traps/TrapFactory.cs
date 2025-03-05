
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
            case TrapType.CodeLock:
                return new CodeLockTrap();
            case TrapType.Memory:
                return new MemoryTrap();
            case TrapType.Blindness:
                return new BlindnessTrap();
            case TrapType.Damage:
                return new DamageTrap(1);
            default:
                throw new ArgumentException("Unknown trap type.");
        }
    }

    public static ITrapEffect CreateRandomTrap()
    {
        TrapType[] trapTypes = { TrapType.Damage, TrapType.CodeLock, TrapType.Memory, TrapType.Blindness };
        TrapType selectedType = trapTypes[random.Next(trapTypes.Length)];
        return CreateTrap(selectedType);
    }
}
