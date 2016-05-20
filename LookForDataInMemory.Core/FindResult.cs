using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookForDataInMemory.Core
{
	public class FindResult
	{
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
