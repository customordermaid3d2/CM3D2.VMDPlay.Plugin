using MMD.VMD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	public class VMDHSConverter
	{
		private abstract class CustomKeyframe<Type>
		{
			public float time
			{
				get;
				set;
			}

			public Type value
			{
				get;
				set;
			}

			public CustomKeyframe(float time, Type value)
			{
				this.time = time;
				this.value = value;
			}
		}

		private class FloatKeyframe : CustomKeyframe<float>
		{
			public FloatKeyframe(float time, float value)
				: base(time, value)
			{
			}

			public unsafe static FloatKeyframe Lerp(FloatKeyframe from, FloatKeyframe to, Vector2 t)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				return new FloatKeyframe(Mathf.Lerp(from.time, to.time, (t).x), Mathf.Lerp(from.value, to.value, (t).y));
			}

			public static void AddBezierKeyframes(byte[] interpolation, int type, FloatKeyframe prev_keyframe, FloatKeyframe cur_keyframe, int interpolationQuality, ref List<FloatKeyframe> keyframes, ref int index)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0043: Unknown result type (might be due to invalid IL or missing references)
				//IL_0044: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_004c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				if (prev_keyframe == null || IsLinear(interpolation, type))
				{
					keyframes.Add(cur_keyframe);
					index = keyframes.Count();
				}
				else
				{
					Vector2 bezierHandle = GetBezierHandle(interpolation, type, 0);
					Vector2 bezierHandle2 = GetBezierHandle(interpolation, type, 1);
					for (int i = 0; i < interpolationQuality; i++)
					{
						float t = (float)(i + 1) / (float)interpolationQuality;
						Vector2 t2 = SampleBezier(bezierHandle, bezierHandle2, t);
						keyframes.Add(Lerp(prev_keyframe, cur_keyframe, t2));
						index = keyframes.Count();
					}
				}
			}
		}

		private class QuaternionKeyframe : CustomKeyframe<Quaternion>
		{
			public QuaternionKeyframe(float time, Quaternion value)
				: base(time, value)
			{
			}//IL_0002: Unknown result type (might be due to invalid IL or missing references)


			public unsafe static QuaternionKeyframe Lerp(QuaternionKeyframe from, QuaternionKeyframe to, Vector2 t)
			{
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_001e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				return new QuaternionKeyframe(Mathf.Lerp(from.time, to.time, (t).x), Quaternion.Slerp(from.value, to.value, (t).y));
			}

			public static void AddBezierKeyframes(byte[] interpolation, int type, QuaternionKeyframe prev_keyframe, QuaternionKeyframe cur_keyframe, int interpolationQuality, ref QuaternionKeyframe[] keyframes, ref int index)
			{
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				if (prev_keyframe == null || IsLinear(interpolation, type))
				{
					keyframes[index++] = cur_keyframe;
				}
				else
				{
					Vector2 bezierHandle = GetBezierHandle(interpolation, type, 0);
					Vector2 bezierHandle2 = GetBezierHandle(interpolation, type, 1);
					for (int i = 0; i < interpolationQuality; i++)
					{
						float t = (float)(i + 1) / (float)interpolationQuality;
						Vector2 t2 = SampleBezier(bezierHandle, bezierHandle2, t);
						keyframes[index++] = Lerp(prev_keyframe, cur_keyframe, t2);
					}
				}
			}
		}

		private Dictionary<string, string> boneNameMap;

		private Dictionary<string, BoneAdjustment> boneAdjustment;

		public float scale = 1f;

		public Vector3 centerBasePos;

		private Vector3 hipCenterDiff;

		public ModelBaselineData HSModelBaseline;

		public string faceGoPath;

		public Dictionary<string, float[]> faceBlendMap;

		public HashSet<string> usedFaceNames;

		private AnimationCurve centerXCurve;

		private AnimationCurve centerYCurve;

		private AnimationCurve centerZCurve;

		private AnimationCurve centerWCurve;

		private static PropertyInfo p_legacy = typeof(AnimationClip).GetProperty("legacy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty);

		private const int TangentModeBothLinear = 21;

		public VMDHSConverter(Dictionary<string, string> boneNameMap, ModelBaselineData hsModelBaseline, Dictionary<string, BoneAdjustment> boneAdjustment, float scale, Vector3 centerBasePos, Vector3 hipPositionAdjust, string faceGoPath, Dictionary<string, float[]> faceBlendMap)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			HSModelBaseline = hsModelBaseline;
			this.boneNameMap = boneNameMap;
			this.boneAdjustment = boneAdjustment;
			this.centerBasePos = centerBasePos;
			hipCenterDiff = hipPositionAdjust;
			this.scale = scale;
			this.faceGoPath = faceGoPath;
			this.faceBlendMap = faceBlendMap;
			usedFaceNames = new HashSet<string>();
		}

		public AnimationClip CreateAnimationClip(VMDFormat format, GameObject assign_pmd, int interpolationQuality)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Expected O, but got Unknown
			//IL_003f: Expected O, but got Unknown
			AnimationClip val = new AnimationClip();
			val.name = format.clip_name;
			if (p_legacy != null)
			{
				p_legacy.SetValue(val, true, null);
			}
			Dictionary<string, string> dic = new Dictionary<string, string>();
			FullSearchBonePath(assign_pmd.transform, assign_pmd.transform, dic);
			Dictionary<string, GameObject> obj = new Dictionary<string, GameObject>();
			FullEntryBoneAnimation(format, val, dic, obj, interpolationQuality);
			CreateKeysForSkin(format, val);
			return val;
		}

		private static Vector2 GetBezierHandle(byte[] interpolation, int type, int ab)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)(int)interpolation[ab * 8 + type], (float)(int)interpolation[ab * 8 + 4 + type]) / 127f;
		}

		private static Vector2 SampleBezier(Vector2 bezierHandleA, Vector2 bezierHandleB, float t)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			Vector2 zero = Vector2.zero;
			Vector2 val = default(Vector2);
			val = new Vector2(1f, 1f);
			Vector2 val2 = Vector2.Lerp(zero, bezierHandleA, t);
			Vector2 val3 = Vector2.Lerp(bezierHandleA, bezierHandleB, t);
			Vector2 val4 = Vector2.Lerp(bezierHandleB, val, t);
			Vector2 val5 = Vector2.Lerp(val2, val3, t);
			Vector2 val6 = Vector2.Lerp(val3, val4, t);
			return Vector2.Lerp(val5, val6, t);
		}

		private static bool IsLinear(byte[] interpolation, int type)
		{
			byte num = interpolation[type];
			byte b = interpolation[4 + type];
			byte b2 = interpolation[8 + type];
			byte b3 = interpolation[12 + type];
			if (num == b)
			{
				return b2 == b3;
			}
			return false;
		}

		private int GetKeyframeCount(List<VMDFormat.Motion> mlist, int type, int interpolationQuality)
		{
			int num = 0;
			for (int i = 0; i < mlist.Count; i++)
			{
				num = ((i <= 0 || IsLinear(mlist[i].interpolation, type)) ? (num + 1) : (num + interpolationQuality));
			}
			return num;
		}

		private void AddDummyKeyframe(ref Keyframe[] keyframes)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			if (keyframes.Length == 1)
			{
				Keyframe[] array = (Keyframe[])new Keyframe[2]
				{
					keyframes[0],
					keyframes[0]
				};
				array[1].time = array[1].time + 1.66666669E-05f;
				array[0].outTangent = 0f;
				array[1].inTangent = 0f;
				keyframes = array;
			}
		}

		private float GetLinearTangentForPosition(Keyframe from_keyframe, Keyframe to_keyframe)
		{
			return (to_keyframe.value - from_keyframe.value) / (to_keyframe.time - from_keyframe.time);
		}

		private float Mod360(float angle)
		{
			if (!(angle < 0f))
			{
				return angle;
			}
			return angle + 360f;
		}

		private float GetLinearTangentForRotation(Keyframe from_keyframe, Keyframe to_keyframe)
		{
			float num = Mod360(to_keyframe.value);
			float num2 = Mod360(from_keyframe.value);
			float num3 = Mod360(num - num2);
			if (num3 < 180f)
			{
				return num3 / (to_keyframe.time - from_keyframe.time);
			}
			return (num3 - 360f) / (to_keyframe.time - from_keyframe.time);
		}

		private unsafe Dictionary<string, Keyframe[]> ToKeyframesForRotation(QuaternionKeyframe[] custom_keys, ref Keyframe[] rx_keys, ref Keyframe[] ry_keys, ref Keyframe[] rz_keys)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0203: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_022f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_028a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_036c: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			rx_keys = (Keyframe[])new Keyframe[custom_keys.Length];
			ry_keys = (Keyframe[])new Keyframe[custom_keys.Length];
			rz_keys = (Keyframe[])new Keyframe[custom_keys.Length];
			Keyframe[] keyframes = (Keyframe[])new Keyframe[custom_keys.Length];
			Keyframe[] keyframes2 = (Keyframe[])new Keyframe[custom_keys.Length];
			Keyframe[] keyframes3 = (Keyframe[])new Keyframe[custom_keys.Length];
			Keyframe[] keyframes4 = (Keyframe[])new Keyframe[custom_keys.Length];
			Dictionary<string, Keyframe[]> dictionary = new Dictionary<string, Keyframe[]>();
			Quaternion val = Quaternion.identity;
			for (int i = 0; i < custom_keys.Length; i++)
			{
				Quaternion value = custom_keys[i].value;
				Vector3 eulerAngles = value.eulerAngles;
				rx_keys[i] = new Keyframe(custom_keys[i].time, (eulerAngles).x);
				ry_keys[i] = new Keyframe(custom_keys[i].time, (eulerAngles).y);
				rz_keys[i] = new Keyframe(custom_keys[i].time, (eulerAngles).z);
				rx_keys[i].tangentMode = 21;
				ry_keys[i].tangentMode = 21;
				rz_keys[i].tangentMode = 21;
				if (i > 0)
				{
					float linearTangentForRotation = GetLinearTangentForRotation(rx_keys[i - 1], rx_keys[i]);
					float linearTangentForRotation2 = GetLinearTangentForRotation(ry_keys[i - 1], ry_keys[i]);
					float linearTangentForRotation3 = GetLinearTangentForRotation(rz_keys[i - 1], rz_keys[i]);
					rx_keys[i - 1].outTangent = linearTangentForRotation;
					ry_keys[i - 1].outTangent = linearTangentForRotation2;
					rz_keys[i - 1].outTangent = linearTangentForRotation3;
					rx_keys[i].inTangent = linearTangentForRotation;
					ry_keys[i].inTangent = linearTangentForRotation2;
					rz_keys[i].inTangent = linearTangentForRotation3;
				}
				Quaternion val2 = Quaternion.Euler(eulerAngles);
				if (i > 0)
				{
					val2 = Quaternion.Slerp(val, val2, 0.9999f);
				}
				float time = custom_keys[i].time;
				keyframes[i] = new Keyframe(time, (val2).x);
				keyframes2[i] = new Keyframe(time, (val2).y);
				keyframes3[i] = new Keyframe(time, (val2).z);
				keyframes4[i] = new Keyframe(time, (val2).w);
				keyframes[i].tangentMode = 21;
				keyframes2[i].tangentMode = 21;
				keyframes3[i].tangentMode = 21;
				keyframes4[i].tangentMode = 21;
				if (i > 0)
				{
					float linearTangentForPosition = GetLinearTangentForPosition(keyframes[i - 1], keyframes[i]);
					float linearTangentForPosition2 = GetLinearTangentForPosition(keyframes2[i - 1], keyframes2[i]);
					float linearTangentForPosition3 = GetLinearTangentForPosition(keyframes3[i - 1], keyframes3[i]);
					float linearTangentForPosition4 = GetLinearTangentForPosition(keyframes4[i - 1], keyframes4[i]);
					keyframes[i - 1].outTangent = linearTangentForPosition;
					keyframes2[i - 1].outTangent = linearTangentForPosition2;
					keyframes3[i - 1].outTangent = linearTangentForPosition3;
					keyframes4[i - 1].outTangent = linearTangentForPosition4;
					keyframes[i].inTangent = linearTangentForPosition;
					keyframes2[i].inTangent = linearTangentForPosition2;
					keyframes3[i].inTangent = linearTangentForPosition3;
					keyframes4[i].inTangent = linearTangentForPosition4;
				}
				val = val2;
			}
			AddDummyKeyframe(ref rx_keys);
			AddDummyKeyframe(ref ry_keys);
			AddDummyKeyframe(ref rz_keys);
			AddDummyKeyframe(ref keyframes);
			AddDummyKeyframe(ref keyframes2);
			AddDummyKeyframe(ref keyframes3);
			AddDummyKeyframe(ref keyframes4);
			dictionary.Add("localRotation.x", keyframes);
			dictionary.Add("localRotation.y", keyframes2);
			dictionary.Add("localRotation.z", keyframes3);
			dictionary.Add("localRotation.w", keyframes4);
			return dictionary;
		}

		private void CreateKeysForRotation(VMDFormat format, AnimationClip clip, string current_bone, string bone_path, int interpolationQuality)
		{
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Expected O, but got Unknown
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Expected O, but got Unknown
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Expected O, but got Unknown
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Expected O, but got Unknown
			try
			{
				if (boneNameMap.ContainsKey(current_bone))
				{
					string text = boneNameMap[current_bone];
					if (!format.motion_list.motion.ContainsKey(text))
					{
						Console.WriteLine("bone {0} not found in motionlist", text);
					}
					else
					{
						Dictionary<string, AnimationCurve> dictionary = null;
						if (text == "右足ＩＫ" || text == "左足ＩＫ")
						{
							string boneName = text.Substring(0, 2) + "IK親";
							dictionary = CollectCurve(format, boneName, interpolationQuality);
						}
						BoneAdjustment boneAdjustment = null;
						if (this.boneAdjustment.ContainsKey(current_bone))
						{
							boneAdjustment = this.boneAdjustment[current_bone];
						}
						List<VMDFormat.Motion> list = format.motion_list.motion[text];
						QuaternionKeyframe[] keyframes = new QuaternionKeyframe[GetKeyframeCount(list, 3, interpolationQuality)];
						QuaternionKeyframe prev_keyframe = null;
						int index = 0;
						Quaternion val = Quaternion.identity;
						for (int i = 0; i < list.Count; i++)
						{
							float num = (float)(double)list[i].flame_no * 0.0333333351f;
							Quaternion val2 = list[i].rotation;
							if (dictionary != null)
							{
								AnimationCurve value = null;
								dictionary.TryGetValue("localEulerAngles.x", out value);
								AnimationCurve value2 = null;
								dictionary.TryGetValue("localEulerAngles.y", out value2);
								AnimationCurve value3 = null;
								dictionary.TryGetValue("localEulerAngles.z", out value3);
								Quaternion identity = Quaternion.identity;
								if (value != null && value2 != null && value3 != null)
								{
									identity = Quaternion.Euler(value.Evaluate(num), value2.Evaluate(num), value3.Evaluate(num));
									val2 *= identity;
								}
							}
							if (boneAdjustment != null)
							{
								val2 = boneAdjustment.GetAdjustedRotation(val2);
							}
							if (i != 0)
							{
								val2 = Quaternion.Slerp(val, val2, 0.99999f);
							}
							QuaternionKeyframe quaternionKeyframe = new QuaternionKeyframe(num, val2);
							QuaternionKeyframe.AddBezierKeyframes(list[i].interpolation, 3, prev_keyframe, quaternionKeyframe, interpolationQuality, ref keyframes, ref index);
							prev_keyframe = quaternionKeyframe;
							val = val2;
						}
						Keyframe[] rx_keys = null;
						Keyframe[] ry_keys = null;
						Keyframe[] rz_keys = null;
						Dictionary<string, Keyframe[]> dictionary2 = ToKeyframesForRotation(keyframes, ref rx_keys, ref ry_keys, ref rz_keys);
						new AnimationCurve(rx_keys);
						new AnimationCurve(ry_keys);
						new AnimationCurve(rz_keys);
						AnimationCurve val3 = new AnimationCurve(dictionary2["localRotation.x"]);
						AnimationCurve val4 = new AnimationCurve(dictionary2["localRotation.y"]);
						AnimationCurve val5 = new AnimationCurve(dictionary2["localRotation.z"]);
						AnimationCurve val6 = new AnimationCurve(dictionary2["localRotation.w"]);
						clip.SetCurve(bone_path, typeof(Transform), "localRotation.x", val3);
						clip.SetCurve(bone_path, typeof(Transform), "localRotation.y", val4);
						clip.SetCurve(bone_path, typeof(Transform), "localRotation.z", val5);
						clip.SetCurve(bone_path, typeof(Transform), "localRotation.w", val6);
						if (text == "センタ\u30fc")
						{
							centerXCurve = val3;
							centerYCurve = val4;
							centerZCurve = val5;
							centerWCurve = val6;
						}
					}
				}
			}
			catch (KeyNotFoundException)
			{
			}
		}

		private Keyframe[] ToKeyframesForLocation(FloatKeyframe[] custom_keys)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			Keyframe[] keyframes = (Keyframe[])new Keyframe[custom_keys.Length];
			for (int i = 0; i < custom_keys.Length; i++)
			{
				keyframes[i] = new Keyframe(custom_keys[i].time, custom_keys[i].value);
				keyframes[i].tangentMode = 21;
				if (i > 0)
				{
					float linearTangentForPosition = GetLinearTangentForPosition(keyframes[i - 1], keyframes[i]);
					keyframes[i - 1].outTangent = linearTangentForPosition;
					keyframes[i].inTangent = linearTangentForPosition;
				}
			}
			AddDummyKeyframe(ref keyframes);
			return keyframes;
		}

		private unsafe void CreateKeysForLocation(VMDFormat format, AnimationClip clip, string current_bone, string bone_path, int interpolationQuality, GameObject current_obj = null)
		{
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_021b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_0222: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02de: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0300: Unknown result type (might be due to invalid IL or missing references)
			//IL_0305: Unknown result type (might be due to invalid IL or missing references)
			//IL_030a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0319: Unknown result type (might be due to invalid IL or missing references)
			//IL_0321: Unknown result type (might be due to invalid IL or missing references)
			//IL_032c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0331: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0352: Unknown result type (might be due to invalid IL or missing references)
			//IL_035d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0378: Unknown result type (might be due to invalid IL or missing references)
			//IL_0380: Unknown result type (might be due to invalid IL or missing references)
			//IL_038b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0390: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03be: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0403: Unknown result type (might be due to invalid IL or missing references)
			//IL_0411: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ca: Expected O, but got Unknown
			//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Expected O, but got Unknown
			//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Expected O, but got Unknown
			try
			{
				if (boneNameMap.ContainsKey(current_bone))
				{
					string text = boneNameMap[current_bone];
					if (!format.motion_list.motion.ContainsKey(text))
					{
						Console.WriteLine("bone {0} not found in motionlist", text);
					}
					else
					{
						bool flag = false;
						if (text == "センタ\u30fc")
						{
							flag = true;
						}
						Dictionary<string, AnimationCurve> dictionary = null;
						if (text == "右足ＩＫ" || text == "左足ＩＫ")
						{
							string boneName = text.Substring(0, 2) + "IK親";
							dictionary = CollectCurve(format, boneName, interpolationQuality);
						}
						Vector3 val = Vector3.zero;
						if (current_obj != null)
						{
							val = current_obj.transform.localPosition;
						}
						List<VMDFormat.Motion> list = format.motion_list.motion[text];
						GetKeyframeCount(list, 0, interpolationQuality);
						GetKeyframeCount(list, 1, interpolationQuality);
						GetKeyframeCount(list, 2, interpolationQuality);
						List<FloatKeyframe> keyframes = new List<FloatKeyframe>();
						List<FloatKeyframe> keyframes2 = new List<FloatKeyframe>();
						List<FloatKeyframe> keyframes3 = new List<FloatKeyframe>();
						FloatKeyframe prev_keyframe = null;
						FloatKeyframe prev_keyframe2 = null;
						FloatKeyframe prev_keyframe3 = null;
						int index = 0;
						int index2 = 0;
						int index3 = 0;
						for (int i = 0; i < list.Count; i++)
						{
							float num = (float)(double)list[i].flame_no * 0.0333333351f;
							Vector3 val2 = list[i].location;
							if (!(val2 == Vector3.zero))
							{
								if (dictionary != null)
								{
									AnimationCurve value = null;
									dictionary.TryGetValue("localPosition.x", out value);
									AnimationCurve value2 = null;
									dictionary.TryGetValue("localPosition.y", out value2);
									AnimationCurve value3 = null;
									dictionary.TryGetValue("localPosition.z", out value3);
									AnimationCurve value4 = null;
									dictionary.TryGetValue("localEulerAngles.x", out value4);
									AnimationCurve value5 = null;
									dictionary.TryGetValue("localEulerAngles.y", out value5);
									AnimationCurve value6 = null;
									dictionary.TryGetValue("localEulerAngles.z", out value6);
									Vector3 zero = Vector3.zero;
									if (value != null && value2 != null && value3 != null)
									{
										zero = new Vector3(value.Evaluate(num), value2.Evaluate(num), value3.Evaluate(num));
									}
									Quaternion val3 = Quaternion.identity;
									if (value4 != null && value5 != null && value6 != null)
									{
										val3 = Quaternion.Euler(value4.Evaluate(num), value5.Evaluate(num), value6.Evaluate(num));
									}
									val2 = zero + val3 * val2;
								}
								val2.z = 0f - (val2).z;
								val2.x = 0f - (val2).x;
								if (flag)
								{
									if (centerXCurve != null)
									{
										try
										{
											float num2 = centerXCurve.Evaluate(num);
											float num3 = centerYCurve.Evaluate(num);
											float num4 = centerZCurve.Evaluate(num);
											float num5 = centerWCurve.Evaluate(num);
											Vector3 val4 = new Quaternion(num2, num3, num4, num5) * hipCenterDiff;
											val2 = centerBasePos + val2 + val4;
										}
										catch (Exception value7)
										{
											Console.WriteLine(value7);
											val2 = val2 + centerBasePos + hipCenterDiff;
										}
									}
									else
									{
										val2 = val2 + centerBasePos + hipCenterDiff;
									}
								}
								if (text == "右足ＩＫ")
								{
									val2 += HSModelBaseline.rightFootPos / scale;
								}
								else if (text == "左足ＩＫ")
								{
									val2 += HSModelBaseline.leftFootPos / scale;
								}
								else if (text == "右つま先ＩＫ")
								{
									val2 += HSModelBaseline.rightToePosRel / scale;
								}
								else if (text == "左つま先ＩＫ")
								{
									val2 += HSModelBaseline.leftToePosRel / scale;
								}
								FloatKeyframe floatKeyframe = new FloatKeyframe(num, (val2).x * scale + (val).x);
								FloatKeyframe floatKeyframe2 = new FloatKeyframe(num, (val2).y * scale + (val).y);
								FloatKeyframe floatKeyframe3 = new FloatKeyframe(num, (val2).z * scale + (val).z);
								FloatKeyframe.AddBezierKeyframes(list[i].interpolation, 0, prev_keyframe, floatKeyframe, interpolationQuality, ref keyframes, ref index);
								FloatKeyframe.AddBezierKeyframes(list[i].interpolation, 1, prev_keyframe2, floatKeyframe2, interpolationQuality, ref keyframes2, ref index2);
								FloatKeyframe.AddBezierKeyframes(list[i].interpolation, 2, prev_keyframe3, floatKeyframe3, interpolationQuality, ref keyframes3, ref index3);
								prev_keyframe = floatKeyframe;
								prev_keyframe2 = floatKeyframe2;
								prev_keyframe3 = floatKeyframe3;
							}
						}
						FloatKeyframe[] custom_keys = keyframes.ToArray();
						FloatKeyframe[] custom_keys2 = keyframes2.ToArray();
						FloatKeyframe[] custom_keys3 = keyframes3.ToArray();
						if (list.Count != 0)
						{
							AnimationCurve val5 = new AnimationCurve(ToKeyframesForLocation(custom_keys));
							AnimationCurve val6 = new AnimationCurve(ToKeyframesForLocation(custom_keys2));
							AnimationCurve val7 = new AnimationCurve(ToKeyframesForLocation(custom_keys3));
							clip.SetCurve(bone_path, typeof(Transform), "localPosition.x", val5);
							clip.SetCurve(bone_path, typeof(Transform), "localPosition.y", val6);
							clip.SetCurve(bone_path, typeof(Transform), "localPosition.z", val7);
						}
					}
				}
			}
			catch (KeyNotFoundException)
			{
			}
		}

		private unsafe Dictionary<string, AnimationCurve> CollectCurve(VMDFormat format, string boneName, int interpolationQuality)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Expected O, but got Unknown
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Expected O, but got Unknown
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Expected O, but got Unknown
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0231: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Expected O, but got Unknown
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Expected O, but got Unknown
			//IL_0298: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Expected O, but got Unknown
			if (!format.motion_list.motion.ContainsKey(boneName))
			{
				return null;
			}
			Dictionary<string, AnimationCurve> dictionary = new Dictionary<string, AnimationCurve>();
			List<VMDFormat.Motion> list = format.motion_list.motion[boneName];
			Vector3 zero = Vector3.zero;
			GetKeyframeCount(list, 0, interpolationQuality);
			GetKeyframeCount(list, 1, interpolationQuality);
			GetKeyframeCount(list, 2, interpolationQuality);
			List<FloatKeyframe> keyframes = new List<FloatKeyframe>();
			List<FloatKeyframe> keyframes2 = new List<FloatKeyframe>();
			List<FloatKeyframe> keyframes3 = new List<FloatKeyframe>();
			FloatKeyframe prev_keyframe = null;
			FloatKeyframe prev_keyframe2 = null;
			FloatKeyframe prev_keyframe3 = null;
			int index = 0;
			int index2 = 0;
			int index3 = 0;
			for (int i = 0; i < list.Count; i++)
			{
				float time = (float)(double)list[i].flame_no * 0.0333333351f;
				FloatKeyframe floatKeyframe = new FloatKeyframe(time, list[i].location.x * scale + (zero).x);
				FloatKeyframe floatKeyframe2 = new FloatKeyframe(time, list[i].location.y * scale + (zero).y);
				FloatKeyframe floatKeyframe3 = new FloatKeyframe(time, list[i].location.z * scale + (zero).z);
				FloatKeyframe.AddBezierKeyframes(list[i].interpolation, 0, prev_keyframe, floatKeyframe, interpolationQuality, ref keyframes, ref index);
				FloatKeyframe.AddBezierKeyframes(list[i].interpolation, 1, prev_keyframe2, floatKeyframe2, interpolationQuality, ref keyframes2, ref index2);
				FloatKeyframe.AddBezierKeyframes(list[i].interpolation, 2, prev_keyframe3, floatKeyframe3, interpolationQuality, ref keyframes3, ref index3);
				prev_keyframe = floatKeyframe;
				prev_keyframe2 = floatKeyframe2;
				prev_keyframe3 = floatKeyframe3;
			}
			if (list.Count != 0)
			{
				FloatKeyframe[] custom_keys = keyframes.ToArray();
				FloatKeyframe[] custom_keys2 = keyframes2.ToArray();
				FloatKeyframe[] custom_keys3 = keyframes3.ToArray();
				AnimationCurve value = new AnimationCurve(ToKeyframesForLocation(custom_keys));
				AnimationCurve value2 = new AnimationCurve(ToKeyframesForLocation(custom_keys2));
				AnimationCurve value3 = new AnimationCurve(ToKeyframesForLocation(custom_keys3));
				dictionary.Add("localPosition.x", value);
				dictionary.Add("localPosition.y", value2);
				dictionary.Add("localPosition.z", value3);
			}
			QuaternionKeyframe[] keyframes4 = new QuaternionKeyframe[GetKeyframeCount(list, 3, interpolationQuality)];
			QuaternionKeyframe prev_keyframe4 = null;
			int index4 = 0;
			for (int j = 0; j < list.Count; j++)
			{
				float time2 = (float)(double)list[j].flame_no * 0.0333333351f;
				Quaternion rotation = list[j].rotation;
				QuaternionKeyframe quaternionKeyframe = new QuaternionKeyframe(time2, rotation);
				QuaternionKeyframe.AddBezierKeyframes(list[j].interpolation, 3, prev_keyframe4, quaternionKeyframe, interpolationQuality, ref keyframes4, ref index4);
				prev_keyframe4 = quaternionKeyframe;
			}
			Keyframe[] rx_keys = null;
			Keyframe[] ry_keys = null;
			Keyframe[] rz_keys = null;
			ToKeyframesForRotation(keyframes4, ref rx_keys, ref ry_keys, ref rz_keys);
			AnimationCurve value4 = new AnimationCurve(rx_keys);
			AnimationCurve value5 = new AnimationCurve(ry_keys);
			AnimationCurve value6 = new AnimationCurve(rz_keys);
			dictionary.Add("localEulerAngles.x", value4);
			dictionary.Add("localEulerAngles.y", value5);
			dictionary.Add("localEulerAngles.z", value6);
			return dictionary;
		}

		private void CreateKeysForSkin(VMDFormat format, AnimationClip clip)
		{
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Expected O, but got Unknown
			usedFaceNames = new HashSet<string>();
			foreach (KeyValuePair<string, List<VMDFormat.SkinData>> item in format.skin_list.skin)
			{
				string key = item.Key;
				if (!faceBlendMap.ContainsKey(key))
				{
					Console.WriteLine("Face Setting for {0} not found. Ignore this face morph.", key);
				}
				else
				{
					List<VMDFormat.SkinData> value = item.Value;
					Keyframe[] keyframes = (Keyframe[])new Keyframe[item.Value.Count];
					for (int i = 0; i < item.Value.Count; i++)
					{
						keyframes[i] = new Keyframe((float)(double)value[i].flame_no * 0.0333333351f, value[i].weight);
						keyframes[i].tangentMode = 21;
						if (i > 0)
						{
							float linearTangentForPosition = GetLinearTangentForPosition(keyframes[i - 1], keyframes[i]);
							keyframes[i - 1].outTangent = linearTangentForPosition;
							keyframes[i].inTangent = linearTangentForPosition;
						}
					}
					AddDummyKeyframe(ref keyframes);
					AnimationCurve val = new AnimationCurve(keyframes);
					clip.SetCurve(faceGoPath + "/" + key, typeof(Transform), "localPosition.z", val);
					usedFaceNames.Add(key);
				}
			}
		}

		private string GetBonePath(Transform transform, Transform root)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			if (transform == root)
			{
				return "";
			}
			if (transform.parent == root)
			{
				return transform.name;
			}
			string bonePath = GetBonePath(transform.parent, root);
			return bonePath + "/" + transform.name;
		}

		private void FullSearchBonePath(Transform root, Transform transform, Dictionary<string, string> dic)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Expected O, but got Unknown
			int childCount = transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				Transform transform2 = transform.GetChild(i);
				FullSearchBonePath(root, transform2, dic);
			}
			string bonePath = GetBonePath(transform, root);
			if (dic.ContainsKey(transform.name))
			{
				string text = dic[transform.name];
			}
			else
			{
				dic.Add(transform.name, bonePath);
			}
		}

		private void FullEntryBoneAnimation(VMDFormat format, AnimationClip clip, Dictionary<string, string> dic, Dictionary<string, GameObject> obj, int interpolationQuality)
		{
			foreach (KeyValuePair<string, string> item in dic)
			{
				GameObject val = null;
				if (obj.ContainsKey(item.Key))
				{
					val = obj[item.Key];
					Rigidbody component = val.GetComponent<Rigidbody>();
					if (component != null && !component.isKinematic)
					{
						continue;
					}
				}
				CreateKeysForLocation(format, clip, item.Key, item.Value, interpolationQuality, val);
				CreateKeysForRotation(format, clip, item.Key, item.Value, interpolationQuality);
			}
		}

		private void GetGameObjects(Dictionary<string, GameObject> obj, GameObject assign_pmd)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Expected O, but got Unknown
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < assign_pmd.transform.childCount; i++)
			{
				Transform val = assign_pmd.transform.GetChild(i);
				try
				{
					obj.Add(val.name, val.gameObject);
				}
				catch (ArgumentException ex)
				{
					Debug.Log((object)ex.Message);
					Debug.Log((object)("An element with the same key already exists in the dictionary. -> " + val.name));
				}
				if (!(val == null))
				{
					GetGameObjects(obj, val.gameObject);
				}
			}
		}
	}
}
