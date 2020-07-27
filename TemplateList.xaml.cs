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
	/// Interaction logic for TemplateList.xaml
	/// </summary>
	public partial class TemplateList : Page
	{
		List<DotnetTemplate> MyItems { get; set; } = new List<DotnetTemplate>();
		public TemplateList()
		{
			InitializeComponent();
			LoadDotnetTemplates();
		}
		private void LoadDotnetTemplates()
		{
			MyItems.Clear();
			ProcessStartInfo psi = new ProcessStartInfo("dotnet", "new -l");
			psi.RedirectStandardOutput = true;
			psi.ErrorDialog = true;
			psi.CreateNoWindow = true;
			Process proc = new Process();
			proc.StartInfo = psi;
			proc.Start();
			var oReader = proc.StandardOutput;
			var firstLine = oReader.ReadLine();
			var secondLine = oReader.ReadLine();

			bool prevBlank = true;
			bool bigBlank = true;
			int[] breakpoints = new int[4];
			int brki = 0;
			for (int i = 0; i < firstLine.Length; i++)
			{
				bool thisIsBlank = char.IsWhiteSpace(firstLine[i]);
				if (!thisIsBlank && bigBlank)
				{
					breakpoints[brki] = i;
					brki++;
				}
				bigBlank = prevBlank && thisIsBlank;
				prevBlank = thisIsBlank;

			}
			while (true)
			{
				var line = oReader.ReadLine();
				if (line.Length < breakpoints[3])
				{
					break;
				}
				string tmp = line.Substring(breakpoints[0], breakpoints[1]).TrimEnd();
				string shortName = line.Substring(breakpoints[1], breakpoints[2] - breakpoints[1]).TrimEnd();
				string langs = line.Substring(breakpoints[2], breakpoints[3] - breakpoints[2]).TrimEnd();
				string tags = line.Substring(breakpoints[3]).TrimEnd();
				MyItems.Add(new DotnetTemplate() { Template = tmp, ShortName = shortName, Languages = langs, Tags = tags });
			}
			MyListView.ItemsSource = null;
			MyListView.ItemsSource = MyItems;
		}

		private void RfrshBtn_Click(object sender, RoutedEventArgs e)
		{
			LoadDotnetTemplates();
		}

		private void NextBtn_Click(object sender, RoutedEventArgs e)
		{
			if (MyListView.SelectedIndex >= 0)
			{
				if (Parent is MainWindow mw)
				{
					mw.Content = new TemplateOptions(MyItems[MyListView.SelectedIndex]);
				}
			}
		}

		private void MyListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			NextBtn.IsEnabled = MyListView.SelectedIndex >= 0;
		}
	}
}
