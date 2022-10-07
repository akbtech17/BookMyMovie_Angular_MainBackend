using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace BookMyMovie_Angular_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        Db01Context db = new Db01Context();

        [HttpPost]
        [Route("signin")]
        public IActionResult ValidateSignIn(Credentials creds) {
            try
            {
                string encodedPassword = EncodePasswordToBase64(creds.Password);
                Akbcustomer customerData = db.Akbcustomers
                    .Where(c => c.Email.ToLower().Equals(creds.Email) && c.Password.Equals(encodedPassword))
                    .FirstOrDefault();
                if (customerData == null) return NotFound();
                return Ok(customerData);
            }
            catch (Exception ex) {
                return BadRequest(ex.InnerException.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(Akbcustomer customerData)
        {
            try
            {
                customerData.CustomerId = null;
                customerData.Password = EncodePasswordToBase64(customerData.Password);
                db.Akbcustomers.Add(customerData);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

		public static string EncodePasswordToBase64(string password)
		{
			try
			{
				byte[] encData_byte = new byte[password.Length];
				encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
				string encodedData = Convert.ToBase64String(encData_byte);
				return encodedData;
			}
			catch (Exception ex)
			{
				throw new Exception("Error in base64Encode" + ex.Message);
			}
		}

		public string DecodeFrom64(string encodedData)
		{
			System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
			System.Text.Decoder utf8Decode = encoder.GetDecoder();
			byte[] todecode_byte = Convert.FromBase64String(encodedData);
			int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
			char[] decoded_char = new char[charCount];
			utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
			string result = new String(decoded_char);
			return result;
		}
	}

    public class Credentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
