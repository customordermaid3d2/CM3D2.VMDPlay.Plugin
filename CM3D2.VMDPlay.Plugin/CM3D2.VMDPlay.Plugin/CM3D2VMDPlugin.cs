using UnityEngine;
using UnityInjector.Attributes;

namespace CM3D2.VMDPlay.Plugin
{
	[PluginFilter("COM3D2x64")]
	[PluginName("COM3D2.VMDPlay.Plugin")]
	[PluginVersion("0.3.11.0")]
	public class CM3D2VMDPlugin : UnityInjector.PluginBase
	{
		public const string NAME = "COM3D2.VMDPlay.Plugin";

		public const string VERSION = "0.3.11.0";

		private void Awake()
		{
		}

		private void Start()
		{
			GameObject val = new GameObject("COM3D2VMDPlayPlugin");
			Object.DontDestroyOnLoad(val);
			VMDAnimationMgr.Install(val);
			DebugHelper.Install(val);
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
