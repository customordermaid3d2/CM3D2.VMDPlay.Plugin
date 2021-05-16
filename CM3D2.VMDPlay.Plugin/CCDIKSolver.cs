using System;
using UnityEngine;

public class CCDIKSolver : UnityEngine.MonoBehaviour
{
	public Transform target;

	public int iterations;

	public float controll_weight;

	public Transform[] chains;

	public bool drawRay;

	public float minDelta;

	public bool useLeg;

	public Quaternion[] lastFrameQ;

	public float lastFrameWeight = 0.9f;

	public void Solve()
	{
		if (useLeg)
		{
			SolveLeg();
		}
		else
		{
			SolveNormal();
		}
	}

	public unsafe void SolveLeg()
	{
		if (lastFrameQ != null)
		{
			for (int i = 0; i < chains.Length; i++)
			{
				Quaternion localRotation = Quaternion.Slerp(chains[i].localRotation, lastFrameQ[i], lastFrameWeight);
				chains[i].localRotation = localRotation;
			}
		}
		else
		{
			lastFrameQ = (Quaternion[])new Quaternion[chains.Length];
		}
		Vector3 val = this.transform.position - chains[1].position;
		float magnitude = val.magnitude;
		val = chains[0].position - chains[1].position;
		float magnitude2 = val.magnitude;
		val = target.position - chains[1].position;
		float magnitude3 = val.magnitude;
		float num = magnitude2 + magnitude3;
		float num2 = -0.5f;
		int j = 0;
		for (int num3 = iterations; j < num3; j++)
		{
			int k = 0;
			for (int num4 = chains.Length; k < num4; k++)
			{
				Transform val2 = chains[k];
				Vector3 position = val2.position;
				Vector3 val3 = target.position - position;
				Vector3 val4 = this.transform.position - position;
				if (drawRay)
				{
					Debug.DrawRay(position, val3, Color.green);
					Debug.DrawRay(position, val4, Color.red);
				}
				float sqrMagnitude = val3.sqrMagnitude;
				float sqrMagnitude2 = val4.sqrMagnitude;
				val3 = val3.normalized;
				val4 = val4.normalized;
				float num5 = Mathf.Acos(Vector3.Dot(val3, val4));
				val = Vector3.Cross(val3, val4);
				Vector3 normalized = val.normalized;
				if (float.IsNaN(num5) || num5 == 0f)
				{
					if (k == 1 && (chains[0].localEulerAngles).x == 0f && magnitude >= num)
					{
						StoreLastQ();
						return;
					}
					if (k != 1 || (chains[0].localEulerAngles).x != 0f || !(sqrMagnitude2 < sqrMagnitude))
					{
						continue;
					}
					normalized = new Vector3(1f, 0f, 0f);
					num5 = num2;
				}
				float num6 = 4f * controll_weight * (float)(k + 1);
				if (num5 > num6)
				{
					num5 = num6;
				}
				if (num5 < 0f - num6)
				{
					num5 = 0f - num6;
				}
				num5 *= 57.29578f;
				if (!float.IsNaN((normalized).x) && !float.IsNaN((normalized).y) && !float.IsNaN((normalized).z))
				{
					Quaternion val5 = Quaternion.AngleAxis(num5, normalized);
					val2.rotation = val5 * val2.rotation;
					limitter(val2);
					if (minDelta != 0f)
					{
						val = target.position - this.transform.position;
						if (val.sqrMagnitude < minDelta)
						{
							StoreLastQ();
							return;
						}
					}
				}
			}
		}
		StoreLastQ();
	}

	private void StoreLastQ()
	{
		for (int i = 0; i < chains.Length; i++)
		{
			lastFrameQ[i] = chains[i].localRotation;
		}
	}

	public unsafe void SolveNormal()
	{
		if (lastFrameQ != null)
		{
			for (int i = 0; i < chains.Length; i++)
			{
				Quaternion localRotation = Quaternion.Slerp(chains[i].localRotation, lastFrameQ[i], lastFrameWeight);
				chains[i].localRotation = localRotation;
			}
		}
		else
		{
			lastFrameQ = (Quaternion[])new Quaternion[chains.Length];
		}
		int j = 0;
		for (int num = iterations; j < num; j++)
		{
			int k = 0;
			for (int num2 = chains.Length; k < num2; k++)
			{
				Transform val = chains[k];
				Vector3 position = val.position;
				Vector3 val2 = target.position - position;
				Vector3 val3 = this.transform.position - position;
				if (drawRay)
				{
					Debug.DrawRay(position, val2, Color.green);
					Debug.DrawRay(position, val3, Color.red);
				}
				val2 = val2.normalized;
				val3 = val3.normalized;
				float num3 = Mathf.Acos(Vector3.Dot(val2, val3));
				if (!float.IsNaN(num3))
				{
					float num4 = 4f * controll_weight * (float)(k + 1);
					if (num3 > num4)
					{
						num3 = num4;
					}
					if (num3 < 0f - num4)
					{
						num3 = 0f - num4;
					}
					num3 *= 57.29578f;
					Vector3 val4 = Vector3.Cross(val2, val3);
					Vector3 normalized = val4.normalized;
					if (!float.IsNaN((normalized).x) && !float.IsNaN((normalized).y) && !float.IsNaN((normalized).z))
					{
						Quaternion val5 = Quaternion.AngleAxis(num3, normalized);
						val.rotation = val5 * val.rotation;
						limitter(val);
						if (minDelta != 0f)
						{
							val4 = target.position - this.transform.position;
							if (val4.sqrMagnitude < minDelta)
							{
								for (int l = 0; l < chains.Length; l++)
								{
									lastFrameQ[l] = chains[l].localRotation;
								}
								return;
							}
						}
					}
				}
			}
		}
		for (int m = 0; m < chains.Length; m++)
		{
			lastFrameQ[m] = chains[m].localRotation;
		}
	}

	private unsafe void limitter(Transform bone)
	{
		if (bone.name.Contains("足首") || (bone.name.Contains("Bip01") && bone.name.Contains("Foot")))
		{
			Vector3 localEulerAngles = bone.localEulerAngles;
			localEulerAngles.z = 0f;
			bone.localRotation = Quaternion.Euler(localEulerAngles);
		}
		else if (bone.name.Contains("ひざ") || (bone.name.Contains("Bip01") && bone.name.Contains("Calf")))
		{
			Vector3 localEulerAngles2 = bone.localEulerAngles;
			if (adjust_rot((localEulerAngles2).y) == adjust_rot((localEulerAngles2).z))
			{
				localEulerAngles2.y = (float)adjust_rot((localEulerAngles2).y);
				localEulerAngles2.z = (float)adjust_rot((localEulerAngles2).z);
			}
			if ((localEulerAngles2).x > 180f)
			{
				localEulerAngles2.x = (localEulerAngles2).x - 360f;
			}
			if ((localEulerAngles2).x < 0f)
			{
				localEulerAngles2.x = 0f;
			}
			else if ((localEulerAngles2).x > 170f)
			{
				localEulerAngles2.x = 170f;
			}
			bone.localRotation = Quaternion.Euler(localEulerAngles2);
		}
	}

	private int adjust_rot(float n)
	{
		if (Mathf.Abs(n) > Mathf.Abs(180f - n) && Mathf.Abs(360f - n) > Mathf.Abs(180f - n))
		{
			return 180;
		}
		return 0;
	}

}
