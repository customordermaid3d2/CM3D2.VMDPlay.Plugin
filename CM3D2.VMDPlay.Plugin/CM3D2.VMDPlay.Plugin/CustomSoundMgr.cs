using System;
using System.IO;
using System.Reflection;
using System.Threading;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	public class CustomSoundMgr
	{
		public AudioClip currentAudioClip;

		private string _audioFilePath;

		private static FieldInfo f_m_AudioFadeBufBgm = typeof(SoundMgr).GetField("m_AudioFadeBufBgm", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);

		public string audioFilePath => _audioFilePath;

		public void ClearClip()
		{
			currentAudioClip = null;
			_audioFilePath = null;
		}

		public bool SetSoundClip(string path)
		{
			AudioClip val = LoadSoundAsClip(path);
			if (val != null)
			{
				currentAudioClip = val;
				_audioFilePath = path;
				return true;
			}
			return false;
		}

		public void PauseBgm()
        {
			//GameMain.Instance.SoundMgr.;
		}
		public void StopBgm()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			GameMain.Instance.SoundMgr.StopBGM(0f);
		}

		public AudioClip LoadSoundAsClip(string path)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Expected O, but got Unknown
			try
			{
				if (File.Exists(path))
				{
					WWW val = new WWW(new Uri(path).AbsoluteUri);
					while (!val.isDone)
					{
						Thread.Sleep(100);
					}
					return val.GetAudioClip();
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
			return null;
		}

		public void PlaySound()
		{
			PlaySound(currentAudioClip);
		}

		public void PlaySound(string path)
		{
			AudioClip audioClip = LoadSoundAsClip(path);
			PlaySound(audioClip);
		}

		public void PlaySound(AudioClip audioClip)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			Console.WriteLine("Playing clip {0}", audioClip.name);
			if (audioClip != null)
			{
				AudioSourceMgr[] componentsInChildren = GameMain.Instance.MainCamera.gameObject.GetComponentsInChildren<AudioSourceMgr>();
				int num = 0;
				AudioSourceMgr val;
				while (true)
				{
					if (num >= componentsInChildren.Length)
					{
						return;
					}
					val = componentsInChildren[num];
					if ((int)val.SoundType == 0)
					{
						break;
					}
					num++;
				}
				val.audiosource.clip = audioClip;
				val.audiosource.volume = (float)GameMain.Instance.SoundMgr.GetVolume(val.SoundType) * 0.01f;
				val.audiosource.loop = false;
				val.audiosource.Play();
			}
		}
	}
}
