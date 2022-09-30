using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookMyMovie_Angular_Backend.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        BookMyMovieContext db = new BookMyMovieContext();

        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            try
            {
                var data = db.Movies.ToList();
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
                var data = db.Movies.Find(id);
                return Ok(data);
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
                var data = db.Movies.Where(m => m.MovieId == id).FirstOrDefault();
                if (data != null)
                {
                    db.Movies.Remove(data);
                    db.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex) {
                return BadRequest("Error " + ex.InnerException.Message);
            }
        }
    }
}
