using System;
using System.Linq;
using UnityEngine;

public class BoneController : UnityEngine.MonoBehaviour
{
	[Serializable]
	public class LiteTransform
	{
		public Vector3 position;

		public Quaternion rotation;

		public LiteTransform(Vector3 p, Quaternion r)
		{
			position = p;
			rotation = r;
		}
	}

	public BoneController additive_parent;

	public float additive_rate;

	public CCDIKSolver ik_solver;

	public BoneController[] ik_solver_targets;

	public bool add_local;

	public bool add_move;

	public bool add_rotate;

	private LiteTransform prev_global_;

	private LiteTransform prev_local_;

	private void Start()
	{
		if (null != ik_solver)
		{
			ik_solver = this.transform.GetComponent<CCDIKSolver>();
			if (ik_solver_targets.Length == 0)
			{
				ik_solver_targets = (from x in Enumerable.Repeat<Transform>(ik_solver.target, 1).Concat(ik_solver.chains)
				select x.GetComponent<BoneController>()).ToArray();
			}
		}
		UpdatePrevTransform();
	}

	public void Process()
	{
		if (null != additive_parent)
		{
			LiteTransform deltaTransform = additive_parent.GetDeltaTransform(add_local);
			if (add_move)
			{
				Transform transform = this.transform;
				transform.localPosition = transform.localPosition + deltaTransform.position * additive_rate;
			}
			if (add_rotate)
			{
				Quaternion val;
				if (0f <= additive_rate)
				{
					val = Quaternion.Slerp(Quaternion.identity, deltaTransform.rotation, additive_rate);
				}
				else
				{
					Quaternion val2 = Quaternion.Inverse(deltaTransform.rotation);
					val = Quaternion.Slerp(Quaternion.identity, val2, 0f - additive_rate);
				}
				Transform transform2 = this.transform;
				transform2.localRotation = transform2.localRotation * val;
			}
		}
	}

	public LiteTransform GetDeltaTransform(bool is_add_local)
	{
		if (!is_add_local)
		{
			return new LiteTransform(this.transform.localPosition - prev_local_.position, Quaternion.Inverse(prev_local_.rotation) * this.transform.localRotation);
		}
		return new LiteTransform(this.transform.position - prev_global_.position, Quaternion.Inverse(prev_global_.rotation) * this.transform.rotation);
	}

	public void UpdatePrevTransform()
	{
		prev_global_ = new LiteTransform(this.transform.position, this.transform.rotation);
		prev_local_ = new LiteTransform(this.transform.localPosition, this.transform.localRotation);
	}

}
