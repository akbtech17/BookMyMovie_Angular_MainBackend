using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace BookMyMovie_Angular_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TransactionController : ControllerBase
	{
		Db01Context db = new Db01Context();
		public static string connAzureStorage = "DefaultEndpointsProtocol=https;AccountName=anshulkumarstorage256;AccountKey=PEdBR+R7xlLxZdCrfCmURzVlWLYEUlpmkGdGJP9diqkcyN9uIfxB9ZHnx3L481ULKBYnOMznHlgA+AStix+Sjg==;EndpointSuffix=core.windows.net";

		[HttpGet]
		[Route("")]
		public IActionResult GetAllTransactions() {
			try
			{
				List<AkbtransactionDetail> transactions = db.AkbtransactionDetails.ToList();
				return Ok(transactions);
			}
			catch (Exception ex)
			{
				return BadRequest();
			}
		}

		[HttpGet]
		[Route("{transactionId}")]
		public IActionResult GetTransactionByTId(int? transactionId) {
			try
			{
				TransactionResponse transactionResponse = new TransactionResponse();

				AkbtransactionDetail transactionDetails = db.AkbtransactionDetails.FirstOrDefault(transaction => transaction.TransactionId == transactionId);
				transactionResponse.TransactionTime = transactionDetails.TransactionTime;
				transactionResponse.TransactionId = transactionDetails.TransactionId;


				Akbmovie movieDetails = db.Akbmovies.FirstOrDefault(movie => movie.MovieId == transactionDetails.MovieId);
				transactionResponse.MovieId = movieDetails.MovieId;
				transactionResponse.MovieName = movieDetails.MovieName;
				transactionResponse.ReleaseDate = movieDetails.ReleaseDate;
				transactionResponse.Ratings = movieDetails.Ratings;
				transactionResponse.Genres = movieDetails.Genres;
				transactionResponse.ImageUrl = movieDetails.ImageUrl;
				transactionResponse.CostPerSeat = movieDetails.CostPerSeat;
				transactionResponse.ShowTime = movieDetails.ShowTime;
				transactionResponse.Duration = movieDetails.Duration;
				transactionResponse.AgeRating = movieDetails.AgeRating;
				transactionResponse.Language = movieDetails.Language;
				transactionResponse.MovieType = movieDetails.MovieType;

				Akbcustomer customerDetails = db.Akbcustomers.FirstOrDefault(customer => customer.CustomerId == transactionDetails.CustomerId);
				transactionResponse.CustomerId = customerDetails.CustomerId;
				transactionResponse.Email = customerDetails.Email;
				transactionResponse.FirstName = customerDetails.FirstName;

				List<AkbtransactionSeat> tranSeats = db.AkbtransactionSeats.Where(ts => ts.TransactionId == transactionId).ToList();
				transactionResponse.Seats = new string[tranSeats.Count];
				int cnt = 0;
				foreach (AkbtransactionSeat seat in tranSeats)
				{
					transactionResponse.Seats[cnt++] = seat.SeatNo;
				}

				transactionResponse.TotalCost = cnt * transactionResponse.CostPerSeat;
				return Ok(transactionResponse);
			}
			catch (Exception ex) 
			{
				return BadRequest(ex.InnerException.Message);
			}
		}

		public TransactionResponse HelpPostTransaction(int? transactionId)
		{
			try
			{
				TransactionResponse transactionResponse = new TransactionResponse();

				AkbtransactionDetail transactionDetails = db.AkbtransactionDetails.FirstOrDefault(transaction => transaction.TransactionId == transactionId);
				transactionResponse.TransactionTime = transactionDetails.TransactionTime;
				transactionResponse.TransactionId = transactionDetails.TransactionId;


				Akbmovie movieDetails = db.Akbmovies.FirstOrDefault(movie => movie.MovieId == transactionDetails.MovieId);
				transactionResponse.MovieId = movieDetails.MovieId;
				transactionResponse.MovieName = movieDetails.MovieName;
				transactionResponse.ReleaseDate = movieDetails.ReleaseDate;
				transactionResponse.Ratings = movieDetails.Ratings;
				transactionResponse.Genres = movieDetails.Genres;
				transactionResponse.ImageUrl = movieDetails.ImageUrl;
				transactionResponse.CostPerSeat = movieDetails.CostPerSeat;
				transactionResponse.ShowTime = movieDetails.ShowTime;
				transactionResponse.Duration = movieDetails.Duration;
				transactionResponse.AgeRating = movieDetails.AgeRating;
				transactionResponse.Language = movieDetails.Language;
				transactionResponse.MovieType = movieDetails.MovieType;

				Akbcustomer customerDetails = db.Akbcustomers.FirstOrDefault(customer => customer.CustomerId == transactionDetails.CustomerId);
				transactionResponse.CustomerId = customerDetails.CustomerId;
				transactionResponse.Email = customerDetails.Email;
				transactionResponse.FirstName = customerDetails.FirstName;

				List<AkbtransactionSeat> tranSeats = db.AkbtransactionSeats.Where(ts => ts.TransactionId == transactionId).ToList();
				transactionResponse.Seats = new string[tranSeats.Count];
				int cnt = 0;
				foreach (AkbtransactionSeat seat in tranSeats)
				{
					transactionResponse.Seats[cnt++] = seat.SeatNo;
				}

				transactionResponse.TotalCost = cnt * transactionResponse.CostPerSeat;
				return transactionResponse;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		[HttpPost]
		[Route("")]
		public IActionResult CreateTransaction(TransactionRequest transactionRequest)
		{
			try
			{
				Guid guid = Guid.NewGuid();
				Random random = new Random();
				int tId = random.Next();

				// 1. post transaction details to transaction table
				AkbtransactionDetail transactionDetails = new AkbtransactionDetail();
				transactionDetails.TransactionId = tId;
				transactionDetails.MovieId = transactionRequest.MovieId;
				transactionDetails.CustomerId = transactionRequest.CustomerId;
				transactionDetails.TransactionTime = transactionRequest.TransactionTime;
				db.AkbtransactionDetails.Add(transactionDetails);
				db.SaveChanges();

				// 2. post transaction seats details to seat table
				int? transactionId = tId;

				if (transactionId != null) {
					foreach (string seat in transactionRequest.Seats)
					{
						db.AkbtransactionSeats.Add(new AkbtransactionSeat { TransactionId = transactionId, SeatNo = seat });
						AkbseatMap akbseatMap = db.AkbseatMaps.Where(sm => sm.MovieId == transactionRequest.MovieId && sm.SeatNo.Equals(seat)).FirstOrDefault();
						akbseatMap.Status = 2;
						db.SaveChanges();
					}
				}


				// post to azure
				Akbcustomer customer = db.Akbcustomers.Where(c => c.CustomerId == transactionRequest.CustomerId).FirstOrDefault();
				Akbmovie movie = db.Akbmovies.Where(m => m.MovieId == transactionRequest.MovieId).FirstOrDefault();
				string seatsBooked = "";
				for (int i = 0; i < transactionRequest.Seats.Length; i++)
				{
					seatsBooked += transactionRequest.Seats[i];
					if (i + 1 < transactionRequest.Seats.Length) seatsBooked += ", ";
				}



				int? totalCost = transactionRequest.Seats.Length * movie.CostPerSeat;

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://business-scenario.azurewebsites.net/api/CaptureHttpRequest?code=u-nM6QTdMX8kutwO4XW-nOEDoYKHbTwDssPSVp1ImG1tAzFuZuguiQ==");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
					string json = ConstructJson(customer.FirstName, customer.Email, movie.MovieName, transactionRequest.Seats.Length.ToString(), totalCost.ToString(), transactionRequest.TransactionTime.ToString(), movie.ShowTime.ToString(), seatsBooked, "BCNF");
                    streamWriter.Write(json);
                }

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                }

                return Ok(HelpPostTransaction(transactionId));
			}
			catch (Exception ex)
			{
				return BadRequest();
			}
			
			
		}

		public static string ConstructJson(string name, string email, string movie, string ticket, string cost, string ttime, string stime, string seatNos, string message) { 
			string json  = "{\"name\":\""+name+"\"," +
				"\"email\":\""+email+"\"," +
				"\"message\":\""+message+"\"," +
				"\"movie\":\""+movie+"\"," +
				"\"ticket\":\""+ticket+"\"," +
				"\"cost\":\""+cost+"\"," +
				"\"transactionTime\":\""+ttime+"\"," +
				"\"showTime\":\""+stime+"\"," +
				"\"seatNos\":\""+seatNos+"\"}";
			return json;
		}

		[HttpDelete]
		[Route("{transactionId}")]
		public IActionResult DeleteTransaction(int? transactionId) {
			try
			{
				AkbtransactionDetail transaction = db.AkbtransactionDetails.Where(transaction => transaction.TransactionId == transactionId).FirstOrDefault();
				List<AkbtransactionSeat> transactionSeats = db.AkbtransactionSeats.Where(transactionSeat => transactionSeat.TransactionId == transactionId).ToList();
				Akbcustomer customer = db.Akbcustomers.Where(customer => customer.CustomerId == transaction.CustomerId).FirstOrDefault();
				Akbmovie movie = db.Akbmovies.Where(movie => movie.MovieId == transaction.MovieId).FirstOrDefault();
				// 1. delete transaction seats details to seat table
				if (transactionId != null)
				{
					foreach (AkbtransactionSeat seat in transactionSeats)
					{
						db.AkbtransactionSeats.Remove(seat);
						AkbseatMap akbseatMap = db.AkbseatMaps.Where(sm => sm.MovieId == transaction.MovieId && sm.SeatNo.Equals(seat.SeatNo)).FirstOrDefault();
						akbseatMap.Status = 0;
						db.SaveChanges();
					}
				}
				string seatsBooked = "";
				foreach (AkbtransactionSeat seat in transactionSeats)
				{
					if (!seatsBooked.Equals("")) seatsBooked += ", ";
					seatsBooked += seat.SeatNo;
				}
				
				db.AkbtransactionDetails.Remove(transaction);
				db.SaveChanges();

                int? totalCost = transactionSeats.Count * movie.CostPerSeat;

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://business-scenario.azurewebsites.net/api/CaptureHttpRequest?code=u-nM6QTdMX8kutwO4XW-nOEDoYKHbTwDssPSVp1ImG1tAzFuZuguiQ==");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = ConstructJson(customer.FirstName, customer.Email, movie.MovieName, transactionSeats.Count.ToString(), totalCost.ToString(), transaction.TransactionTime.ToString(), movie.ShowTime.ToString(), seatsBooked, "BCNL");
                    streamWriter.Write(json);
                }

                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                }
                return Ok();
			}
			catch (Exception ex) 
			{
				return BadRequest(ex.InnerException.Message);
			}
		}


		[HttpGet]
		[Route("cid/{customerId}")]
		public IActionResult GetTransactionsByCustomerId(int customerId) {
			try
			{
				List<AkbtransactionDetail> transactions = db.AkbtransactionDetails.Where(transaction => transaction.CustomerId == customerId).ToList();
				List<TransactionResponse> response = new List<TransactionResponse>();

				foreach (AkbtransactionDetail transaction in transactions)
				{
					response.Add(HelpPostTransaction(transaction.TransactionId));
				}

				return Ok(response);
			}
			catch (Exception ex) 
			{
				return BadRequest();		
			}
		}
	}
	public class TransactionRequest 
	{
		public int TransactionId { get; set; }
		public int MovieId { get; set; }
		public int CustomerId { get; set; }
		public DateTime TransactionTime { get; set; }
		public string[] Seats { get; set; }
	}

	public class TransactionResponse
	{
		public int? TransactionId { get; set; }
		public DateTime TransactionTime { get; set; }
		public string[] Seats { get; set; }

		public int? TotalCost { get; set; }

		public int? CustomerId { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }


		public int? MovieId { get; set; }
		public string MovieName { get; set; }
		public DateTime? ReleaseDate { get; set; }
		public int? Ratings { get; set; }
		public string Genres { get; set; }
		public string ImageUrl { get; set; }
		public int? CostPerSeat { get; set; }
		public DateTime? ShowTime { get; set; }
		public string Duration { get; set; }
		public string AgeRating { get; set; }
		public string Language { get; set; }
		public string MovieType { get; set; }

	}
}
