using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookMyMovie_Angular_Backend.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        Db01Context db = new Db01Context();

        [HttpGet]
        [Route("")]
        public IActionResult GetMovieList()
        {
            try
            {
                List<Akbmovie> movies = db.Akbmovies.ToList();
                return Ok(movies);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpGet]
        [Route("{movieId}")]
        public IActionResult GetMovieDetails(int movieId)
        {
            try
            {
                Akbmovie movieDetails = db.Akbmovies.Where(movie => movie.MovieId == movieId).FirstOrDefault();
                if (movieDetails == null) return NotFound();
                return Ok(movieDetails);
			}
            catch (Exception ex)
            {
                return BadRequest("Error " + ex.InnerException.Message);
            }
        }

        [HttpPost]
        public IActionResult AddMovie(Akbmovie movieDetails)
        {
            try
            {
                movieDetails.MovieId = null;
                db.Akbmovies.Add(movieDetails);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateMovie(Akbmovie movieDetails)
        {
            try
            {
                Akbmovie movieOldDetails = db.Akbmovies
                    .Where(m => m.MovieId == movieDetails.MovieId)
                    .FirstOrDefault();
                if (movieOldDetails != null)
                {   
                    movieOldDetails.MovieName = movieDetails.MovieName;
                    movieOldDetails.ReleaseDate = movieDetails.ReleaseDate; 
                    movieOldDetails.Ratings = movieDetails.Ratings; 
                    movieOldDetails.Genres = movieDetails.Genres; 
                    movieOldDetails.ImageUrl = movieDetails.ImageUrl; 
                    movieOldDetails.CostPerSeat = movieDetails.CostPerSeat; 
                    movieOldDetails.ShowTime = movieDetails.ShowTime;
                    movieOldDetails.Duration = movieDetails.Duration;
                    movieOldDetails.AgeRating = movieDetails.AgeRating;
                    movieOldDetails.Language = movieDetails.Language;
                    movieOldDetails.MovieType = movieDetails.MovieType;
                    movieOldDetails.IsAdult = movieDetails.IsAdult;
				}
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

		[HttpDelete]
		[Route("{id}")]
		public IActionResult DeleteMovie(int id)
		{
			try
			{
                // check the transaction
                AkbtransactionDetail transactionDetail = db.AkbtransactionDetails.Where(t => t.MovieId == id).FirstOrDefault();
                if (transactionDetail != null) return BadRequest("Can't delete as there are transactions associated with this movie");
                // remove seats 
                List<AkbseatMap> movieSeats = db.AkbseatMaps.Where(s => s.MovieId == id).ToList();
                foreach (AkbseatMap akbseat in movieSeats)
                {
                    db.AkbseatMaps.Remove(akbseat);
                }
                db.SaveChanges();
				Akbmovie movieDetails = db.Akbmovies
					.Where(m => m.MovieId == id)
					.FirstOrDefault();

				if (movieDetails != null)
				{
					db.Akbmovies.Remove(movieDetails);
					db.SaveChanges();
				}
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest("Error " + ex.InnerException.Message);
			}
		}

        [HttpGet]
        [Route("seatmap/{movieId}")]
        public IActionResult GetSeatMapOfMovieByMovieId(int movieId)
        {
            try
            {
                var seatMap = db.AkbseatMaps.Where(seat => seat.MovieId == movieId).Select(seatDetails => new { seatDetails.SeatNo, seatDetails.Status});
                return Ok(seatMap);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.InnerException.Message);
            }
        }
	}
}
