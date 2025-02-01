using System.Collections.Generic;

namespace MazeRunners
{
    public class MovieSelectionModel
    {
        private List<Movies> availableMovies;

        public MovieSelectionModel()
        {
            availableMovies = new List<Movies>
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
        }

        public bool IsMovieAvailable(Movies movie)
        {
            return availableMovies.Contains(movie);
        }

        public void AssignMovieToPlayer(int playerId, Movies movie)
        {
            if (!IsMovieAvailable(movie))
                return;

            availableMovies.Remove(movie);
        }

        public bool CanSelectMovie()
        {
            return GameManager.Instance.PlayerCount < 4;
        }

    }
}
