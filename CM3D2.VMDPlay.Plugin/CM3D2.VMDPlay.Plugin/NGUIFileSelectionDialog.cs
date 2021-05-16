using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	public class NGUIFileSelectionDialog : UnityEngine.MonoBehaviour
	{
		private readonly int PanelWidth = 800;

		private readonly int PanelHeight = 560;

		private Vector3 panelPosition = Vector3.zero;

		private UIAtlas uiAtlasDialog = UIUtils.GetSystemDialogAtlas();

		private UIAtlas uiAtlasCommon = UIUtils.FindAtlas("AtlasCommon");

		private UIAtlas uiAtlasSceneEdit = UIUtils.FindAtlas("AtlasSceneEdit");

		private Font font = UIUtils.GetDefaultFont();

		private static NGUIFileSelectionDialog _instance;

		public string selectedFile;

		private UIPanel uiPanel;

		private UITable uiTable;

		private UIScrollView uiScrollView;

		private UISprite uiBGSprite;

		private UITable uiTableDirList;

		private Action OnCancel;

		private Action<string> OnFileSelected;

		protected string m_currentDirectory;

		protected string m_filePattern;

		protected Texture2D m_directoryImage;

		protected Texture2D m_fileImage;

		protected FileBrowserType m_browserType;

		protected string m_newDirectory;

		protected string[] m_currentDirectoryParts;

		protected string[] m_files;

		protected int m_selectedFile;

		protected string[] m_nonMatchingFiles;

		protected int m_selectedNonMatchingDirectory;

		protected string[] m_directories;

		protected int m_selectedDirectory;

		protected string[] m_nonMatchingDirectories;

		protected bool m_currentDirectoryMatches;

		public string CurrentDirectory
		{
			get
			{
				return m_currentDirectory;
			}
			set
			{
				SetNewDirectory(value);
				SwitchDirectoryNow();
			}
		}

		public string SelectionPattern
		{
			get
			{
				return m_filePattern;
			}
			set
			{
				m_filePattern = value;
				ReadDirectoryContents();
			}
		}

		public Texture2D DirectoryImage
		{
			get
			{
				return m_directoryImage;
			}
			set
			{
				m_directoryImage = value;
				BuildContent();
			}
		}

		public Texture2D FileImage
		{
			get
			{
				return m_fileImage;
			}
			set
			{
				m_fileImage = value;
				BuildContent();
			}
		}

		public FileBrowserType BrowserType
		{
			get
			{
				return m_browserType;
			}
			set
			{
				m_browserType = value;
				ReadDirectoryContents();
			}
		}

		public static NGUIFileSelectionDialog Instance
		{
			get
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				if (_instance == null)
				{
					try
					{
						_instance = new GameObject("NGUIFileSelectionDialog").AddComponent<NGUIFileSelectionDialog>();
						UnityEngine.Object.DontDestroyOnLoad(_instance.gameObject);
						_instance.CreateUI();
						_instance.Hide();
					}
					catch (Exception value)
					{
						Console.WriteLine(value);
					}
				}
				return _instance;
			}
		}

		private void Start()
		{
			CurrentDirectory = Application.dataPath;
		}

		public static void ShowDialog(string currentDir, string filePattern, Action<string> onFileSelected, Action onCancel)
		{
			Instance.Show(currentDir, filePattern, onFileSelected, onCancel);
		}

		public void Show(string currentDir, string filePattern, Action<string> onFileSelected, Action onCancel)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				if (!uiPanel.gameObject.activeSelf)
				{
					uiPanel.gameObject.SetActive(true);
				}
				m_filePattern = filePattern;
				SetNewDirectory(currentDir);
				OnFileSelected = onFileSelected;
				OnCancel = onCancel;
				SwitchDirectoryNow();
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}

		public void Hide()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			uiPanel.gameObject.SetActive(false);
		}

		public void CreateUI()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Expected O, but got Unknown
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Expected O, but got Unknown
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Expected O, but got Unknown
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Expected O, but got Unknown
			//IL_032d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_0349: Unknown result type (might be due to invalid IL or missing references)
			//IL_0384: Unknown result type (might be due to invalid IL or missing references)
			//IL_039e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_040d: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Expected O, but got Unknown
			//IL_0444: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			//IL_046b: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0486: Unknown result type (might be due to invalid IL or missing references)
			//IL_0497: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0511: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_0522: Unknown result type (might be due to invalid IL or missing references)
			//IL_0557: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_060c: Unknown result type (might be due to invalid IL or missing references)
			//IL_062d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0632: Unknown result type (might be due to invalid IL or missing references)
			//IL_0656: Unknown result type (might be due to invalid IL or missing references)
			if (!(uiPanel != null))
			{
				m_fileImage = new Texture2D(32, 32, (TextureFormat)5, false);
				//m_fileImage.LoadImage(VMDResources.file_icon);
				m_fileImage.Apply();
				m_directoryImage = new Texture2D(32, 32, (TextureFormat)5, false);
				//m_directoryImage.LoadImage(VMDResources.folder_icon);
				int num = 30;
				GameObject val = GameObject.Find("UI Root");
				Vector3 localPosition = default(Vector3);
				localPosition = new Vector3(100f, 100f, 0f);
				uiPanel = NGUITools.AddChild<UIPanel>(val);
				uiPanel.name = "FileSelectionDialog";
				uiPanel.transform.localPosition = localPosition;
				GameObject val2 = uiPanel.gameObject;
				uiBGSprite = NGUITools.AddChild<UISprite>(val2);
				uiBGSprite.name = "BG";
				uiBGSprite.atlas = uiAtlasDialog;
				uiBGSprite.spriteName = "cm3d2_dialog_frame";
				uiBGSprite.type = (UIBasicSprite.Type)1;
				uiBGSprite.SetDimensions(PanelWidth, PanelHeight);
				UISprite val3 = NGUITools.AddChild<UISprite>(val2);
				val3.name = "TitleTab";
				val3.depth = uiBGSprite.depth - 1;
				val3.atlas = uiAtlasDialog;
				val3.spriteName = "cm3d2_dialog_frame";
				val3.type = (UIBasicSprite.Type)1;
				val3.SetDimensions(PanelWidth, 80);
				val3.autoResizeBoxCollider = true;
				val3.gameObject.AddComponent<UIDragObject>().target = val2.transform;
				val3.gameObject.AddComponent<BoxCollider>().isTrigger = true;
				NGUITools.UpdateWidgetCollider(val3.gameObject);
				val3.transform.localPosition = new Vector3(0f, (float)uiBGSprite.height / 2f + 4f, 0f);
				UILabel obj = val3.gameObject.AddComponent<UILabel>();
				obj.depth = val3.depth + 1;
				obj.width = val3.width;
				obj.color = Color.white;
				obj.trueTypeFont = font;
				obj.fontSize = 18;
				obj.text = "File Selection";
				GameObject val4 = UIUtils.SetCloneChild(val3.gameObject, UIUtils.GetButtonTemplateGo(), "CancelButton");
				val4.SetActive(true);
				UIButton component = val4.GetComponent<UIButton>();
				UILabel component2 = val4.transform.GetChild(0).gameObject.GetComponent<UILabel>();
				component2.width = 60;
				component2.fontSize = 18;
				component2.spacingX = 0;
				component2.supportEncoding = true;
				component2.overflowMethod = (UILabel.Overflow)2;
				component2.alignment = (NGUIText.Alignment)2;
				component2.text = "Cancel";
				int num2 = component2.width + 10;
				component.GetComponent<UISprite>().SetDimensions(num2, 30);
				NGUITools.UpdateWidgetCollider(val4);
				val4.transform.localPosition = new Vector3((float)val3.width / 2f - (float)num2 - 10f, 0f, 0f);
				EventDelegate item = new EventDelegate(this, "OnCancelClick");
				component.onClick.Add(item);
				uiTableDirList = NGUITools.AddChild<UITable>(val2);
				uiTableDirList.cellAlignment = 0;
				uiTableDirList.columns = 0;
				uiTableDirList.gameObject.transform.localPosition = new Vector3((float)(-uiBGSprite.width) / 2f + 50f, (float)uiBGSprite.height / 2f - 50f, 0f);
				uiTableDirList.padding = new Vector2(4f, 0f);
				uiTableDirList.keepWithinPanel = true;
				UIPanel obj2 = NGUITools.AddChild<UIPanel>(val2);
				obj2.name = "ScrollView";
				obj2.sortingOrder =  uiPanel.sortingOrder + 1;
				obj2.clipping = (UIDrawCall.Clipping)3;
				obj2.SetRect(0f, 0f, (float)uiBGSprite.width, (float)(uiBGSprite.height - 110 - num));
				obj2.transform.localPosition = new Vector3(-25f, (float)(-num), 0f);
				GameObject val5 = obj2.gameObject;
				uiScrollView = val5.AddComponent<UIScrollView>();
				uiScrollView.contentPivot = (UIWidget.Pivot)4;
				uiScrollView.movement = (UIScrollView.Movement)1;
				uiScrollView.scrollWheelFactor = 1.5f;
				uiBGSprite.gameObject.AddComponent<UIDragScrollView>().scrollView = uiScrollView;
				uiBGSprite.gameObject.AddComponent<BoxCollider>();
				NGUITools.UpdateWidgetCollider(uiBGSprite.gameObject);
				UIScrollBar val6 = NGUITools.AddChild<UIScrollBar>(val2);
				val6.value = 0f;
				val6.gameObject.AddComponent<BoxCollider>();
				val6.transform.localPosition = new Vector3((float)uiBGSprite.width / 2f - 30f, 0f, 0f);
				Transform transform = val6.transform;
				transform.localRotation = transform.localRotation * Quaternion.Euler(0f, 0f, -90f);
				UIWidget val7 = NGUITools.AddChild<UIWidget>(val6.gameObject);
				val7.name = "DummyFore";
				val7.height = 15;
				val7.width = uiBGSprite.height;
				UISprite val8 = NGUITools.AddChild<UISprite>(val6.gameObject);
				val8.name = "Thumb";
				val8.depth = uiBGSprite.depth + 1;
				val8.atlas = uiAtlasSceneEdit;
				val8.spriteName = "cm3d2_edit_slidercursor";
				val8.type = (UIBasicSprite.Type)1;
				val8.SetDimensions(15, 15);
				val8.gameObject.AddComponent<BoxCollider>();
				val6.foregroundWidget = val7;
				val6.thumb = val8.transform;
				NGUITools.UpdateWidgetCollider(val7.gameObject);
				NGUITools.UpdateWidgetCollider(val8.gameObject);
				uiScrollView.verticalScrollBar = val6;
				uiTable = NGUITools.AddChild<UITable>(val5);
				uiTable.pivot = (UIWidget.Pivot)4;
				uiTable.columns = 1;
				uiTable.padding = new Vector2(10f, 5f);
				uiTable.hideInactive = true;
				uiTable.keepWithinPanel = true;
				uiTable.sorting = 0;
			}
		}

		private void BuildContent()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Expected O, but got Unknown
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Expected O, but got Unknown
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Expected O, but got Unknown
			ClearChildUnits(uiTable.gameObject);
			for (int i = 0; i < m_directories.Length; i++)
			{
				AddDirectory(m_directories[i]);
			}
			for (int j = 0; j < m_nonMatchingDirectories.Length; j++)
			{
				AddDirectory(m_nonMatchingDirectories[j]);
			}
			for (int k = 0; k < m_files.Length; k++)
			{
				AddFile(m_files[k]);
			}
			uiTable.Reposition();
			uiScrollView.ResetPosition();
			uiScrollView.verticalScrollBar.value = 0f;
			ClearChildUnits(uiTableDirList.gameObject);
			string text = "";
			for (int l = 0; l < m_currentDirectoryParts.Length; l++)
			{
				string text2 = m_currentDirectoryParts[l];
				if (text2 != "")
				{
					text = text + text2 + "\\";
				}
				GameObject val = UIUtils.SetCloneChild(uiTableDirList.gameObject, UIUtils.GetButtonTemplateGo(), "DirBtn_" + l);
				val.SetActive(true);
				UIButton component = val.GetComponent<UIButton>();
				UILabel component2 = val.transform.GetChild(0).gameObject.GetComponent<UILabel>();
				component2.width = 20;
				component2.fontSize = 18;
				component2.spacingX = 0;
				component2.supportEncoding = true;
				component2.overflowMethod = (UILabel.Overflow)2;
				component2.alignment = (NGUIText.Alignment)2;
				component2.text = (text2 == "") ? "<X>" : text2;
				int num = component2.width + 10;
				component.GetComponent<UISprite>().SetDimensions(num, 30);
				NGUITools.UpdateWidgetCollider(val);
				EventDelegate val2 = new EventDelegate(this, "OnDirListItemClick");
				if (val2 == null)
				{
					Console.WriteLine("del is null");
				}
				val2.parameters[0].value = (object)text;
				component.onClick.Add(val2);
			}
			uiTableDirList.Reposition();
		}

		private void ClearChildUnits(GameObject go)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			int childCount = go.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				UnityEngine.Object.Destroy(go.transform.GetChild(i).gameObject);
			}
		}

		private void AddDirectory(string name)
		{
			AddItem(name, m_directoryImage, "OnDirectoryClick");
		}

		private void AddFile(string name)
		{
			AddItem(name, m_fileImage, "OnFileClick");
		}

		private void AddItem(string label, Texture2D icon, string methodName)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_018c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Expected O, but got Unknown
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			int num = 500;
			int num2 = 40;
			int num3 = 32;
			int num4 = 32;
			int num5 = 4;
			UIWidget obj = NGUITools.AddChild<UIWidget>(uiTable.gameObject);
			obj.depth = uiBGSprite.depth + 1;
			obj.name = "Unit" + label;
			obj.SetDimensions(num, num2);
			UISprite val = obj.gameObject.AddComponent<UISprite>();
			val.name = "Base";
			val.depth = uiBGSprite.depth + 1;
			val.atlas = uiAtlasCommon;
			val.spriteName = "cm3d2_common_plate_white";
			val.SetDimensions(num, num2);
			val.type = (UIBasicSprite.Type)1;
			UI2DSprite obj2 = NGUITools.AddChild<UI2DSprite>(obj.gameObject);
			obj2.depth = val.depth + 1;
			obj2.SetDimensions(num3, num4);
			obj2.transform.localPosition = new Vector3((float)(-num / 2 + num5 + num3 / 2), 0f, 0f);
			obj2.sprite2D = Sprite.Create(icon, new Rect(0f, 0f, (float)num3, (float)num4), Vector2.zero);
			UILabel obj3 = NGUITools.AddChild<UILabel>(obj.gameObject);
			obj3.SetDimensions(num - num3 - num5 * 3, num2);
			obj3.gameObject.transform.localPosition = new Vector3((float)(num3 + num5 * 2), 0f, 0f);
			obj3.depth = val.depth + 1;
			obj3.trueTypeFont = font;
			obj3.fontSize = 20;
			obj3.color = Color.black;
			obj3.text = label;
			obj3.alignment = (NGUIText.Alignment)1;
			UISprite obj4 = NGUITools.AddChild<UISprite>(obj.gameObject);
			obj4.name = "Frame";
			obj4.SetDimensions(num, num2);
			obj4.depth = val.depth + 1;
			obj4.atlas = uiAtlasCommon;
			obj4.spriteName = "cm3d2_common_lineframe_white";
			obj4.alpha = 0f;
			val.gameObject.AddComponent<BoxCollider>().isTrigger = true;
			UIButton obj5 = obj.gameObject.AddComponent<UIButton>();
			EventDelegate val2 = new EventDelegate(this, methodName);
			val2.parameters[0].value = (object)label;
			obj5.onClick.Add(val2);
			NGUITools.UpdateWidgetCollider(obj.gameObject);
		}

		private void OnDirListItemClick(string path)
		{
			CurrentDirectory = path;
		}

		private void OnDirectoryClick(string name)
		{
			if (m_currentDirectory == null || m_currentDirectory == "")
			{
				CurrentDirectory = name;
			}
			else
			{
				CurrentDirectory = CurrentDirectory + "\\" + name;
			}
		}

		private void OnCancelClick()
		{
			if (OnCancel != null)
			{
				OnCancel();
			}
			else
			{
				Hide();
			}
		}

		private void OnFileClick(string name)
		{
			selectedFile = CurrentDirectory + "\\" + name;
			if (OnFileSelected != null)
			{
				OnFileSelected(selectedFile);
			}
		}

		private void ReadDirectoryContents()
		{
			if (m_currentDirectory == null || m_currentDirectory == "")
			{
				try
				{
					string[] logicalDrives = Directory.GetLogicalDrives();
					m_directories = new string[logicalDrives.Length];
					for (int i = 0; i < logicalDrives.Length; i++)
					{
						m_directories[i] = logicalDrives[i];
					}
					m_files = new string[0];
					m_currentDirectoryParts = new string[1]
					{
						""
					};
					BuildContent();
					m_newDirectory = null;
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
					DriveInfo[] drives = DriveInfo.GetDrives();
					m_directories = new string[drives.Length];
					for (int j = 0; j < drives.Length; j++)
					{
						m_directories[j] = drives[j].Name;
					}
					m_files = new string[0];
					m_currentDirectoryParts = new string[1]
					{
						""
					};
					BuildContent();
					m_newDirectory = null;
				}
			}
			else
			{
				string[] array = (!m_currentDirectory.EndsWith("\\")) ? m_currentDirectory.Split(Path.DirectorySeparatorChar) : m_currentDirectory.Substring(0, m_currentDirectory.Length - 1).Split(Path.DirectorySeparatorChar);
				m_currentDirectoryParts = new string[array.Length + 1];
				m_currentDirectoryParts[0] = "";
				for (int k = 0; k < array.Length; k++)
				{
					m_currentDirectoryParts[k + 1] = array[k];
				}
				if (SelectionPattern != null)
				{
					string directoryName = Path.GetDirectoryName(m_currentDirectory);
					string[] array2 = (directoryName != null) ? Directory.GetDirectories(directoryName, SelectionPattern) : Directory.GetDirectories(m_currentDirectory + "\\");
					m_currentDirectoryMatches = (Array.IndexOf(array2, m_currentDirectory) >= 0);
				}
				else
				{
					m_currentDirectoryMatches = false;
				}
				if (BrowserType == FileBrowserType.File || SelectionPattern == null)
				{
					m_directories = Directory.GetDirectories(m_currentDirectory);
					m_nonMatchingDirectories = new string[0];
				}
				else
				{
					m_directories = Directory.GetDirectories(m_currentDirectory, SelectionPattern);
					List<string> list = new List<string>();
					string[] directories = Directory.GetDirectories(m_currentDirectory);
					foreach (string text in directories)
					{
						if (Array.IndexOf(m_directories, text) < 0)
						{
							list.Add(text);
						}
					}
					m_nonMatchingDirectories = list.ToArray();
					for (int m = 0; m < m_nonMatchingDirectories.Length; m++)
					{
						int num = m_nonMatchingDirectories[m].LastIndexOf(Path.DirectorySeparatorChar);
						m_nonMatchingDirectories[m] = m_nonMatchingDirectories[m].Substring(num + 1);
					}
					Array.Sort(m_nonMatchingDirectories);
				}
				for (int n = 0; n < m_directories.Length; n++)
				{
					m_directories[n] = m_directories[n].Substring(m_directories[n].LastIndexOf(Path.DirectorySeparatorChar) + 1);
				}
				if (BrowserType == FileBrowserType.Directory || SelectionPattern == null)
				{
					m_files = Directory.GetFiles(m_currentDirectory);
					m_nonMatchingFiles = new string[0];
				}
				else
				{
					m_files = Directory.GetFiles(m_currentDirectory, SelectionPattern);
					List<string> list2 = new List<string>();
					string[] directories = Directory.GetFiles(m_currentDirectory);
					foreach (string text2 in directories)
					{
						if (Array.IndexOf(m_files, text2) < 0)
						{
							list2.Add(text2);
						}
					}
					m_nonMatchingFiles = list2.ToArray();
					for (int num2 = 0; num2 < m_nonMatchingFiles.Length; num2++)
					{
						m_nonMatchingFiles[num2] = Path.GetFileName(m_nonMatchingFiles[num2]);
					}
					Array.Sort(m_nonMatchingFiles);
				}
				for (int num3 = 0; num3 < m_files.Length; num3++)
				{
					m_files[num3] = Path.GetFileName(m_files[num3]);
				}
				Array.Sort(m_files);
				BuildContent();
				m_newDirectory = null;
			}
		}

		protected void SetNewDirectory(string directory)
		{
			if (directory != null)
			{
				directory = directory.Replace('/', '\\');
			}
			m_newDirectory = directory;
		}

		protected void SwitchDirectoryNow()
		{
			if (m_newDirectory != null && !(m_currentDirectory == m_newDirectory))
			{
				if (Directory.Exists(m_newDirectory))
				{
					m_newDirectory = Path.GetFullPath(m_newDirectory);
				}
				m_currentDirectory = m_newDirectory;
				uiScrollView.ResetPosition();
				m_selectedDirectory = (m_selectedNonMatchingDirectory = (m_selectedFile = -1));
				ReadDirectoryContents();
			}
		}

		//public NGUIFileSelectionDialog()
		//	: this()
		//{
		//}//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)

	}
}
