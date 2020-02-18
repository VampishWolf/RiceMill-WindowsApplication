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
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Text.RegularExpressions;
/*using MessageBox = System.Windows.MessageBox;*/

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

		/*private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.RowIndex >= 0)
			{

				*//*DataGridViewRow row = this.DataGridView.Rows[e.RowIndex];*//*
				DataGridViewRow row = DataGridView.SelectedRows[1];

				bagsCountInput.Text = row.Cells["BagsCount"].Value.ToString();
				totalWeightInput.Text = row.Cells["ItemName"].Value.ToString();
				rateInput.Text = row.Cells["Weight"].Value.ToString();
				moistAllowedInput.Text = row.Cells["Rate"].Value.ToString();
				moistInput.Text = row.Cells["Moisture"].Value.ToString();

			}

		}*/

		private void Add_Click(object sender, RoutedEventArgs e)
		{
			try
			{				
				int bagscount = Convert.ToInt32(bagsCountInput.Text);
				float totalweight = Convert.ToInt32(totalWeightInput.Text);
				float weight = totalweight / bagscount;
				float rate = Convert.ToInt32(rateInput.Text);
				float amount = bagscount * rate;
				float moistallowed = Convert.ToInt32(moistAllowedInput.Text);
				float moist = Convert.ToInt32(moistInput.Text);
				float claim;				
				if (moist <= 18){
					claim = 0;
				} else
				{
					claim = moist - moistallowed;
				}
				float claimant = (claim * amount) / 100;
				float finalamount = (bagscount * rate) - claimant;
				// DataGridView.Rows.Add( bagscount, itemNameInput.Text, weight, rate, moist, claim, amount );
				DataGridView.Items.Add(new { BagsCount = bagscount, ItemName = itemNameInput.Text, Weight = weight, Rate = rate, Amt = amount, Moisture = moist, Claim = claim, Amount = finalamount });
				// DataGridView.Items.Add(bagscount);
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
				float totalweight = Convert.ToInt32(totalWeightInput.Text);
				float weight = totalweight / bagscount;
				float rate = Convert.ToInt32(rateInput.Text);
				float amount = bagscount * rate;
				float moistallowed = Convert.ToInt32(moistAllowedInput.Text);
				float moist = Convert.ToInt32(moistInput.Text);
				float claim;
				if (moist <= 18)
				{
					claim = 0;
				}
				else
				{
					claim = moist - moistallowed;
				}
				float claimant = (claim * amount) / 100;
				float finalamount = (bagscount * rate) - claimant;
				// DataGridView.Rows.Add( bagscount, itemNameInput.Text, weight, rate, moist, claim, amount );
				DataGridView.Items.Add(new { BagsCount = bagscount, ItemName = itemNameInput.Text, Weight = weight, Rate = rate, Amt = amount, Moisture = moist, Claim = claim, Amount = finalamount });
				// DataGridView.Items.Add(bagscount);
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
		private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
		{
			Regex regex = new Regex("[^0-9]+");
			e.Handled = regex.IsMatch(e.Text);
		}

		
	}
}
