using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	public class GUILayoutx
	{
		public delegate void DoubleClickCallback(int index);

		public static int SelectionList(int selected, GUIContent[] list)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			return SelectionList(selected, list, (GUIStyle)("List Item"), null);
		}

		public static int SelectionList(int selected, GUIContent[] list, GUIStyle elementStyle)
		{
			return SelectionList(selected, list, elementStyle, null);
		}

		public static int SelectionList(int selected, GUIContent[] list, DoubleClickCallback callback)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			return SelectionList(selected, list, (GUIStyle)("List Item"), callback);
		}

		public static int SelectionList(int selected, GUIContent[] list, GUIStyle elementStyle, DoubleClickCallback callback)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Invalid comparison between Unknown and I4
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Invalid comparison between Unknown and I4
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < list.Length; i++)
			{
				Rect rect = GUILayoutUtility.GetRect(list[i], elementStyle);
				bool flag = rect.Contains(Event.current.mousePosition);
				if (flag && (int)Event.current.type == 0)
				{
					selected = i;
					Event.current.Use();
				}
				else if (flag && callback != null && (int)Event.current.type == 1)
				{
					callback(i);
					Event.current.Use();
				}
				else if ((int)Event.current.type == 7)
				{
					elementStyle.Draw(rect, list[i], flag, false, i == selected, false);
				}
			}
			return selected;
		}

		public static int SelectionList(int selected, string[] list)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			return SelectionList(selected, list, (GUIStyle)("List Item"), null);
		}

		public static int SelectionList(int selected, string[] list, GUIStyle elementStyle)
		{
			return SelectionList(selected, list, elementStyle, null);
		}

		public static int SelectionList(int selected, string[] list, DoubleClickCallback callback)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Expected O, but got Unknown
			return SelectionList(selected, list, (GUIStyle)("List Item"), callback);
		}

		public static int SelectionList(int selected, string[] list, GUIStyle elementStyle, DoubleClickCallback callback)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Invalid comparison between Unknown and I4
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Invalid comparison between Unknown and I4
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < list.Length; i++)
			{
				Rect rect = GUILayoutUtility.GetRect(new GUIContent(list[i]), elementStyle);
				bool flag = rect.Contains(Event.current.mousePosition);
				if (flag && (int)Event.current.type == 0)
				{
					selected = i;
					Event.current.Use();
				}
				else if (flag && callback != null && (int)Event.current.type == 1)
				{
					callback(i);
					Event.current.Use();
				}
				else if ((int)Event.current.type == 7)
				{
					elementStyle.Draw(rect, list[i], flag, false, i == selected, false);
				}
			}
			return selected;
		}
	}
}
