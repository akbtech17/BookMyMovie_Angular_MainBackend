﻿using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Http;
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
    }
}
