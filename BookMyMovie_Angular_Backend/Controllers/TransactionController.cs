﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;

namespace BookMyMovie_Angular_Backend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TransactionController : ControllerBase
	{
		public static string connAzureStorage = "DefaultEndpointsProtocol=https;AccountName=anshulkumarstorage256;AccountKey=PEdBR+R7xlLxZdCrfCmURzVlWLYEUlpmkGdGJP9diqkcyN9uIfxB9ZHnx3L481ULKBYnOMznHlgA+AStix+Sjg==;EndpointSuffix=core.windows.net";

		[HttpPost]
		[Route("")]
		public IActionResult CreateTransaction(TransactionDetails transactionDetails)
		{
			try
			{
				string message = "Hi, Anshul first message to queue";
				AddMessageToQueue(message);
			}
			catch (Exception ex) 
			{
				return BadRequest(ex.InnerException.Message);
			}
			return Ok();
		}

		public static void AddMessageToQueue(string message) 
		{
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connAzureStorage);
			CloudQueueClient cloudQueueClient = storageAccount.CreateCloudQueueClient();
			CloudQueue cloudQueue = cloudQueueClient.GetQueueReference("myqueue");
			CloudQueueMessage queueMessage = new CloudQueueMessage(message);
			cloudQueue.AddMessageAsync(queueMessage);
		}
	}

	public class TransactionDetails 
	{
		public string firstName { get; set; }
		public string email { get; set; }
		public int movieId { get; set; }
		public string movieName { get; set; }
		public DateTime showTime { get; set; }
		public int seatCost { get; set; }
		public string[] selectedSeats { get; set; }
		public int noOfSelectedSeats { get; set; }
		public int totalCost { get; set; }

	}
}