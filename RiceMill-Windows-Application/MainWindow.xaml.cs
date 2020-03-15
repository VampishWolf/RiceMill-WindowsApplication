using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Paragraph = iTextSharp.text.Paragraph;
using System.Data;

namespace RiceMill_Windows_Application
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			
		}
		private void MainWindow_Loaded(object sender, EventArgs e)
		{
			DispatcherTimer timer = new DispatcherTimer();
			timer.Tick += Timer_Tick;
			timer.Start();

		}
		void Timer_Tick(object sender, EventArgs e)
		{
			dateDisplayLabel.Content = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
		}

		public int serialnumber = 1;

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int bagscount = Convert.ToInt32(bagsCountInput.Text);
				var name = itemNameInput.Text;
				float totalweight = Convert.ToInt32(totalWeightInput.Text);
				float weight = totalweight / bagscount;
				float rate = Convert.ToInt32(rateInput.Text);
				float amount = bagscount * rate;
				float moistallowed = Convert.ToInt32(moistAllowedInput.Text);
				float moist = Convert.ToInt32(moistInput.Text);
				float claim;
				if (moist <= 18) {
					claim = 0;
				}
				else {
					claim = moist - moistallowed;
				}
				if (moist >= 100) {
					MessageBox.Show("Invalid Moist!");
					return;
				}
				float claimant = (claim * amount) / 100;
				float finalamount = amount - claimant;
				DataGridView.Items.Add(new BindingData { Number = serialnumber, BagsCount = bagscount, ItemName = name, Weight = weight, Rate = rate, Amount = amount, Moisture = moist, Claim = claim, FinalAmount = finalamount });
				serialnumber += 1;
			} catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
			
		}
		private void Update_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int bagscount = Convert.ToInt32(bagsCountInput.Text);
				var name = itemNameInput.Text;
				float totalweight = Convert.ToInt32(totalWeightInput.Text);
				float weight = totalweight / bagscount;
				float rate = Convert.ToInt32(rateInput.Text);
				float amount = bagscount * rate;
				float moistallowed = Convert.ToInt32(moistAllowedInput.Text);
				float moist = Convert.ToInt32(moistInput.Text);
				float claim;
				if (moist <= 18) {
					claim = 0;
				}
				else {
					claim = moist - moistallowed;
				}
				if (moist >= 100) {
					MessageBox.Show("Invalid Moist!");
					return;
				}
				float claimant = (claim * amount) / 100;
				float finalamount = amount - claimant;
				var grid = DataGridView;
				if (grid.SelectedItem != null)
				{
					BindingData item = (BindingData)grid.SelectedItem;
					item.BagsCount = bagscount;
					item.ItemName = name;
					item.Weight = weight;
					item.Rate = rate;
					item.Amount = amount;
					item.Moisture = moist;
					item.Claim = claim;
					item.FinalAmount = finalamount;

					grid.Items.Refresh();
				}

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}

		}
		private void Delete_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var grid = DataGridView;
				if (grid.SelectedIndex >= 0)
				{
					for (int i = 0; i <= grid.SelectedItems.Count; i++)
					{
						grid.Items.Remove(grid.SelectedItems[i]);
					};
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}

		}

		/*private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			var grid = DataGridView;
			if (grid.SelectedItem != null)
			{								
				BindingData item = (BindingData)grid.SelectedItem;
				bagsCountInput.Text = item.BagsCount.ToString();
			}
		}*/

		private void Reset_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var grid = DataGridView;
				grid.Items.Clear();
				grid.Items.Refresh();
				serialnumber = 1;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
		}

		private void Print_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (partyNameInput.Text == "" || addressInput.Text == "" || vehicleNumberInput.Text == "" || totalWeightInput.Text == "" || itemNameInput.Text == "Choose an Item" || bagsCountInput.Text == "" || rateInput.Text == "" || moistInput.Text == "" || moistAllowedInput.Text == "")
				{
					MessageBox.Show("Please enter all the required fields");
					return;
				}

				// File Naming Convention
				/*string fileName = string.Empty;
				DateTime fileCreationDatetime = DateTime.Now;
				fileName = string.Format("{0}.pdf", fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + fileCreationDatetime.ToString(@"HHmmss"));*/
				/*string pdfPath = Server.MapPath(@"~\PDFs\") + fileName;*/						// Leave Commented

				string paddyPrintDocumentPath = "D:\\PaddyPrint.PDF";                           //Comment Later
				Document document = new Document(PageSize.A5.Rotate(), 5, 5, 5, 5);
				PdfWriter.GetInstance(document, new FileStream(paddyPrintDocumentPath, FileMode.Create));
				document.Open();

				Chunk lineBreaks = new Chunk("\n");
				
				PdfPTable pageHeader = new PdfPTable(2)
				{
					WidthPercentage = 100
				};
				PdfPCell gst = new PdfPCell(new Phrase(new Chunk("GST: 20AAECR5776M1Z1", FontFactory.GetFont("Verdana", 10))));
				PdfPCell contact = new PdfPCell(new Phrase(new Chunk("Mob: +91 8210622847 \n+91 7004310634", FontFactory.GetFont("Verdana", 10))));
			
				gst.Border = contact.Border = 0;
				gst.HorizontalAlignment = Element.ALIGN_LEFT;
				contact.HorizontalAlignment = Element.ALIGN_RIGHT;

				pageHeader.AddCell(gst);
				pageHeader.AddCell(contact);

				Paragraph pageTitle = new Paragraph
				{
					new Chunk("RAMESWARA RICE MILL PVT. LTD. \n", FontFactory.GetFont("HELVETICA", 14, Font.BOLD)),
					new Chunk("Saharpura, Jamtara - 815351 (Jharkhand)", FontFactory.GetFont("HELVETICA", 10)),
				};
				pageTitle.Alignment = Element.ALIGN_CENTER;

				//-----------------------------------------------------------------------------------------------------------------------------------------------
				// Header ENDS

			

				PdfPTable pageBody = new PdfPTable(4);
				pageBody.SetWidths(new float[] { 8f, 12f, 8f, 12f });

				PdfPCell partyName = new PdfPCell(new Phrase(new Chunk("Party Name: ", FontFactory.GetFont("Verdana", 10))));
				PdfPCell partyNameData = new PdfPCell(new Phrase(new Chunk(partyNameInput.Text.ToString(), FontFactory.GetFont("Verdana", 10))));
				PdfPCell date = new PdfPCell(new Phrase(new Chunk("Date: ", FontFactory.GetFont("Verdana", 10))));
				PdfPCell dateData = new PdfPCell(new Phrase(new Chunk(dateDisplayLabel.Content.ToString(), FontFactory.GetFont("Verdana", 10))));
				PdfPCell partyAddress = new PdfPCell(new Phrase(new Chunk("Party Address: ", FontFactory.GetFont("Verdana", 10))));
				PdfPCell partyAddressData = new PdfPCell(new Phrase(new Chunk(addressInput.Text.ToString(), FontFactory.GetFont("Verdana", 10))));
				PdfPCell moistAllowed = new PdfPCell(new Phrase(new Chunk("Moist Allowed: ", FontFactory.GetFont("Verdana", 10))));
				PdfPCell moistAllowedData = new PdfPCell(new Phrase(new Chunk(moistAllowedInput.Text.ToString() + " %", FontFactory.GetFont("Verdana", 10))));
				PdfPCell vehicleNumber = new PdfPCell(new Phrase(new Chunk("Vehicle Number: ", FontFactory.GetFont("Verdana", 10))));
				PdfPCell vehicleNumberData = new PdfPCell(new Phrase(new Chunk(vehicleNumberInput.Text.ToString(), FontFactory.GetFont("Verdana", 10))));
				PdfPCell totalWeight = new PdfPCell(new Phrase(new Chunk("Total Weight: ", FontFactory.GetFont("Verdana", 10))));
				PdfPCell totalWeightData = new PdfPCell(new Phrase(new Chunk(totalWeightInput.Text.ToString(), FontFactory.GetFont("Verdana", 10))));
				
				PdfPCell blank = new PdfPCell();


				partyName.Border = partyNameData.Border = date.Border = dateData.Border = partyAddress.Border = partyAddressData.Border = moistAllowed.Border = moistAllowedData.Border = vehicleNumber.Border = vehicleNumberData.Border = totalWeight.Border = totalWeightData.Border = blank.Border = 0;
				pageBody.AddCell(partyName);
				pageBody.AddCell(partyNameData);
				pageBody.AddCell(date);
				pageBody.AddCell(dateData);
				pageBody.AddCell(partyAddress);
				pageBody.AddCell(partyAddressData);
				pageBody.AddCell(moistAllowed);
				pageBody.AddCell(moistAllowedData);
				pageBody.AddCell(vehicleNumber);
				pageBody.AddCell(vehicleNumberData);
				pageBody.AddCell(blank);
				pageBody.AddCell(blank);
				pageBody.AddCell(totalWeight);
				pageBody.AddCell(totalWeightData);
				pageBody.AddCell(blank);
				pageBody.AddCell(blank);
				
							
				//-----------------------------------------------------------------------------------------------------------------------------------------------
				// Table Starts

				PdfPTable pageBodyDataGrid = new PdfPTable(9) { WidthPercentage = 90 };
				pageBodyDataGrid.SetWidths(new float[] { 2f, 8f, 4f, 4f, 4f, 4f, 4f, 4f, 6f});
				
				PdfPCell seqNo = new PdfPCell(new Phrase(new Chunk("#", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
				PdfPCell itemName = new PdfPCell(new Phrase(new Chunk("Item Name", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
				PdfPCell bagsCount = new PdfPCell(new Phrase(new Chunk("Bags Count", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
				PdfPCell weight = new PdfPCell(new Phrase(new Chunk("Weight", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
				PdfPCell amount = new PdfPCell(new Phrase(new Chunk("Rate", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
				PdfPCell rate = new PdfPCell(new Phrase(new Chunk("Amount", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
				PdfPCell moist = new PdfPCell(new Phrase(new Chunk("Moist", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
				PdfPCell claim = new PdfPCell(new Phrase(new Chunk("Claim", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
				PdfPCell finalAmount = new PdfPCell(new Phrase(new Chunk("Final Amount", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
				seqNo.BackgroundColor = itemName.BackgroundColor = bagsCount.BackgroundColor = weight.BackgroundColor = amount.BackgroundColor = rate.BackgroundColor = moist.BackgroundColor = claim.BackgroundColor = finalAmount.BackgroundColor = BaseColor.LIGHT_GRAY;

				pageBodyDataGrid.AddCell(seqNo);
				pageBodyDataGrid.AddCell(itemName);
				pageBodyDataGrid.AddCell(bagsCount);
				pageBodyDataGrid.AddCell(weight);
				pageBodyDataGrid.AddCell(rate);
				pageBodyDataGrid.AddCell(amount);
				pageBodyDataGrid.AddCell(moist);
				pageBodyDataGrid.AddCell(claim);
				pageBodyDataGrid.AddCell(finalAmount);


		
				document.Add(pageHeader);
				document.Add(pageTitle);
				document.Add(lineBreaks);
				document.Add(pageBody);
				document.Add(lineBreaks);
				document.Add(pageBodyDataGrid);

				MessageBox.Show("Document created Successfully! Click OK to continue...");
				document.Close();
				System.Diagnostics.Process.Start(@paddyPrintDocumentPath);

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
		}
	
		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9.]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		public class BindingData
		{
			public int Number { get; set; }
			public int BagsCount { get; set; }
			public string ItemName { get; set; }
			public float Weight { get; set; }
			public float Rate { get; set; }
			public float Amt { get; set; }
			public float Moisture { get; set; }
			public float Claim { get; set; }
			public float Amount { get; set; }
			public float FinalAmount { get; set; }
		}
	}	
}
