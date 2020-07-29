using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
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
	/// Interaction logic for Options.xaml
	/// </summary>
	public partial class TemplateOptions : Page
	{
		public DotnetTemplate ChosenTemplate { get; set; }

		public TemplateOptions(DotnetTemplate temp)
		{
			InitializeComponent();
			ChosenTemplate = temp;
			SelectedTxt.Text = ChosenTemplate.Template;
			SetAvailableOptions();
			GenerateCommand();
		}

		void SetAvailableOptions()
		{
			ProcessStartInfo psi = new ProcessStartInfo("dotnet", $"new {ChosenTemplate.ShortName} -h");
			psi.RedirectStandardOutput = true;
			psi.ErrorDialog = true;
			psi.CreateNoWindow = true;
			Process proc = new Process();
			proc.StartInfo = psi;
			proc.Start();
			var fulltext = proc.StandardOutput.ReadToEnd();

			FrameworkCombo.Items.Clear();
			string[] frameworks = { "net5.0", "netcoreapp3.1", "netcoreapp3.0", "netcoreapp2.2", "netcoreapp2.1", "netcoreapp2.0" };
			foreach (var fw in frameworks)
			{
				if (fulltext.Contains(fw))
				{
					FrameworkCombo.Items.Add(fw);
				}
			}
			if (FrameworkCombo.Items.Count > 0)
			{
				FrameworkCombo.SelectedIndex = 0;
			}
			else
			{
				FrameworkCombo.IsEnabled = false;
			}

			LangCombo.Items.Clear();
			string[] langs = { "C#", "F#", "VB" };
			foreach (var lang in langs)
			{
				if (ChosenTemplate.Languages.Contains(lang))
				{
					LangCombo.Items.Add(lang);
				}
			}
			if (LangCombo.Items.Count > 0)
			{
				LangCombo.SelectedIndex = 0;
			}
			else
			{
				LangCombo.IsEnabled = false;
			}
		}

		string GenerateCommand()
		{
			if (ChosenTemplate == null)
			{
				return "-h";
			}
			string cmd = $"new {ChosenTemplate.ShortName}";
			if (DryCheck.IsChecked == true)
			{
				cmd += " --dry-run";
			}
			if (ForceCheck.IsChecked == true)
			{
				cmd += " --force";
			}
			if (LangCombo.SelectedIndex >= 0)
			{
				cmd += $" --language \"{LangCombo.SelectedItem}\"";
			}
			if (!string.IsNullOrWhiteSpace(NameTxt.Text))
			{
				cmd += $" --name \"{NameTxt.Text}\"";
			}
			if (!string.IsNullOrWhiteSpace(BrowseTxt.Text))
			{
				cmd += $" --output \"{BrowseTxt.Text}\"";
			}
			if (FrameworkCombo.SelectedIndex >= 0)
			{
				cmd += $" --framework \"{FrameworkCombo.SelectedItem}\"";
			}
			CommandTxt.Text = "dotnet " + cmd;
			return cmd;
		}

		private void Back_Click(object sender, RoutedEventArgs e)
		{
			if (Parent is MainWindow mw)
			{
				mw.Content = new TemplateList();
			}
		}

		private void Combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			GenerateCommand();
		}

		private void OptionsChanged(object sender, TextChangedEventArgs e)
		{
			GenerateCommand();
		}

		private void Checked(object sender, RoutedEventArgs e)
		{
			GenerateCommand();
		}

		private void BrowseBtn_Click(object sender, RoutedEventArgs e)
		{
			VistaFolderBrowserDialog folderDiag = new VistaFolderBrowserDialog();
			folderDiag.ShowDialog();
			if (!string.IsNullOrWhiteSpace(folderDiag.SelectedPath))
			{
				BrowseTxt.Text = folderDiag.SelectedPath;
			}
		}

		private void Run_Click(object sender, RoutedEventArgs e)
		{
			if (Parent is MainWindow mw)
			{
				mw.Content = new Results(GenerateCommand());
			}
		}
	}
}
