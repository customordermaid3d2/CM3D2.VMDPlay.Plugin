using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin
{
	public class FileBrowser
	{
		public delegate void FinishedCallback(string path);

		protected string m_currentDirectory;

		protected string m_filePattern;

		protected Texture2D m_directoryImage;

		protected Texture2D m_fileImage;

		protected FileBrowserType m_browserType;

		protected string m_newDirectory;

		protected string[] m_currentDirectoryParts;

		protected string[] m_files;

		protected GUIContent[] m_filesWithImages;

		protected int m_selectedFile;

		protected string[] m_nonMatchingFiles;

		protected GUIContent[] m_nonMatchingFilesWithImages;

		protected int m_selectedNonMatchingDirectory;

		protected string[] m_directories;

		protected GUIContent[] m_directoriesWithImages;

		protected int m_selectedDirectory;

		protected string[] m_nonMatchingDirectories;

		protected GUIContent[] m_nonMatchingDirectoriesWithImages;

		protected bool m_currentDirectoryMatches;

		protected GUIStyle m_centredText;

		protected string m_name;

		protected Rect m_screenRect;

		protected Vector2 m_scrollPosition;

		protected FinishedCallback m_callback;

		public string CurrentDirectory
		{
			get
			{
				return m_currentDirectory;
			}
			set
			{
				SetNewDirectory(value);
				SwitchDirectoryNow();
			}
		}

		public string SelectionPattern
		{
			get
			{
				return m_filePattern;
			}
			set
			{
				m_filePattern = value;
				ReadDirectoryContents();
			}
		}

		public Texture2D DirectoryImage
		{
			get
			{
				return m_directoryImage;
			}
			set
			{
				m_directoryImage = value;
				BuildContent();
			}
		}

		public Texture2D FileImage
		{
			get
			{
				return m_fileImage;
			}
			set
			{
				m_fileImage = value;
				BuildContent();
			}
		}

		public FileBrowserType BrowserType
		{
			get
			{
				return m_browserType;
			}
			set
			{
				m_browserType = value;
				ReadDirectoryContents();
			}
		}

		protected GUIStyle CentredText
		{
			get
			{
				if (m_centredText == null)
				{
					m_centredText = new GUIStyle(GUI.skin.label);
					m_centredText.alignment = (TextAnchor)3;
					m_centredText.fixedHeight = GUI.skin.button.fixedHeight;
				}
				return m_centredText;
			}
		}

		public FileBrowser(Rect screenRect, string name, FinishedCallback callback)
		{
			m_name = name;
			m_screenRect = screenRect;
			m_browserType = FileBrowserType.File;
			m_callback = callback;

			string stringValue = Settings.Instance.GetStringValue("DefaultDir", "", true);
			SetNewDirectory((stringValue != null && stringValue != "") ? stringValue : Directory.GetCurrentDirectory());
			SwitchDirectoryNow();
		}

		protected void SetNewDirectory(string directory)
		{
			m_newDirectory = directory;
		}

		protected void SwitchDirectoryNow()
		{
			if (m_newDirectory != null && !(m_currentDirectory == m_newDirectory))
			{
				m_currentDirectory = m_newDirectory;
				m_scrollPosition = Vector2.zero;
				m_selectedDirectory = (m_selectedNonMatchingDirectory = (m_selectedFile = -1));
				ReadDirectoryContents();
			}
		}

		protected void ReadDirectoryContents()
		{
			if (m_currentDirectory == null || m_currentDirectory == "")
			{
				try
				{
					string[] logicalDrives = Directory.GetLogicalDrives();
					m_directories = new string[logicalDrives.Length];
					for (int i = 0; i < logicalDrives.Length; i++)
					{
						m_directories[i] = logicalDrives[i];
					}
					m_files = new string[0];
					m_currentDirectoryParts = new string[1]
					{
						""
					};
					BuildContent();
					m_newDirectory = null;
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
					DriveInfo[] drives = DriveInfo.GetDrives();
					m_directories = new string[drives.Length];
					for (int j = 0; j < drives.Length; j++)
					{
						m_directories[j] = drives[j].Name;
					}
					m_files = new string[0];
					m_currentDirectoryParts = new string[1]
					{
						""
					};
					BuildContent();
					m_newDirectory = null;
				}
			}
			else
			{
				string[] array = m_currentDirectory.Split(Path.DirectorySeparatorChar);
				m_currentDirectoryParts = new string[array.Length + 1];
				m_currentDirectoryParts[0] = "";
				for (int k = 0; k < array.Length; k++)
				{
					m_currentDirectoryParts[k + 1] = array[k];
				}
				if (SelectionPattern != null)
				{
					string directoryName = Path.GetDirectoryName(m_currentDirectory);
					string[] array2 = (directoryName != null) ? Directory.GetDirectories(directoryName, SelectionPattern) : Directory.GetDirectories(m_currentDirectory + "\\");
					m_currentDirectoryMatches = (Array.IndexOf(array2, m_currentDirectory) >= 0);
				}
				else
				{
					m_currentDirectoryMatches = false;
				}
				if (BrowserType == FileBrowserType.File || SelectionPattern == null)
				{
					m_directories = Directory.GetDirectories(m_currentDirectory);
					m_nonMatchingDirectories = new string[0];
				}
				else
				{
					m_directories = Directory.GetDirectories(m_currentDirectory, SelectionPattern);
					List<string> list = new List<string>();
					string[] directories = Directory.GetDirectories(m_currentDirectory);
					foreach (string text in directories)
					{
						if (Array.IndexOf(m_directories, text) < 0)
						{
							list.Add(text);
						}
					}
					m_nonMatchingDirectories = list.ToArray();
					for (int m = 0; m < m_nonMatchingDirectories.Length; m++)
					{
						int num = m_nonMatchingDirectories[m].LastIndexOf(Path.DirectorySeparatorChar);
						m_nonMatchingDirectories[m] = m_nonMatchingDirectories[m].Substring(num + 1);
					}
					Array.Sort(m_nonMatchingDirectories);
				}
				for (int n = 0; n < m_directories.Length; n++)
				{
					m_directories[n] = m_directories[n].Substring(m_directories[n].LastIndexOf(Path.DirectorySeparatorChar) + 1);
				}
				if (BrowserType == FileBrowserType.Directory || SelectionPattern == null)
				{
					m_files = Directory.GetFiles(m_currentDirectory);
					m_nonMatchingFiles = new string[0];
				}
				else
				{
					m_files = Directory.GetFiles(m_currentDirectory, SelectionPattern);
					List<string> list2 = new List<string>();
					string[] directories = Directory.GetFiles(m_currentDirectory);
					foreach (string text2 in directories)
					{
						if (Array.IndexOf(m_files, text2) < 0)
						{
							list2.Add(text2);
						}
					}
					m_nonMatchingFiles = list2.ToArray();
					for (int num2 = 0; num2 < m_nonMatchingFiles.Length; num2++)
					{
						m_nonMatchingFiles[num2] = Path.GetFileName(m_nonMatchingFiles[num2]);
					}
					Array.Sort(m_nonMatchingFiles);
				}
				for (int num3 = 0; num3 < m_files.Length; num3++)
				{
					m_files[num3] = Path.GetFileName(m_files[num3]);
				}
				Array.Sort(m_files);
				BuildContent();
				m_newDirectory = null;
			}
		}

		protected void BuildContent()
		{
			m_directoriesWithImages = (GUIContent[])new GUIContent[m_directories.Length];
			for (int i = 0; i < m_directoriesWithImages.Length; i++)
			{
				m_directoriesWithImages[i] = new GUIContent(m_directories[i], DirectoryImage);
			}
			m_nonMatchingDirectoriesWithImages = (GUIContent[])new GUIContent[m_nonMatchingDirectories.Length];
			for (int j = 0; j < m_nonMatchingDirectoriesWithImages.Length; j++)
			{
				m_nonMatchingDirectoriesWithImages[j] = new GUIContent(m_nonMatchingDirectories[j], DirectoryImage);
			}
			m_filesWithImages = (GUIContent[])new GUIContent[m_files.Length];
			for (int k = 0; k < m_filesWithImages.Length; k++)
			{
				m_filesWithImages[k] = new GUIContent(m_files[k], FileImage);
			}
			m_nonMatchingFilesWithImages = (GUIContent[])new GUIContent[m_nonMatchingFiles.Length];
			for (int l = 0; l < m_nonMatchingFilesWithImages.Length; l++)
			{
				m_nonMatchingFilesWithImages[l] = new GUIContent(m_nonMatchingFiles[l], FileImage);
			}
		}

		public unsafe void OnGUIAsWindow(int winID)
		{
			m_screenRect = GUI.Window(winID, m_screenRect, OnGUIWindow, m_name, GUI.skin.window);
		}

		public void OnGUI()
		{
			GUILayout.BeginArea(m_screenRect, m_name, GUI.skin.window);
			OnGUI(-1);
			GUILayout.EndArea();
		}

		public void OnGUIWindow(int winID)
		{
			OnGUI(winID);
			GUI.DragWindow();
		}

		public void OnGUI(int winID)
		{
			GUI.skin.GetStyle("List Item").alignment = (TextAnchor)3;
			GUILayout.BeginHorizontal( );
			for (int i = 0; i < m_currentDirectoryParts.Length; i++)
			{
				if (i == m_currentDirectoryParts.Length - 1)
				{
					GUILayout.Label(m_currentDirectoryParts[i], CentredText  );
				}
				else if (GUILayout.Button(m_currentDirectoryParts[i]  ))
				{
					if (i == 0)
					{
						SetNewDirectory("");
					}
					else
					{
						string text = m_currentDirectory;
						for (int num = m_currentDirectoryParts.Length - 1; num > i; num--)
						{
							text = Path.GetDirectoryName(text);
						}
						SetNewDirectory(text);
					}
				}
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			m_scrollPosition = GUILayout.BeginScrollView(m_scrollPosition, false, true, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.box  );
			m_selectedDirectory = GUILayoutx.SelectionList(m_selectedDirectory, m_directoriesWithImages, (GUILayoutx.DoubleClickCallback)DirectoryDoubleClickCallback);
			if (m_selectedDirectory > -1)
			{
				m_selectedFile = (m_selectedNonMatchingDirectory = -1);
			}
			m_selectedNonMatchingDirectory = GUILayoutx.SelectionList(m_selectedNonMatchingDirectory, m_nonMatchingDirectoriesWithImages, (GUILayoutx.DoubleClickCallback)NonMatchingDirectoryDoubleClickCallback);
			if (m_selectedNonMatchingDirectory > -1)
			{
				m_selectedDirectory = (m_selectedFile = -1);
			}
			GUI.enabled = BrowserType == FileBrowserType.File;
			m_selectedFile = GUILayoutx.SelectionList(m_selectedFile, m_filesWithImages, (GUILayoutx.DoubleClickCallback)FileDoubleClickCallback);
			GUI.enabled = true;
			if (m_selectedFile > -1)
			{
				m_selectedDirectory = (m_selectedNonMatchingDirectory = -1);
			}
			GUI.enabled = false;
			GUILayoutx.SelectionList(-1, m_nonMatchingFilesWithImages);
			GUI.enabled = true;
			GUILayout.EndScrollView();
			GUILayout.BeginHorizontal( );
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Cancel", (GUILayoutOption[])new GUILayoutOption[1]
			{
				GUILayout.Width(50f)
			}))
			{
				m_callback(null);
			}
			if (BrowserType == FileBrowserType.File)
			{
				GUI.enabled = m_selectedFile > -1;
			}
			else if (SelectionPattern == null)
			{
				GUI.enabled = m_selectedDirectory > -1;
			}
			else
			{
				GUI.enabled = m_selectedDirectory > -1 || (m_currentDirectoryMatches && m_selectedNonMatchingDirectory == -1 && m_selectedFile == -1);
			}
			if (GUILayout.Button("Select", (GUILayoutOption[])new GUILayoutOption[1]
			{
				GUILayout.Width(50f)
			}))
			{
				if (BrowserType == FileBrowserType.File)
				{
					m_callback(Path.Combine(m_currentDirectory, m_files[m_selectedFile]));
				}
				else if (m_selectedDirectory > -1)
				{
					m_callback(Path.Combine(m_currentDirectory, m_directories[m_selectedDirectory]));
				}
				else
				{
					m_callback(m_currentDirectory);
				}
			}
			GUI.enabled = true;
			GUILayout.EndHorizontal();
			if ((int)Event.current.type == 7)
			{
				SwitchDirectoryNow();
			}
		}

		protected void FileDoubleClickCallback(int i)
		{
			if (BrowserType == FileBrowserType.File)
			{
				m_callback(Path.Combine(m_currentDirectory, m_files[i]));
			}
		}

		protected void DirectoryDoubleClickCallback(int i)
		{
			if (m_directories[i].Contains(":"))
			{
				SetNewDirectory(m_directories[i]);
			}
			else
			{
				SetNewDirectory(Path.Combine(m_currentDirectory, m_directories[i]));
			}
		}

		protected void NonMatchingDirectoryDoubleClickCallback(int i)
		{
			if (m_nonMatchingDirectories[i].Contains(":"))
			{
				SetNewDirectory(m_nonMatchingDirectories[i]);
			}
			else
			{
				SetNewDirectory(Path.Combine(m_currentDirectory, m_nonMatchingDirectories[i]));
			}
		}
	}
}
