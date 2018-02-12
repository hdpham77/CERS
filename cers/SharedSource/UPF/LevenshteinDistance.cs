/* Based off of the following article.
 * http://www.codeproject.com/Articles/11157/An-improvement-on-capturing-similarity-between-str
 *
 */

using System;
using System.Linq;

namespace UPF
{
	public static class LevenshteinDistance
	{
		private static int Min3(int a, int b, int c)
		{
			return System.Math.Min(System.Math.Min(a, b), c);
		}

		public static int ComputeDistance(string s, string t)
		{
			int n = s.Length;
			int m = t.Length;
			int[,] distance = new int[n + 1, m + 1]; // matrix
			int cost = 0;

			if (n == 0) return m;
			if (m == 0) return n;
			//init1
			for (int i = 0; i <= n; distance[i, 0] = i++)
			{
			}

			for (int j = 0; j <= m; distance[0, j] = j++)
			{
			}

			//find min distance
			for (int i = 1; i <= n; i++)
			{
				for (int j = 1; j <= m; j++)
				{
					cost = (t.Substring(j - 1, 1) == s.Substring(i - 1, 1) ? 0 : 1);
					distance[i, j] = Min3(distance[i - 1, j] + 1,
						distance[i, j - 1] + 1,
						distance[i - 1, j - 1] + cost);
				}
			}

			return distance[n, m];
		}

		public static float GetSimilarity(string first, string second)
		{
			float distance = ComputeDistance(first, second);

			float maxLength = first.Length;
			if (maxLength < (float)second.Length)
			{
				maxLength = second.Length;
			}

			float minimumLen = first.Length;
			if (minimumLen > (float)second.Length)
				minimumLen = second.Length;

			float result = 0F;

			if (maxLength == 0.0F)
			{
				result = 1.0F;
			}
			else
			{
				result = maxLength - distance;
			}

			return result;
		}
	}
}