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
		

		[HttpPost]
		[Route("")]
		public IActionResult CreateTransaction(TransactionRequest transactionRequest)
		{
			try
			{
				// 1. post transaction details to transaction table
				AkbtransactionDetail transactionDetails = new AkbtransactionDetail();
				transactionDetails.TransactionId = null;
				transactionDetails.MovieId = transactionRequest.MovieId;
				transactionDetails.CustomerId = transactionRequest.CustomerId;
				transactionDetails.TransactionTime = transactionRequest.TransactionTime;
				db.AkbtransactionDetails.Add(transactionDetails);
				db.SaveChanges();

				// 2. post transaction seats details to seat table
				int? transactionId = db.AkbtransactionDetails.Where(t =>
				t.MovieId == transactionRequest.MovieId &&
				t.CustomerId == transactionRequest.CustomerId &&
				t.TransactionTime.Equals(transactionRequest.TransactionTime)).FirstOrDefault().TransactionId;

				if (transactionId != null) {
					foreach (string seat in transactionRequest.Seats)
					{
						db.AkbtransactionSeats.Add(new AkbtransactionSeat { TransactionId = transactionId, SeatNo = seat });
						AkbseatMap akbseatMap = db.AkbseatMaps.Where(sm => sm.MovieId == transactionRequest.MovieId && sm.SeatNo.Equals(seat)).FirstOrDefault();
						akbseatMap.Status = 2;
						db.SaveChanges();
					}
				}
			}
			catch (Exception ex)
			{
				return BadRequest();
			}
			
			return Ok();
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
}



//public static string connAzureStorage = "DefaultEndpointsProtocol=https;AccountName=anshulkumarstorage256;AccountKey=PEdBR+R7xlLxZdCrfCmURzVlWLYEUlpmkGdGJP9diqkcyN9uIfxB9ZHnx3L481ULKBYnOMznHlgA+AStix+Sjg==;EndpointSuffix=core.windows.net";

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

//	public static void AddMessageToQueue(string message) 
//	{
//		CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connAzureStorage);
//		CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
//		CloudQueue cloudQueue = cloudQueueClient.GetQueueReference("transactions");
//		CloudQueueMessage queueMessage = new CloudQueueMessage(message);
//		cloudQueue.AddMessageAsync(queueMessage);
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
