using System.Collections.Generic;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	public class KeyUtil
	{
		private List<KeyCode> supportKeys = new List<KeyCode>();

		private string key;

		public static KeyUtil Parse(string keyPattern)
		{
			string[] array = keyPattern.Split('+');
			List<KeyCode> list = new List<KeyCode>();
			string text = "";
			if (array.Length == 1)
			{
				text = array[0].ToLower();
			}
			else
			{
				for (int i = 0; i < array.Length - 1; i++)
				{
					string a = array[i].ToLower();
					if (a == "ctrl")
					{
						list.Add((KeyCode)306);
						list.Add((KeyCode)305);
					}
					else if (a == "shift")
					{
						list.Add((KeyCode)304);
						list.Add((KeyCode)303);
					}
					else if (a == "alt")
					{
						list.Add((KeyCode)308);
						list.Add((KeyCode)307);
					}
				}
				text = array[array.Length - 1].ToLower();
			}
			return new KeyUtil
			{
				supportKeys = list,
				key = text
			};
		}

		public bool TestKeyUp()
		{
			if (Input.GetKeyUp(key))
			{
				return TestSupports();
			}
			return false;
		}

		public bool TestKeyDown()
		{
			if (Input.GetKeyUp(key))
			{
				return TestSupports();
			}
			return false;
		}

		public bool TestSupports()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < supportKeys.Count; i += 2)
			{
				if (!Input.GetKey(supportKeys[i]) && !Input.GetKey(supportKeys[i + 1]))
				{
					return false;
				}
			}
			return true;
		}
	}
}
