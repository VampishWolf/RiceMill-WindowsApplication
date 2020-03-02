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
		private void Reset_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var grid = DataGridView;
				grid.Items.Clear();
				grid.Items.Refresh();
				serialnumber = 0;
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
				System.Windows.Controls.PrintDialog printDlg = new System.Windows.Controls.PrintDialog();
				if (printDlg.ShowDialog() == true)
				{
					//get selected printer capabilities
					System.Printing.PrintCapabilities capabilities = printDlg.PrintQueue.GetPrintCapabilities(printDlg.PrintTicket);

					//get scale of the print wrt to screen of WPF visual
					double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / this.ActualWidth, capabilities.PageImageableArea.ExtentHeight /
						   this.ActualHeight);

					//Transform the Visual to scale
					this.LayoutTransform = new ScaleTransform(scale, scale);

					//get the size of the printer page
					Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

					//update the layout of the visual to the printer page size.
					this.Measure(sz);
					this.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));

					//now print the visual to printer to fit on the one page.
					printDlg.PrintVisual(this, "First Fit to Page WPF Print");

				}
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
