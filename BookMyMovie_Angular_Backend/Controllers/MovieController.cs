using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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
        public IActionResult Get()
        {
            try
            {
                var data = db.Akbmovies.ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("Error " + ex.InnerException.Message);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var data = db.Akbmovies.Find(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest("Error " + ex.InnerException.Message);
            }
        }

        [HttpPost]
        public IActionResult InsertMovie(Akbmovie movie)
        {
            try
            {
                movie.MovieId = null;
                db.Akbmovies.Add(movie);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Error " + ex.InnerException.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteMovie(int id) {
            try
            {
                var data = db.Akbmovies.Where(m => m.MovieId == id).FirstOrDefault();
                if (data != null)
                {
                    db.Akbmovies.Remove(data);
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex) {
                return BadRequest("Error " + ex.InnerException.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateMovie(Akbmovie new_data)
        {
            try
            {
                var old_data = db.Akbmovies.Where(m => m.MovieId == new_data.MovieId).FirstOrDefault();
                if (old_data != null)
                {   
                    old_data.MovieName = new_data.MovieName;
                    old_data.ReleaseDate = new_data.ReleaseDate; 
                    old_data.Ratings = new_data.Ratings; 
                    old_data.Genres = new_data.Genres; 
                    old_data.ImageUrl = new_data.ImageUrl; 
                    old_data.CostPerSeat = new_data.CostPerSeat; 
                    old_data.ShowTime = new_data.ShowTime;
                    old_data.Duration = new_data.Duration;
                    old_data.AgeRating = new_data.AgeRating;
                    old_data.Language = new_data.Language;
                    old_data.MovieType = new_data.MovieType;
                }
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Error " + ex.InnerException.Message);
            }
        }
    }
}
