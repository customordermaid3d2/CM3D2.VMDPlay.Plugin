using System;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	public class BoneAdjustment
	{
		public string boneName;

		public string spec;

		private string[] xyz;

		private float[] sign;

		public Quaternion rotAdjustment = Quaternion.identity;

		private Vector3 rotAdjustmentVec = Vector3.zero;

		public bool rotAxisAdjustment;

		private Vector3 rotAxisAdjustmentVec = Vector3.zero;

		public Vector3 axisX = Vector3.right;

		public Vector3 axisY = Vector3.up;

		public Vector3 axisZ = Vector3.forward;

		public float rotationScale = 1f;

		public float RotX
		{
			get
			{
				return rotAdjustmentVec.x;
			}
			set
			{
				Vector3 val = rotAdjustmentVec;
				val.x = value;
				SetRotAdjustment(val);
			}
		}

		public float RotY
		{
			get
			{
				return rotAdjustmentVec.y;
			}
			set
			{
				Vector3 val = rotAdjustmentVec;
				val.y = value;
				SetRotAdjustment(val);
			}
		}

		public float RotZ
		{
			get
			{
				return rotAdjustmentVec.z;
			}
			set
			{
				Vector3 val = rotAdjustmentVec;
				val.z = value;
				SetRotAdjustment(val);
			}
		}

		public static BoneAdjustment Init(string boneName, string axisSpec, Vector3 rotAdjustment, bool rotAxisAdjustment)
		{
			BoneAdjustment boneAdjustment = new BoneAdjustment();
			boneAdjustment.boneName = boneName;
			boneAdjustment.SetSpec(axisSpec);
			boneAdjustment.SetRotAdjustment(rotAdjustment);
			boneAdjustment.rotAxisAdjustment = rotAxisAdjustment;
			return boneAdjustment;
		}

		public void SetRotAdjustment(Vector3 rot)
		{
			rotAdjustmentVec = rot;
			rotAdjustment = Quaternion.Euler(rot);
		}

		public void SetAxisAdjustment(Vector3 rotV)
		{
			rotAxisAdjustmentVec = rotV;
			Quaternion val = Quaternion.Euler(rotV);
			axisX = val * Vector3.right;
			axisY = val * Vector3.up;
			axisZ = val * Vector3.forward;
			if (rotV != Vector3.zero)
			{
				rotAxisAdjustment = true;
			}
			else
			{
				rotAxisAdjustment = false;
			}
		}

		public void SetSpec(string spec)
		{
			string[] array = spec.Split(',');
			float[] array2 = new float[3]
			{
				1f,
				1f,
				1f
			};
			for (int i = 0; i < array2.Length; i++)
			{
				array[i] = array[i].Trim();
				if (array[i].StartsWith("-"))
				{
					array[i] = array[i].Substring(1).Trim();
					array2[i] = -1f;
				}
			}
			this.spec = spec;
			xyz = array;
			sign = array2;
		}

		public unsafe Vector3 adjustAxis(Vector3 v)
		{
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			switch (xyz[0])
			{
			case "x":
				num = v.x * sign[0];
				break;
			case "y":
				num = v.y * sign[0];
				break;
			case "z":
				num = v.z * sign[0];
				break;
			default:
				throw new Exception("Unexpected x: " + xyz[0]);
			}
			switch (xyz[1])
			{
			case "x":
				num2 = v.x * sign[1];
				break;
			case "y":
				num2 = v.y * sign[1];
				break;
			case "z":
				num2 = v.z * sign[1];
				break;
			default:
				throw new Exception("Unexpected x: " + xyz[2]);
			}
			switch (xyz[2])
			{
			case "x":
				num3 = v.x * sign[2];
				break;
			case "y":
				num3 = v.y * sign[2];
				break;
			case "z":
				num3 = v.z * sign[2];
				break;
			default:
				throw new Exception("Unexpected x: " + xyz[2]);
			}
			return new Vector3(num, num2, num3);
		}

		public unsafe Quaternion GetAdjustedRotation(Quaternion baseRot)
		{
			Vector3 eulerAngles = baseRot.eulerAngles;
			eulerAngles = adjustAxis(eulerAngles);
			Quaternion val = (!rotAxisAdjustment) ? (Quaternion.Euler(eulerAngles) * rotAdjustment) : (Quaternion.AngleAxis((eulerAngles).y, axisY) * Quaternion.AngleAxis((eulerAngles).x, axisX) * Quaternion.AngleAxis((eulerAngles).z, axisZ) * rotAdjustment);
			if (rotationScale != 1f)
			{
				val = Quaternion.Slerp(Quaternion.identity, val, rotationScale);
			}
			return val;
		}

		public unsafe string GetAdjustedRotationV(float x, float y, float z)
		{
			Quaternion adjustedRotation = GetAdjustedRotation(Quaternion.Euler(x, y, z));
			Vector3 eulerAngles = adjustedRotation.eulerAngles;
			return $"({(eulerAngles).x}, {(eulerAngles).y}, {(eulerAngles).z})";
		}
	}
}
