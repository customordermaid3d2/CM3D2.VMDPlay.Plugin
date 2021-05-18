//using GearMenu;
using CM3D2.VMDPlay.Plugin.Utill;
using COM3D2API;
using GearMenu;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	public class CM3D2VMDGUI2 : UnityEngine.MonoBehaviour
	{
		//public Maid focusChara;

		public bool pinObject;

		private bool isUIVisible;

		private VMDAnimationController lastController;

		private string lastFilename;

		//protected FileBrowser m_fileBrowser;

		protected Texture2D m_directoryImage;

		protected Texture2D m_fileImage;

		private readonly int PanelWidth = 800;

		private readonly int PanelHeight = 560;

		private UIPanel uiPanel;

		private UITable uiMainTable;

		private UITable uiNoMaidTable;

		private UILabel uiMaidNameLabel;

		private UIButton uiVMDEnabledButton;

		private UILabel uiVMDEnabledLabel;

		private UILabel uiVMDNameLabel;

		private UIInput uiVMDFileNameInput;

		private UIInput uiSoundFileNameInput;

		private string gearMenuButtonName = "COM3D2.VMDPlay.PluginMenu";

		private Dictionary<string, float> sliderMax = new Dictionary<string, float>();

		public NGUIFileSelectionDialog fileSelectionDialog => NGUIFileSelectionDialog.Instance;

		private bool isGear = true;
		private bool isGearCheck = false;

		public CM3D2VMDGUI2(){
			RegisterOrClearGearMenuButton();
		}

		private void Start()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void Update()
		{

			//if (focusChara == null)
			//{
			//	focusChara = FindFirstMaid();
				if (MaidControlleUtill.Maid != null)
			//	{
					UpdateUIStatus();
			//	}
			//}
		}

		private void OnLevelWasLoaded(int level)
		{
			isUIVisible = false;
			if (!isGearCheck)
			{
				isGear = bool.Parse(Settings.Instance.GetStringValue("IsGear", "True", true));
				isGearCheck = true;
			}
			if (isGear)
			{
				//RegisterOrClearGearMenuButton();
			}
		}

		public void Clear()
		{
			//focusChara = null;
			lastController = null;
			lastFilename = null;
		}

		private Maid FindFirstMaid()
		{
			CharacterMgr val = GameMain.Instance.CharacterMgr;
			for (int i = 0; i < val.GetMaidCount(); i++)
			{
				Maid val2 = val.GetMaid(i);
				if (val2 != null && val2.body0.isLoadedBody)
				{
					return val2;
				}
			}
			return null;
		}



		public void SetPrevNextMaid(bool next)
		{
			MaidControlleUtill.PrevNextMaid();
			UpdateUIStatus();
		}

		public void UpdateUIStatus()
		{
			if (!(uiPanel == null))
			{
				if (MaidControlleUtill.Maid == null)
				{
					uiMainTable.gameObject.SetActive(false);
					uiNoMaidTable.gameObject.SetActive(true);
				}
				else
				{
					uiMainTable.gameObject.SetActive(true);
					uiNoMaidTable.gameObject.SetActive(false);
					uiMaidNameLabel.text = MaidControlleUtill.Maid.status.fullNameEnStyle;
					VMDAnimationController vMDAnimationController = MaidControlleUtill.VMDAnimationController;
					if (vMDAnimationController != lastController)
					{
						lastFilename = vMDAnimationController.lastLoadedVMD;
						lastController = vMDAnimationController;
					}
					if (lastFilename == null)
					{
						lastFilename = "";
					}
					if (vMDAnimationController.lastLoadedVMD != null && File.Exists(vMDAnimationController.lastLoadedVMD))
					{
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(vMDAnimationController.lastLoadedVMD);
						uiVMDNameLabel.text = fileNameWithoutExtension;
					}
					else
					{
						uiVMDNameLabel.text = "";
					}
					uiVMDEnabledLabel.text = (vMDAnimationController.VMDAnimEnabled ? "ON" : "OFF");
					UIUtils.SetToggleButtonColor(uiVMDEnabledButton, vMDAnimationController.VMDAnimEnabled);
				}
			}
		}

		public void ToggleVMDEnabled()
		{
			if (!(MaidControlleUtill.VMDAnimationController == null))
			{

				MaidControlleUtill.VMDAnimationController.VMDAnimEnabled = !MaidControlleUtill.VMDAnimationController.VMDAnimEnabled;
				UpdateUIStatus();
				
			}
		}

		public void LoadVMD()
		{
			VMDAnimationController currentController = GetCurrentController();
			if (!(currentController == null))
			{
				string value = uiVMDFileNameInput.value;
				currentController.LoadVMDAnimation(value);
				if (VMDAnimationMgr.Instance.SoundMgr.currentAudioClip != null)
				{
					VMDAnimationMgr.Instance.SoundMgr.StopBgm();
					VMDAnimationMgr.Instance.SoundMgr.PlaySound();
				}
			}
		}

		public void ReloadVMD()
		{
			VMDAnimationController currentController = GetCurrentController();
			if (!(currentController == null))
			{
				currentController.ReloadVMDAnimation();
			}
		}

		public void BrowseVMDFile()
		{
			string value = uiVMDFileNameInput.value;
			string currentDir = Application.dataPath;
			if (!string.IsNullOrEmpty(value))
			{
				if (File.Exists(value))
				{
					currentDir = Path.GetDirectoryName(Path.GetFullPath(value));
				}
				else if (Directory.Exists(value))
				{
					currentDir = Path.GetFullPath(value);
				}
				else
				{
					string stringValue = Settings.Instance.GetStringValue("DefaultDir", "", true);
					if (stringValue != null && stringValue != "")
					{
						currentDir = stringValue;
					}
				}
			}
			else
			{
				string stringValue = Settings.Instance.GetStringValue("DefaultDir", "", true);
				if (stringValue != null && stringValue != "")
				{
					currentDir = stringValue;
				}
			}
			fileSelectionDialog.Show(currentDir, "*.vmd", delegate(string path)
			{
				uiVMDFileNameInput.value = path;
				fileSelectionDialog.Hide();
			}, delegate
			{
				fileSelectionDialog.Hide();
			});
		}

		public void LoadSound()
		{
			CustomSoundMgr soundMgr = VMDAnimationMgr.Instance.SoundMgr;
			string value = uiSoundFileNameInput.value;
			soundMgr.SetSoundClip(value);
		}

		public void ClearSound()
		{
			VMDAnimationMgr.Instance.SoundMgr.ClearClip();
		}

		public void BrowseSoundFile()
		{
			string value = uiSoundFileNameInput.value;
			string currentDir = Application.dataPath;
			if (!string.IsNullOrEmpty(value))
			{
				if (File.Exists(value))
				{
					currentDir = Path.GetDirectoryName(Path.GetFullPath(value));
				}
				else if (Directory.Exists(value))
				{
					currentDir = Path.GetFullPath(value);
				}
				else
				{
					string stringValue = Settings.Instance.GetStringValue("DefaultDir", "", true);
					if (stringValue != null && stringValue != "")
					{
						currentDir = stringValue;
					}
				}
			}
			else
			{
				string stringValue = Settings.Instance.GetStringValue("DefaultDir", "", true);
				if (stringValue != null && stringValue != "")
				{
					currentDir = stringValue;
				}
			}
			fileSelectionDialog.Show(currentDir, "*.wav", delegate(string path)
			{
				uiSoundFileNameInput.value = path;
				fileSelectionDialog.Hide();
			}, delegate
			{
				fileSelectionDialog.Hide();
			});
		}

		private VMDAnimationController GetCurrentController()
		{
			return MaidControlleUtill.VMDAnimationController;
		}

		private void CreateUI()
		{
			if (!(uiPanel != null))
			{
				Font font = UIUtils.GetDefaultFont();
				UIAtlas systemDialogAtlas = UIUtils.GetSystemDialogAtlas();
				UIAtlas val = UIUtils.FindAtlas("AtlasCommon");
				
				GameObject val2 = GameObject.Find("UI Root");

				Vector3 localPosition = default(Vector3);
				localPosition = new Vector3(960f - (float)PanelWidth / 2f - 50f, 100f, 0f);
				uiPanel = NGUITools.AddChild<UIPanel>(val2);
				uiPanel.name = "VMD Play UI";
				uiPanel.transform.localPosition = localPosition;

				GameObject val3 = uiPanel.gameObject;
				UISprite uiBGSprite = NGUITools.AddChild<UISprite>(val3);
				uiBGSprite.name = "BG";
				uiBGSprite.atlas = systemDialogAtlas;
				uiBGSprite.spriteName = "cm3d2_dialog_frame";
				uiBGSprite.type = (UIBasicSprite.Type)1;
				uiBGSprite.SetDimensions(PanelWidth, PanelHeight);


				UISprite val4 = NGUITools.AddChild<UISprite>(val3);
				val4.name = "TitleTab";
				val4.depth = uiBGSprite.depth - 1;
				val4.atlas = systemDialogAtlas;
				val4.spriteName = "cm3d2_dialog_frame";
				val4.type = (UIBasicSprite.Type)1;
				val4.SetDimensions(300, 80);
				val4.autoResizeBoxCollider = true;
				val4.gameObject.AddComponent<UIDragObject>().target = val3.transform;
				val4.gameObject.AddComponent<BoxCollider>().isTrigger = true;


				NGUITools.UpdateWidgetCollider(val4.gameObject);
				val4.transform.localPosition = new Vector3((float)uiBGSprite.width / 2f + 4f, (float)(uiBGSprite.height - val4.width) / 2f, 0f);
				Transform transform = val4.transform;
				transform.localRotation = transform.localRotation * Quaternion.Euler(0f, 0f, -90f);
				UILabel obj = val4.gameObject.AddComponent<UILabel>();
				obj.depth = val4.depth + 1;
				obj.width = val4.width;
				obj.color = Color.white;
				obj.trueTypeFont = font;
				obj.fontSize = 18;
				obj.text = "COM3D2.VMDPlay.Plugin 0.3.11.0";


				float num2 = (float)uiBGSprite.height / 2f;
				UITable uiTable = NGUITools.AddChild<UITable>(val3);
				uiTable.pivot = (UIWidget.Pivot)1;
				uiTable.columns = 1;
				uiTable.padding = new Vector2(10f, 5f);
				uiTable.hideInactive = true;
				uiTable.keepWithinPanel = true;
				uiTable.transform.localPosition = new Vector3(0f, (float)PanelHeight / 2f - 40f, 0f);
				uiMainTable = uiTable;
				Func<string, GameObject, int, int, UILabel> func = delegate(string text, GameObject parent, int width, int height)
				{
					UILabel obj11 = NGUITools.AddChild<UILabel>(parent);
					obj11.depth = uiBGSprite.depth + 1;
					obj11.width = width;
					obj11.height = height;
					obj11.alignment = (NGUIText.Alignment)1;
					obj11.trueTypeFont = font;
					obj11.fontSize = 18;
					obj11.spacingX = 0;
					obj11.supportEncoding = true;
					obj11.text = text;
					obj11.color = Color.white;
					return obj11;
				};
				UITable val5 = NGUITools.AddChild<UITable>(NGUITools.AddChild(uiTable.gameObject));
				val5.pivot = 0;
				val5.columns = 5;
				val5.padding = new Vector2(2f, 0f);

				// error
				GameObject obj2 = UIUtils.SetCloneChild(val5.gameObject, UIUtils.GetButtonTemplateGo(), "PrevBtn");
				obj2.SetActive(true);
				UIButton component = obj2.GetComponent<UIButton>();
				component.GetComponent<UISprite>().SetDimensions(30, 30);
				UILabel componentInChildren = obj2.GetComponentInChildren<UILabel>();
				componentInChildren.width = 30;
				componentInChildren.fontSize = 18;
				componentInChildren.spacingX = 0;
				componentInChildren.supportEncoding = true;
				componentInChildren.color = Color.white;
				componentInChildren.text = "<";
				EventDelegate val6 = new EventDelegate(this, "SetPrevNextMaid");
				val6.parameters[0].value = ((object)false);
				component.onClick.Add(val6);
				GameObject obj3 = UIUtils.SetCloneChild(val5.gameObject, UIUtils.GetButtonTemplateGo(), "NextBtn");
				obj3.SetActive(true);
				UIButton component2 = obj3.GetComponent<UIButton>();
				component2.GetComponent<UISprite>().SetDimensions(30, 30);
				UILabel componentInChildren2 = obj3.GetComponentInChildren<UILabel>();
				componentInChildren2.width = 30;
				componentInChildren2.fontSize = 18;
				componentInChildren2.spacingX = 0;
				componentInChildren2.supportEncoding = true;
				componentInChildren2.color = Color.white;
				componentInChildren2.text = ">";
				EventDelegate val7 = new EventDelegate(this, "SetPrevNextMaid");
				val7.parameters[0].value = ((object)true);
				component2.onClick.Add(val7);
				uiMaidNameLabel = func("<maid>", val5.gameObject, 200, 30);
				GameObject obj4 = UIUtils.SetCloneChild(val5.gameObject, UIUtils.GetButtonTemplateGo(), "OnOffBtn");
				obj4.SetActive(true);
				UIButton component3 = obj4.GetComponent<UIButton>();
				component3.GetComponent<UISprite>().SetDimensions(60, 30);
				UILabel componentInChildren3 = obj4.GetComponentInChildren<UILabel>();
				componentInChildren3.width = 60;
				componentInChildren3.fontSize = 18;
				componentInChildren3.spacingX = 0;
				componentInChildren3.supportEncoding = true;
				componentInChildren3.text = "ON";
				uiVMDEnabledLabel = componentInChildren3;
				EventDelegate item = new EventDelegate(this, "ToggleVMDEnabled");
				component3.onClick.Add(item);
				uiVMDEnabledButton = component3;
				uiVMDNameLabel = func("", val5.gameObject, 300, 30);
				UITable val8 = NGUITools.AddChild<UITable>(NGUITools.AddChild(uiTable.gameObject));
				val8.pivot = 0;
				val8.columns = 5;
				val8.padding = new Vector2(5f, 5f);
				func("VMD", val8.gameObject, 60, 30);
				GameObject obj5 = UIUtils.SetCloneChild(val8.gameObject, UIUtils.GetButtonTemplateGo(), "LoadBtn");
				obj5.SetActive(true);
				UIButton component4 = obj5.GetComponent<UIButton>();
				component4.GetComponent<UISprite>().SetDimensions(60, 30);
				UILabel componentInChildren4 = obj5.GetComponentInChildren<UILabel>();
				componentInChildren4.width = 60;
				componentInChildren4.fontSize = 18;
				componentInChildren4.spacingX = 0;
				componentInChildren4.supportEncoding = true;
				componentInChildren4.text = "Load";
				EventDelegate item2 = new EventDelegate(this, "LoadVMD");
				component4.onClick.Add(item2);
				GameObject obj6 = UIUtils.SetCloneChild(val8.gameObject, UIUtils.GetButtonTemplateGo(), "ReloadBtn");
				obj6.SetActive(true);
				UIButton component5 = obj6.GetComponent<UIButton>();
				component5.GetComponent<UISprite>().SetDimensions(60, 30);
				UILabel componentInChildren5 = obj6.GetComponentInChildren<UILabel>();
				componentInChildren5.width = 60;
				componentInChildren5.fontSize = 18;
				componentInChildren5.spacingX = 0;
				componentInChildren5.supportEncoding = true;
				componentInChildren5.text = "Reload";
				EventDelegate item3 = new EventDelegate(this, "ReloadVMD");
				component5.onClick.Add(item3);
				UISprite val9 = NGUITools.AddChild<UISprite>(val8.gameObject);
				val9.name = "Title";
				val9.depth = uiPanel.depth + 1;
				val9.atlas = val;
				val9.spriteName = "cm3d2_common_plate_white";
				val9.SetDimensions(400, 30);
				val9.type = (UIBasicSprite.Type)1;
				UISprite val10 = NGUITools.AddChild<UISprite>(val9.gameObject);
				val10.name = "VMDFilePathInput";
				val10.atlas = val;
				val10.depth = val9.depth + 1;
				val10.spriteName = "cm3d2_common_lineframe_gray";
				val10.type = (UIBasicSprite.Type)1;
				val10.SetDimensions(400, 30);
				val10.gameObject.AddComponent<BoxCollider>().isTrigger = true;
				UILabel val11 = NGUITools.AddChild<UILabel>(val10.gameObject);
				val11.name = "Label";
				val11.depth = val10.depth + 1;
				val11.SetDimensions(val9.width - 10, 30);
				val11.trueTypeFont = font;
				val11.fontSize = 18;
				val11.text = "";
				val11.color = Color.black;
				val11.overflowMethod = (UILabel.Overflow)1;
				val11.alignment = (NGUIText.Alignment)1;
				UIInput val12 = val10.gameObject.AddComponent<UIInput>();
				val12.label = val11;
				val12.activeTextColor = Color.black;
				val12.inputType = 0;
				val12.caretColor = new Color(0.1f, 0.1f, 0.3f, 1f);
				val12.selectionColor = new Color(0.3f, 0.3f, 0.6f, 0.8f);
				val12.defaultText = "";
				val12.value = "";
				NGUITools.UpdateWidgetCollider(val10.gameObject);
				uiVMDFileNameInput = val12;
				GameObject obj7 = UIUtils.SetCloneChild(val8.gameObject, UIUtils.GetButtonTemplateGo(), "BrowseBtn");
				obj7.SetActive(true);
				UIButton component6 = obj7.GetComponent<UIButton>();
				component6.GetComponent<UISprite>().SetDimensions(60, 30);
				UILabel componentInChildren6 = obj7.GetComponentInChildren<UILabel>();
				componentInChildren6.width = 60;
				componentInChildren6.fontSize = 18;
				componentInChildren6.spacingX = 0;
				componentInChildren6.supportEncoding = true;
				componentInChildren6.text = "...";
				EventDelegate item4 = new EventDelegate(this, "BrowseVMDFile");
				component6.onClick.Add(item4);
				UITable val13 = NGUITools.AddChild<UITable>(NGUITools.AddChild(uiTable.gameObject));
				val13.pivot = 0;
				val13.columns = 5;
				val13.padding = new Vector2(5f, 5f);
				func("Sound", val13.gameObject, 60, 30);
				GameObject obj8 = UIUtils.SetCloneChild(val13.gameObject, UIUtils.GetButtonTemplateGo(), "LoadBtn");
				obj8.SetActive(true);
				UIButton component7 = obj8.GetComponent<UIButton>();
				component7.GetComponent<UISprite>().SetDimensions(60, 30);
				UILabel componentInChildren7 = obj8.GetComponentInChildren<UILabel>();
				componentInChildren7.width = 60;
				componentInChildren7.fontSize = 18;
				componentInChildren7.spacingX = 0;
				componentInChildren7.supportEncoding = true;
				componentInChildren7.text = "Load";
				EventDelegate item5 = new EventDelegate(this, "LoadSound");
				component7.onClick.Add(item5);
				GameObject obj9 = UIUtils.SetCloneChild(val13.gameObject, UIUtils.GetButtonTemplateGo(), "ClearBtn");
				obj9.SetActive(true);
				UIButton component8 = obj9.GetComponent<UIButton>();
				component8.GetComponent<UISprite>().SetDimensions(60, 30);
				UILabel componentInChildren8 = obj9.GetComponentInChildren<UILabel>();
				componentInChildren8.width = 60;
				componentInChildren8.fontSize = 18;
				componentInChildren8.spacingX = 0;
				componentInChildren8.supportEncoding = true;
				componentInChildren8.text = "Clear";
				EventDelegate item6 = new EventDelegate(this, "ClearSound");
				component8.onClick.Add(item6);
				UISprite val14 = NGUITools.AddChild<UISprite>(val13.gameObject);
				val14.name = "Title";
				val14.depth = uiPanel.depth + 1;
				val14.atlas = val;
				val14.spriteName = "cm3d2_common_plate_white";
				val14.SetDimensions(400, 30);
				val14.type = (UIBasicSprite.Type)1;
				UISprite val15 = NGUITools.AddChild<UISprite>(val14.gameObject);
				val15.name = "SoundFilePathInput";
				val15.atlas = val;
				val15.depth = val14.depth + 1;
				val15.spriteName = "cm3d2_common_lineframe_gray";
				val15.type = (UIBasicSprite.Type)1;
				val15.SetDimensions(400, 30);
				val15.gameObject.AddComponent<BoxCollider>().isTrigger = true;
				UILabel val16 = NGUITools.AddChild<UILabel>(val15.gameObject);
				val16.name = "Label";
				val16.depth = val15.depth + 1;
				val16.SetDimensions(val14.width - 10, 30);
				val16.trueTypeFont = font;
				val16.fontSize = 18;
				val16.text = "";
				val16.color = Color.black;
				val16.overflowMethod = (UILabel.Overflow)1;
				val16.alignment = (NGUIText.Alignment)1;
				UIInput val17 = val15.gameObject.AddComponent<UIInput>();
				val17.label = val16;
				val17.activeTextColor = Color.black;
				val17.inputType = 0;
				val17.caretColor = new Color(0.1f, 0.1f, 0.3f, 1f);
				val17.selectionColor = new Color(0.3f, 0.3f, 0.6f, 0.8f);
				val17.defaultText = "";
				val17.value = "";
				NGUITools.UpdateWidgetCollider(val15.gameObject);
				uiSoundFileNameInput = val17;
				GameObject obj10 = UIUtils.SetCloneChild(val13.gameObject, UIUtils.GetButtonTemplateGo(), "BrowseBtn");
				obj10.SetActive(true);
				UIButton component9 = obj10.GetComponent<UIButton>();
				component9.GetComponent<UISprite>().SetDimensions(60, 30);
				UILabel componentInChildren9 = obj10.GetComponentInChildren<UILabel>();
				componentInChildren9.width = 60;
				componentInChildren9.fontSize = 18;
				componentInChildren9.spacingX = 0;
				componentInChildren9.supportEncoding = true;
				componentInChildren9.text = "...";
				EventDelegate item7 = new EventDelegate(this, "BrowseSoundFile");
				component9.onClick.Add(item7);

				val5.Reposition();
				val8.Reposition();
				val13.Reposition();
				uiTable.Reposition();

				uiNoMaidTable = NGUITools.AddChild<UITable>(val3);
				uiNoMaidTable.pivot = (UIWidget.Pivot)1;
				uiNoMaidTable.columns = 1;
				uiNoMaidTable.padding = new Vector2(10f, 5f);
				uiNoMaidTable.hideInactive = true;
				uiNoMaidTable.keepWithinPanel = true;
				uiNoMaidTable.transform.localPosition = new Vector3(0f, (float)PanelHeight / 2f - 50f, 0f);
				func("No Maid Selected.", uiNoMaidTable.gameObject, 400, 40);
				uiNoMaidTable.Reposition();
				uiNoMaidTable.gameObject.SetActive(false);

				UpdateUIStatus();

				val3.SetActive(false);


			}
		}

		private void ShowUI(bool visible)
		{
			Debug.Log("ShowUI" + uiPanel == null);
			if (uiPanel == null)
			{
                try
                {
                    CreateUI();
                }
                catch (Exception e)
                {
					Debug.LogWarning(
						"CreateUI"
						);
					Debug.LogWarning(
						e.ToString()
						);
                }
			}
			Debug.Log("ShowUI" + uiPanel == null);
			if (uiPanel != null)
			{
				isUIVisible = !isUIVisible;
				uiPanel.gameObject.SetActive(visible);
			}
		}

		private void RegisterOrClearGearMenuButton()
		{
			//if (!Buttons.Contains(gearMenuButtonName))
			{
				//GameObject gameObject = Buttons.Add(gearMenuButtonName, "", GetGearMenuIconPng(!isUIVisible), OnGearMenuClick);
				SystemShortcutAPI.AddButton(gearMenuButtonName, OnGearMenuClick, gearMenuButtonName, GetGearMenuIconPng(!isUIVisible));
				//UpdateGearMenuIcon(gameObject);
			}
		}

		//private void OnGearMenuClick(GameObject gameObject)
		private void OnGearMenuClick()
		{
			ShowUI(!isUIVisible);
			//UpdateGearMenuIcon(gameObject);
		}
		private byte[] GetGearMenuIconPng(bool forUIVisible)
		{
			return DefaultIcon.Png;
		}

		/*
		private void UpdateGearMenuIcon(GameObject gameObject)
		{
			if (isUIVisible)
			{
				Buttons.SetFrameColor(gameObject, Color.red);
				Buttons.SetText(gameObject, "VMDPlay UIを隠す");
			}
			else
			{
				Buttons.SetFrameColor(gameObject, Color.gray);
				Buttons.SetText(gameObject, "VMDPlay UIを開く");
			}
		}
		*/
	}
}
