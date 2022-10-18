using BookMyMovie_Angular_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

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
				string message = $"Name : {customer.FirstName}\n" +
					$"Email : {customer.Email}\n" +
					$"Seat No : {seatsBooked}\n" +
					$"No of Seats : {transactionRequest.Seats.Length}\n" +
					$"Total Cost : {transactionRequest.Seats.Length*movie.CostPerSeat}\n" +
					$"Movie Name : {movie.MovieName}\n" +
					$"Show Time : {movie.ShowTime}";
				AddMessageToQueue(message);
				return Ok(HelpPostTransaction(transactionId));
			}
			catch (Exception ex)
			{
				return BadRequest();
			}
			
			
		}

		public static void AddMessageToQueue(string message)
		{
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connAzureStorage);
			CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
			CloudQueue cloudQueue = cloudQueueClient.GetQueueReference("transactions");
			CloudQueueMessage queueMessage = new CloudQueueMessage(message);
			cloudQueue.AddMessageAsync(queueMessage);
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




//	[HttpPost]
//	[Route("")]
//	public IActionResult CreateTransaction(TransactionDetails transactionDetails)
//	{
//		try
//		{
//			string seatsBooked = "";
//			for (int i = 0; i < transactionDetails.selectedSeats.Length; i++)
//			{
//				seatsBooked+=transactionDetails.selectedSeats[i];
//				if (i + 1 < transactionDetails.selectedSeats.Length) seatsBooked+= ", ";
//			}
//			string message = $"Name : {transactionDetails.firstName}\n" +
//				$"Email : {transactionDetails.email}\n" +
//				$"Seat No : {seatsBooked}\n" +
//				$"No of Seats : {transactionDetails.noOfSelectedSeats}\n" +
//				$"Total Cost : {transactionDetails.totalCost}\n" +
//				$"Movie Name : {transactionDetails.movieName}\n" +
//				$"Show Time : {transactionDetails.showTime}";
//			AddMessageToQueue(message);
//		}
//		catch (Exception ex) 
//		{
//			return BadRequest(ex.InnerException.Message);
//		}
//		return Ok();
//	}



//public class TransactionDetails 
//{
//	public string firstName { get; set; }
//	public string email { get; set; }
//	public int movieId { get; set; }
//	public string movieName { get; set; }
//	public DateTime showTime { get; set; }
//	public int seatCost { get; set; }
//	public string[] selectedSeats { get; set; }
//	public int noOfSelectedSeats { get; set; }
//	public int totalCost { get; set; }

//}
