using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookForDataInMemory.Core
{
	public class FindResult
	{
		public object SearchValue = null;
		public List<string> Paths = new List<string>();

		/// <summary>
		/// Не подошедшие пути.
		/// </summary>
		public List<string> CheckedPaths = new List<string>();

		public void PrepareResults()
		{
			if (Paths == null)
				Paths = new List<string>();

			Paths = Paths.OrderBy(r => r.Length).ToList();

			for (int i = 0; i < Paths.Count; i++)
			{
				if (Paths[i].Length > 5)
					Paths[i] = Paths[i].Substring(5);
			}

			for (int i = 0; i < CheckedPaths.Count; i++)
				if (CheckedPaths[i].Length > 5)
					CheckedPaths[i] = CheckedPaths[i].Substring(5);
		}

		public void AddFrom(FindResult source)
		{
			if (Paths == null)
				Paths = new List<string>();

			Paths.AddRange(source.Paths);
			CheckedPaths.AddRange(source.CheckedPaths);
		}
	}
}
