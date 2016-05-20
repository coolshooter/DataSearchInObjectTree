using System;
using System.Collections.Generic;
using System.Data;
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
using LookForDataInMemory.Core;

namespace LookForDataInMemory.Wpf
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public List<int> Items;
		public List<Data> Items2;
		public DataSet DS = new DataSet();

		public MainWindow()
		{
			InitializeComponent();
			Loaded += MainWindow_Loaded;
		}

		void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			List<int> list = new List<int>() { 1, 2, 5 };
			List<Data> list2 = new List<Data>() { new Data() };
			Items = list;
			Items2 = list2;
			DS.Tables.Add("User");
			DS.Tables[0].Columns.Add("Id");
			DS.Tables[0].Columns.Add("Name");
			DS.Tables[0].Columns.Add("Birthdate");
			DS.Tables[0].Rows.Add("1", "Vasya", new DateTime(2000, 1, 1));
			DS.Tables[0].Rows.Add("2", "Petya", new DateTime(2000, 2, 2));

			var res1 = Engine.Find(2000, this, 7);
			var res2 = Engine.Find("cool text", this);

			bool found1 = res1.Paths.Any();
			bool found2 = res2.Paths.Any();
		}

		public class Data
		{
			public string A = "Some cool text 4 you";
		}
	}
}
