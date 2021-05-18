using CM3D2.VMDPlay.Plugin.Utill;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	public class VMDAnimationMgr : UnityEngine.MonoBehaviour
	{
		private static VMDAnimationMgr _instance;

		public CM3D2VMDGUI gui;

		public CM3D2VMDGUI2 gui2;

		private KeyUtil UIKey;

		public CustomSoundMgr SoundMgr = new CustomSoundMgr();

		//todo : 이거 막지 말고 딕셔녀리 추가하자
		//public List<VMDAnimationController> controllers = new List<VMDAnimationController>();
		//public Dictionary<Maid, VMDAnimationController> maidControllers = new Dictionary<Maid, VMDAnimationController>();		

		public static VMDAnimationMgr Instance => _instance;

		private void Start()
		{
			string stringValue = Settings.Instance.GetStringValue("UIKey", "Ctrl+I", true);
			UIKey = KeyUtil.Parse(stringValue);
		}

		public static VMDAnimationMgr Install(GameObject container)
		{
			if (_instance == null)
			{
				_instance = container.AddComponent<VMDAnimationMgr>();
			}
			return _instance;
		}

		public void PlayAll()
		{
			foreach (var controller in MaidControlleUtill.Controllers)
			{
				controller.Play();
			}
		}
		
		public void ClearAll()
		{
			foreach (var controller in MaidControlleUtill.Controllers)
			{
				controller.Stop();
				controller.lastLoadedVMD=string.Empty;
			}
		}

		public void StopAll()
		{
			foreach (var controller in MaidControlleUtill.Controllers)
			{
				controller.Stop();
			}
		}

		public void PauseAll()
		{
			foreach (var controller in MaidControlleUtill.Controllers)
			{
				controller.Pause();
			}
		}

		private void OnLevelWasLoaded(int level)
		{
			if (gui == null)
			{
				gui = new GameObject("GUI").AddComponent<CM3D2VMDGUI>();
				gui.transform.parent = this.transform;
				gui.gameObject.SetActive(false);
			}
			else
			{
				gui.gameObject.SetActive(false);
				//CM3D2VMDGUI.focusChara = null;
			}
			if (gui2 == null)
			{
				gui2 = new GameObject("GUI2").AddComponent<CM3D2VMDGUI2>();
				gui2.transform.parent = this.transform;
			}
		}

		private void Update()
		{
			try
			{
				if (UIKey.TestKeyDown())
				{
					ToggleGUI();
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}

		public void ToggleGUI()
		{
			gui.gameObject.SetActive(!gui.gameObject.activeSelf);
		}

		public void ShowGui(bool show)
		{
			gui.gameObject.SetActive(show);
		}
		
	}
}
