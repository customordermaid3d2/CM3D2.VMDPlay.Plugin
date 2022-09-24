using CM3D2.VMDPlay.Plugin.Utill;
using COM3D2.Lilly.Plugin;
using COM3D2.Lilly.Plugin.Utill;
using MMD.VMD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	public class VMDAnimationController : UnityEngine.MonoBehaviour
	{
		public class IKWeightData
		{
			public bool disableToeIK;

			public float footIKPosWeight = 1f;

			public float footIKRotWeight;

			public int footIteration = 40;

			public float footControlWeight = 0.04f;

			public int toeIteration = 3;

			public float toeControlWeight = 0.4f;

			public float minDelta;
		}

		public class QuickAdjust
		{
			private VMDAnimationController mgr;

			private float[] _shoulder = {0,0,0}; //{-15f, -20f, 0f};

			private float[] _armUp = {0,0,40}; //40f;

			private float[] _armLow = {0,0,0}; //10f;

			private float _scaleModel = 1f;

			public int AdjustCount()
			{
				const int count = 10;
				return count;
			}
			public float[] ConvertToArray()
			{
				var adjustParams = new float[AdjustCount()];
				adjustParams[0] = _shoulder[0];
				adjustParams[1] = _shoulder[1];
				adjustParams[2] = _shoulder[2];
				adjustParams[3] = _armUp[0];
				adjustParams[4] = _armUp[1];
				adjustParams[5] = _armUp[2];
				adjustParams[6] = _armLow[0];
				adjustParams[7] = _armLow[1];
				adjustParams[8] = _armLow[2];
				adjustParams[9] = _scaleModel;
				return adjustParams;
			}

			internal void ParseAdjust(SongMotionUtill.motionAndTime mat)
            {
				if(mat.adjustParams == null) return;
				
                int dstCount = mat.adjustParams.Count();
				int srcCount = AdjustCount();

				if(srcCount > dstCount)	return;
				_shoulder[0] = mat.adjustParams[0];
				_shoulder[1] = mat.adjustParams[1];
				_shoulder[2] = mat.adjustParams[2];
				_armUp[0] = mat.adjustParams[3];
				_armUp[1] = mat.adjustParams[4];
				_armUp[2] = mat.adjustParams[5];
				_armLow[0] = mat.adjustParams[6];
				_armLow[1] = mat.adjustParams[7];
				_armLow[2] = mat.adjustParams[8];
				_scaleModel = mat.adjustParams[9];

				Set();
            }

			public float ShoulderX
			{
				get
				{
					return _shoulder[0];
				}
				set
				{
					if (_shoulder[0] != value)
					{
						_shoulder[0] = value;
						Set();
					}
				}
			}
			public float ShoulderY
			{
				get
				{
					return _shoulder[1];
				}
				set
				{
					if (_shoulder[1] != value)
					{
						_shoulder[1] = value;
						Set();
					}
				}
			}
			public float ShoulderZ
			{
				get
				{
					return _shoulder[2];
				}
				set
				{
					if (_shoulder[2] != value)
					{
						_shoulder[2] = value;
						Set();
					}
				}
			}

			public float ArmUpX
			{
				get
				{
					return _armUp[0];
				}
				set
				{
					if (_armUp[0] != value)
					{
						_armUp[0] = value;
						Set();
					}
				}
			}

			public float ArmUpY
			{
				get
				{
					return _armUp[1];
				}
				set
				{
					if (_armUp[1] != value)
					{
						_armUp[1] = value;
						Set();
					}
				}
			}

			public float ArmUpZ
			{
				get
				{
					return _armUp[2];
				}
				set
				{
					if (_armUp[2] != value)
					{
						_armUp[2] = value;
						Set();
					}
				}
			}

			public float ArmLowX
			{
				get
				{
					return _armLow[0];
				}
				set
				{
					if (_armLow[0] != value)
					{
						_armLow[0] = value;
						Set();
					}
				}
			}

			public float ArmLowY
			{
				get
				{
					return _armLow[1];
				}
				set
				{
					if (_armLow[1] != value)
					{
						_armLow[1] = value;
						Set();
					}
				}
			}

			public float ArmLowZ
			{
				get
				{
					return _armLow[2];
				}
				set
				{
					if (_armLow[2] != value)
					{
						_armLow[2] = value;
						Set();
					}
				}
			}

			public float ScaleModel
			{
				get
				{
					return _scaleModel;
				}
				set
				{
					if (_scaleModel != value)
					{
						_scaleModel = value;
						Set();
					}
				}
			}

			public QuickAdjust(VMDAnimationController mgr)
			{
				this.mgr = mgr;
			}

			public void Set()
			{
				try
				{
					mgr.boneAdjust[mgr._shoulderKey[0]].SetRotAdjustment(new Vector3(_shoulder[0], _shoulder[1], _shoulder[2]));
					mgr.boneAdjust[mgr._shoulderKey[1]].SetRotAdjustment(new Vector3(_shoulder[0], -1f * _shoulder[1], -1f * _shoulder[2]));
					mgr.boneAdjust[mgr._armUpKey[0]].SetRotAdjustment(new Vector3(_armUp[0], _armUp[1], _armUp[2]));
					mgr.boneAdjust[mgr._armUpKey[1]].SetRotAdjustment(new Vector3(_armUp[0], -1 * _armUp[1], -1f * _armUp[2]));
					mgr.boneAdjust[mgr._armLowKey[0]].SetRotAdjustment(new Vector3(_armLow[0], -1 * _armLow[1], _armLow[2]));
					mgr.boneAdjust[mgr._armLowKey[1]].SetRotAdjustment(new Vector3(_armLow[0], -1 * _armLow[1], -1f * _armLow[2]));
					mgr.boneAdjust[mgr._armUpKey[0]].SetAxisAdjustment(new Vector3(_shoulder[0], -1f * _shoulder[1], -1f * _shoulder[2]));
					mgr.boneAdjust[mgr._armUpKey[1]].SetAxisAdjustment(new Vector3(_shoulder[0], _shoulder[1], _shoulder[2]));
					mgr.boneAdjust[mgr._armLowKey[0]].SetAxisAdjustment(new Vector3(_armUp[0], -1 * _armUp[1], -1f * _armUp[2]));
					mgr.boneAdjust[mgr._armLowKey[1]].SetAxisAdjustment(new Vector3(_armUp[0], _armUp[1], _armUp[2]));
					mgr.scale = mgr.scaleBase * _scaleModel;
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
				}
			}
        }

		public class AutoTrack : UnityEngine.MonoBehaviour
		{
			public Dictionary<GameObject, GameObject> map;

			public Dictionary<GameObject, GameObject> map2;

			internal VMDAnimationController controller;

			private string[] posPart = { "Bip01" };
			
			private Dictionary<string, float> prevFootValZ = new Dictionary<string, float>();

			private void FixedUpdate()
			{
				try
				{
					if (controller.VMDAnimEnabled && map != null)
					{
						controller.ProcessFootIK();
						foreach (GameObject key in map.Keys)
						{
							GameObject obj = map[key];
							if (obj == null|| obj?.name == null)
							{
								MyLog.LogWarning("FixedUpdate null");
								controller.VMDAnimEnabled = false;
								controller.Init();
								return;
							}
							if (obj.name.Contains("Foot"))
							{
								obj.transform.rotation = key.transform.rotation;
								Vector3 v = obj.transform.localEulerAngles;
								if (!prevFootValZ.ContainsKey(obj.name))
								{
									prevFootValZ.Add(obj.name, v.z);
								}
								/*if (v.x > 30f && v.x < 180f)
								{
									v.x = 30f;
								}
								else if (v.x < 330f && v.x > 180f)
								{
									v.x = 330f;
								}
								if (v.y > 30f && v.y < 180f)
								{
									v.y = 30f;
								}
								else if (v.y < 330f && v.y > 180f)
								{
									v.y = 330f;
								}*/
								v.x = 0;
								v.y = 0;
								if (v.z > 60f && v.z < 180f)
								{
									v.z = 60f;
								}
								else if (v.z < 300f && v.z > 180f)
								{
									v.z = 300f;
								}
								float f = prevFootValZ[obj.name];
								if (f > 180f)
								{
									f -= 360f;
								}
								float f2 = v.z;
								if (f2 > 180f)
								{
									f2 -= 360f;
								}
								if (f2 - f > 10f)
								{
									v.z = f + 10f;
								}
								else if (f2 - f < -10f)
								{
									v.z = f - 10f;
								}
								prevFootValZ[obj.name] = v.z;
								obj.transform.localEulerAngles = v;
							}
							else if (controller.enableHeadRotate || !obj.name.Contains("Head"))
							{
								obj.transform.rotation = key.transform.rotation;
							}
							if (posPart.Contains(obj.name))
							{
								obj.transform.position = key.transform.position;
							}
						}
					}
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
				}
			}

		}

		public class DestroyListener : UnityEngine.MonoBehaviour
		{
			public VMDAnimationController controller;

			private void OnDestroy()
			{
				VMDAnimationMgr.Instance.gui.Clear();
				Console.WriteLine("Destroy VMD Controller. {0}", controller);
				UnityEngine.Object.Destroy(controller);
			}

		}

		public Maid maid;

		private string[] _shoulderKey;

		private string[] _armUpKey;

		private string[] _armLowKey;

		private string clipname;

		private float _speed = 1f;

		public float scale = 0.085f;

		public Vector3 centerBasePos = new Vector3(0f, 6.5f, 0f);

		private float scaleBase = 0.1f;

		private Vector3 centerBasePosBase = new Vector3(0f, 6f, 0f);

		public Vector3 hipPositionAdjust = new Vector3(0f, 2f, 0f);

		public bool enableIK = true;

		public Dictionary<string, string> boneNameMap;

		private Transform t_hips;

		private Transform t_leftHeel;

		private Transform t_rightHeel;

		private Transform t_leftFoot;

		private Transform t_rightFoot;

		private Transform t_leftLowerLeg;

		private Transform t_rightLowerLeg;

		private Transform t_leftUpperLeg;

		private Transform t_rightUpperLeg;

		private Transform t_leftFootIK;

		private Transform t_rightFootIK;

		private Transform t_leftToe;

		private Transform t_rightToe;

		private Transform t_leftToeIK;

		private Transform t_rightToeIK;

		private const string FOOT_IK_NAME_BASE = "_FOOT_IK_";

		private const string TOE_IK_NAME_BASE = "_TOE_IK_";

		private CCDIKSolver leftLegIKSolver;

		private CCDIKSolver rightLegIKSolver;

		public ModelBaselineData ModelBaseline = new ModelBaselineData();

		private Dictionary<string, Quaternion> initialRotations = new Dictionary<string, Quaternion>();

		private DefaultCharaAnimOverride animeOverride;

		public AutoTrack autoTrack;

		public GameObject vmdAnimationGo;

		private const string FACE_GO_NAME = "_face";

		public CM3D2FaceAnimeController faceController;

		public IKWeightData IKWeight = new IKWeightData();

		public QuickAdjust quickAdjust;

		public string lastAdjustedBone;

		public BoneAdjustment lastModifiedAdjustment;

		public Dictionary<string, BoneAdjustment> boneAdjust = new Dictionary<string, BoneAdjustment>();

		public string lastLoadedVMD;

		public Animation animationForVMD;

		private bool vmdAnimEnabled;

		private bool loop;

		public bool enableHeadRotate = true;

		public float timeShiftNow = 0;

		//private static float bgmvolume = 1f;
		//public float BgmVolume
		//{
		//	get
		//	{
		//		return bgmvolume;
		//	}
		//	set
		//	{
		//		bgmvolume = value;
		//		if (pAS != null && pAS.volume != value)
		//		{
		//			pAS.volume = value;
		//		}
		//	}
		//}
		private static bool syncToBGM = true;
		public bool SyncToBGM
		{
			get
			{
				return syncToBGM;
			}
			set
			{
				syncToBGM = value;
				if (value)
				{
					syncToAnim = false;
				}
			}
		}
		private static bool syncToAnim = false;
		public bool SyncToAnim
		{
			get
			{
				return syncToAnim;
			}
			set
			{
				syncToAnim = value;
				if (value)
				{
					SyncToBGM = false;
				}
			}
		}

		private List<string> boneNames = new List<string>
		{
			"Bip01",
			"Bip01/Bip01 Spine",
			"Bip01/Bip01 Spine/Bip01 Spine0a",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 Neck",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 Neck/Bip01 Head",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger0",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger01",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger02",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger1",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger11",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger12",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger2",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger21",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger22",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger3",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger31",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger32",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger4",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger41",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 L Clavicle/Bip01 L UpperArm/Bip01 L Forearm/Bip01 L Hand/Bip01 L Finger42",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger0",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger01",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger02",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger1",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger11",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger12",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger2",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger21",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger22",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger3",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger31",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger32",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger4",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger41",
			"Bip01/Bip01 Spine/Bip01 Spine0a/Bip01 Spine1/Bip01 Spine1a/Bip01 R Clavicle/Bip01 R UpperArm/Bip01 R Forearm/Bip01 R Hand/Bip01 R Finger42",
			"Bip01/Bip01 Pelvis",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe0",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe0/Bip01 L Toe01",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe0/Bip01 L Toe01/Bip01 L Toe0Nub",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe1",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe1/Bip01 L Toe11",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe1/Bip01 L Toe11/Bip01 L Toe1Nub",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe2",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe2/Bip01 L Toe21",
			"Bip01/Bip01 Pelvis/Bip01 L Thigh/Bip01 L Calf/Bip01 L Foot/Bip01 L Toe2/Bip01 L Toe21/Bip01 L Toe2Nub",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe0",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe0/Bip01 R Toe01",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe0/Bip01 R Toe01/Bip01 R Toe0Nub",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe1",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe1/Bip01 R Toe11",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe1/Bip01 R Toe11/Bip01 R Toe1Nub",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe2",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe2/Bip01 R Toe21",
			"Bip01/Bip01 Pelvis/Bip01 R Thigh/Bip01 R Calf/Bip01 R Foot/Bip01 R Toe2/Bip01 R Toe21/Bip01 R Toe2Nub",
			"_FOOT_IK_L",
			"_FOOT_IK_R",
			"_FOOT_IK_L/_TOE_IK_L",
			"_FOOT_IK_R/_TOE_IK_R"
		};

		private static FieldInfo f_m_nSubMeshCount = typeof(TMorph).GetField("m_nSubMeshCount", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

		private static FieldInfo f_m_bws = typeof(TMorph).GetField("m_bws", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

		private static FieldInfo f_m_bDut = typeof(TMorph).GetField("m_bDut", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField);

		private static FieldInfo f_m_mesh = typeof(TMorph).GetField("m_mesh", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField);

		public float headRemoveZ = 1.5f;

		public float headRemoveZ2 = 1.48f;

		public float headRemoveY2 = 0.02f;

		public bool headMeshShrinked;

		public int[][] headTriBackUp;

		//private IKEffectedBoneAdjustment[] ikBoneAdjustment;

		public bool faceAnimeEnabled
		{
			get
			{
				return faceController.enabled;
			}
			set
			{
				faceController.enabled = value;
			}
		}

		public bool VMDAnimEnabled
		{
			get
			{
				return vmdAnimEnabled;
			}
			set
			{
				if (vmdAnimEnabled != value)
				{
					if (value)
					{
						if (animeOverride.DefaultAnimeEnabled)
						{
							animeOverride.Aquire(this);
							vmdAnimEnabled = true;
							OnVMDAnimEnabledChanged();
						}
					}
					else
					{
						animeOverride.Release(this);
						if (animeOverride.DefaultAnimeEnabled)
						{
							vmdAnimEnabled = false;
							OnVMDAnimEnabledChanged();
						}
					}
				}
			}
		}

		public float speed
		{
			get
			{
				return _speed;
			}
			set
			{
				if (_speed != value)
				{
					_speed = value;
					SetSpeed(_speed);
				}
			}
		}

		public bool Loop
		{
			get
			{
				return loop;
			}
			set
			{
				if (loop != value)
				{
					if (clipname != null)
					{
						if (value)
						{
							GetAnimation()[clipname].clip.wrapMode = (WrapMode)2;
						}
						else
						{
							GetAnimation()[clipname].clip.wrapMode = (WrapMode)1;
						}
					}
					loop = value;
				}
			}
		}

		public unsafe static VMDAnimationController Install(Maid maid)
		{
			VMDAnimationController vMDAnimationController = null;// maid.gameObject.GetComponent<VMDAnimationController>();
            if (VMDAnimationMgr.Instance.maidcontrollers.ContainsKey(maid))
            {
				vMDAnimationController = VMDAnimationMgr.Instance.maidcontrollers[maid];
            }
            else
            {
				vMDAnimationController = maid.gameObject.GetComponent<VMDAnimationController>();
            }			
			if (vMDAnimationController == null)
			{
				MyLog.LogMessage("Install",vMDAnimationController != null);
				if (maid.body0 == null || (maid.body0).m_Bones == null || (maid.body0).Face == null || !maid.body0.isLoadedBody)
				{
					return null;
				}
				vMDAnimationController = maid.gameObject.AddComponent<VMDAnimationController>();
				vMDAnimationController.Init(maid);
				//VMDAnimationMgr.Instance.controllers.Add(vMDAnimationController);
				if (VMDAnimationMgr.Instance.maidcontrollers.ContainsKey(maid))
				{
					VMDAnimationMgr.Instance.maidcontrollers[maid]=vMDAnimationController ;
				}
				else
				{					
					VMDAnimationMgr.Instance.maidcontrollers.Add(maid,vMDAnimationController);
				}
				(maid.body0).m_Bones.gameObject.AddComponent<DestroyListener>().controller = vMDAnimationController;
			}
			return vMDAnimationController;
		}

		private void Init(Maid maid=null)
		{
            if (maid!=null)
            {
				this.maid = maid;
            }
			_shoulderKey = new string[2]
			{
				"Bip01 L Clavicle",
				"Bip01 R Clavicle"
			};
			_armUpKey = new string[2]
			{
				"Bip01 L UpperArm",
				"Bip01 R UpperArm"
			};
			_armLowKey = new string[2]
			{
				"Bip01 L Forearm",
				"Bip01 R Forearm"
			};
			quickAdjust = new QuickAdjust(this);
			ResetBoneNameMap();
			ResetRotationMap();
			CreateClone();
			animeOverride = DefaultCharaAnimOverride.Get(maid);
			GameObject val = new GameObject("_face");
			val.transform.parent = vmdAnimationGo.transform;
			val.transform.localPosition = Vector3.zero;
			val.transform.localRotation = Quaternion.identity;
			faceController = val.AddComponent<CM3D2FaceAnimeController>();
			faceController.Init(maid);
			
			//this.StartCoroutine(DisableDefaultAnimIKCo());// 랙 주범 <- 
		}

		private unsafe void CreateClone()
		{
			vmdAnimationGo = new GameObject("vmdAnimation");
			vmdAnimationGo.transform.parent = (maid.body0).m_Bones.transform.parent;
			vmdAnimationGo.transform.localPosition = (maid.body0).m_Bones.transform.localPosition;
			vmdAnimationGo.transform.localRotation = (maid.body0).m_Bones.transform.localRotation;
			ResetBoneRotations();
			LoadAndCreate(boneNames, vmdAnimationGo, maid.body0);
		}

		private void LoadAndCreate(List<string> pathList, GameObject dummyGo, TBody tbody)
		{
			Dictionary<GameObject, GameObject> dictionary = new Dictionary<GameObject, GameObject>();
			Dictionary<GameObject, GameObject> dictionary2 = new Dictionary<GameObject, GameObject>();
			for (int i = 0; i < pathList.Count(); i++)
			{
				string text = pathList[i];
				GameObject val = CreateBone(dummyGo, text);
				Transform val2 = tbody.m_Bones.transform.Find(text);
				Transform val3 = tbody.m_Bones2.transform.Find(text);
				if (val2 != null)
				{
					val.transform.position = val2.position;
					val.transform.localRotation = Quaternion.identity;
					Transform val4 = new GameObject("MR").transform;
					val4.transform.parent = val.transform;
					val4.transform.localPosition = Vector3.zero;
					val4.transform.rotation = val2.rotation;
					dictionary[val4.gameObject] = val2.gameObject;
					dictionary2[val4.gameObject] = val3.gameObject;
				}
			}
			Transform val5 = dummyGo.transform;
			t_hips = CMT.SearchObjName(val5, "Bip01", true);
			t_leftLowerLeg = CMT.SearchObjName(val5, "Bip01 L Calf", true);
			t_leftUpperLeg = CMT.SearchObjName(val5, "Bip01 L Thigh", true);
			t_rightLowerLeg = CMT.SearchObjName(val5, "Bip01 R Calf", true);
			t_rightUpperLeg = CMT.SearchObjName(val5, "Bip01 R Thigh", true);
			t_leftFoot = CMT.SearchObjName(val5, "Bip01 L Foot", true);
			t_rightFoot = CMT.SearchObjName(val5, "Bip01 R Foot", true);
			t_leftHeel = CMT.SearchObjName(val5, "Bip01 L Toe0Nub", true);
			t_rightHeel = CMT.SearchObjName(val5, "Bip01 R Toe0Nub", true);
			t_leftToe = CMT.SearchObjName(val5, "Bip01 L Toe2Nub", true);
			t_rightToe = CMT.SearchObjName(val5, "Bip01 R Toe2Nub", true);
			t_leftFootIK = val5.Find("_FOOT_IK_L").transform;
			t_leftFootIK.transform.parent = vmdAnimationGo.transform;
			t_leftFootIK.transform.position = t_leftFoot.position;
			t_leftFootIK.transform.rotation = Quaternion.identity;
			t_leftToeIK = t_leftFootIK.Find("_TOE_IK_L").transform;
			t_leftToeIK.parent = t_leftFootIK;
			t_leftToeIK.position = t_leftToe.position;
			t_leftToeIK.localRotation = Quaternion.identity;
			t_rightFootIK = val5.Find("_FOOT_IK_R").transform;
			t_rightFootIK.transform.parent = vmdAnimationGo.transform;
			t_rightFootIK.transform.position = t_rightFoot.position;
			t_rightFootIK.transform.rotation = t_rightFoot.rotation;
			t_rightToeIK = t_rightFootIK.Find("_TOE_IK_R").transform;
			t_rightToeIK.parent = t_rightFootIK;
			t_rightToeIK.position = t_rightToe.position;
			t_rightToeIK.localRotation = Quaternion.identity;
			leftLegIKSolver = GetLegIK(t_leftFootIK, t_leftFoot, t_leftLowerLeg, t_leftUpperLeg);
			rightLegIKSolver = GetLegIK(t_rightFootIK, t_rightFoot, t_rightLowerLeg, t_rightUpperLeg);
			animationForVMD = dummyGo.AddComponent<Animation>();
			autoTrack = dummyGo.AddComponent<AutoTrack>();
			autoTrack.map = dictionary;
			autoTrack.map2 = dictionary2;
			autoTrack.controller = this;
		}

		private void Remap()
		{
			List<string> list = boneNames;
			GameObject val = vmdAnimationGo;
			TBody val2 = maid.body0;
			Dictionary<GameObject, GameObject> dictionary = new Dictionary<GameObject, GameObject>();
			Dictionary<GameObject, GameObject> dictionary2 = new Dictionary<GameObject, GameObject>();
			for (int i = 0; i < list.Count(); i++)
			{
				string text = list[i];
				GameObject val3 = val.transform.Find(text).gameObject;
				Transform val4 = val2.m_Bones.transform.Find(text);
				Transform val5 = val2.m_Bones2.transform.Find(text);
				if (val4 != null)
				{
					Transform val6 = val3.transform.Find("MR");
					dictionary[val6.gameObject] = val4.gameObject;
					dictionary2[val6.gameObject] = val5.gameObject;
				}
			}
			autoTrack = val.GetComponent<AutoTrack>();
			autoTrack.map = dictionary;
			autoTrack.map2 = dictionary2;
		}

		private static GameObject CreateBone(GameObject parent, string path)
		{
			int num = path.IndexOf("/");
			if (num < 0)
			{
				Transform val = parent.transform.Find(path);
				if (val == null)
				{
					val = new GameObject(path).transform;
					val.parent = parent.transform;
					val.localPosition = Vector3.zero;
					val.localRotation = Quaternion.identity;
				}
				return val.gameObject;
			}
			string text = path.Substring(0, num);
			string text2 = path.Substring(num + 1);
			Transform val2 = parent.transform.Find(text);
			if (val2 == null)
			{
				val2 = new GameObject(text).transform;
				val2.parent = parent.transform;
				val2.localPosition = Vector3.zero;
				val2.localRotation = Quaternion.identity;
			}
			if (text2 == "")
			{
				return val2.gameObject;
			}
			return CreateBone(val2.gameObject, text2);
		}

		public void ResetBoneNameMap()
		{
			Dictionary<string, string> dictionary = boneNameMap = new Dictionary<string, string>
			{
				{
					"Bip01",
					"センター"
				},
				{
					"Bip01 Spine",
					"上半身"
				},
				{
					"Bip01 Spine1",
					"上半身2"
				},
				{
					"Bip01 Spine1a",
					"上半身2先"
				},
				{
					"Bip01 Neck",
					"首"
				},
				{
					"Bip01 Head",
					"頭"
				},
				{
					"Bip01 L Clavicle",
					"左肩"
				},
				{
					"Bip01 L UpperArm",
					"左腕"
				},
				{
					"Bip01 L Forearm",
					"左ひじ"
				},
				{
					"Bip01 L Hand",
					"左手首"
				},
				{
					"Bip01 R Clavicle",
					"右肩"
				},
				{
					"Bip01 R UpperArm",
					"右腕"
				},
				{
					"Bip01 R Forearm",
					"右ひじ"
				},
				{
					"Bip01 R Hand",
					"右手首"
				},
				{
					"Bip01 Pelvis",
					"下半身"
				},
				{
					"Bip01 L Thigh",
					"左足"
				},
				{
					"Bip01 L Calf",
					"左ひざ"
				},
				{
					"Bip01 L Foot",
					"左足首"
				},
				{
					"Bip01 R Thigh",
					"右足"
				},
				{
					"Bip01 R Calf",
					"右ひざ"
				},
				{
					"Bip01 R Foot",
					"右足首"
				},
				{
					"Bip01 L Finger01",
					"左親指１"
				},
				{
					"Bip01 L Finger02",
					"左親指２"
				},
				{
					"Bip01 L Finger1",
					"左人指１"
				},
				{
					"Bip01 L Finger11",
					"左人指２"
				},
				{
					"Bip01 L Finger12",
					"左人指３"
				},
				{
					"Bip01 L Finger2",
					"左中指１"
				},
				{
					"Bip01 L Finger21",
					"左中指２"
				},
				{
					"Bip01 L Finger22",
					"左中指３"
				},
				{
					"Bip01 L Finger3",
					"左薬指１"
				},
				{
					"Bip01 L Finger31",
					"左薬指２"
				},
				{
					"Bip01 L Finger32",
					"左薬指３"
				},
				{
					"Bip01 L Finger4",
					"左小指１"
				},
				{
					"Bip01 L Finger41",
					"左小指２"
				},
				{
					"Bip01 L Finger42",
					"左小指３"
				},
				{
					"Bip01 R Finger01",
					"右親指１"
				},
				{
					"Bip01 R Finger02",
					"右親指２"
				},
				{
					"Bip01 R Finger1",
					"右人指１"
				},
				{
					"Bip01 R Finger11",
					"右人指２"
				},
				{
					"Bip01 R Finger12",
					"右人指３"
				},
				{
					"Bip01 R Finger2",
					"右中指１"
				},
				{
					"Bip01 R Finger21",
					"右中指２"
				},
				{
					"Bip01 R Finger22",
					"右中指３"
				},
				{
					"Bip01 R Finger3",
					"右薬指１"
				},
				{
					"Bip01 R Finger31",
					"右薬指２"
				},
				{
					"Bip01 R Finger32",
					"右薬指３"
				},
				{
					"Bip01 R Finger4",
					"右小指１"
				},
				{
					"Bip01 R Finger41",
					"右小指２"
				},
				{
					"Bip01 R Finger42",
					"右小指３"
				},
				{
					"_FOOT_IK_L",
					"左足ＩＫ"
				},
				{
					"_FOOT_IK_R",
					"右足ＩＫ"
				},
				{
					"_TOE_IK_L",
					"左つま先ＩＫ"
				},
				{
					"_TOE_IK_R",
					"右つま先ＩＫ"
				}
			};
		}

		public void AddBoneNameMap(string boneName, string vmxBone)
		{
			boneNameMap[boneName] = vmxBone;
		}

		public void SetCurrentRot(string name)
		{
			if (boneAdjust.ContainsKey(name))
			{
				lastAdjustedBone = name;
				lastModifiedAdjustment = boneAdjust[name];
			}
		}

		public void RemoveBoneNameMap(string boneName)
		{
			if (boneNameMap.ContainsKey(boneName))
			{
				boneNameMap.Remove(boneName);
			}
		}

		public void ClearRotationMap()
		{
			boneAdjust = new Dictionary<string, BoneAdjustment>();
			lastAdjustedBone = null;
			lastModifiedAdjustment = null;
		}

		public void ResetRotationMap()
		{
			boneAdjust = new Dictionary<string, BoneAdjustment>();
			AddRotAxisMap("Bip01", "-x,y,-z");
			AddRotAxisMap("Bip01 Spine", "-x,y,-z");
			AddRotAxisMap("Bip01 Spine1", "-x,y,-z");
			AddRotAxisMap("Bip01 Neck", "-x,y,-z");
			AddRotAxisMap("Bip01 Head", "-x,y,-z");
			AddRotAxisMap("Bip01 L Clavicle", "-x,y,-z");
			AddRotAxisMap("Bip01 L UpperArm", "-x,y,-z");
			AddRotAxisMap("Bip01 L Forearm", "-x,y,-z");
			AddRotAxisMap("Bip01 L Hand", "-x,y,-z");
			AddRotAxisMap("Bip01 R Clavicle", "-x,y,-z");
			AddRotAxisMap("Bip01 R UpperArm", "-x,y,-z");
			AddRotAxisMap("Bip01 R Forearm", "-x,y,-z");
			AddRotAxisMap("Bip01 R Hand", "-x,y,-z");
			AddRotAxisMap("Bip01 Pelvis", "-x,y,-z");
			AddRotAxisMap("Bip01 L Thigh", "-x,y,-z");
			AddRotAxisMap("Bip01 L Calf", "-x,y,z");
			AddRotAxisMap("Bip01 L Foot", "-x,y,-z");
			AddRotAxisMap("Bip01 R Thigh", "-x,y,-z");
			AddRotAxisMap("Bip01 R Calf", "-x,y,-z");
			AddRotAxisMap("Bip01 R Foot", "-x,y,-z");
			AddRotAxisMap("_FOOT_IK_L", "-x,y,-z");
			AddRotAxisMap("_FOOT_IK_R", "-x,y,-z");
			AddRotAxisMap("_TOE_IK_L", "-x,y,-z");
			AddRotAxisMap("_TOE_IK_R", "-x,y,-z");
			string[] array = new string[2]
			{
				"L",
				"R"
			};
			foreach (string text in array)
			{
				for (int j = 0; j <= 4; j++)
				{
					AddRotAxisMap("Bip01 " + text + " Finger" + j, "-x,y,-z");
					AddRotAxisMap("Bip01 " + text + " Finger" + j + "1", "-x,y,-z");
					AddRotAxisMap("Bip01 " + text + " Finger" + j + "2", "-x,y,-z");
				}
			}
			quickAdjust.Set();
			boneAdjust[_armLowKey[0]].rotAxisAdjustment = true;
			boneAdjust[_armLowKey[1]].rotAxisAdjustment = true;
			boneAdjust["Bip01 Neck"].rotationScale = 0.8f;
			AddInitialRotation("Bip01", 270f, 180f, 270f);
			AddInitialRotation("Bip01 Spine", 270f, 90f, 0f);
			AddInitialRotation("Bip01 Spine0a", 0f, 0f, 0f);
			AddInitialRotation("Bip01 Spine1", 0f, 0f, 0f);
			AddInitialRotation("Bip01 Spine1a", 0f, 0f, 0f);
			AddInitialRotation("Bip01 Neck", 0f, 0f, 0f);
			AddInitialRotation("Bip01 Head", 0f, 0f, 0f);
			AddInitialRotation("Bip01 L Clavicle", 0f, 270f, 180f);
			AddInitialRotation("Bip01 L UpperArm", 90f, 0f, 0f);
			AddInitialRotation("Bip01 L Forearm", 0f, 0f, 0f);
			AddInitialRotation("Bip01 L Hand", 180f, 0f, 0f);
			AddInitialRotation("Bip01 R Clavicle", 0f, 90f, 180f);
			AddInitialRotation("Bip01 R UpperArm", -90f, 0f, 0f);
			AddInitialRotation("Bip01 R Forearm", 0f, 0f, 0f);
			AddInitialRotation("Bip01 R Hand", 180f, 0f, 0f);
			AddInitialRotation("Bip01 Pelvis", 270f, 90f, 0f);
			AddInitialRotation("Bip01 L Thigh", 0f, 180f, 0f);
			AddInitialRotation("Bip01 L Calf", 0f, 0f, 0f);
			AddInitialRotation("Bip01 L Foot", 0f, 0f, 0f);
			AddInitialRotation("Bip01 R Thigh", 0f, 180f, 0f);
			AddInitialRotation("Bip01 R Calf", 0f, 0f, 0f);
			AddInitialRotation("Bip01 R Foot", 0f, 0f, 0f);
			array = new string[2]
			{
				"L",
				"R"
			};
			foreach (string text2 in array)
			{
				for (int k = 0; k <= 4; k++)
				{
					AddInitialRotation("Bip01 " + text2 + " Finger" + k, 0f, 0f, 0f);
					AddInitialRotation("Bip01 " + text2 + " Finger" + k + "1", 0f, 0f, 0f);
					AddInitialRotation("Bip01 " + text2 + " Finger" + k + "2", 0f, 0f, 0f);
				}
			}
			for (int l = 0; l < 3; l++)
			{
				AddInitialRotation("Bip01 L Toe" + l, 0f, 0f, 270f);
				AddInitialRotation("Bip01 L Toe" + l + "1", 0f, 0f, 0f);
				AddInitialRotation("Bip01 L Toe" + l + "Nub", 0f, 0f, 180f);
				AddInitialRotation("Bip01 R Toe" + l, 0f, 0f, 270f);
				AddInitialRotation("Bip01 R Toe" + l + "1", 0f, 0f, 0f);
				AddInitialRotation("Bip01 R Toe" + l + "Nub", 0f, 0f, 0f);
			}
			AddInitialRotation("_FOOT_IK_L", 0f, 0f, 0f);
			AddInitialRotation("_FOOT_IK_R", 0f, 0f, 0f);
			AddInitialRotation("_TOE_IK_L", 0f, 0f, 0f);
			AddInitialRotation("_TOE_IK_R", 0f, 0f, 0f);
		}

		public void AddInitialRotation(string boneName, float x, float y, float z)
		{
			initialRotations[boneName] = Quaternion.Euler(x, y, z);
		}

		public void AddRotAxisMap(string spec)
		{
			AddRotAxisMap(lastAdjustedBone, spec);
		}

		public void AddRotAxisMap(string boneName, string spec)
		{
			try
			{
				if (boneName != null)
				{
					BoneAdjustment value = null;
					boneAdjust.TryGetValue(boneName, out value);
					if (value == null)
					{
						value = BoneAdjustment.Init(boneName, spec, Vector3.zero, false);
						boneAdjust[boneName] = value;
					}
					else
					{
						value.SetSpec(spec);
					}
					lastAdjustedBone = boneName;
					lastModifiedAdjustment = value;
				}
			}
			catch (Exception arg)
			{
				Console.WriteLine("Parse error: {0}. {1}", spec, arg);
			}
		}

		public void AddRotationMap(string boneName, float x, float y, float z, bool adjustAxis, float axisX, float axisY, float axisZ)
		{
			if (boneNameMap.ContainsKey(boneName))
			{
				Vector3 rotAdjustment = default(Vector3);
				rotAdjustment = new Vector3(x, y, z);
				Vector3 axisAdjustment = default(Vector3);
				axisAdjustment = new Vector3(axisX, axisY, axisZ);
				BoneAdjustment value = null;
				boneAdjust.TryGetValue(boneName, out value);
				if (value == null)
				{
					value = BoneAdjustment.Init(boneName, "x,y,z", rotAdjustment, adjustAxis);
					value.SetAxisAdjustment(axisAdjustment);
					boneAdjust[boneName] = value;
				}
				else
				{
					value.SetRotAdjustment(rotAdjustment);
					value.rotAxisAdjustment = adjustAxis;
					value.SetAxisAdjustment(axisAdjustment);
				}
				lastAdjustedBone = boneName;
				lastModifiedAdjustment = value;
			}
		}

		public void AddRotationMap(string boneName, float x, float y, float z)
		{
			AddRotationMap(boneName, x, y, z, false, 0f, 0f, 0f);
		}

		public void RemoveRotationMap(string boneName)
		{
			if (boneAdjust.ContainsKey(boneName))
			{
				boneAdjust[boneName].SetRotAdjustment(Vector3.zero);
			}
		}

		public void Dump()
		{
			using (StreamWriter streamWriter = File.CreateText("__rotinfo.txt"))
			{
				foreach (string key in boneAdjust.Keys)
				{
					BoneAdjustment boneAdjustment = boneAdjust[key];
					streamWriter.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}", key, boneAdjustment.spec, boneAdjustment.RotX, boneAdjustment.RotY, boneAdjustment.RotZ, boneAdjustment.rotAxisAdjustment);
				}
			}
		}

		public void ReloadVMDAnimation()
		{
			if (lastLoadedVMD != null)
			{
				LoadVMDAnimation(lastLoadedVMD);
			}
		}


		public unsafe void LoadVMDAnimation(string path, bool play=false)
		{
			try
			{
				if (maid != null)
				{
					subPath = path.Substring(0, path.LastIndexOf(@"\"));
					using (BinaryReader bin = new BinaryReader(File.OpenRead(path)))
					{
						VMDFormat format = VMDLoader.Load(bin, path, clipname);
						Transform val = (maid.body0).m_Bones.transform;
						if (animationForVMD == null)
						{
							animationForVMD = val.gameObject.GetComponent<Animation>();
						}
						animationForVMD.Stop();
						DisableEnableDefaultAnim();
						InitializeBasePositions();
						AnimationClip val2 = new VMDHSConverter(boneNameMap, ModelBaseline, boneAdjust, scale, centerBasePos, hipPositionAdjust, "_face", faceController.VMDFaceName).CreateAnimationClip(format, vmdAnimationGo.gameObject, 4);
						if (loop)
						{
							val2.wrapMode = (WrapMode)2;
						}
						else
						{
							val2.wrapMode = (WrapMode)1;
						}
						lastLoadedVMD = path;
						clipname = "VMDAnim";
						animationForVMD.AddClip(val2, clipname);
						animationForVMD.clip = val2;
						animationForVMD[clipname].speed = _speed;
						if (play)
						{
							//PlayMp3Music();
							animationForVMD.Play(clipname);
						}
					}
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}

		//private static AudioSource pAS = null;
		private string subPath = "";

		/*
		private void PlayMp3Music()
		{
			if (pAS != null)
			{
				GameMain.Destroy(pAS);
				pAS = null;
			}
			System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(subPath);
			System.IO.FileInfo[] files = di.GetFiles("*.ogg", System.IO.SearchOption.AllDirectories);
			if (files.Length != 0)
			{
				pAS = GameMain.Instance.MainCamera.gameObject.AddComponent<AudioSource>();
				WWW www = new WWW("file:///" + files[0].FullName);
				while (www.progress < 1)
				{
				}
				pAS.clip = www.GetAudioClip(false, true);
				//pAS.volume = bgmvolume;
				//pAS.volume = GameMain.Instance.SoundMgr.GetAudioSourceBgm().volume;
				pAS.volume = (float)GameMain.Instance.SoundMgr.GetVolume(AudioSourceMgr.Type.Bgm) * 0.01f;
				pAS.Play();
				GameMain.Instance.SoundMgr.StopAll();
			}
		}
		*/
		public void Play()
		{
			if (clipname != null && vmdAnimEnabled)
			{
				/*
				if (pAS == null)
				{
					PlayMp3Music();
				}
				else
				{
					pAS.Play();
					GameMain.Instance.SoundMgr.StopAll();
				}
				*/
				GetAnimation().Stop(clipname);
				GetAnimation()[clipname].speed = _speed;
				GetAnimation().Play(clipname);
			}
		}

		public bool isPause=false;

		public void Pause()
		{
			if (clipname != null && vmdAnimEnabled)
			{
                if (GetAnimation()[clipname].speed == 0f)
                {
					GetAnimation()[clipname].speed = _speed;
				}
                else
                {
					GetAnimation()[clipname].speed = 0f;
                }
			}
			/*
			if (pAS != null)
			{
				pAS.Pause();
			}
			*/
			//AudioManager.Pause();
		}


		public void Stop()
		{
			if (clipname != null && vmdAnimEnabled)
			{
				GetAnimation().Stop(clipname);
				/*
				if (pAS != null)
				{
					pAS.Stop();
				}
				*/
				//AudioManager.Stop();
				if (faceController != null)
				{
					faceController.ResetFaceValues();
				}
			}
		}

		public void Restart()
		{
			if (clipname != null && vmdAnimEnabled)
			{
				Animation animation = GetAnimation();
				animation[clipname].normalizedTime = 0f;
				animation[clipname].speed = _speed;
				animation.Play(clipname);
				if (AudioManager.audiosource != null)
				{
					AudioManager.audiosource.Play();
					GameMain.Instance.SoundMgr.StopAll();
				}
				if (faceController != null)
				{
					faceController.ResetFaceValues();
				}
			}
		}

		public void SetAnimPosition(float time)
		{
			if (clipname != null && vmdAnimEnabled)
			{
				GetAnimation()[clipname].time = time;
			}
		}

		private IEnumerator DisableDefaultAnimIKCo()
		{
			while (true)
			{
				yield return (object)null;
				ResetEffectorWeight();
			}
		}

		private void DisableEnableDefaultAnim()
		{
			if (vmdAnimEnabled)
			{
				maid.body0.StopAnime("ALL");
				maid.body0.GetAnimation().enabled = false;
				HideShowHead(false);
			}
			else
			{
				maid.body0.GetAnimation().enabled = true;
				HideShowHead(true);
			}
		}

		private unsafe void HideShowHead(bool show)
		{
			try
			{
				TMorph val = (maid.body0).goSlot[0].morph;
				if (show)
				{
					val.FixVisibleFlag();
				}
				else if (headMeshShrinked)
				{
					val.FixVisibleFlag();
				}
				else
				{
					Vector3 val2 = Vector3.zero;
					int num = 0;
					int num2 = (int)f_m_nSubMeshCount.GetValue(val);
					headTriBackUp = new int[val.m_nSubMeshOriTri.Length][];
					Vector3[] vOriVert = val.m_vOriVert;
					for (int i = 0; i < num2; i++)
					{
						headTriBackUp[i] = new int[val.m_nSubMeshOriTri[i].Length];
						val.m_nSubMeshOriTri[i].CopyTo(headTriBackUp[i], 0);
						int[] array = new int[val.m_nSubMeshOriTri[i].Length];
						val.m_nSubMeshOriTri[i].CopyTo(array, 0);
						BoneWeight[] array2 = (BoneWeight[])f_m_bws.GetValue(val);
						for (int j = 0; j < array.Length / 3; j++)
						{
							int num3 = 3;
							for (int k = 0; k < 3; k++)
							{
								int num4 = array[j * 3 + k];
								BoneWeight val3 = array2[num4];
								if (val3.boneIndex0 == 5)
								{
									val2 += vOriVert[num4];
									num++;
									if (vOriVert[num4].z >= headRemoveZ)
									{
										num3--;
										break;
									}
									if (vOriVert[num4].z >= headRemoveZ2 && vOriVert[num4].y < headRemoveY2)
									{
										num3--;
										break;
									}
								}
							}
							if (num3 != 3)
							{
								for (int l = 0; l < 3; l++)
								{
									array[j * 3 + l] = 0;
								}
							}
						}
						array.CopyTo(val.m_nSubMeshOriTri[i], 0);
					}
					val2 /= (float)num;
					headMeshShrinked = true;
					f_m_bDut.SetValue(val, true);
					val.FixVisibleFlag();
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}

		private unsafe void ResetEffectorWeight()
		{
			if (vmdAnimEnabled)
			{
				//(maid.body0).tgtHandL = null;
				//(maid.body0).tgtHandR = null;
			}
		}

		private void LateUpdate()
		{
			try
			{
				if (maid != null)
				{
					Animation animation = GetAnimation();
					if (!(animation == null) && animation.isPlaying)
					{
						try
						{
							if (VMDAnimEnabled)
							{
								maid.m_goOffset.transform.localPosition = Vector3.zero;

								if (AudioManager.audiosource != null)
								{
									if (syncToBGM)
									{
										SetAnimPosition(AudioManager.audiosource.time+ timeShiftNow);
									}
									else if (syncToAnim)
									{
										AudioManager.audiosource.time = GetAnimTime()-timeShiftNow;
									}
								}
							}
						}
						catch (Exception value)
						{
							Console.WriteLine(value);
						}
					}
				}
			}
			catch (Exception value2)
			{
				Console.WriteLine(value2);
			}
			finally
			{
				bool vMDAnimEnabled = VMDAnimEnabled;
			}
		}

		public void ProcessFootIK()
		{
			ProcessFootIK1();
		}

		public void ProcessFootIK1()
		{
			if (enableIK)
			{
				if (leftLegIKSolver == null || rightLegIKSolver == null)
				{
					Console.WriteLine("Leg IK Solver is null.");
				}
				else
				{
					leftLegIKSolver.controll_weight = IKWeight.footControlWeight;
					leftLegIKSolver.iterations = IKWeight.footIteration;
					leftLegIKSolver.minDelta = IKWeight.minDelta * 0.0001f;
					leftLegIKSolver.useLeg = true;
					leftLegIKSolver.Solve();
					rightLegIKSolver.controll_weight = IKWeight.footControlWeight;
					rightLegIKSolver.iterations = IKWeight.footIteration;
					rightLegIKSolver.minDelta = IKWeight.minDelta * 0.0001f;
					rightLegIKSolver.useLeg = true;
					rightLegIKSolver.Solve();
					if (!IKWeight.disableToeIK)
					{
						CCDIKSolver toeIK = GetToeIK(t_leftFoot, t_leftToe, t_leftToeIK);
						CCDIKSolver toeIK2 = GetToeIK(t_rightFoot, t_rightToe, t_rightToeIK);
						toeIK.controll_weight = IKWeight.toeControlWeight;
						toeIK.iterations = IKWeight.toeIteration;
						toeIK.Solve();
						toeIK2.controll_weight = IKWeight.toeControlWeight;
						toeIK2.iterations = IKWeight.toeIteration;
						toeIK2.Solve();
					}
				}
			}
		}

		private unsafe void InitializeBasePositions()
		{
			ResetBoneRotations((maid.body0).m_Bones.transform, false);
			ResetBoneRotations((maid.body0).m_Bones2.transform, false);
			ResetBoneRotations(vmdAnimationGo.transform, true);
			Transform val = vmdAnimationGo.transform;
			Vector3 val2 = (t_leftToe.position + t_rightToe.position) / 2f;
			Vector3 val3 = (t_leftFoot.position + t_rightFoot.position) / 2f;
			Vector3 val4 = val.InverseTransformPoint(val2);
			Vector3 val5 = val.InverseTransformPoint(val3);
			Vector3 val6 = default(Vector3);
			val6 = new Vector3((val5).x, (val4).y, (val5).z);
			Vector3 val7 = val.TransformPoint(val6);
			Vector3 val8 = val.transform.position - val7;
			Transform transform = t_hips.transform;
			transform.position = transform.position + val8;
			ModelBaseline.hipsPos = val.InverseTransformPoint(t_hips.transform.position);
			ModelBaseline.leftFootPos = val.InverseTransformPoint(t_leftFoot.transform.position);
			ModelBaseline.leftToePos = val.InverseTransformPoint(t_leftToe.transform.position);
			ModelBaseline.leftToePosRel = t_leftFoot.InverseTransformPoint(t_leftToe.position);
			ModelBaseline.rightFootPos = val.InverseTransformPoint(t_rightFoot.transform.position);
			ModelBaseline.rightToePos = val.InverseTransformPoint(t_rightToe.transform.position);
			ModelBaseline.rightToePosRel = t_rightFoot.InverseTransformPoint(t_rightToe.position);
			t_leftFootIK.position = t_leftFoot.position;
			t_leftToeIK.position = t_leftToe.position;
			t_rightFootIK.position = t_rightFoot.position;
			t_rightToeIK.position = t_rightToe.position;
		}

		public unsafe void ResetBoneRotations()
		{
			Transform root = (maid.body0).m_Bones.transform;
			ResetBoneRotations(root, false);
		}

		public void ResetBoneRotations(Transform root, bool forceZero = false)
		{
			foreach (string key in initialRotations.Keys)
			{
				Quaternion localRotation = initialRotations[key];
				Transform val = CMT.SearchObjName(root, key, true);
				if (val != null)
				{
					val.localRotation = Quaternion.identity;
					if (!forceZero)
					{
						val.localRotation = localRotation;
					}
				}
			}
		}

		private CCDIKSolver GetLegIK(Transform footIK, Transform foot, Transform lowerLeg, Transform upperLeg)
		{
			CCDIKSolver cCDIKSolver = footIK.gameObject.GetComponent<CCDIKSolver>();
			if (cCDIKSolver == null)
			{
				cCDIKSolver = footIK.gameObject.AddComponent<CCDIKSolver>();
				cCDIKSolver.target = foot;
				cCDIKSolver.chains = (Transform[])new Transform[2]
				{
					lowerLeg,
					upperLeg
				};
			}
			return cCDIKSolver;
		}

		private CCDIKSolver GetToeIK(Transform foot, Transform toe, Transform toeIK)
		{
			CCDIKSolver cCDIKSolver = toeIK.gameObject.GetComponent<CCDIKSolver>();
			if (cCDIKSolver == null)
			{
				cCDIKSolver = toeIK.gameObject.AddComponent<CCDIKSolver>();
				cCDIKSolver.target = toe;
				cCDIKSolver.chains = (Transform[])new Transform[1]
				{
					foot
				};
			}
			return cCDIKSolver;
		}

		private void OnDestroy()
		{
			this.StopAllCoroutines();
			if (AudioManager.audiosource != null)
			{
				GameMain.Destroy(AudioManager.audiosource);
				AudioManager.audiosource = null;
			}
            foreach (var item in VMDAnimationMgr.Instance.maidcontrollers)
            {
                if (item.Value== this)
                {
					VMDAnimationMgr.Instance.maidcontrollers.Remove(item.Key);
				}
            }
			//VMDAnimationMgr.Instance.maidcontrollers.Remove(this);
			UnityEngine.Object.Destroy(animeOverride);
			UnityEngine.Object.Destroy(vmdAnimationGo);
			UnityEngine.Object.Destroy(faceController.gameObject);
		}

		private Animation GetAnimation()
		{
			return animationForVMD;
		}

		private void SetSpeed(float speed)
		{
			Animation animation = GetAnimation();
			if (animation != null && clipname != null)
			{
				animation[clipname].speed = speed;
			}
		}

		private float GetSpeed()
		{
			Animation animation = GetAnimation();
			if (animation != null && clipname != null)
			{
				return animation[clipname].speed;
			}
			return 0f;
		}

		public float GetAnimTime()
		{
			if (clipname != null)
			{
				return GetAnimation()[clipname].time;
			}
			return 0f;
		}

		public void DeleteAnim()
		{
			Animation animation = GetAnimation();
			if (animation != null && clipname != null)
			{
				AnimationClip val = animation.GetClip(clipname);
				if (val != null)
				{
					animation.RemoveClip(val);
				}
				clipname = null;
			}
		}

		private void OnVMDAnimEnabledChanged()
		{
			DisableEnableDefaultAnim();
			if (animationForVMD != null)
			{
				animationForVMD.enabled = vmdAnimEnabled;
			}
		}


	}
}
