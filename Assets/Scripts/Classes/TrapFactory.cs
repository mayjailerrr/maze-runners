using System;
using System.Collections.Generic;

public static class TrapFactory
{
    private static readonly List<Func<ITrapEffect>> TrapGenerators = new List<Func<ITrapEffect>>()
    {
        () => new FreezeTrap(),
        () => new DamageTrap(),
        () => new SlowTrap()
    };

    private static readonly Random Random = new Random();

    public static ITrapEffect CreateRandomTrap()
    {
        int index = Random.Next(TrapGenerators.Count);
        return TrapGenerators[index]();
    }
}
