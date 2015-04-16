
namespace SRF
{

	public static class DebugUtil
	{

		/// <summary>
		/// Simple helper function for creating debug toggles in unity inspectors
		/// </summary>
		/// <param name="boolean"></param>
		/// <returns></returns>
		public static bool DebugToggle(ref bool boolean)
		{

#if UNITY_EDITOR

			if (boolean) {
				boolean = false;
				return true;
			}

#endif

			return false;

		}


	}

}