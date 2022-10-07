using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookMyMovie_Angular_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        Db01Context db = new Db01Context();

        [HttpPost]
        [Route("signin")]
        public IActionResult ValidateSignIn(Credentials creds)
        {
            try
            {
                Akbadmin adminData = db.Akbadmins
                    .Where(c => c.Email.ToLower().Equals(creds.Email) && c.Password.Equals(creds.Password))
                    .FirstOrDefault();
                if (adminData == null) return NotFound();
                return Ok(adminData);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }
	}
}
