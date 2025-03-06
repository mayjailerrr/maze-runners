using System.Collections.Generic;

public static class CollectibleFactory
{
    public static List<Collectible> CreateCollectibles(Movies movie, int playerID)
    {
        List<Collectible> collectibles = new List<Collectible>();

        switch (movie)
        {
            case Movies.Ponyo:
                collectibles.Add(new Collectible(CollectibleName.Ham, "A piece of ham, Ponyo's favorite!"));
                collectibles.Add(new Collectible(CollectibleName.Potion, "Drink this to rule the 7 seas!"));
                collectibles.Add(new Collectible(CollectibleName.Ship, "A toy ship from Sosuke"));
                break;

            case Movies.HowlsMovingCastle:
                collectibles.Add(new Collectible(CollectibleName.Calcifer, "The magical star Calcifer"));
                collectibles.Add(new Collectible(CollectibleName.Feather, "A feather from Howl"));
                collectibles.Add(new Collectible(CollectibleName.MagicDoor, "A door to everywhere"));
                break;

            case Movies.PrincessMononoke:
                collectibles.Add(new Collectible(CollectibleName.CrystalDagger, "A dagger from Ashitaka"));
                collectibles.Add(new Collectible(CollectibleName.Kodama, "A small tree spirit"));
                collectibles.Add(new Collectible(CollectibleName.MononokeMask, "A mask worn by San"));
                break;

            case Movies.KikisDeliveryService:
                collectibles.Add(new Collectible(CollectibleName.Cage, "Inside there is a doll black cat or a real black cat?"));
                collectibles.Add(new Collectible(CollectibleName.Radio, "A radio from Kiki's dad"));
                collectibles.Add(new Collectible(CollectibleName.RedShoes, "The iconic red shoes"));
                break;
            
            case Movies.SpiritedAway:
                collectibles.Add(new Collectible(CollectibleName.BathTokens, "Tokens from the bathhouse"));
                collectibles.Add(new Collectible(CollectibleName.Hairband, "A hairband from Zeniba"));
                collectibles.Add(new Collectible(CollectibleName.Susuwatari, "A soot sprite"));
                break;
            
            case Movies.MyNeighborTotoro:
                collectibles.Add(new Collectible(CollectibleName.Acorn, "A magical acorn from Totoro"));
                collectibles.Add(new Collectible(CollectibleName.ChibiTotoro, "A small Totoro"));
                collectibles.Add(new Collectible(CollectibleName.Corn, "A corn for mom"));
                break;
            
            case Movies.PorcoRosso:
                collectibles.Add(new Collectible(CollectibleName.Hydroplane, "Curtis' hydroplane"));
                collectibles.Add(new Collectible(CollectibleName.SmallPlane, "Porco's small plane"));
                collectibles.Add(new Collectible(CollectibleName.BrownRadio, "A radio from Porco"));
                break;
            
            case Movies.Arietty:
                collectibles.Add(new Collectible(CollectibleName.Teacup, "A tiny teacup from the Borrowers"));
                collectibles.Add(new Collectible(CollectibleName.NeedleSword, "A sword made from a needle"));
                collectibles.Add(new Collectible(CollectibleName.SugarCube, "A large sugar cube for Arietty"));
                break;

            default:
                UnityEngine.Debug.LogError("Movie not found in CollectibleFactory.");
                break;
        }

        foreach (Collectible collectible in collectibles)
        {
            collectible.AssignPlayerID(playerID);
        }

        return collectibles;
    }
}
