using System.Collections.Generic;
using MazeRunners;
using UnityEngine;

public static class PieceFactory
{
    public static List<Piece> CreatePieces(Movies movie)
    {
        List<Piece> pieces = new List<Piece>();
        
        switch (movie)
        {
            case Movies.Ponyo:
                pieces.Add(new Piece("Ponyo", 10, 3, PieceAbilities.GetAbility(AbilityName.AbsorbAbility)));
                pieces.Add(new Piece("Sosuke", 10, 1, PieceAbilities.GetAbility(AbilityName.Shield)));
                pieces.Add(new Piece("Granmamare", 10, 3, PieceAbilities.GetAbility(AbilityName.Clone)));
                break;

            case Movies.HowlsMovingCastle:
                pieces.Add(new Piece("Howl", 5, 4,  PieceAbilities.GetAbility(AbilityName.RampartBuilder)));
                pieces.Add(new Piece("Sophie", 3, 1, PieceAbilities.GetAbility(AbilityName.AbsorbDamage)));
                pieces.Add(new Piece("TurnipHead", 2, 1, PieceAbilities.GetAbility(AbilityName.SpeedBoost)));
                break;

            case Movies.PrincessMononoke:
                pieces.Add(new Piece("San", 3, 2, PieceAbilities.GetAbility(AbilityName.HealthDamage)));
                pieces.Add(new Piece("Ashitaka", 3, 2, PieceAbilities.GetAbility(AbilityName.SpeedBoost)));
                pieces.Add(new Piece("Eboshi", 2, 1, PieceAbilities.GetAbility(AbilityName.WallBomb)));
                break;
            
            case Movies.KikisDeliveryService:
                pieces.Add(new Piece("Kiki", 5, 3, PieceAbilities.GetAbility(AbilityName.Teleport)));
                pieces.Add(new Piece("Tombo", 2, 3, PieceAbilities.GetAbility(AbilityName.SpeedBoost)));
                pieces.Add(new Piece("Jiji", 2, 3, PieceAbilities.GetAbility(AbilityName.Clone)));
                break;

            case Movies.SpiritedAway:
                pieces.Add(new Piece("Chihiro", 3, 1, PieceAbilities.GetAbility(AbilityName.AbsorbDamage)));
                pieces.Add(new Piece("Haku", 5, 3, PieceAbilities.GetAbility(AbilityName.Freeze)));
                pieces.Add(new Piece("NoFace", 1, 1, PieceAbilities.GetAbility(AbilityName.Invisibility)));
                break;

            case Movies.MyNeighborTotoro:
                pieces.Add(new Piece("Totoro", 5, 2, PieceAbilities.GetAbility(AbilityName.Invisibility)));
                pieces.Add(new Piece("Satsuki", 5, 2, PieceAbilities.GetAbility(AbilityName.Shield)));
                pieces.Add(new Piece("Mei", 5, 2, PieceAbilities.GetAbility(AbilityName.WallBuilder)));
                break;
            
            case Movies.PorcoRosso:
                pieces.Add(new Piece("Porco", 3, 2, PieceAbilities.GetAbility(AbilityName.WallBuilder)));
                pieces.Add(new Piece("Fio", 2, 2, PieceAbilities.GetAbility(AbilityName.Shield)));
                pieces.Add(new Piece("Gina", 2, 2, PieceAbilities.GetAbility(AbilityName.Clone)));
                break;

            case Movies.Arietty:
                pieces.Add(new Piece("Arietty", 4, 3, PieceAbilities.GetAbility(AbilityName.WallDestroyer)));
                pieces.Add(new Piece("Sho", 5, 2, PieceAbilities.GetAbility(AbilityName.Shield)));
                pieces.Add(new Piece("Spiller", 5, 3, PieceAbilities.GetAbility(AbilityName.HealthDamage)));
                break;
            
            
            default:
                Debug.LogError("Movie not found in PieceFactory.");
                break;
        }

        return pieces;
    }
}
