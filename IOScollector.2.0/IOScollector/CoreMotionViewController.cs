using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using CoreMotion;

namespace CoreMotion
{
	public partial class CoreMotionViewController : UIViewController
	{
		//private CMMotionManager motionManager;
		//variables
		string sas = "https://ucltt.blob.core.windows.net/collector/?sv=2015-04-05&sr=c&sig=E3KK%2BaWJVw8vemkDM8%2BsV9n7K5SLdgstXX1RuSTvBsc%3D&st=2015-12-14T11%3A30%3A06Z&se=2016-06-14T10%3A30%3A06Z&sp=rwdl&comp=list&restype=container";
		StringBuilder builder = new StringBuilder();
		DateTime starttime;
		DateTime stoptime;
		double[] max = new double[4];
		double[] min = new double[4];
		double[] avg = new double[4];
		double[] sum = { 0.0, 0.0, 0.0, 0.0 };
		double[] reading = new double[4];
		//max[0]is max for resultant, [1] for x,[2] for y, [3]for z; same as min,avg,sum，reading
		int count = -1;
		CMMotionManager motionManager = new CMMotionManager ();
		string[] items = { "iOS_Car", "iOS_Bus", "iOS_Train", "iOS_Metro", "iOS_Walk", "iOS_Bike", "Cancel" };

		partial void UIButton21_TouchUpInside (UIButton sender)
		{
			starttime = DateTime.Now;
			builder.Append("{\"Type\":\"Training Data\",")
				.Append(" \"StartTime\": \"" + starttime.ToString() + "\",")
				.Append("\"Record\":[");
			StartButton.Enabled = false;
			StopButton.Enabled = true;
			motionManager.StartAccelerometerUpdates (NSOperationQueue.CurrentQueue, (data, error) => 
				{
					double dataX = data.Acceleration.X*10;
					double dataY = data.Acceleration.Y*10;
					double dataZ = data.Acceleration.Z*10;
					this.lblX.Text = dataX.ToString ("0.00000000");
					this.lblY.Text = dataY.ToString ("0.00000000");
					this.lblZ.Text = dataZ.ToString ("0.00000000");
					int i;
					reading[1] = -dataX;
					reading[2] = -dataX;
					reading[3] = -dataX;
					reading[0] = reading[1] * reading[1] + reading[2] * reading[2] + reading[3] * reading[3];
					if (count == -1)
					{
						for (i = 0; i < 4; i++)
						{
							max[i] = reading[i];
							min[i] = reading[i];
						}
						count++;
					}
					if (count == 90)
					{
						//calculate resultant acceleration
						max[0] = Math.Sqrt(max[0]);
						min[0] = Math.Sqrt(min[0]);
						avg[0] = sum[0] / 90;
						avg[0] = Math.Sqrt(avg[0]);
						for (i = 1; i < 4; i++)
						{
							avg[i] = sum[i] / 90;
						}
						//write json
						builder.Append("{\"max_resultant\":" + max[0].ToString() + ",")
							.Append("\"min_resultant\":" + min[0].ToString() + ",")
							.Append("\"avg_resultant\":" + avg[0].ToString() + ",")
							.Append("\"max_x\":" + max[1].ToString() + ",")
							.Append("\"min_x\":" + min[1].ToString() + ",")
							.Append("\"avg_x\":" + avg[1].ToString() + ",")
							.Append("\"max_y\":" + max[2].ToString() + ",")
							.Append("\"min_y\":" + min[2].ToString() + ",")
							.Append("\"avg_y\":" + avg[2].ToString() + ",")
							.Append("\"max_z\":" + max[3].ToString() + ",")
							.Append("\"min_z\":" + min[3].ToString() + ",")
							.Append("\"avg_z\":" + avg[3].ToString() + "},");
						//renew max min sum
						for (i = 0; i < 4; i++)
						{
							max[i] = reading[i];
							min[i] = reading[i];
							sum[i] = 0.0;
						}
						count = 0;
					}
					for (i = 0; i < 4; i++)
					{
						if (max[i] < reading[i]) max[i] = reading[i];
						if (min[i] > reading[i]) min[i] = reading[i];
						sum[i] += reading[i];
					}
					count++;
				} );
		}

		partial void UIButton25_TouchUpInside (UIButton sender)
		{
			motionManager.StopAccelerometerUpdates();
			stoptime = DateTime.Now;
			builder.Length--;
			builder.Append("],");
			builder.Append("\"EndTime\":\"" + stoptime.ToString()+"\",");
			var alert = UIAlertController.Create ("Mode", builder.ToString(), UIAlertControllerStyle.Alert);
			alert.AddAction (UIAlertAction.Create ("iOS_Car",UIAlertActionStyle.Default, async action => {
				builder.Append("\"Mode\":\"Car\"}"); await UseContainerSAS(sas,"iOS_Car",builder.ToString());}));

			alert.AddAction (UIAlertAction.Create ("iOS_Bus",UIAlertActionStyle.Default, async action => {
				builder.Append("\"Mode\":\"Bus\"}"); await UseContainerSAS(sas,"iOS_Bus",builder.ToString());}));
		
			alert.AddAction (UIAlertAction.Create ("iOS_Train",UIAlertActionStyle.Default, async action => {
					builder.Append("\"Mode\":\"Train\"}"); await UseContainerSAS(sas,"iOS_Train",builder.ToString());}));
			
			alert.AddAction (UIAlertAction.Create ("iOS_Metro",UIAlertActionStyle.Default, async action => {
					builder.Append("\"Mode\":\"Metro\"}"); await UseContainerSAS(sas,"iOS_Metro",builder.ToString());}));
			
			alert.AddAction (UIAlertAction.Create ("iOS_Walk",UIAlertActionStyle.Default, async action => {
					builder.Append("\"Mode\":\"Walk\"}"); await UseContainerSAS(sas,"iOS_Walk",builder.ToString());}));

			alert.AddAction (UIAlertAction.Create ("iOS_Bike",UIAlertActionStyle.Default, async action => {
					builder.Append("\"Mode\":\"Bike\"}"); await UseContainerSAS(sas,"iOS_Bike",builder.ToString());}));

			alert.AddAction (UIAlertAction.Create ("Cancel", UIAlertActionStyle.Cancel, null));
			PresentViewController (alert, animated: true, completionHandler: null);
		}  
		public CoreMotionViewController () : base ("CoreMotionViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			//StartButton.TouchUpInside += (object sender, EventArgs e) => 


		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}

		static async Task UseContainerSAS(string sas, string mode, string json)
		{
			//Try performing container operations with the SAS provided.
			//to perform container operations on Microsft Azure Blob Storage 
			//Return a reference to the container using the SAS URI.
			CloudBlobContainer container = new CloudBlobContainer(new Uri(sas));
			string date = DateTime.Now.ToString();
			try
			{
				//Write operation: write a new blob to the container.
				CloudBlockBlob blob = container.GetBlockBlobReference(mode + date + ".json");

				string blobcontent = json; 
				MemoryStream msWrite = new
					MemoryStream(Encoding.UTF8.GetBytes(blobcontent));
				msWrite.Position = 0;
				using (msWrite)
				{
					await blob.UploadFromStreamAsync(msWrite);
				}
				Console.WriteLine("Write operation succeeded for SAS " + sas);
				Console.WriteLine();
			}
			catch(Exception e)
			{ 
				Console.WriteLine("Write operation failed for SAS " + sas);
				Console.WriteLine("Additional error information: " + e.Message);
				Console.WriteLine();
			}
		} 
	}
}
		
		/*catch (Exception e)
		{
			Console.WriteLine("Write operation failed for SAS " + sas);
			Console.WriteLine("Additional error information: " + e.Message);
			Console.WriteLine();
		}
		try
		{
			//Read operation: Get a reference to one of the blobs in the container and read it.
			CloudBlockBlob blob = container.GetBlockBlobReference("sasblob_” + date + “.txt");
			string data = await blob.DownloadTextAsync();

			Console.WriteLine("Read operation succeeded for SAS " + sas);
			Console.WriteLine("Blob contents: " + data);
		}
		catch (Exception e)
		{
			Console.WriteLine("Additional error information: " + e.Message);
			Console.WriteLine("Read operation failed for SAS " + sas);
			Console.WriteLine();
		}
		Console.WriteLine();
		try
		{*/


