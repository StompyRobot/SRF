using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

public static class StringExtensions
{

	[StringFormatMethod("formatString")]
	public static string Fmt(this string formatString, params object[] args)
	{
		return String.Format(formatString, args);
	}

}