using System;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	public class CameraCtrlOff : UnityEngine.MonoBehaviour
	{
		public delegate void SetCameraDelegate(bool enable);

		private SetCameraDelegate cameraControllerFunc;

		public CM3D2VMDGUI ikInfoGui;

		private bool tempCamCtrlOn;

		private bool _cameraCtrlOff = true;

		public bool cameraCtrlOff
		{
			get
			{
				return _cameraCtrlOff;
			}
			set
			{
				try
				{
					if (cameraControllerFunc != null)
					{
						cameraControllerFunc(!value);
					}
					_cameraCtrlOff = value;
				}
				catch
				{
				}
			}
		}

		private void Start()
		{
		}

		private void OnLevelWasInitialized(int level)
		{
			cameraControllerFunc = null;
		}

		private void Update()
		{
			if (GameMain.Instance.VRMode)
			{
				return;
			}
			if (cameraControllerFunc == null)
			{
				CameraControllerInit();
			}
			if (ikInfoGui != null)
			{
				//GUIUtility.get_hotControl();
				if (ikInfoGui.visibleGUI || tempCamCtrlOn)
				{
					if (Input.GetKey((KeyCode)308) || Input.GetKey((KeyCode)307))
					{
						if (cameraCtrlOff)
						{
							cameraCtrlOff = false;
							tempCamCtrlOn = true;
							ikInfoGui.visibleGUI = false;
						}
					}
					else if (!cameraCtrlOff)
					{
						cameraCtrlOff = true;
						tempCamCtrlOn = false;
						ikInfoGui.visibleGUI = true;
					}
				}
				else if (cameraCtrlOff)
				{
					cameraCtrlOff = false;
					tempCamCtrlOn = false;
				}
			}
		}

		private void OnEnable()
		{
			cameraCtrlOff = true;
		}

		private void OnDisable()
		{
			cameraCtrlOff = false;
		}

		private bool CameraControllerInit()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			bool result = false;
			try
			{
				Console.WriteLine("Install Camera Control");
				UltimateOrbitCamera cameraControl = GameMain.Instance.MainCamera.gameObject.GetComponent<UltimateOrbitCamera>();
				if (!(cameraControl == null))
				{
					cameraControllerFunc = delegate(bool enable)
					{
						try
						{
							cameraControl.enabled = enable;
						}
						catch
						{
						}
					};
					result = true;
					return result;
				}
				Console.WriteLine("camera contoller not found");
				return false;
			}
			catch
			{
				Console.WriteLine("exception : camera contoller setting failed");
				return result;
			}
		}

		/*public CameraCtrlOff()
			: this()
		{
		}*/
	}
}
