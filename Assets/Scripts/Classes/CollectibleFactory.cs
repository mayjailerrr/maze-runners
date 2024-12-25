using System.Collections.Generic;
using MazeRunners;

public static class CollectibleFactory
{
    public static List<Collectible> CreateCollectibles(Movies movie)
    {
        List<Collectible> collectibles = new List<Collectible>();

        switch (movie)
        {
            case Movies.Ponyo:
                collectibles.Add(new Collectible("Ham", "A piece of ham, Ponyo's favorite!"));
                collectibles.Add(new Collectible("Potion", "Drink this to rule the 7 seas!"));
                collectibles.Add(new Collectible("Ship", "A toy ship from Sosuke"));
                break;

            case Movies.HowlsMovingCastle:
                collectibles.Add(new Collectible("Calcifer", "The magical star Calcifer"));
                collectibles.Add(new Collectible("Feather", "A feather from Howl"));
                collectibles.Add(new Collectible("Magic Door", "A door to everywhere"));
                break;

            case Movies.PrincessMononoke:
                collectibles.Add(new Collectible("Crystal Dagger", "A dagger from Ashitaka"));
                collectibles.Add(new Collectible("Kodama", "A small tree spirit"));
                collectibles.Add(new Collectible("Mononoke Mask", "A mask worn by San"));
                break;

            case Movies.KikisDeliveryService:
                collectibles.Add(new Collectible("Cage", "Inside there is a doll black cat or a real black cat?"));
                collectibles.Add(new Collectible("Radio", "A radio from Kiki's dad"));
                collectibles.Add(new Collectible("Red Shoes", "The iconic red shoes"));
                break;

            case Movies.SpiritedAway:
                collectibles.Add(new Collectible("Bath Tokens", "Tokens from the bathhouse"));
                collectibles.Add(new Collectible("Hairband", "A hairband from Zeniba"));
                collectibles.Add(new Collectible("Susuwatari", "A soot sprite"));
                break;

            case Movies.MyNeighborTotoro:
                collectibles.Add(new Collectible("Acorn", "A magical acorn from Totoro"));
                collectibles.Add(new Collectible("Chibi-Totoro", "A small Totoro"));
                collectibles.Add(new Collectible("Corn", "A corn for mom"));
                break;

            case Movies.PorcoRosso:
                collectibles.Add(new Collectible("Pilot Goggles", "Porco's goggles"));
                collectibles.Add(new Collectible("Propeller", "A propeller from Porco's plane"));
                collectibles.Add(new Collectible("Amelia's Scarf", "A scarf from Amelia's aviator outfit"));
                break;

            case Movies.Arietty:
                collectibles.Add(new Collectible("Teacup", "A tiny teacup from the Borrowers"));
                collectibles.Add(new Collectible("Needle Sword", "A sword made from a needle"));
                collectibles.Add(new Collectible("Sugar Cube", "A large sugar cube for Arietty"));
                break;

            default:
                UnityEngine.Debug.LogError("Movie not found in CollectibleFactory.");
                break;
        }

        return collectibles;
    }
}
