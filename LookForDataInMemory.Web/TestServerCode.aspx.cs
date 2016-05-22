using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LookForDataInMemory.Core;

namespace LookForDataInMemory.Web
{
	public partial class TestServerCode : System.Web.UI.Page
	{
		public object Items;
		public List<Data> Items2;
		public DataSet DS = new DataSet();

		protected void Page_Load(object sender, EventArgs e)
		{
			FillData();

			var res1 = Engine.Find(50, this);
			var res2 = Engine.Find(2000, this, 7);
			var res3 = Engine.Find("cool text", this);

			ClearResult();
			AppendResult(res1);
			AppendResult(res2);
			AppendResult(res3);

			txtResult.Text += "\n\nПримерный код для поиска в списке:\n";
			txtResult.Text +=
@"
object items = new List<string>() { ""10"", ""20"", ""50"" };

/// ищем значение ""50""
var res1 = Engine.Find(""50"", items);

/// или 50 (тоже найдется)
var res2 = Engine.Find(50, items);

/// количество найденных путей к свойствам, содержащим искомое значение:
int count1 = res1.Paths.Count;
int count2 = res2.Paths.Count;
";
        }

		void ClearResult()
		{
			txtResult.Text = "";
		}

		void AppendResult(FindResult res)
		{
			txtResult.Text += "\n\nОчередной поиск:\n";
			txtResult.Text += "\nИскали значение: " + res.SearchValue;

			if (res.Paths.Any())
			{
				txtResult.Text += "\nРезультаты в виде путей к свойствам:";

				foreach (var path in res.Paths)
					txtResult.Text += "\n" + path;
            }
			else
				txtResult.Text += "\nНичего не найдено";

			txtResult.Text += "\nИсследовано " + 
				res.Paths.Count + res.CheckedPaths.Count + " вариантов";

			txtResult.Text = txtResult.Text.TrimStart();
        }

		void FillData()
		{
			//Items = new List<int>() { 10, 20, 50 };
			Items = new List<string>() { "10", "20", "50" };
			Items2 = new List<Data>() { new Data("Some cool text 4 you") };

			DS.Tables.Add("User");
			DS.Tables[0].Columns.Add("Id");
			DS.Tables[0].Columns.Add("Name");
			DS.Tables[0].Columns.Add("Birthdate");
			DS.Tables[0].Rows.Add("1", "Vasya", new DateTime(2000, 1, 1));
			DS.Tables[0].Rows.Add("2", "Petya", new DateTime(2000, 2, 2));
		}

		public class Data
		{
			public string Text { get; set; }

			public Data(string text)
			{
				Text = text;
            }
		}
	}
}