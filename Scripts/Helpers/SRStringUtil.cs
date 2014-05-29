using UnityEngine;
using System.Collections;

public static class SRStringUtil
{

	private const int IntToStringBufferSize = 512;

	private static string[] _intBuffer;

	private static void Init()
	{

		_intBuffer = new string[IntToStringBufferSize];

		for (int i = 0; i < IntToStringBufferSize; i++) {
			_intBuffer[i] = i.ToString("000");
		}

	}

	/// <summary>
	/// Convert i to string using static cached buffer
	/// </summary>
	/// <param name="i"></param>
	/// <returns></returns>
	public static string IntToString(int i)
	{

		if(_intBuffer == null)
			Init();

		if (i < 0 || i >= _intBuffer.Length) {
			Debug.LogWarning("Int exceeds buffer size ({0} > {1})".Fmt(i, IntToStringBufferSize));
			return i.ToString("000");
		}

		return _intBuffer[i];

	}

}
