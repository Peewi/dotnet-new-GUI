using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dotnet_new_GUI
{
	/// <summary>
	/// Interaction logic for Results.xaml
	/// </summary>
	public partial class Results : Page
	{
		public Results(string dotnetArgs)
		{
			InitializeComponent();

			ProcessStartInfo psi = new ProcessStartInfo("dotnet", dotnetArgs);
			psi.RedirectStandardOutput = true;
			psi.RedirectStandardError = true;
			//psi.ErrorDialog = true;
			psi.CreateNoWindow = true;
			psi.UseShellExecute = false;
			Process proc = new Process();
			proc.StartInfo = psi;
			proc.Start();
			ResultsOutput.Text += proc.StandardOutput.ReadToEnd();
			ResultsOutput.Text += proc.StandardError.ReadToEnd();
		}

		private void NewProjectBtn_Click(object sender, RoutedEventArgs e)
		{
			if (Parent is MainWindow mw)
			{
				mw.Content = new TemplateList();
			}
		}
	}
}
