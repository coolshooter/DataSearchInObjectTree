using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LookForDataInMemory.Core
{
	public class ObjectsEqualResult
	{
		public bool AreEqual { get; set; }
		public bool ContainsText { get; set; }

		public string TunePath(string path)
		{
			if (ContainsText)
				return path + " (в тексте)";
			else
				return path;
		}
	}
}
