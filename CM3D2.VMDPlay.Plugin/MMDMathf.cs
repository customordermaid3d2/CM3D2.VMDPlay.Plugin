using System;
using UnityEngine;

public class MMDMathf
{
	public static Matrix4x4 CreateRotationXMatrix(float rad)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Matrix4x4 identity = Matrix4x4.identity;
		float num = Mathf.Cos(rad);
		float num2 = Mathf.Sin(rad);
		identity.m11 = num;
		identity.m12 = 0f - num2;
		identity.m21 = num2;
		identity.m22 = num;
		return identity;
	}

	public static Matrix4x4 CreateRotationYMatrix(float rad)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Matrix4x4 identity = Matrix4x4.identity;
		float num = Mathf.Cos(rad);
		float num2 = Mathf.Sin(rad);
		identity.m00 = num;
		identity.m02 = num2;
		identity.m20 = 0f - num2;
		identity.m22 = num;
		return identity;
	}

	public static Matrix4x4 CreateRotationZMatrix(float rad)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Matrix4x4 identity = Matrix4x4.identity;
		float num = Mathf.Cos(rad);
		float num2 = Mathf.Sin(rad);
		identity.m01 = num;
		identity.m02 = 0f - num2;
		identity.m11 = num2;
		identity.m12 = num;
		return identity;
	}

	public static Matrix4x4 CreateRotationMatrixFromRollPitchYaw(float r, float p, float y)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		Matrix4x4 identity = Matrix4x4.identity;
		float num = Mathf.Cos(r);
		float num2 = Mathf.Sin(r);
		float num3 = Mathf.Cos(p);
		float num4 = Mathf.Sin(p);
		float num5 = Mathf.Cos(y);
		float num6 = Mathf.Sin(y);
		identity.m00 = num * num3;
		identity.m01 = num * num4 * num6 - num2 * num5;
		identity.m02 = num * num4 * num5 + num2 * num6;
		identity.m10 = num2 * num3;
		identity.m11 = num2 * num4 * num6 + num * num5;
		identity.m12 = num2 * num4 * num5 - num * num6;
		identity.m20 = 0f - num4;
		identity.m21 = num3 * num6;
		identity.m22 = num3 * num5;
		return identity;
	}

	public unsafe static Vector3 CreatePositionFromMatrix(Matrix4x4 m)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3((m).m30, (m).m31, (m).m33);
	}

	public unsafe static Quaternion CreateQuaternionFromRotationMatrix(Matrix4x4 m)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		Quaternion val = default(Quaternion);
		val.x = ((m).m00 + (m).m11 + (m).m22 + 1f) * 0.25f;
		val.y = ((m).m00 - (m).m11 - (m).m22 + 1f) * 0.25f;
		val.z = (0f - (m).m00 + (m).m11 - (m).m22 + 1f) * 0.25f;
		val.w = (0f - (m).m00 - (m).m11 + (m).m22 + 1f) * 0.25f;
		if ((val).x < 0f)
		{
			val.x = 0f;
		}
		if ((val).y < 0f)
		{
			val.y = 0f;
		}
		if ((val).z < 0f)
		{
			val.z = 0f;
		}
		if ((val).w < 0f)
		{
			val.w = 0f;
		}
		val.x = Mathf.Sqrt((val).x);
		val.y = Mathf.Sqrt((val).y);
		val.z = Mathf.Sqrt((val).z);
		val.w = Mathf.Sqrt((val).w);
		if ((val).x >= (val).y && (val).x >= (val).z && (val).x >= (val).w)
		{
			val.x *= 1f;
			val.y *= Sign((m).m22 - (m).m13);
			val.z *= Sign((m).m03 - (m).m21);
			val.w *= Sign((m).m11 - (m).m02);
		}
		else if ((val).y >= (val).x && (val).y >= (val).z && (val).y >= (val).w)
		{
			val.x *= Sign((m).m22 - (m).m13);
			val.y *= 1f;
			val.z *= Sign((m).m11 + (m).m02);
			val.w *= Sign((m).m03 + (m).m21);
		}
		else if ((val).z >= (val).x && (val).z >= (val).y && (val).z >= (val).w)
		{
			val.x *= Sign((m).m03 - (m).m21);
			val.y *= Sign((m).m11 + (m).m02);
			val.z *= 1f;
			val.w *= Sign((m).m22 + (m).m13);
		}
		else if ((val).w >= (val).x && (val).w >= (val).y && (val).w >= (val).z)
		{
			val.x *= Sign((m).m11 - (m).m02);
			val.y *= Sign((m).m21 + (m).m03);
			val.z *= Sign((m).m22 + (m).m13);
			val.w *= 1f;
		}
		float num = 1f / Norm((val).x, (val).y, (val).z, (val).w);
		val.x *= num;
		val.y *= num;
		val.z *= num;
		val.w *= num;
		return val;
	}

	private static float Sign(float x)
	{
		if (!(x >= 0f))
		{
			return -1f;
		}
		return 1f;
	}

	private static float Norm(float a, float b, float c, float d)
	{
		return Mathf.Sqrt(a * a + b * b + c * c + d * d);
	}
}
