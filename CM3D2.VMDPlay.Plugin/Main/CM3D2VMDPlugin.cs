using BepInEx;
using CM3D2.VMDPlay.Plugin.Utill;
using COM3D2.Lilly.Plugin;
using COM3D2.VMDPlay.Plugin;
using COM3D2API;
using GearMenu;
using HarmonyLib;
using System;
using System.IO;
using UnityEngine;
//using UnityInjector.Attributes;

namespace CM3D2.VMDPlay.Plugin
{
	//[PluginFilter("COM3D2x64")]
	//[PluginName("COM3D2.VMDPlay.Plugin")]
	//[PluginVersion("0.3.11.0")]

	[BepInPlugin("COM3D2.VMDPlay.Plugin", "COM3D2.VMDPlay.Plugin", "0.3.11.2")]// ���� ��Ģ ����. �ݵ�� 2~4���� ���ڱ������� �ؾ���. ���ؼ��� ���о����
	[BepInProcess("COM3D2x64.exe")]
	public class CM3D2VMDPlugin : BaseUnityPlugin//UnityInjector.PluginBase
	{
		public static CM3D2VMDPlugin Instance;

		public const string NAME = "COM3D2.VMDPlay.Plugin";

		public const string VERSION = "0.3.11.2";

		public CM3D2VMDPlugin()
        {
			Instance = this;
			SongMotionUtill.path= Path.GetDirectoryName( this.Config.ConfigFilePath ) + @"\COM3D2.VMDPlay.Plugin.json";
			Settings.config = this.Config;
			Debug.Log("https://github.com/customordermaid3d2/CM3D2.VMDPlay.Plugin");
			//new SongMotionDic();
		}

		private void Awake()
		{
			MyLog.LogMessage("Awake");
			SongMotionUtill.Deserialize();
			Harmony.CreateAndPatchAll(typeof(CharacterMgrPatch));
		}

		private void Start()
		{
			GameObject val = new GameObject("COM3D2VMDPlayPlugin");
			UnityEngine.Object.DontDestroyOnLoad(val);
			VMDAnimationMgr.Install(val);
			DebugHelper.Install(val);
			SystemShortcutAPI.AddButton("VMDPlayPlugin", new Action(delegate () { VMDAnimationMgr.Instance.ToggleGUI(); }), "VMDPlayPlugin", MyUtill.ExtractResource(Properties.Resources.icon));
		}

		public void togGUI()
		{
			VMDAnimationMgr.Instance.ToggleGUI();
		}

		

		private void OnLevelWasLoaded(int level)
		{
		}
		
	}
}
