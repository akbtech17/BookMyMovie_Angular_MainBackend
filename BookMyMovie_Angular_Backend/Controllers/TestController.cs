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
		CustomerController customerController = new CustomerController();

		[HttpGet]
		[Route("set-database")]
		public IActionResult SetDatabase()
		{
			try
			{
				db.Database.ExecuteSqlInterpolated($"AKBSetDB");
				Akbcustomer customer1 = new Akbcustomer();
				customer1.Email = "akb.tech17@gmail.com";
				customer1.Password = "12@usT34";
				customer1.FirstName = "Anshul";
				customer1.Gender = "Male";
				customerController.Register(customer1);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.InnerException.Message);
			}
			return Ok("Anshul have set the Database\n" +
				"Keep Working\n" +
				"All the Best");
		}
	}
}
