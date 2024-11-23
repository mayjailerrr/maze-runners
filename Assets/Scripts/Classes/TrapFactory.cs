using System;
using System.Collections.Generic;

public static class TrapFactory
{
    private static readonly List<Func<Trap>> TrapGenerators = new List<Func<Trap>>()
    {
        () => new FreezeTrap(),
        () => new DamageTrap(),
        () => new SlowTrap()
    };

    private static readonly Random Random = new Random();

    public static Trap CreateRandomTrap()
    {
        int index = Random.Next(TrapGenerators.Count);
        return TrapGenerators[index]();
    }
}
