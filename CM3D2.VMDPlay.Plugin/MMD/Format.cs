using CM3D2.VMDPlay.Plugin;
using HexDump;
using System;
using System.IO;
using UnityEngine;

namespace MMD
{
	public class Format : IComparable
	{
		protected int count;

		protected string ConvertByteToString(byte[] bytes)
		{
			if (bytes[0] != 0)
			{
				int i;
				for (i = 0; i < bytes.Length && bytes[i] != 0; i++)
				{
				}
				byte[] array = new byte[i];
				for (int j = 0; j < i; j++)
				{
					array[j] = bytes[j];
				}
				try
				{
					return ToEncoding.ToUnicode(array);
				}
				catch (Exception)
				{
					byte[] array2 = new byte[array.Length - 1];
					for (int k = 0; k < array.Length - 1; k++)
					{
						array2[k] = array[k];
					}
					try
					{
						return ToEncoding.ToUnicode(array2);
					}
					catch (Exception ex)
					{
						Console.WriteLine("Failed to read buf: {0}", Utils.HexDump(array, 16));
						using (FileStream fileStream = File.Create("__debug.dat"))
						{
							fileStream.Write(array, 0, array.Length);
						}
						throw ex;
					}
				}
			}
			return "";
		}

		protected float[] ReadSingles(BinaryReader bin, uint count)
		{
			float[] array = new float[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = bin.ReadSingle();
			}
			return array;
		}

		protected Vector3 ReadSinglesToVector3(BinaryReader bin)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			float[] array = new float[3];
			for (int i = 0; i < 3; i++)
			{
				array[i] = bin.ReadSingle();
			}
			return new Vector3(array[0], array[1], array[2]);
		}

		protected Vector2 ReadSinglesToVector2(BinaryReader bin)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			float[] array = new float[2];
			for (int i = 0; i < 2; i++)
			{
				array[i] = bin.ReadSingle();
			}
			return new Vector2(array[0], array[1]);
		}

		protected Color ReadSinglesToColor(BinaryReader bin)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			float[] array = new float[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = bin.ReadSingle();
			}
			return new Color(array[0], array[1], array[2], array[3]);
		}

		protected Color ReadSinglesToColor(BinaryReader bin, float fix_alpha)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			float[] array = new float[3];
			for (int i = 0; i < 3; i++)
			{
				array[i] = bin.ReadSingle();
			}
			return new Color(array[0], array[1], array[2], fix_alpha);
		}

		protected uint[] ReadUInt32s(BinaryReader bin, uint count)
		{
			uint[] array = new uint[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = bin.ReadUInt32();
			}
			return array;
		}

		protected ushort[] ReadUInt16s(BinaryReader bin, uint count)
		{
			ushort[] array = new ushort[count];
			for (uint num = 0u; num < count; num++)
			{
				array[num] = bin.ReadUInt16();
			}
			return array;
		}

		protected Quaternion ReadSinglesToQuaternion(BinaryReader bin)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			float[] array = new float[4];
			for (int i = 0; i < 4; i++)
			{
				array[i] = bin.ReadSingle();
			}
			return new Quaternion(array[0], array[1], array[2], array[3]);
		}

		public int CompareTo(object obj)
		{
			return count - ((Format)obj).count;
		}
	}
}
