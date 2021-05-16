using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace MMD.VMD
{
	public class VMDFormat
	{
		public class Header : Format
		{
			public string vmd_header;

			public string vmd_model_name;

			public Header(BinaryReader bin)
			{
				vmd_header = ConvertByteToString(bin.ReadBytes(30));
				vmd_model_name = ConvertByteToString(bin.ReadBytes(20));
			}
		}

		public class MotionList : Format
		{
			public uint motion_count;

			public Dictionary<string, List<Motion>> motion;

			public MotionList(BinaryReader bin)
			{
				motion_count = bin.ReadUInt32();
				motion = new Dictionary<string, List<Motion>>();
				Motion[] array = new Motion[motion_count];
				for (int i = 0; i < motion_count; i++)
				{
					array[i] = new Motion(bin);
				}
				Array.Sort(array);
				for (int j = 0; j < motion_count; j++)
				{
					try
					{
						motion.Add(array[j].bone_name, new List<Motion>());
					}
					catch
					{
					}
				}
				for (int k = 0; k < motion_count; k++)
				{
					motion[array[k].bone_name].Add(array[k]);
				}
			}
		}

		public class Motion : Format
		{
			public string bone_name;

			public uint flame_no;

			public Vector3 location;

			public Quaternion rotation;

			public byte[] interpolation;

			public Motion(BinaryReader bin)
			{
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				//IL_003b: Unknown result type (might be due to invalid IL or missing references)
				bone_name = ConvertByteToString(bin.ReadBytes(15));
				flame_no = bin.ReadUInt32();
				location = ReadSinglesToVector3(bin);
				rotation = ReadSinglesToQuaternion(bin);
				interpolation = bin.ReadBytes(64);
				count = (int)flame_no;
			}

			public byte GetInterpolation(int i, int j, int k)
			{
				return interpolation[i * 16 + j * 4 + k];
			}

			public void SetInterpolation(byte val, int i, int j, int k)
			{
				interpolation[i * 16 + j * 4 + k] = val;
			}
		}

		public class SkinList : Format
		{
			public uint skin_count;

			public Dictionary<string, List<SkinData>> skin;

			public SkinList(BinaryReader bin)
			{
				skin_count = bin.ReadUInt32();
				skin = new Dictionary<string, List<SkinData>>();
				SkinData[] array = new SkinData[skin_count];
				for (int i = 0; i < skin_count; i++)
				{
					array[i] = new SkinData(bin);
				}
				Array.Sort(array);
				for (int j = 0; j < skin_count; j++)
				{
					try
					{
						skin.Add(array[j].skin_name, new List<SkinData>());
					}
					catch
					{
					}
				}
				for (int k = 0; k < skin_count; k++)
				{
					skin[array[k].skin_name].Add(array[k]);
				}
			}
		}

		public class SkinData : Format
		{
			public string skin_name;

			public uint flame_no;

			public float weight;

			public SkinData(BinaryReader bin)
			{
				skin_name = ConvertByteToString(bin.ReadBytes(15));
				flame_no = bin.ReadUInt32();
				weight = bin.ReadSingle();
				count = (int)flame_no;
			}
		}

		public class CameraList : Format
		{
			public uint camera_count;

			public CameraData[] camera;

			public CameraList(BinaryReader bin)
			{
				camera_count = bin.ReadUInt32();
				camera = new CameraData[camera_count];
				for (int i = 0; i < camera_count; i++)
				{
					camera[i] = new CameraData(bin);
				}
				Array.Sort(camera);
			}
		}

		public class CameraData : Format
		{
			public uint flame_no;

			public float length;

			public Vector3 location;

			public Vector3 rotation;

			public byte[] interpolation;

			public uint viewing_angle;

			public byte perspective;

			public CameraData(BinaryReader bin)
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0033: Unknown result type (might be due to invalid IL or missing references)
				flame_no = bin.ReadUInt32();
				length = bin.ReadSingle();
				location = ReadSinglesToVector3(bin);
				rotation = ReadSinglesToVector3(bin);
				interpolation = bin.ReadBytes(24);
				viewing_angle = bin.ReadUInt32();
				perspective = bin.ReadByte();
				count = (int)flame_no;
			}

			public byte GetInterpolation(int i, int j)
			{
				return interpolation[i * 6 + j];
			}

			public void SetInterpolation(byte val, int i, int j)
			{
				interpolation[i * 6 + j] = val;
			}
		}

		public class LightList : Format
		{
			public uint light_count;

			public LightData[] light;

			public LightList(BinaryReader bin)
			{
				light_count = bin.ReadUInt32();
				light = new LightData[light_count];
				for (int i = 0; i < light_count; i++)
				{
					light[i] = new LightData(bin);
				}
				Array.Sort(light);
			}
		}

		public class LightData : Format
		{
			public uint flame_no;

			public Color rgb;

			public Vector3 location;

			public LightData(BinaryReader bin)
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				flame_no = bin.ReadUInt32();
				rgb = ReadSinglesToColor(bin, 1f);
				location = ReadSinglesToVector3(bin);
				count = (int)flame_no;
			}
		}

		public class SelfShadowList : Format
		{
			public uint self_shadow_count;

			public SelfShadowData[] self_shadow;

			public SelfShadowList(BinaryReader bin)
			{
				self_shadow_count = bin.ReadUInt32();
				self_shadow = new SelfShadowData[self_shadow_count];
				for (int i = 0; i < self_shadow_count; i++)
				{
					self_shadow[i] = new SelfShadowData(bin);
				}
				Array.Sort(self_shadow);
			}
		}

		public class SelfShadowData : Format
		{
			public uint flame_no;

			public byte mode;

			public float distance;

			public SelfShadowData(BinaryReader bin)
			{
				flame_no = bin.ReadUInt32();
				mode = bin.ReadByte();
				distance = bin.ReadSingle();
				count = (int)flame_no;
			}
		}

		public string name;

		public string path;

		public string folder;

		public string clip_name;

		public GameObject pmd;

		public Header header;

		public MotionList motion_list;

		public SkinList skin_list;

		public LightList light_list;

		public CameraList camera_list;

		public SelfShadowList self_shadow_list;

		private int read_count;

		private void EntryPathes(string path)
		{
			this.path = path;
			string[] array = path.Split('/');
			name = array[array.Length - 1];
			name = name.Split('.')[0];
			folder = array[0];
			for (int i = 1; i < array.Length - 1; i++)
			{
				folder = folder + "/" + array[i];
			}
		}

		public VMDFormat(BinaryReader bin, string path, string clip_name)
		{
			try
			{
				this.clip_name = clip_name;
				header = new Header(bin);
				read_count++;
				motion_list = new MotionList(bin);
				read_count++;
				skin_list = new SkinList(bin);
				read_count++;
				camera_list = new CameraList(bin);
				read_count++;
				light_list = new LightList(bin);
				read_count++;
				self_shadow_list = new SelfShadowList(bin);
				read_count++;
			}
			catch (EndOfStreamException ex)
			{
				Debug.Log((object)ex.Message);
				if (read_count <= 0)
				{
					header = null;
				}
				if (read_count <= 1 || motion_list.motion_count == 0)
				{
					motion_list = null;
				}
				if (read_count <= 2 || skin_list.skin_count == 0)
				{
					skin_list = null;
				}
				if (read_count <= 3 || camera_list.camera_count == 0)
				{
					camera_list = null;
				}
				if (read_count <= 4 || light_list.light_count == 0)
				{
					light_list = null;
				}
				if (read_count <= 5 || self_shadow_list.self_shadow_count == 0)
				{
					self_shadow_list = null;
				}
			}
		}
	}
}
