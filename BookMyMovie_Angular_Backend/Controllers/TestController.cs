using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookMyMovie_Angular_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		Db01Context db = new Db01Context();

		[HttpGet]
		[Route("set-database")]
		public IActionResult Get()
		{
			try
			{
				db.Database.ExecuteSqlInterpolated($"AKBSetDB");
			}
			catch (Exception ex)
			{
				return BadRequest("Error " + ex.InnerException.Message);
			}
			return Ok("Anshul have set the Database\n" +
				"Keep Working\n" +
				"All the Best");
		}
	}
}
