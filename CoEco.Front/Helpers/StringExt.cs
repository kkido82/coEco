using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoEco.Front.Helpers
{
	public static class StringExt
	{
		public static int[] ParseInts(this string str)
		{
			var nums = new List<int>();

			foreach (var part in str.Split(','))
			{
				if (int.TryParse(part,out int num))
					nums.Add(num);
			}

			return nums.ToArray();
		}
	}
}