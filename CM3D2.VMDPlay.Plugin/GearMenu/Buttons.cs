using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;

namespace GearMenu
{
	public static class Buttons
	{
		private class OnRepositionHandler
		{
			public string Version;

			private static string[] OnlineButtonNames = new string[7]
			{
				"Config",
				"Ss",
				"SsUi",
				"Shop",
				"ToTitle",
				"Info",
				"Exit"
			};

			private static string[] OfflineButtonNames = new string[6]
			{
				"Config",
				"Ss",
				"SsUi",
				"ToTitle",
				"Info",
				"Exit"
			};

			public OnRepositionHandler(string version)
			{
				Version = version;
			}

			public void OnReposition()
			{
			}

			public unsafe void PreOnReposition()
			{
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
				//IL_0116: Unknown result type (might be due to invalid IL or missing references)
				//IL_0125: Unknown result type (might be due to invalid IL or missing references)
				//IL_014f: Unknown result type (might be due to invalid IL or missing references)
				//IL_015c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0161: Unknown result type (might be due to invalid IL or missing references)
				//IL_019a: Unknown result type (might be due to invalid IL or missing references)
				//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
				//IL_0218: Unknown result type (might be due to invalid IL or missing references)
				//IL_021d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0222: Unknown result type (might be due to invalid IL or missing references)
				//IL_0227: Unknown result type (might be due to invalid IL or missing references)
				//IL_0230: Unknown result type (might be due to invalid IL or missing references)
				//IL_0235: Unknown result type (might be due to invalid IL or missing references)
				//IL_0257: Unknown result type (might be due to invalid IL or missing references)
				//IL_025c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0261: Unknown result type (might be due to invalid IL or missing references)
				UIGrid gridUI = GridUI;
				UISprite baseSprite = BaseSprite;
				float num = 0.75f;
				float pixelSizeAdjustment = UIRoot.GetPixelSizeAdjustment(Base);
				gridUI.cellHeight = gridUI.cellWidth;
				gridUI.arrangement = (UIGrid.Arrangement)2;
				gridUI.sorting = 0;
				gridUI.pivot = (UIWidget.Pivot)2;
				gridUI.maxPerLine = (int)((float)Screen.width / (gridUI.cellWidth / pixelSizeAdjustment) * num);
				List<Transform> childList = gridUI.GetChildList();
				int count = childList.Count;
				int num2 = Math.Min(gridUI.maxPerLine, count);
				int num3 = Math.Max(1, (count - 1) / gridUI.maxPerLine + 1);
				int num4 = (int)(gridUI.cellWidth * 3f / 2f + 8f);
				int num5 = (int)(gridUI.cellHeight / 2f);
				float num6 = (float)num5 * 1.5f + 1f;
				baseSprite.pivot = (UIWidget.Pivot)2;
				baseSprite.width = (int)((float)num4 + gridUI.cellWidth * (float)num2);
				baseSprite.height = (int)((float)num5 + gridUI.cellHeight * (float)num3 + 2f);
				Base.transform.localPosition = new Vector3(946f, 502f + num6, 0f);
				Grid.transform.localPosition = new Vector3(-2f + (float)(-num2 - 1 + num3 - 1) * gridUI.cellWidth, -1f - num6, 0f);
				int num7 = 0;
				string[] array = GameMain.Instance.CMSystem.NetUse ? OnlineButtonNames : OfflineButtonNames;
				foreach (Transform item in childList)
				{
					int num9 = num7++;
					int num10 = Array.IndexOf(array, item.gameObject.name);
					if (num10 >= 0)
					{
						num9 = num10;
					}
					float num11 = (float)(-num9 % gridUI.maxPerLine + num2 - 1) * gridUI.cellWidth;
					float num12 = (float)(num9 / gridUI.maxPerLine) * gridUI.cellHeight;
					item.localPosition = new Vector3(num11, 0f - num12, 0f);
				}
				UISprite sysShortcutExplanation = SysShortcutExplanation;
				Vector3 localPosition = sysShortcutExplanation.gameObject.transform.localPosition;
				localPosition.y = (Base.transform.localPosition).y - (float)baseSprite.height - (float)sysShortcutExplanation.height;
				sysShortcutExplanation.gameObject.transform.localPosition = localPosition;
			}
		}

		private static string Name_ = "CM3D2.GearMenu.Buttons";

		private static string Version_ = Name_ + " 0.0.2.0";

		public static readonly Color DefaultFrameColor = new Color(1f, 1f, 1f, 0f);

		public static string Name => Name_;

		public static string Version => Version_;

		public static SystemShortcut SysShortcut => GameMain.Instance.SysShortcut;

		public static UIPanel SysShortcutPanel => SysShortcut.GetComponent<UIPanel>();

		public static UISprite SysShortcutExplanation
		{
			get
			{
				FieldInfo field = typeof(SystemShortcut).GetField("m_spriteExplanation", BindingFlags.Instance | BindingFlags.NonPublic);
				if (field == null)
				{
					return null;
				}
				return field.GetValue((object)SysShortcut) as UISprite;
			}
		}

		public static GameObject Base => SysShortcut.gameObject.transform.Find("Base")
			.gameObject;

		public static UISprite BaseSprite => Base.GetComponent<UISprite>();

		public static GameObject Grid => Base.gameObject.transform.Find("Grid")
			.gameObject;

		public static UIGrid GridUI => Grid.GetComponent<UIGrid>();

		public static GameObject Add(PluginBase plugin, byte[] pngData, Action<GameObject> action)
		{
			return Add(null, plugin, pngData, action);
		}

		public static GameObject Add(string name, PluginBase plugin, byte[] pngData, Action<GameObject> action)
		{
			PluginNameAttribute val = Attribute.GetCustomAttribute(((object)plugin).GetType(), typeof(PluginNameAttribute)) as PluginNameAttribute;
			PluginVersionAttribute val2 = Attribute.GetCustomAttribute(((object)plugin).GetType(), typeof(PluginVersionAttribute)) as PluginVersionAttribute;
			string arg = (val == null) ? plugin.name : val.Name;
			string arg2 = (val2 == null) ? string.Empty : val2.Version;
			string label = $"{arg} {arg2}";
			return Add(name, label, pngData, action);
		}

		public static GameObject Add(string label, byte[] pngData, Action<GameObject> action)
		{
			return Add(null, label, pngData, action);
		}

		public unsafe static GameObject Add(string name, string label, byte[] pngData, Action<GameObject> action)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Expected O, but got Unknown
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Expected O, but got Unknown
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			GameObject goButton = null;
			if (Contains(name))
			{
				Remove(name);
			}
			if (action == null)
			{
				return goButton;
			}
			try
			{
				goButton = NGUITools.AddChild(Grid, UTY.GetChildObject(Grid, "Config", true));
				if (name != null)
				{
					goButton.name = name;
				}
				EventDelegate.Set(goButton.GetComponent<UIButton>().onClick, () => { action(goButton); });
				UIEventTrigger component = goButton.GetComponent<UIEventTrigger>();
				EventDelegate.Add(component.onHoverOut, () => { SysShortcut.VisibleExplanation(null, false); });
				EventDelegate.Add(component.onDragStart, () => { SysShortcut.VisibleExplanation(null, false); });
				SetText(goButton, label);
				if (pngData == null)
				{
					pngData = DefaultIcon.Png;
				}
				UISprite component2 = goButton.GetComponent<UISprite>();
				component2.type = (UIBasicSprite.Type)3;
				component2.fillAmount = 0f;
				Texture2D val3 = new Texture2D(1, 1);
				val3.LoadImage(pngData);
				UITexture obj = NGUITools.AddWidget<UITexture>(goButton);
				obj.material = new Material(obj.shader);
				obj.material.mainTexture = val3;
				obj.MakePixelPerfect();
				Reposition();
			}
			catch
			{
				if (goButton != null)
				{
					NGUITools.Destroy(goButton);
					goButton = null;
				}
				throw;
			}
			return goButton;
		}

		public static void Remove(string name)
		{
			Remove(Find(name));
		}

		public static void Remove(GameObject go)
		{
			NGUITools.Destroy(go);
			Reposition();
		}

		public static bool Contains(string name)
		{
			return Find(name) != null;
		}

		public static bool Contains(GameObject go)
		{
			return Contains(go.name);
		}

		public static void SetFrameColor(string name, Color color)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			SetFrameColor(Find(name), color);
		}

		public static void SetFrameColor(GameObject go, Color color)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			UITexture componentInChildren = go.GetComponentInChildren<UITexture>();
			if (!(componentInChildren == null))
			{
				Texture2D val = componentInChildren.mainTexture as Texture2D;
				if (!(val == null))
				{
					for (int i = 1; i < val.width - 1; i++)
					{
						val.SetPixel(i, 0, color);
						val.SetPixel(i, val.height - 1, color);
					}
					for (int j = 1; j < val.height - 1; j++)
					{
						val.SetPixel(0, j, color);
						val.SetPixel(val.width - 1, j, color);
					}
					val.Apply();
				}
			}
		}

		public static void ResetFrameColor(string name)
		{
			ResetFrameColor(Find(name));
		}

		public static void ResetFrameColor(GameObject go)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			SetFrameColor(go, DefaultFrameColor);
		}

		public static void SetText(string name, string label)
		{
			SetText(Find(name), label);
		}

		public unsafe static void SetText(GameObject go, string label)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Invalid comparison between Unknown and I4
			UIEventTrigger component = go.GetComponent<UIEventTrigger>();
			component.onHoverOver.Clear();
			EventDelegate.Add(component.onHoverOver, () => { SysShortcut.VisibleExplanation(label, label != null); });
			if ((int)go.GetComponent<UIButton>().state == 1)
			{
				SysShortcut.VisibleExplanation(label, label != null);
			}
		}

		private static GameObject Find(string name)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Expected O, but got Unknown
			Transform val = GridUI.GetChildList().FirstOrDefault((Transform c) => c.gameObject.name == name);
			if (!(val == null))
			{
				return val.gameObject;
			}
			return null;
		}

		private static void Reposition()
		{
			SetAndCallOnReposition(GridUI);
			GridUI.repositionNow = true;
		}

		private unsafe static void SetAndCallOnReposition(UIGrid uiGrid)
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Expected O, but got Unknown
			string onRepositionVersion = GetOnRepositionVersion(uiGrid);
			if (onRepositionVersion != null)
			{
				if (onRepositionVersion == string.Empty || string.Compare(onRepositionVersion, Version, false) < 0)
				{
					uiGrid.onReposition = new UIGrid.OnReposition(Reposition);
				}
				if (uiGrid.onReposition != null)
				{
					object target = ((Delegate)uiGrid.onReposition).Target;
					target?.GetType().GetMethod("PreOnReposition")?.Invoke(target, new object[0]);
				}
			}
		}

		private static string GetOnRepositionVersion(UIGrid uiGrid)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Expected O, but got Unknown
			if (uiGrid.onReposition != null)
			{
				return string.Empty;
			}
			object target = ((Delegate)uiGrid.onReposition).Target;
			if (target == null)
			{
				return null;
			}
			Type type = target.GetType();
			if (type == null)
			{
				return null;
			}
			FieldInfo field = type.GetField("Version", BindingFlags.Instance | BindingFlags.Public);
			if (field == null)
			{
				return null;
			}
			string text = field.GetValue(target) as string;
			if (text == null || !text.StartsWith(Name))
			{
				return null;
			}
			return text;
		}
	}
}
