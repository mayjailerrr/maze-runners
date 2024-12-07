using System.Collections.Generic;
using MazeRunners;

namespace MazeRunners
{
    public class MovieSelectionModel
    {
        public List<Movies> AvailableMovies { get; private set; }
        public Dictionary<int, Movies> PlayerMovieSelections { get; private set; }

        public MovieSelectionModel()
        {
            AvailableMovies = new List<Movies>
            {
                Movies.Ponyo,
                Movies.HowlsMovingCastle,
                Movies.PrincessMononoke,
                Movies.KikisDeliveryService,
                Movies.SpiritedAway,
                Movies.MyNeighborTotoro,
                Movies.PorcoRosso,
                Movies.Arietty
            };
            PlayerMovieSelections = new Dictionary<int, Movies>();
        }

        public bool IsMovieAvailable(Movies movie)
        {
            return AvailableMovies.Contains(movie);
        }

        public void AssignMovieToPlayer(int playerId, Movies movie)
        {
            if (!IsMovieAvailable(movie)) return;

            AvailableMovies.Remove(movie);
            PlayerMovieSelections[playerId] = movie;
        }
    }
}
