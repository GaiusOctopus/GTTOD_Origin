using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace GILES
{
	public static class pb_StringUtil
	{
		public static string ToStringF(this IEnumerable val)
		{
			return val.ToStringF('\n');
		}

		public static string ToStringF(this IEnumerable val, char delimiter)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (object item in val)
			{
				stringBuilder.Append(item.ToString());
				stringBuilder.Append(delimiter);
			}
			return stringBuilder.ToString();
		}

		public static string SplitCamelCase(this string str)
		{
			char[] array = Regex.Replace(str, "(?=\\p{Lu}\\p{Ll})|(?<=\\p{Ll})(?=\\p{Lu})", " ", RegexOptions.Compiled).Trim().ToCharArray();
			array[0] = char.ToUpper(array[0]);
			return new string(array);
		}

		public static string Truncate(this string value, int length)
		{
			if (value.Length > length)
			{
				return value.Substring(0, length);
			}
			return value;
		}
	}
}
