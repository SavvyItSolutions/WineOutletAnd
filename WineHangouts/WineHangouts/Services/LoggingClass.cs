using System;
using System.Text;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Auth;
using Android.Util;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net;
using Android.App;

namespace WineHangouts
{
    public static class LoggingClass
    {
        //static string logspath = CreateDirectoryForLogs();
		public static StorageCredentials sc;
		public static CloudStorageAccount storageaccount;
		public static CloudBlobClient blobClient;
		public static CloudBlobContainer container;
		public static CloudAppendBlob append;
        public static string userid=CurrentUser.getUserId();
        public static void pathcre()
        {
            if (userid == "0"||userid==null)
            {
                userid =CurrentUser.GetGuestId();
                if (userid == "0"|| userid == null)
                {
                    userid = "DefaultLogs";
                }
            }
        }
		public static async void UploadErrorLogs()
		{
            var TaskA = new System.Threading.Tasks.Task(async () =>
            {
            pathcre();
            try
            {

                sc = new StorageCredentials("icsintegration", "+7UyQSwTkIfrL1BvEbw5+GF2Pcqh3Fsmkyj/cEqvMbZlFJ5rBuUgPiRR2yTR75s2Xkw5Hh9scRbIrb68GRCIXA==");
                storageaccount = new CloudStorageAccount(sc, true);
                blobClient = storageaccount.CreateCloudBlobClient();
                container = blobClient.GetContainerReference("userlogs");
                await container.CreateIfNotExistsAsync(); //{
                append = container.GetAppendBlobReference(userid + ".csv"); //}
                if (!await append.ExistsAsync())
                {
                    await append.CreateOrReplaceAsync();
                }
            

            }
			catch (Exception exe)
			{
				Log.Error("Error", exe.Message);
			}
            });
            TaskA.Start();
        }
        public static void LogInfo(string info,int screenid)
		{
            var TaskA = new System.Threading.Tasks.Task(() =>
            {
                pathcre();
                try
			    {
				
				    append = container.GetAppendBlobReference(userid + ".csv");
				    DateTime date1 = DateTime.UtcNow;
			
			        append.AppendTextAsync(string.Format("{0},{1},{2},{3}", "Info", date1.ToString("MM/dd/yyyy hh:mm:ss.fff"), info, screenid + "\n"));

			    }
			    catch (Exception exe)
			    {
			    	Log.Error("Error", exe.Message);
			    }
            });
            TaskA.Start();
        }
		public static  void LogInfoEx(string info,int screenid)
		{
            var TaskA = new System.Threading.Tasks.Task(() =>
            {
                pathcre();
            try
			{
				
				DateTime date = DateTime.UtcNow;
				append = container.GetAppendBlobReference(userid + ".csv");
				var tasks = new Task[1];
				for (int i = 0; i < 1; i++)
				tasks[i] =  append.AppendTextAsync(string.Format("{0},{1},{2},{3}", "Info", date.ToString("MM/dd/yyyy hh:mm:ss.fff"), info,screenid + "\n"));
				Task.WaitAll(tasks);

			}
			catch (Exception exe)
			{
				Log.Error("Error", exe.Message);
			}
            });
            TaskA.Start();
        }
		public static void LogServiceInfo(string info, string servicename)
        {
            var TaskA = new System.Threading.Tasks.Task(() =>
            {
                pathcre();
            try
            {
				UploadErrorLogs();
		        append = container.GetAppendBlobReference(userid + ".csv");
				DateTime date1 = DateTime.UtcNow;
				var tasks = new Task[1];
				for (int i = 0; i < 1; i++)


					tasks[i]= append.AppendTextAsync(string.Format("{0},{1},{2},{3}", "Service", date1.ToString("MM/dd/yyyy hh:mm:ss.fff"), info, servicename +"\n"));
				Task.WaitAll(tasks);
                }
            catch (Exception exe)
            {
                Log.Error("Error", exe.Message);
            }
        });
            TaskA.Start();
        }
        public static void LogError(string error,int screenid,string lineno)
        {
            var TaskA = new System.Threading.Tasks.Task(() =>
            {
                pathcre();
            try
            {
				append = container.GetAppendBlobReference(userid+ ".csv");
				DateTime date2 = DateTime.UtcNow;
				append.AppendTextAsync(string.Format("{0},{1},{2},{3},{4}", "Exception", date2.ToString("MM/dd/yyyy hh:mm:ss.fff"), error, lineno, screenid + "\n"));
			}
            catch (Exception exe)
            {
                Log.Error("Error", exe.Message);
            }
            });
            TaskA.Start();
        }
		public static void LogTime(string info,string time)
		{
            var TaskA = new System.Threading.Tasks.Task(() =>
            {
                pathcre();
            try
			{
				append = container.GetAppendBlobReference(CurrentUser.getUserId() + ".csv");
				DateTime date2 = DateTime.UtcNow;
				append.AppendTextAsync(string.Format("{0},{1},{2},{3}", "Time", date2.ToString("MM/dd/yyyy hh:mm:ss.fff"), info, time+ "\n"));
			}
			catch (Exception exe)
			{
				Log.Error("Error", exe.Message);
			}
            });
            TaskA.Start();
		}
	}
}