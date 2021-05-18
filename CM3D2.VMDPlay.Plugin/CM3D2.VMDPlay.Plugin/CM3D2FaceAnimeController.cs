using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	public class CM3D2FaceAnimeController : UnityEngine.MonoBehaviour
	{
		public class FaceBlendModifier
		{
			public CM3D2FaceAnimeController controller;

			public int index;

			public string name;

			private float _value;

			public float Value
			{
				get
				{
					return _value;
				}
				set
				{
					if (_value != value && 0f <= value && value <= 1f)
					{
						_value = value;
						Apply();
					}
				}
			}

			public void Reset()
			{
				_value = 0f;
				Apply();
			}

			public void Apply()
			{
				controller.faceMorph.SetBlendValues(index, _value);
				controller.faceMorph.FixBlendValues();
			}
		}

		public class FaceBlendSetPattern
		{
			public CM3D2FaceAnimeController controller;

			public TMorph face;

			public string faceName;

			public Dictionary<string, FaceBlendSetPattern> Set
			{
				get
				{
					Apply();
					return controller.BlendSet;
				}
			}

			public void Apply()
			{
				face.ClearBlendValues();
				face.MulBlendValues(faceName, 1f);
				face.FixBlendValues();
			}

			public void Dump()
			{
				Console.WriteLine(faceName);
				float[] array = face.dicBlendSet[faceName];
				for (int i = 0; i < face.MorphCount; i++)
				{
					if (face.BlendDatas[i] != null)
					{
						string name = face.BlendDatas[i].name;
						float num = array[i];
						Console.WriteLine("{0}: {1}", name, num);
					}
				}
			}
		}

		private Maid maid;

		private TMorph faceMorph;

		public bool enableFaceAnime = true;

		public Dictionary<string, FaceBlendModifier> BlendMod;

		public Dictionary<string, FaceBlendSetPattern> BlendSet;

		public Dictionary<string, float[]> VMDFaceName = new Dictionary<string, float[]>();

		public Dictionary<string, Transform> VMDFaceBlendValues = new Dictionary<string, Transform>();

		public HashSet<string> FacesToCheck;

		private bool loaded;

		public bool mabataki
		{
			get
			{
				return maid.boMabataki;
			}
			set
			{
				maid.boMabataki = value;
			}
		}

		public bool AnimeEnabled
		{
			get
			{
				return maid.boFaceAnime;
			}
			set
			{
				maid.boFaceAnime = value;
			}
		}

		public void Init(Maid maid)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			this.maid = maid;
			this.StartCoroutine(LoadDicDelayedCo());
		}

		private IEnumerator LoadDicDelayedCo()
		{
			while (true)
			{
				while (maid.IsBusy|| !maid.body0.isLoadedBody)
				{
					yield return (object)new WaitForEndOfFrame();
				}
				faceMorph = ((maid.body0).Face).morph;
				if (faceMorph != null)
				{
					try
					{
						InitDic();
						InitFaceBlendDic();
						InitDummyGo();
						loaded = true;
						yield break;
					}
					catch (Exception value)
					{
						Console.WriteLine(value);
					}
				}
			}
		}

		private void InitFaceBlendDic()
		{
			FacesToCheck = new HashSet<string>();
			AddVMDFaceFromSettings("Face0", "真面目,mayuv,1");
			AddVMDFaceFromSettings("Face1", "困る,mayuha,0.53,mayuvhalf,0.47");
			AddVMDFaceFromSettings("Face2", "にこり,mayuha,0.25,mayuup,0.35");
			AddVMDFaceFromSettings("Face3", "怒り,mayuup,0.22,mayuv,0.95");
			AddVMDFaceFromSettings("Face4", "上,mayuup,1");
			AddVMDFaceFromSettings("Face5", "まばたき,eyeclose,1");
			AddVMDFaceFromSettings("Face6", "笑顔,eyeclose2,1");
			AddVMDFaceFromSettings("Face7", "ウィンク,eyeclose2,0.06,eyeclose6,0.93");
			AddVMDFaceFromSettings("Faec8", "ウィンク２,eyeclose2,0.06,eyeclose5,0.93");
			AddVMDFaceFromSettings("Face9", "びっくり,eyebig,0.96,hitomih,0.28,hitomis,0.28");
			AddVMDFaceFromSettings("Face10", "じと目,eyeclose,0.19,eyeclose3,0.61,hitomih,0.35");
			AddVMDFaceFromSettings("Face11", "あ,moutha,1");
			AddVMDFaceFromSettings("Face12", "い,mouthi,1");
			AddVMDFaceFromSettings("Face13", "う,mouthc,1");
			AddVMDFaceFromSettings("Face14", "にやり,mouthup,1");
			string stringValue = Settings.Instance.GetStringValue("CustomFaceCount", null, true);
			if (stringValue == null)
			{
				Settings.Instance.SetStringValue("CustomFaceCount", "0");
				Settings.Instance.SetStringValue("CustomFace0", "");
			}
			try
			{
				int num = int.Parse(stringValue);
				for (int i = 0; i < num; i++)
				{
					AddVMDFaceFromSettings(@"CustomFace" + i, null);
				}
			}
			catch (Exception)
			{
				Console.WriteLine("Failed to load Custom Face info.");
			}
		}

		public void AddVMDFaceFromSettings(string key, string defaultValue)
		{
			string stringValue = Settings.Instance.GetStringValue(key, defaultValue, (defaultValue != null ? true : false));
			if (!string.IsNullOrEmpty(stringValue))
			{
				try
				{
					string[] array = stringValue.Split(',');
					string vmdFaceName = array[0].Trim();
					Dictionary<string, float> dictionary = new Dictionary<string, float>();
					for (int i = 1; i < array.Length; i += 2)
					{
						string key2 = array[i].Trim();
						float num2 = dictionary[key2] = float.Parse(array[i + 1].Trim());
					}
					AddVMDFace(vmdFaceName, dictionary);
				}
				catch (Exception value)
				{
					Console.WriteLine("Failed to parse Face info: {0}", stringValue);
					Console.WriteLine(value);
				}
			}
		}

		private void AddVMDFace(string vmdFaceName, Dictionary<string, float> valueMap)
		{
			float[] array = new float[faceMorph.BlendDatas.Count];
			foreach (string key in valueMap.Keys)
			{
				float num = valueMap[key];
				bool flag = false;
				for (int i = 0; i < faceMorph.BlendDatas.Count; i++)
				{
					BlendData val = faceMorph.BlendDatas[i];
					if (val != null && val.name == key)
					{
						array[i] = num;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					Console.WriteLine("BlendName {0} not found for {1}", key, vmdFaceName);
				}
				else
				{
					Console.WriteLine("BlendName {0} set for {1} successfully", key, vmdFaceName);
				}
			}
			VMDFaceName[vmdFaceName] = array;
			FacesToCheck.Add(vmdFaceName);
		}

		private void InitDummyGo()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Expected O, but got Unknown
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Expected O, but got Unknown
			foreach (string key in VMDFaceName.Keys)
			{
				GameObject val = new GameObject(key);
				val.transform.parent = this.transform;
				val.transform.localPosition = Vector3.zero;
				val.transform.localRotation = Quaternion.identity;
				VMDFaceBlendValues[key] = val.transform;
			}
		}

		private void InitDic()
		{
			BlendMod = new Dictionary<string, FaceBlendModifier>();
			for (int i = 0; i < faceMorph.MorphCount; i++)
			{
				BlendData val = faceMorph.BlendDatas[i];
				if (val != null)
				{
					FaceBlendModifier faceBlendModifier = new FaceBlendModifier();
					faceBlendModifier.controller = this;
					faceBlendModifier.index = i;
					faceBlendModifier.name = val.name;
					BlendMod[val.name] = faceBlendModifier;
				}
			}
			BlendSet = new Dictionary<string, FaceBlendSetPattern>();
			foreach (string key in faceMorph.dicBlendSet.Keys)
			{
				FaceBlendSetPattern faceBlendSetPattern = new FaceBlendSetPattern();
				faceBlendSetPattern.controller = this;
				faceBlendSetPattern.face = faceMorph;
				faceBlendSetPattern.faceName = key;
				BlendSet[faceBlendSetPattern.faceName] = faceBlendSetPattern;
			}
		}

		private unsafe void LateUpdate()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			if (loaded)
			{
				try
				{
					if (maid != null && maid.body0.isLoadedBody)
					{
						if (faceMorph == null)
						{
							faceMorph = ((maid.body0).Face).morph;
						}
						if (enableFaceAnime)
						{
							UpdateFace();
						}
					}
					else
					{
						faceMorph = null;
					}
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
				}
			}
		}

		public unsafe void UpdateFace()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			faceMorph.ClearBlendValues();
			foreach (string item in FacesToCheck)
			{
				float z = (VMDFaceBlendValues[item].localPosition).z;
				if (z > 0f)
				{
					float[] array = VMDFaceName[item];
					for (int i = 0; i < faceMorph.MorphCount; i++)
					{
						float num = array[i] * z;
						faceMorph.SetBlendValues(i, faceMorph.GetBlendValues(i) + num);
						if (faceMorph.GetBlendValues(i) > 1f)
						{
							faceMorph.SetBlendValues(i, 1f);
						}
					}
				}
			}
			faceMorph.FixBlendValues();
		}

		public void DumpMorphNames()
		{
			for (int i = 0; i < faceMorph.MorphCount; i++)
			{
				BlendData val = faceMorph.BlendDatas[i];
				if (val != null)
				{
					Console.WriteLine(val.name);
				}
			}
		}

		public unsafe float GetValue(string vmdFaceName)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			Transform value = null;
			VMDFaceBlendValues.TryGetValue(vmdFaceName, out value);
			if (value == null)
			{
				return 0f;
			}
			return (value.localPosition).z;
		}

		public void SetValue(string vmdFaceName, float value)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			VMDFaceBlendValues[vmdFaceName].localPosition = new Vector3(0f, 0f, value);
		}

		public void ResetFaceValues()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			foreach (string key in VMDFaceBlendValues.Keys)
			{
				VMDFaceBlendValues[key].localPosition = Vector3.zero;
			}
		}

		private void OnDestroy()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			foreach (Transform value in VMDFaceBlendValues.Values)
			{
				UnityEngine.Object.Destroy(value.gameObject);
			}
		}

		//public CM3D2FaceAnimeController()
		//	: this()
		//{
		//}
	}
}
