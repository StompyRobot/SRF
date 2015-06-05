using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class SRFStringExtensions
{
#if UNITY_EDITOR
	[JetBrains.Annotations.StringFormatMethod("formatString")]
#endif
	public static string Fmt(this string formatString, params object[] args)
	{
		return String.Format(formatString, args);
	}

}