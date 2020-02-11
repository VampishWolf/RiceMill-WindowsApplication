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

			//this.Left = SystemParameters.PrimaryScreenWidth - this.Width;
			//this.Top = SystemParameters.WorkArea.Bottom - this.Height;

			DispatcherTimer timer = new DispatcherTimer();
			timer.Tick += Timer_Tick;
			timer.Start();
			
		}
		void Timer_Tick(object sender, EventArgs e)
		{
			dateDisplayLabel.Content = DateTime.Now.ToString("dd/MM/yyyy	hh:mm:ss tt");
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int bagscount = Convert.ToInt32(bagsCountInput.Text);
				DataGridView.Items.Add(bagscount);
			} catch (Exception ex)
			{
				MessageBox.Show(ex.Message.ToString());
			}
			
		}
	}
}
