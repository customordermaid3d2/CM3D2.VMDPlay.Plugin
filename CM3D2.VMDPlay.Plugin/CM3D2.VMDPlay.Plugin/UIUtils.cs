using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	internal class UIUtils
	{
		public const int UIRootWidth = 1920;

		public const int UIRootHeight = 1080;

		private static GameObject _goButtonTemplate;

		private static Font _font;

		public static UIAtlas GetSystemDialogAtlas()
		{
			return FindAtlas("SystemDialog");
		}

		public static Font GetDefaultFont()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			if (_font == null)
			{
				_font = GameObject.Find("SystemUI Root").GetComponentsInChildren<UILabel>()[0].trueTypeFont;
			}
			return _font;
		}

		public static GameObject GetButtonTemplateGo()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Expected O, but got Unknown
			if (_goButtonTemplate == null)
			{
				_goButtonTemplate = (UnityEngine.Object.Instantiate(GameMain.Instance.gameObject.transform.Find("SystemUI Root/ConfigPanel/Screen/FullScreen/On").gameObject,
					Vector3.zero, Quaternion.identity) as GameObject);
				_goButtonTemplate.GetComponent<UIButton>().onClick.Clear();
				_goButtonTemplate.SetActive(false);
			}
			return _goButtonTemplate;
		}

		public unsafe static void SetToggleButtonColor(UIButton button, bool b)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			Color defaultColor = button.defaultColor;
			button.defaultColor = new Color((defaultColor).r, (defaultColor).g, (defaultColor).b, b ? 1f : 0.5f);
		}

		public static UIAtlas FindAtlas(string s)
		{
			return new List<UIAtlas>(Resources.FindObjectsOfTypeAll<UIAtlas>()).FirstOrDefault((UIAtlas a) => a.name == s);
		}

		public static GameObject SetCloneChild(GameObject parent, GameObject orignal, string name)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			GameObject val = UnityEngine.Object.Instantiate(orignal, Vector3.zero, Quaternion.identity) as GameObject;
			if (!(val))
			{
				return null;
			}
			val.name = name;
			SetChild(parent, val);
			return val;
		}

		public static void SetChild(GameObject parent, GameObject child)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			child.layer = parent.layer;
			child.transform.parent = parent.transform;
			child.transform.localPosition = Vector3.zero;
			child.transform.localScale = Vector3.one;
			child.transform.rotation = Quaternion.identity;
		}

		public static Transform FindChild(Transform tr, string s)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			return UIUtils.FindChild(tr.gameObject, s).transform;
		}

		public static GameObject FindChild(GameObject go, string s)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Expected O, but got Unknown
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Expected O, but got Unknown
			if (go == null)
			{
				return null;
			}
			GameObject val = null;
			foreach (Transform item in go.transform)
			{
				Transform val2 = item;
				if (val2.gameObject.name == s)
				{
					return val2.gameObject;
				}
				val = UIUtils.FindChild(val2.gameObject, s);
				if ((val))
				{
					return val;
				}
			}
			return null;
		}
	}
}
