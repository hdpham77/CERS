using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPF
{
	public static class NumericExtensionMethods
	{
		public static bool ContainedIn(this int value, params int[] ints)
		{
			return ints.Contains(value);
		}

		public static bool ContainedIn(this int value, List<int> ints)
		{
			return ints.Contains(value);
		}

		public static int PagesRequired(this int totalRecordCount, int pageSize)
		{
			int totalPageCount = 0;
			int quotient = 0;
			int mod = 0;
			quotient = Math.DivRem(totalRecordCount, pageSize, out quotient);
			quotient = (quotient == 0) ? quotient + 1 : quotient;
			if (totalRecordCount > pageSize)
			{
				mod = totalRecordCount % pageSize;
				mod = (mod == 0) ? 0 : 1;
			}

			totalPageCount += quotient + mod;

			return totalPageCount;
		}
	}
}