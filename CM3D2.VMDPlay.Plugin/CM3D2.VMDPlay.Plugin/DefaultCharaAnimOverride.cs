using System.Collections;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	public class DefaultCharaAnimOverride : UnityEngine.MonoBehaviour
	{
		private bool defaultAnimeEnabled = true;

		public MonoBehaviour currentController;

		private Maid maid;

		public bool DefaultAnimeEnabled => defaultAnimeEnabled;

		public Animator defaultAnimator => null;

		public static DefaultCharaAnimOverride Get(Maid maid)
		{
			DefaultCharaAnimOverride defaultCharaAnimOverride = maid.gameObject.GetComponent<DefaultCharaAnimOverride>();
			if (defaultCharaAnimOverride == null)
			{
				defaultCharaAnimOverride = maid.gameObject.AddComponent<DefaultCharaAnimOverride>();
				defaultCharaAnimOverride.Init(maid);
			}
			return defaultCharaAnimOverride;
		}

		public void Init(Maid maid)
		{
			this.maid = maid;
			this.StartCoroutine(DisableDefaultAnimIKCo());
		}

		private IEnumerator DisableDefaultAnimIKCo()
		{
			while (true)
			{
				yield return (object)null;
				DisableEnableDefaultAnim();
			}
		}

		public void DisableEnableDefaultAnim()
		{
			bool defaultAnimeEnabled2 = defaultAnimeEnabled;
		}

		public void Aquire(MonoBehaviour controller)
		{
			if (defaultAnimeEnabled)
			{
				defaultAnimeEnabled = false;
				currentController = controller;
				DisableEnableDefaultAnim();
			}
		}

		public void Release(MonoBehaviour controller)
		{
			if (!defaultAnimeEnabled && controller == currentController)
			{
				defaultAnimeEnabled = true;
				currentController = null;
				DisableEnableDefaultAnim();
			}
		}

		public void IKUpdate()
		{
			bool defaultAnimeEnabled2 = defaultAnimeEnabled;
		}

		public void IKLateUpdate()
		{
			bool defaultAnimeEnabled2 = defaultAnimeEnabled;
		}

		public void ResetBoneRotations()
		{
			Transform val = maid.body0.gameObject.transform;
			for (int i = 0; i < val.childCount; i++)
			{
				InitializeBasePositions(val.GetChild(i));
			}
		}

		private void InitializeBasePositions(Transform t)
		{
			if (!t.name.StartsWith("cf_J_sk_") && !t.name.Contains("_J_Head_s") && !t.name.Contains("_J_Mune00"))
			{
				t.localRotation = Quaternion.identity;
				for (int i = 0; i < t.childCount; i++)
				{
					InitializeBasePositions(t.GetChild(i));
				}
			}
		}

		private void OnDestroy()
		{
			this.StopAllCoroutines();
		}

	}
}
