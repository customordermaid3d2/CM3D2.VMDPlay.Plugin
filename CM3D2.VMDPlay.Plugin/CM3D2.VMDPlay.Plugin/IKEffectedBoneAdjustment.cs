using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	internal class IKEffectedBoneAdjustment
	{
		private Transform bone;

		private const int MAX = 10;

		private int lastApplied = -1;

		private int historyIndex = -1;

		private Quaternion[] histories = (Quaternion[])new Quaternion[10];

		public float maxDeltaAngle = 5f;

		public IKEffectedBoneAdjustment(Transform bone)
		{
			this.bone = bone;
		}

		public void LateUpdate()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			Quaternion localRotation = bone.localRotation;
			historyIndex++;
			if (historyIndex > 10)
			{
				historyIndex = 0;
			}
			histories[historyIndex] = localRotation;
			if (lastApplied == -1)
			{
				lastApplied = historyIndex;
			}
			else if (historyIndex == lastApplied)
			{
				lastApplied = historyIndex;
			}
			else
			{
				Quaternion val = histories[lastApplied];
				if (Quaternion.Angle(localRotation, val) > maxDeltaAngle)
				{
					bone.localRotation = histories[lastApplied];
				}
				else
				{
					lastApplied = historyIndex;
				}
			}
		}
	}
}
