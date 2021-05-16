using ExIni;
using System;
using System.IO;
using System.Reflection;

namespace CM3D2.VMDPlay.Plugin
{
	internal class Settings
	{
		private static Settings instance;

		private IniFile file;
		
		public static readonly string IniDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Config\";

		public static readonly string IniFileName = IniDirectory + "vmdplay.ini";

		public const string DEFAULT_SECTION_NAME = "VMDPlay";

		public static Settings Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Settings();
				}
				return instance;
			}
		}

		public Settings()
		{
			Load();
		}

		private void Load()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			if (File.Exists(IniFileName))
			{
				file = IniFile.FromFile(IniFileName);
			}
			else
			{
				file = IniFile.FromString("[VMDPlay]");
				file.Save(IniFileName);
			}
		}

		public bool GetBoolValue(string keyString, bool defaultValue, bool saveDefault = false)
		{
			string stringValue = GetStringValue(keyString, null, false);
			if (stringValue != null)
			{
				try
				{
					return bool.Parse(stringValue);
				}
				catch (Exception)
				{
				}
			}
			if (saveDefault)
			{
				SetBoolValue(keyString, defaultValue);
			}
			return defaultValue;
		}

		public float GetFloatValue(string keyString, float defaultValue, bool saveDefault = false)
		{
			string stringValue = GetStringValue(keyString, null, false);
			if (stringValue != null)
			{
				try
				{
					return float.Parse(stringValue);
				}
				catch (Exception)
				{
				}
			}
			if (saveDefault)
			{
				SetFloatValue(keyString, defaultValue);
			}
			return defaultValue;
		}

		public string GetStringValue(string keyString, string defaultValue, bool saveDefault = false)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if (file.HasSection("VMDPlay"))
			{
				IniSection val = file.GetSection("VMDPlay");
				if (val != null && val.HasKey(keyString))
				{
					IniKey value = val.GetKey(keyString);
					if (value != null)
					{
						try
						{
							return value.Value;
						}
						catch (Exception)
						{
						}
					}
				}
			}
			if (saveDefault)
			{
				SetStringValue(keyString, defaultValue);
			}
			return defaultValue;
		}

		public void SetBoolValue(string keyString, bool value)
		{
			SetStringValue(keyString, value.ToString());
		}

		public void SetFloatValue(string keyString, float value)
		{
			SetStringValue(keyString, value.ToString());
		}

		public void SetStringValue(string keyString, string value)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			if (!file.HasSection("VMDPlay"))
			{
				file.CreateSection("VMDPlay");
			}
			IniSection val = file.GetSection("VMDPlay");
			if (!val.HasKey(keyString))
			{
				val.CreateKey(keyString);
			}
			val.GetKey(keyString).Value = value;
			Save();

		}

		public void Save()
		{
			try
			{
				if (!Directory.Exists(IniDirectory))
				{
					Directory.CreateDirectory(IniDirectory);
				}
				file.Save(IniFileName);
			}
			catch (Exception)
			{
				Console.WriteLine("Failed to save");
			}
		}
	}
}
