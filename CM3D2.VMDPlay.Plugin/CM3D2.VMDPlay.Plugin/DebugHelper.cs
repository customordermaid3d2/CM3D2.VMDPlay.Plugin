using System;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	internal class DebugHelper : UnityEngine.MonoBehaviour
	{
		private static DebugHelper _instance;

		private Mesh _boneMarkerMesh;

		private Material _boneMarkerMat;

		public static bool fixEnabled = true;

		public static float weight = 0.3f;

		public static float axisX = 30f;

		public static float axisY = 0f;

		public static float axisZ = 0f;

		public static float MARKER_SIZE = 0.01f;

		private Texture2D _boneMarkerTex;

		public static DebugHelper Install(GameObject container)
		{
			if (_instance == null)
			{
				_instance = container.AddComponent<DebugHelper>();
			}
			return _instance;
		}

		public void CopyShader(GameObject go)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Expected O, but got Unknown
			Shader val = go.GetComponent<MeshRenderer>().material.shader;
			Console.WriteLine(val.name);
			SetMaterial(val);
		}

		private void SetMaterial(Shader shader)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			_boneMarkerMat = new Material(shader);
			if (_boneMarkerTex == null)
			{
				_boneMarkerTex = new Texture2D(1, 1, (TextureFormat)5, false);
				_boneMarkerTex.SetPixel(0, 0, new Color(0.8f, 0.8f, 0f, 0.5f));
				_boneMarkerTex.Apply();
			}
			_boneMarkerMat.mainTexture = _boneMarkerTex;
		}

		private void Start()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Expected O, but got Unknown
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Expected O, but got Unknown
			GameObject val = GameObject.CreatePrimitive(0);
			MeshFilter component = val.GetComponent<MeshFilter>();
			val.GetComponent<MeshRenderer>();
			_boneMarkerMesh = component.mesh;
			Vector3[] vertices = _boneMarkerMesh.vertices;
			Vector3[] array = (Vector3[])new Vector3[vertices.Length];
			for (int i = 0; i < vertices.Length; i++)
			{
				array[i] = vertices[i] * MARKER_SIZE;
			}
			_boneMarkerMesh.vertices = array;
			if (Shader.Find("Unlit/Transparent") != null)
			{
				SetMaterial(Shader.Find("Unlit/Transparent"));
			}
			UnityEngine.Object.DestroyImmediate(val);
		}

		private void Update()
		{
		}

		public void InstallBoneVisualizer(Transform t)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Expected O, but got Unknown
			if (!(t == null))
			{
				if (t.gameObject.GetComponent<Renderer>() == null)
				{
					t.gameObject.AddComponent<MeshFilter>().sharedMesh = _boneMarkerMesh;
					MeshRenderer obj = t.gameObject.AddComponent<MeshRenderer>();
					t.gameObject.layer = LayerMask.NameToLayer("AbsolutFront");
					obj.sharedMaterial = _boneMarkerMat;
				}
				for (int i = 0; i < t.childCount; i++)
				{
					Transform t2 = t.GetChild(i);
					InstallBoneVisualizer(t2);
				}
			}
		}

		public void ShowHideSub(bool show, Transform t)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			MeshRenderer component = t.gameObject.GetComponent<MeshRenderer>();
			if (component != null && component.sharedMaterial == _boneMarkerMat)
			{
				component.enabled = show;
			}
			for (int i = 0; i < t.childCount; i++)
			{
				ShowHideSub(show, t.GetChild(i));
			}
		}

		private Material CreateZTransShader()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Expected O, but got Unknown
			return new Material("Shader \"Custom/ColorZOrder\" {\r\nProperties {\r\n\t_Color (\"Main Color\", Color) = (1,1,1,1)\r\n\t_MainTex (\"Base (RGB) Trans (A)\", 2D) = \"white\" {}\r\n}\r\n\r\nSubShader {\r\n\tTags {\"Queue\"=\"Transparent+2\" \"IgnoreProjector\"=\"True\" \"RenderType\"=\"Transparent\"}\r\n\tLOD 200\r\n\r\nCGPROGRAM\r\n#pragma surface surf Lambert alpha\r\n\r\nsampler2D _MainTex;\r\nfixed4 _Color;\r\n\r\nstruct Input {\r\n\tfloat2 uv_MainTex;\r\n};\r\n\r\nvoid surf (Input IN, inout SurfaceOutput o) {\r\n\tfixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;\r\n\to.Albedo = c.rgb;\r\n\to.Alpha = c.a;\r\n}\r\nENDCG\r\n}\r\n\r\nFallback \"Transparent/Unlit\"\r\n}");
		}

		private void Clear()
		{
		}

		//public DebugHelper()
		//	: this()
		//{
		//}
	}
}
