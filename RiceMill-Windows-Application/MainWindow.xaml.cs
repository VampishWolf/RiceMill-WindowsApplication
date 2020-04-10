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
using PrintDialog = System.Windows.Controls.PrintDialog;

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
		
		private void Unlock_Click(object sender, RoutedEventArgs e)
		{
			partyNameInput.IsEnabled = addressInput.IsEnabled = vehicleNumberInput.IsEnabled = totalWeightInput.IsEnabled = totalBagsInput.IsEnabled = true;
			LockedImage.Visibility = UnlockedImage.Visibility;
			UnlockedImage.Visibility = Visibility.Visible;
		}

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (partyNameInput.Text == "")
				{
					MessageBox.Show("Please enter the Party Name", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
				else if (addressInput.Text == "")
				{
					MessageBox.Show("Please enter the Party Address", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
				else if (vehicleNumberInput.Text == "")
				{
					MessageBox.Show("Please enter the Vehicle Number", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
				else if (totalWeightInput.Text == "")
				{
					MessageBox.Show("Please enter the Total Weight", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
				else if (totalBagsInput.Text == "")
				{
					MessageBox.Show("Please enter the Total Bags", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
				else if (itemNameInput.SelectedIndex == 0)
				{
					MessageBox.Show("Please select the Item Name", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
				else if (bagsCountInput.Text == "")
				{
					MessageBox.Show("Please enter the Bags count", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
				else if (rateInput.Text == "")
				{
					MessageBox.Show("Please enter the Rate", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
				else if (moistInput.Text == "")
				{
					MessageBox.Show("Please enter the Moisture", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
								
				float totalweight = float.Parse(totalWeightInput.Text);
				int totalbags = Convert.ToInt32(totalBagsInput.Text);
				var name = itemNameInput.Text;
				int bagscount = Convert.ToInt32(bagsCountInput.Text);				
				if (totalbags < bagscount)
				{
					MessageBox.Show("Bags Count cannot be more than Total Bags!", "Warning Message", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
				
				float averageweight = totalweight / totalbags;
				float finalWeight = averageweight * bagscount;
				float roundedFinalWeight = (float)Math.Round(finalWeight, 2);
				float rate = float.Parse(rateInput.Text);
				float amount = roundedFinalWeight * rate;
				float roundedAmount = (float)Math.Round(amount, 2);
				float moistallowed = float.Parse(moistAllowedInput.Text);
				float moist = float.Parse(moistInput.Text);
				float claim;
				if (moist <= 18)
				{
					claim = 0;
				}
				else
				{
					claim = moist - moistallowed;
				}
				if (moist >= 100)
				{
					MessageBox.Show("Invalid Moist!", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				float claimant = (claim * amount) / 100;
				float roundedClaimant = (float)Math.Round(claimant, 2);
				float finalamount = roundedAmount - roundedClaimant;
				var grid = DataGridView;
				int gridRows = grid.Items.Count;

				grid.Items.Add(new BindingData { Number = serialnumber, BagsCount = bagscount, ItemName = name, Weight = roundedFinalWeight, Rate = rate, Amount = roundedAmount, Moisture = moist, Claim = roundedClaimant, FinalAmount = finalamount });
				grid.Items.Refresh();
				serialnumber += 1;

				partyNameInput.IsEnabled = addressInput.IsEnabled = vehicleNumberInput.IsEnabled = totalWeightInput.IsEnabled = totalBagsInput.IsEnabled = false;
				UnlockedImage.Visibility = LockedImage.Visibility;
				LockedImage.Visibility = Visibility.Visible;
				itemNameInput.SelectedIndex = 0;
				bagsCountInput.Text = rateInput.Text = moistInput.Text = "";

			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
		}

		private void Update_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (DataGridView.Items.Count == 0)
				{
					MessageBox.Show("Please enter some data first", "Warning Message", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
				else if (itemNameInput.SelectedIndex == 0)
				{
					MessageBox.Show("Please select the Item from the Table", "Warning Message", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
				else if (bagsCountInput.Text == "")
				{
					MessageBox.Show("Please enter the Bags count", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
				else if (rateInput.Text == "")
				{
					MessageBox.Show("Please enter the Rate", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
				else if (moistInput.Text == "")
				{
					MessageBox.Show("Please enter the Moisture", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}

				float totalweight = float.Parse(totalWeightInput.Text);
				int totalbags = Convert.ToInt32(totalBagsInput.Text);
				int bagscount = Convert.ToInt32(bagsCountInput.Text);
				var name = itemNameInput.Text;
				float averageweight = totalweight / totalbags;
				float finalWeight = averageweight * bagscount;
				float roundedFinalWeight = (float)Math.Round(finalWeight, 2);
				float rate = float.Parse(rateInput.Text);
				float amount = roundedFinalWeight * rate;
				float roundedAmount = (float)Math.Round(amount, 2);
				float moistallowed = float.Parse(moistAllowedInput.Text);
				float moist = float.Parse(moistInput.Text);
				float claim;
				if (moist <= 18)
				{
					claim = 0;
				}
				else
				{
					claim = moist - moistallowed;
				}
				if (moist >= 100)
				{
					MessageBox.Show("Invalid Moist!", "Error Message", MessageBoxButton.OK, MessageBoxImage.Error);
					return;
				}
				float claimant = (claim * amount) / 100;
				float roundedClaimant = (float)Math.Round(claimant, 2);
				float finalamount = roundedAmount - roundedClaimant;
				
				var grid = DataGridView;
				if (grid.SelectedItem != null)
				{
					BindingData item = (BindingData)grid.SelectedItem;
					item.BagsCount = bagscount;
					item.ItemName = name;
					item.Weight = roundedFinalWeight;
					item.Rate = rate;
					item.Amount = roundedAmount;
					item.Moisture = moist;
					item.Claim = roundedClaimant;
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
				if (DataGridView.Items.Count == 0)
				{
					MessageBox.Show("Please enter some data first", "Warning Message", MessageBoxButton.OK, MessageBoxImage.Warning);
					return;
				}
				else if (kantaInput.SelectedIndex == 0)
				{
					MessageBox.Show("Please enter the Kanta Deduction", "Information Message", MessageBoxButton.OK, MessageBoxImage.Information);
					return;
				}
				
				string fileName = string.Empty;                                         // FILE NAMING CONVENTION
				/*DateTime fileCreationDatetime = DateTime.Now;*/                       // USE TO ADD DATE IN FILENAME
				fileName = "PaddyPrint.PDF"; /*string.Format("{0}.pdf", fileCreationDatetime.ToString(@"yyyyMMdd") + "_" + fileCreationDatetime.ToString(@"HHmmss"));*/

				//-----------------------------------------------------------------------------------------------------------------------------------------------
				// HEADER STARTS

				Document document = new Document(PageSize.A5.Rotate(), 5, 5, 5, 5);

				//var file = Path.GetTempFileName();
				//string filepath = Path.GetTempPath();
				//string strFilename = Path.GetFileName(file);

				using (MemoryStream memoryStream = new MemoryStream())
				{
					var grid = DataGridView;
					int gridRows = grid.Items.Count;

					int tempSeqNoVariable = 1;
					int bagsCountTotal = 0;
					float weightTotal = 0;
					float amountTotal = 0;
					float finalAmountTotal = 0;
					int totalbags = Convert.ToInt32(totalBagsInput.Text);

					for (int i = 0; i < gridRows; i++)
					{
						BindingData item = (BindingData)grid.Items[i];
						bagsCountTotal = Convert.ToInt32(item?.BagsCount.ToString()) + bagsCountTotal;
						weightTotal = float.Parse(item?.Weight.ToString()) + weightTotal;
					}
					if (totalbags < bagsCountTotal)
					{
						MessageBox.Show("Bags entered are more than the Total Bags value!\nPlease make the corrections.", "Warning Message", MessageBoxButton.OK, MessageBoxImage.Warning);
						return;
					}
					PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
					//PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

					Chunk lineBreaks = new Chunk("\n");
					PdfPCell blank = new PdfPCell();

					PdfPTable pageHeader = new PdfPTable(3)
					{
						WidthPercentage = 100
					};
					PdfPCell gst = new PdfPCell(new Phrase(new Chunk("GST: 20AAECR5776M1Z1", FontFactory.GetFont("Verdana", 10))));
					PdfPCell farmerPaddy = new PdfPCell(new Phrase(new Chunk("\nFARMER PADDY", FontFactory.GetFont("Verdana", 8))));
					PdfPCell purchaseMemo = new PdfPCell(new Phrase(new Chunk("Purchase Memo\n", FontFactory.GetFont("Verdana", 8, Font.UNDERLINE))));
					PdfPCell contact = new PdfPCell(new Phrase(new Chunk("Mob: +91 8210622847 \n+91 7004310634", FontFactory.GetFont("Verdana", 10))));

					gst.Border = contact.Border = farmerPaddy.Border = purchaseMemo.Border = blank.Border = 0;
					gst.HorizontalAlignment = Element.ALIGN_LEFT;
					farmerPaddy.HorizontalAlignment = purchaseMemo.HorizontalAlignment = Element.ALIGN_CENTER;
					contact.HorizontalAlignment = Element.ALIGN_RIGHT;

					pageHeader.AddCell(gst);
					pageHeader.AddCell(farmerPaddy);
					pageHeader.AddCell(contact);
					pageHeader.AddCell(blank);
					pageHeader.AddCell(purchaseMemo);
					pageHeader.AddCell(blank);

					Paragraph pageTitle = new Paragraph
					{
					new Chunk("RAMESWARA RICE MILL PVT. LTD. \n", FontFactory.GetFont("HELVETICA", 14, Font.BOLD)),
					new Chunk("Saharpura, Jamtara - 815351 (Jharkhand)", FontFactory.GetFont("HELVETICA", 10)),
					};
					pageTitle.Alignment = Element.ALIGN_CENTER;

					//-----------------------------------------------------------------------------------------------------------------------------------------------
					// BODY ENDS


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
					PdfPCell totalweight = new PdfPCell(new Phrase(new Chunk("Total Weight: ", FontFactory.GetFont("Verdana", 10))));
					PdfPCell totalWeightData = new PdfPCell(new Phrase(new Chunk(totalWeightInput.Text.ToString() + "     (Qtls.)", FontFactory.GetFont("Verdana", 10))));


					partyName.Border = partyNameData.Border = date.Border = dateData.Border = partyAddress.Border = partyAddressData.Border = moistAllowed.Border = moistAllowedData.Border = vehicleNumber.Border = vehicleNumberData.Border = totalweight.Border = totalWeightData.Border = 0;
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
					pageBody.AddCell(totalweight);
					pageBody.AddCell(totalWeightData);
					pageBody.AddCell(blank);
					pageBody.AddCell(blank);


					/*pageBody.HorizontalAlignment = Element.ALIGN_LEFT;*/
					//-----------------------------------------------------------------------------------------------------------------------------------------------
					// TABLE STARTS

					PdfPTable pageBodyDataGrid = new PdfPTable(9) { WidthPercentage = 90 };
					pageBodyDataGrid.SetWidths(new float[] { 2f, 8f, 4f, 4f, 4f, 4f, 4f, 4f, 6f });

					PdfPCell seqNo = new PdfPCell(new Phrase(new Chunk("S.No.", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell itemName = new PdfPCell(new Phrase(new Chunk("Item Name", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell bagsCount = new PdfPCell(new Phrase(new Chunk("Bags Count", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell weight = new PdfPCell(new Phrase(new Chunk("Weight", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell rate = new PdfPCell(new Phrase(new Chunk("Rate", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell amount = new PdfPCell(new Phrase(new Chunk("Amount", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
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

					
					for (int i = 0; i < gridRows; i++)
					{
						BindingData item = (BindingData)grid.Items[i];

						PdfPCell seqNoData = new PdfPCell(new Phrase(new Chunk(tempSeqNoVariable.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
						PdfPCell itemNameData = new PdfPCell(new Phrase(new Chunk(item?.ItemName.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
						PdfPCell bagsCountData = new PdfPCell(new Phrase(new Chunk(item?.BagsCount.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
						PdfPCell weightData = new PdfPCell(new Phrase(new Chunk(item?.Weight.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
						PdfPCell rateData = new PdfPCell(new Phrase(new Chunk(item?.Rate.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
						PdfPCell amountData = new PdfPCell(new Phrase(new Chunk(item?.Amount.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
						PdfPCell moistData = new PdfPCell(new Phrase(new Chunk(item?.Moisture.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
						PdfPCell claimData = new PdfPCell(new Phrase(new Chunk(item?.Claim.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
						PdfPCell finalAmountData = new PdfPCell(new Phrase(new Chunk(item?.FinalAmount.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));

						amountTotal = float.Parse(item?.Amount.ToString()) + amountTotal;
						finalAmountTotal = float.Parse(item?.FinalAmount.ToString()) + finalAmountTotal;

						pageBodyDataGrid.AddCell(seqNoData);
						pageBodyDataGrid.AddCell(itemNameData);
						pageBodyDataGrid.AddCell(bagsCountData);
						pageBodyDataGrid.AddCell(weightData);
						pageBodyDataGrid.AddCell(rateData);
						pageBodyDataGrid.AddCell(amountData);
						pageBodyDataGrid.AddCell(moistData);
						pageBodyDataGrid.AddCell(claimData);
						pageBodyDataGrid.AddCell(finalAmountData);
						tempSeqNoVariable += 1;
					}

					PdfPTable pageFooter = new PdfPTable(9) { WidthPercentage = 90 };
					pageFooter.SetWidths(new float[] { 2f, 8f, 4f, 4f, 4f, 4f, 4f, 4f, 6f });

					PdfPCell totalVariable = new PdfPCell(new Phrase(new Chunk("Total", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell bagsTotalData = new PdfPCell(new Phrase(new Chunk(bagsCountTotal.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell weightTotalData = new PdfPCell(new Phrase(new Chunk(weightTotal.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell amountTotalData = new PdfPCell(new Phrase(new Chunk(amountTotal.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell finalAmountTotalData = new PdfPCell(new Phrase(new Chunk(finalAmountTotal.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell totalBlank = new PdfPCell();

					pageFooter.AddCell(totalBlank);
					pageFooter.AddCell(totalVariable);
					pageFooter.AddCell(bagsTotalData);
					pageFooter.AddCell(weightTotalData);
					pageFooter.AddCell(totalBlank);
					pageFooter.AddCell(amountTotalData);
					pageFooter.AddCell(totalBlank);
					pageFooter.AddCell(totalBlank);
					pageFooter.AddCell(finalAmountTotalData);

					//-----------------------------------------------------------------------------------------------------------------------------------------------
					// DEDUCTION STARTS

					int kantaDeduction = Convert.ToInt32(kantaInput.Text);
					float gaadiExpenseDeduction = (float.Parse(totalWeightInput.Text)) * 4;
					int roundedGaadiExpenseDeduction = (int)Math.Round(gaadiExpenseDeduction, 0);
					float amountAfterDeduction = 0;
					int roundedAmountAfterDeduction = 0;

					if (finalAmountTotal != 0)
					{
						amountAfterDeduction = finalAmountTotal - (kantaDeduction + gaadiExpenseDeduction);
						roundedAmountAfterDeduction = (int)Math.Round(amountAfterDeduction, 0);
					}

					PdfPTable amountDeductions = new PdfPTable(5);
					amountDeductions.SetWidths(new float[] { 8f, 4f, 13f, 8f, 4f });

					PdfPCell kantaDeductionAmount = new PdfPCell(new Phrase(new Chunk("Dharam Kanta: ", FontFactory.GetFont("HELVETICA", 8, Font.BOLD))));
					PdfPCell kantaDeductionAmountData = new PdfPCell(new Phrase(new Chunk("- " + kantaDeduction.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell gaadiExpenseAmount = new PdfPCell(new Phrase(new Chunk("Gaadi Expense: ", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell gaadiExpenseAmountData = new PdfPCell(new Phrase(new Chunk("- " + roundedGaadiExpenseDeduction.ToString(), FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell resultantAmount = new PdfPCell(new Phrase(new Chunk("Amount Payable: ", FontFactory.GetFont("Verdana", 8, Font.BOLD))));
					PdfPCell resultantAmountData = new PdfPCell(new Phrase(new Chunk(roundedAmountAfterDeduction.ToString() + "/-", FontFactory.GetFont("Verdana", 8, Font.BOLD))));


					kantaDeductionAmount.Border = kantaDeductionAmountData.Border = gaadiExpenseAmount.Border = gaadiExpenseAmountData.Border = resultantAmount.Border = resultantAmountData.Border = moistAllowed.Border = moistAllowedData.Border = vehicleNumber.Border = vehicleNumberData.Border = totalweight.Border = totalWeightData.Border = blank.Border = 0;

					amountDeductions.AddCell(kantaDeductionAmount);
					amountDeductions.AddCell(kantaDeductionAmountData);
					amountDeductions.AddCell(blank);
					amountDeductions.AddCell(blank);
					amountDeductions.AddCell(blank);
					amountDeductions.AddCell(gaadiExpenseAmount);
					amountDeductions.AddCell(gaadiExpenseAmountData);
					amountDeductions.AddCell(blank);
					amountDeductions.AddCell(resultantAmount);
					amountDeductions.AddCell(resultantAmountData);



					document.Open();
					document.Add(lineBreaks);
					document.Add(pageHeader);
					document.Add(pageTitle);
					document.Add(lineBreaks);
					document.Add(pageBody);
					document.Add(lineBreaks);
					document.Add(pageBodyDataGrid);
					document.Add(pageFooter);
					document.Add(lineBreaks);
					document.Add(amountDeductions);

					MessageBoxResult result = MessageBox.Show("Are you sure you want to print the Document?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
					switch (result)
					{
						case MessageBoxResult.Yes:
							MessageBox.Show("Document created Successfully! Click OK to continue...", "Result", MessageBoxButton.OK, MessageBoxImage.Information);
							break;
						case MessageBoxResult.No:
							document.Close();
							return;
					}

					document.Close();
					System.Diagnostics.Process.Start(@fileName);

				}


			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
		}

		private void NumericValidation(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}
		private void NumericFloatValidation(object sender, TextCompositionEventArgs e)
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
