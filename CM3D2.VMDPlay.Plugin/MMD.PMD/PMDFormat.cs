using System.IO;
using UnityEngine;

namespace MMD.PMD
{
	public class PMDFormat : Format
	{
		public class Header : Format
		{
			public byte[] magic;

			public float version;

			public string model_name;

			public string comment;

			public Header(BinaryReader bin)
			{
				magic = bin.ReadBytes(3);
				version = bin.ReadSingle();
				model_name = ConvertByteToString(bin.ReadBytes(20));
				comment = ConvertByteToString(bin.ReadBytes(256));
			}
		}

		public class VertexList : Format
		{
			public uint vert_count;

			public Vertex[] vertex;

			public VertexList(BinaryReader bin)
			{
				vert_count = bin.ReadUInt32();
				vertex = new Vertex[vert_count];
				for (int i = 0; i < vert_count; i++)
				{
					vertex[i] = new Vertex(bin);
				}
			}
		}

		public class Vertex : Format
		{
			public Vector3 pos;

			public Vector3 normal_vec;

			public Vector2 uv;

			public ushort[] bone_num;

			public byte bone_weight;

			public byte edge_flag;

			public Vertex(BinaryReader bin)
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_0028: Unknown result type (might be due to invalid IL or missing references)
				pos = ReadSinglesToVector3(bin);
				normal_vec = ReadSinglesToVector3(bin);
				uv = ReadSinglesToVector2(bin);
				bone_num = ReadUInt16s(bin, 2u);
				bone_weight = bin.ReadByte();
				edge_flag = bin.ReadByte();
			}
		}

		public class FaceVertexList : Format
		{
			public uint face_vert_count;

			public ushort[] face_vert_index;

			public FaceVertexList(BinaryReader bin)
			{
				face_vert_count = bin.ReadUInt32();
				face_vert_index = ReadUInt16s(bin, face_vert_count);
			}
		}

		public class MaterialList : Format
		{
			public uint material_count;

			public Material[] material;

			public MaterialList(BinaryReader bin)
			{
				material_count = bin.ReadUInt32();
				material = new Material[material_count];
				for (int i = 0; i < material_count; i++)
				{
					material[i] = new Material(bin);
				}
			}
		}

		public class Material : Format
		{
			public Color diffuse_color;

			public float alpha;

			public float specularity;

			public Color specular_color;

			public Color mirror_color;

			public byte toon_index;

			public byte edge_flag;

			public uint face_vert_count;

			public string texture_file_name;

			public string sphere_map_name;

			private string CutTheUnknownDotSlash(string str)
			{
				string str2 = "";
				string[] array = str.Split('/');
				if (array[0] == ".")
				{
					str2 += array[1];
					for (int i = 2; i < array.Length; i++)
					{
						str2 = str2 + "/" + array[i];
					}
				}
				else
				{
					str2 = str;
				}
				return str2;
			}

			public Material(BinaryReader bin)
			{
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_003d: Unknown result type (might be due to invalid IL or missing references)
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				diffuse_color = ReadSinglesToColor(bin, 1f);
				alpha = bin.ReadSingle();
				specularity = bin.ReadSingle();
				specular_color = ReadSinglesToColor(bin, 1f);
				mirror_color = ReadSinglesToColor(bin, 1f);
				toon_index = bin.ReadByte();
				edge_flag = bin.ReadByte();
				face_vert_count = bin.ReadUInt32();
				string text = ConvertByteToString(bin.ReadBytes(20));
				if (!string.IsNullOrEmpty(text.Trim()))
				{
					string[] array = text.Trim().Split('*');
					foreach (string text2 in array)
					{
						string str = "";
						string extension = Path.GetExtension(text2);
						if (extension == ".sph" || extension == ".spa")
						{
							sphere_map_name = text2;
						}
						else
						{
							if (text2.Split('/')[0] == ".")
							{
								string[] array2 = text2.Split('/');
								for (int j = 1; j < array2.Length - 1; j++)
								{
									str = str + array2[j] + "/";
								}
								str += array2[array2.Length - 1];
							}
							else
							{
								str = text2;
							}
							texture_file_name = str;
						}
					}
				}
				else
				{
					sphere_map_name = "";
					texture_file_name = "";
				}
				if (string.IsNullOrEmpty(texture_file_name))
				{
					texture_file_name = "";
				}
			}
		}

		public class BoneList : Format
		{
			public ushort bone_count;

			public Bone[] bone;

			public BoneList(BinaryReader bin)
			{
				bone_count = bin.ReadUInt16();
				bone = new Bone[bone_count];
				for (int i = 0; i < bone_count; i++)
				{
					bone[i] = new Bone(bin);
				}
			}
		}

		public class Bone : Format
		{
			public string bone_name;

			public ushort parent_bone_index;

			public ushort tail_pos_bone_index;

			public byte bone_type;

			public ushort ik_parent_bone_index;

			public Vector3 bone_head_pos;

			public Bone(BinaryReader bin)
			{
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0052: Unknown result type (might be due to invalid IL or missing references)
				bone_name = ConvertByteToString(bin.ReadBytes(20));
				parent_bone_index = bin.ReadUInt16();
				tail_pos_bone_index = bin.ReadUInt16();
				bone_type = bin.ReadByte();
				ik_parent_bone_index = bin.ReadUInt16();
				bone_head_pos = ReadSinglesToVector3(bin);
			}
		}

		public class IKList : Format
		{
			public ushort ik_data_count;

			public IK[] ik_data;

			public IKList(BinaryReader bin)
			{
				ik_data_count = bin.ReadUInt16();
				ik_data = new IK[ik_data_count];
				for (int i = 0; i < ik_data_count; i++)
				{
					ik_data[i] = new IK(bin);
				}
			}
		}

		public class IK : Format
		{
			public ushort ik_bone_index;

			public ushort ik_target_bone_index;

			public byte ik_chain_length;

			public ushort iterations;

			public float control_weight;

			public ushort[] ik_child_bone_index;

			public IK(BinaryReader bin)
			{
				ik_bone_index = bin.ReadUInt16();
				ik_target_bone_index = bin.ReadUInt16();
				ik_chain_length = bin.ReadByte();
				iterations = bin.ReadUInt16();
				control_weight = bin.ReadSingle();
				ik_child_bone_index = ReadUInt16s(bin, ik_chain_length);
			}
		}

		public class SkinList : Format
		{
			public ushort skin_count;

			public SkinData[] skin_data;

			public SkinList(BinaryReader bin)
			{
				skin_count = bin.ReadUInt16();
				skin_data = new SkinData[skin_count];
				for (int i = 0; i < skin_count; i++)
				{
					skin_data[i] = new SkinData(bin);
				}
			}
		}

		public class SkinData : Format
		{
			public string skin_name;

			public uint skin_vert_count;

			public byte skin_type;

			public SkinVertexData[] skin_vert_data;

			public SkinData(BinaryReader bin)
			{
				skin_name = ConvertByteToString(bin.ReadBytes(20));
				skin_vert_count = bin.ReadUInt32();
				skin_type = bin.ReadByte();
				skin_vert_data = new SkinVertexData[skin_vert_count];
				for (int i = 0; i < skin_vert_count; i++)
				{
					skin_vert_data[i] = new SkinVertexData(bin);
				}
			}
		}

		public class SkinVertexData : Format
		{
			public uint skin_vert_index;

			public Vector3 skin_vert_pos;

			public SkinVertexData(BinaryReader bin)
			{
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				skin_vert_index = bin.ReadUInt32();
				skin_vert_pos = ReadSinglesToVector3(bin);
			}
		}

		public class SkinNameList : Format
		{
			public byte skin_disp_count;

			public ushort[] skin_index;

			public SkinNameList(BinaryReader bin)
			{
				skin_disp_count = bin.ReadByte();
				skin_index = ReadUInt16s(bin, skin_disp_count);
			}
		}

		public class BoneNameList : Format
		{
			public byte bone_disp_name_count;

			public string[] disp_name;

			public BoneNameList(BinaryReader bin)
			{
				bone_disp_name_count = bin.ReadByte();
				disp_name = new string[bone_disp_name_count];
				for (int i = 0; i < bone_disp_name_count; i++)
				{
					disp_name[i] = ConvertByteToString(bin.ReadBytes(50));
				}
			}
		}

		public class BoneDisplayList : Format
		{
			public uint bone_disp_count;

			public BoneDisplay[] bone_disp;

			public BoneDisplayList(BinaryReader bin)
			{
				bone_disp_count = bin.ReadUInt32();
				bone_disp = new BoneDisplay[bone_disp_count];
				for (int i = 0; i < bone_disp_count; i++)
				{
					bone_disp[i] = new BoneDisplay(bin);
				}
			}
		}

		public class BoneDisplay : Format
		{
			public ushort bone_index;

			public byte bone_disp_frame_index;

			public BoneDisplay(BinaryReader bin)
			{
				bone_index = bin.ReadUInt16();
				bone_disp_frame_index = bin.ReadByte();
			}
		}

		public class EnglishHeader : Format
		{
			public byte english_name_compatibility;

			public string model_name_eg;

			public string comment_eg;

			public EnglishHeader(BinaryReader bin)
			{
				english_name_compatibility = bin.ReadByte();
				model_name_eg = ConvertByteToString(bin.ReadBytes(20));
				comment_eg = ConvertByteToString(bin.ReadBytes(256));
			}
		}

		public class EnglishBoneNameList : Format
		{
			public string[] bone_name_eg;

			public EnglishBoneNameList(BinaryReader bin, int boneCount)
			{
				bone_name_eg = new string[boneCount];
				for (int i = 0; i < boneCount; i++)
				{
					bone_name_eg[i] = ConvertByteToString(bin.ReadBytes(20));
				}
			}
		}

		public class EnglishSkinNameList : Format
		{
			public string[] skin_name_eg;

			public EnglishSkinNameList(BinaryReader bin, int skinCount)
			{
				skin_name_eg = new string[skinCount];
				for (int i = 0; i < skinCount - 1; i++)
				{
					skin_name_eg[i] = ConvertByteToString(bin.ReadBytes(20));
				}
			}
		}

		public class EnglishBoneDisplayList : Format
		{
			public string[] disp_name_eg;

			public EnglishBoneDisplayList(BinaryReader bin, int boneDispNameCount)
			{
				disp_name_eg = new string[boneDispNameCount];
				for (int i = 0; i < boneDispNameCount; i++)
				{
					disp_name_eg[i] = ConvertByteToString(bin.ReadBytes(50));
				}
			}
		}

		public class ToonTextureList : Format
		{
			public string[] toon_texture_file;

			public ToonTextureList(BinaryReader bin)
			{
				toon_texture_file = new string[10];
				for (int i = 0; i < toon_texture_file.Length; i++)
				{
					toon_texture_file[i] = ConvertByteToString(bin.ReadBytes(100));
				}
			}
		}

		public class RigidbodyList : Format
		{
			public uint rigidbody_count;

			public Rigidbody[] rigidbody;

			public RigidbodyList(BinaryReader bin)
			{
				rigidbody_count = bin.ReadUInt32();
				rigidbody = new Rigidbody[rigidbody_count];
				for (int i = 0; i < rigidbody_count; i++)
				{
					rigidbody[i] = new Rigidbody(bin);
				}
			}
		}

		public class Rigidbody : Format
		{
			public string rigidbody_name;

			public int rigidbody_rel_bone_index;

			public byte rigidbody_group_index;

			public ushort rigidbody_group_target;

			public byte shape_type;

			public float shape_w;

			public float shape_h;

			public float shape_d;

			public Vector3 pos_pos;

			public Vector3 pos_rot;

			public float rigidbody_weight;

			public float rigidbody_pos_dim;

			public float rigidbody_rot_dim;

			public float rigidbody_recoil;

			public float rigidbody_friction;

			public byte rigidbody_type;

			public Rigidbody(BinaryReader bin)
			{
				//IL_0071: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_007e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				rigidbody_name = ConvertByteToString(bin.ReadBytes(20));
				rigidbody_rel_bone_index = bin.ReadUInt16();
				rigidbody_group_index = bin.ReadByte();
				rigidbody_group_target = bin.ReadUInt16();
				shape_type = bin.ReadByte();
				shape_w = bin.ReadSingle();
				shape_h = bin.ReadSingle();
				shape_d = bin.ReadSingle();
				pos_pos = ReadSinglesToVector3(bin);
				pos_rot = ReadSinglesToVector3(bin);
				rigidbody_weight = bin.ReadSingle();
				rigidbody_pos_dim = bin.ReadSingle();
				rigidbody_rot_dim = bin.ReadSingle();
				rigidbody_recoil = bin.ReadSingle();
				rigidbody_friction = bin.ReadSingle();
				rigidbody_type = bin.ReadByte();
			}
		}

		public class RigidbodyJointList : Format
		{
			public uint joint_count;

			public Joint[] joint;

			public RigidbodyJointList(BinaryReader bin)
			{
				joint_count = bin.ReadUInt32();
				joint = new Joint[joint_count];
				for (int i = 0; i < joint_count; i++)
				{
					joint[i] = new Joint(bin);
				}
			}
		}

		public class Joint : Format
		{
			public string joint_name;

			public uint joint_rigidbody_a;

			public uint joint_rigidbody_b;

			public Vector3 joint_pos;

			public Vector3 joint_rot;

			public Vector3 constrain_pos_1;

			public Vector3 constrain_pos_2;

			public Vector3 constrain_rot_1;

			public Vector3 constrain_rot_2;

			public Vector3 spring_pos;

			public Vector3 spring_rot;

			public Joint(BinaryReader bin)
			{
				//IL_0035: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0042: Unknown result type (might be due to invalid IL or missing references)
				//IL_0047: Unknown result type (might be due to invalid IL or missing references)
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_006e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0076: Unknown result type (might be due to invalid IL or missing references)
				//IL_007b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0083: Unknown result type (might be due to invalid IL or missing references)
				//IL_0088: Unknown result type (might be due to invalid IL or missing references)
				//IL_0090: Unknown result type (might be due to invalid IL or missing references)
				//IL_0095: Unknown result type (might be due to invalid IL or missing references)
				joint_name = ConvertByteToString(bin.ReadBytes(20));
				joint_rigidbody_a = bin.ReadUInt32();
				joint_rigidbody_b = bin.ReadUInt32();
				joint_pos = ReadSinglesToVector3(bin);
				joint_rot = ReadSinglesToVector3(bin);
				constrain_pos_1 = ReadSinglesToVector3(bin);
				constrain_pos_2 = ReadSinglesToVector3(bin);
				constrain_rot_1 = ReadSinglesToVector3(bin);
				constrain_rot_2 = ReadSinglesToVector3(bin);
				spring_pos = ReadSinglesToVector3(bin);
				spring_rot = ReadSinglesToVector3(bin);
			}
		}

		public string path;

		public string name;

		public string folder;

		public GameObject caller;

		public Header head;

		public VertexList vertex_list;

		public FaceVertexList face_vertex_list;

		public MaterialList material_list;

		public BoneList bone_list;

		public IKList ik_list;

		public SkinList skin_list;

		public SkinNameList skin_name_list;

		public BoneNameList bone_name_list;

		public BoneDisplayList bone_display_list;

		public EnglishHeader eg_head;

		public EnglishBoneNameList eg_bone_name_list;

		public EnglishSkinNameList eg_skin_name_list;

		public EnglishBoneDisplayList eg_bone_display_list;

		public ToonTextureList toon_texture_list;

		public RigidbodyList rigidbody_list;

		public RigidbodyJointList rigidbody_joint_list;

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

		public PMDFormat(BinaryReader bin, GameObject caller, string path)
		{
			EntryPathes(path);
			this.caller = caller;
			try
			{
				head = new Header(bin);
				vertex_list = new VertexList(bin);
				face_vertex_list = new FaceVertexList(bin);
				material_list = new MaterialList(bin);
				bone_list = new BoneList(bin);
				ik_list = new IKList(bin);
				read_count++;
				skin_list = new SkinList(bin);
				read_count++;
				skin_name_list = new SkinNameList(bin);
				bone_name_list = new BoneNameList(bin);
				bone_display_list = new BoneDisplayList(bin);
				eg_head = new EnglishHeader(bin);
				eg_bone_name_list = new EnglishBoneNameList(bin, bone_list.bone_count);
				eg_skin_name_list = new EnglishSkinNameList(bin, skin_list.skin_count);
				eg_bone_display_list = new EnglishBoneDisplayList(bin, bone_name_list.bone_disp_name_count);
				toon_texture_list = new ToonTextureList(bin);
				rigidbody_list = new RigidbodyList(bin);
				rigidbody_joint_list = new RigidbodyJointList(bin);
			}
			catch
			{
				Debug.Log((object)"Don't read full format");
			}
		}
	}
}
