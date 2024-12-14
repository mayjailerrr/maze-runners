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
                pieces.Add(new Piece("Ponyo", 4, 3, null));
                pieces.Add(new Piece("Sosuke", 2, 1, PieceAbilities.GetAbility(AbilityName.SpeedBoost)));
                pieces.Add(new Piece("Granmamare", 5, 3, null));
                break;

            case Movies.HowlsMovingCastle:
                pieces.Add(new Piece("Howl", 5, 4, null));
                pieces.Add(new Piece("Sophie", 3, 1, null));
                pieces.Add(new Piece("TurnipHead", 2, 1, null));
                break;

            // case Movies.PrincessMononoke:
            //     pieces.Add(new Piece("San", 3, 2));
            //     pieces.Add(new Piece("Ashitaka", 3, 2));
            //     pieces.Add(new Piece("Wolf", 2, 1));
            //     break;
            
            // case Movies.KikisDeliveryService:
            //     pieces.Add(new Piece("Kiki", 5, 3));
            //     pieces.Add(new Piece("Tombo", 2, 3));
            //     pieces.Add(new Piece("Jiji", 2, 3));
            //     break;

            // case Movies.SpiritedAway:
            //     pieces.Add(new Piece("Chihiro", 3, 1));
            //     pieces.Add(new Piece("Haku", 5, 3));
            //     pieces.Add(new Piece("No-Face", 1, 1));
            //     break;

            // case Movies.MyNeighborTotoro:
            //     pieces.Add(new Piece("Totoro", 5));
            //     pieces.Add(new Piece("Satsuki",));
            //     pieces.Add(new Piece("Mei", ));
            //     break;
            
            // case Movies.PorcoRosso:
            //     pieces.Add(new Piece("", ));
            //     pieces.Add(new Piece("",));
            //     pieces.Add(new Piece("", ));
            //     break;

            // case Movies.Arietty:
            //     pieces.Add(new Piece("Arietty", 4 ));
            //     pieces.Add(new Piece("",));
            //     pieces.Add(new Piece("", ));
            //     break;
            

            default:
                Debug.LogError("Movie not found in PieceFactory.");
                break;
        }

        return pieces;
    }
}
