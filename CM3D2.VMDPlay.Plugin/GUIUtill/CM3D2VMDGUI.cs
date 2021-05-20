using CM3D2.VMDPlay.Plugin.GUIUtill;
using CM3D2.VMDPlay.Plugin.Utill;
using COM3D2.Lilly.Plugin;
using COM3D2.Lilly.Plugin.Utill;
using COM3D2.VMDPlay.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

using System.Linq;

namespace CM3D2.VMDPlay.Plugin
{
    public class CM3D2VMDGUI : UnityEngine.MonoBehaviour
    {
        System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
        //VistaOpenFileDialog dialog;
        //VistaOpenFileDialog openFileDialog;

        public static Maid focusChara;

        private CameraCtrlOff cameraCtrl;

        private int windowID = 87238;

        private int dialogWindowID = 87248;

        private Rect windowRect = new Rect(0f, 300f, 660f, 560f);

        private string windowTitle = "COM3D2 VMDPlay Plugin Lilly";

        private Texture2D windowBG = new Texture2D(1, 1, (TextureFormat)5, false);

        public bool visibleGUI = true;

        private bool isVR;

        //private int currentTab;

        private float sliderLabelWidth = 100f;

        private float sliderWidth = 240f;

        private float valueLabelWidth = 70f;

        public bool pinObject;

        public Dictionary<string, Action> AdditionalMenus = new Dictionary<string, Action>();

        private static MethodInfo m_Apply = typeof(GUISkin).GetMethod("Apply", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod);

        private Dictionary<string, GUIStyle> styleBackup = new Dictionary<string, GUIStyle>();

        private VMDAnimationController lastController;

        private string lastFilename = string.Empty;
        private string oggFilename = string.Empty;

        //protected FileBrowser m_fileBrowser;

        protected Texture2D m_directoryImage;

        protected Texture2D m_fileImage;

        //private Vector2 vmdAreaScrollPos;

        private Dictionary<string, float> sliderMax = new Dictionary<string, float>();

        private void Awake()
        {
            styleBackup = new Dictionary<string, GUIStyle>();
            /*
            BackupGUIStyle("Button");
            BackupGUIStyle("Label");
            BackupGUIStyle("Toggle");
            */

        }

        private void Start()
        {
            if (GameMain.Instance.VRMode)
            {
                Console.WriteLine("VMDPlayPlugin:VR Mode detect");
                isVR = true;
            }
            else
            {
                Console.WriteLine("VMDPlayPlugin:non VR Mode detect");
            }
            windowBG.SetPixel(0, 0, Color.black);
            windowBG.Apply();
            if (cameraCtrl == null)
            {
                cameraCtrl = this.gameObject.AddComponent<CameraCtrlOff>();
                cameraCtrl.ikInfoGui = this;
                cameraCtrl.enabled = true;
            }
        }

        private void OnEnable()
        {
            if (cameraCtrl != null)
            {
                cameraCtrl.enabled = true;
            }
        }

        private void OnDisable()
        {
            if (cameraCtrl != null)
            {
                cameraCtrl.enabled = false;
            }
        }

        private void Update()
        {
        }

        public void Clear()
        {
            focusChara = null;
            lastController = null;
            lastFilename = null;
        }

        private unsafe void OnGUI()
        {
            if (visibleGUI)
            {
               // try
               // {
                    /*
                    GUIStyle val = new GUIStyle(GUI.skin.window);
                    if (GUI.skin.GetStyle("List Item") != null)
                    {
                        GUIStyle[] array = (GUIStyle[])new GUIStyle[GUI.skin.customStyles.Length + 1];
                        for (int i = 0; i < GUI.skin.customStyles.Length; i++)
                        {
                            array[i] = GUI.skin.customStyles[i];
                        }
                        GUIStyle val2 = new GUIStyle(GUI.skin.button);
                        val2.name = "List Item";
                        array[GUI.skin.customStyles.Length] = val2;
                        GUI.skin.customStyles = array;
                        m_Apply.Invoke((object)GUI.skin, new object[0]);
                    }
                    if (isVR)
                    {
                        val.onNormal.background = windowBG;
                        val.normal.background = windowBG;
                        val.hover.background = windowBG;
                        val.focused.background = windowBG;
                        val.active.background = windowBG;
                        val.hover.textColor = Color.white;
                        val.onHover.textColor = Color.white;
                    }
                    */
                    /*
					if (m_fileBrowser != null)
					{
						m_fileBrowser.OnGUIAsWindow(dialogWindowID);
					}
					else
					*/
                 //   {
                        windowRect = GUI.Window(windowID, windowRect, FuncWindowGUI, windowTitle);
                        //windowRect = GUI.Window(windowID, windowRect, FuncWindowGUI, windowTitle, val);
                //    }
                //}
                //catch (Exception value)
                //{
                //    Console.WriteLine(value);
                //}
            }
        }

        private float CalcAdjustedSliderMax(float value)
        {
            if (value <= 1f)
            {
                return 1f;
            }
            if (value <= 10f)
            {
                return 10f;
            }
            return 100f;
        }

        //GUIStyleUtill gui;
        GUILayoutOptionUtill gui=GUILayoutOptionUtill.Instance;

        private unsafe void FuncWindowGUI(int winID)
        {
            /*
            styleBackup = new Dictionary<string, GUIStyle>();
            BackupGUIStyle("Button");
            BackupGUIStyle("Label");
            BackupGUIStyle("Toggle");
            */
            try
            {
                if (GUIUtility.hotControl == 0)
                {
                    cameraCtrl.enabled = false;
                }
                if (Event.current.type == EventType.MouseDown)
                {
                    GUI.FocusControl("");
                    GUI.FocusWindow(winID);
                    cameraCtrl.enabled = true;
                    cameraCtrl.cameraCtrlOff = true;
                }
                GUI.enabled = true;
                /*
                GUIStyle style = GUI.skin.GetStyle("Button");
                style.normal.textColor = Color.white;
                style.alignment = (TextAnchor)4;
                GUIStyle style2 = GUI.skin.GetStyle("Label");
                style2.normal.textColor = Color.white;
                style2.alignment = (TextAnchor)3;
                style2.wordWrap = false;
                GUIStyle style3 = GUI.skin.GetStyle("Toggle");
                style3.normal.textColor = Color.white;
                style3.onNormal.textColor = Color.white;
                */
                GUILayout.BeginVertical( );
                if (focusChara != null && (focusChara.body0 == null || focusChara.body0.m_Bones == null))
                {
                    focusChara = null;
                }
                if (focusChara == null)
                {
                    focusChara = FindFirstMaid();
                }

                DrawVMDAnimationArea();

                GUILayout.EndVertical();
                GUI.DragWindow();
            }
            catch (Exception value)
            {
                Console.WriteLine(value.ToString());
            }
            finally
            {
                /*
                RestoreGUIStyle("Button");
                RestoreGUIStyle("Label");
                RestoreGUIStyle("Toggle");
                */
            }
        }

        /// <summary>
        /// 최적화 완료?
        /// </summary>
        /// <returns></returns>
        private Maid FindFirstMaid()
        {
            /*
			CharacterMgr val = GameMain.Instance.CharacterMgr;
			for (int i = 0; i < val.GetMaidCount(); i++)
			{
				Maid val2 = val.GetMaid(i);
				if (val2 != null && val2.body0.isLoadedBody)
				{
					return val2;
				}
			}
			*/
            if (CharacterMgrPatch.maids.Count > 0)
            {
                return CharacterMgrPatch.maids[0];
            }
            return null;
        }

        /// <summary>
        /// 최적화 완료?
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        private Maid FindPrevNextMaid(bool next)
        {
            /*
			List<Maid> list = new List<Maid>();
			CharacterMgr val = GameMain.Instance.CharacterMgr;
			for (int i = 0; i < val.GetMaidCount(); i++)
			{
				Maid val2 = val.GetMaid(i);
				if (val2 != null && val2.body0.isLoadedBody)
				{
					list.Add(val2);
				}
			}
			*/
            if (CharacterMgrPatch.maids.Count == 0)
            {
                return null;
            }
            if (focusChara != null)
            {
                int num = CharacterMgrPatch.maids.IndexOf(focusChara);
                if (num >= 0)
                {
                    num += (next ? 1 : (-1));
                    num = (num + CharacterMgrPatch.maids.Count) % CharacterMgrPatch.maids.Count;
                    return CharacterMgrPatch.maids[num];
                }
            }
            return CharacterMgrPatch.maids[0];
        }

        /*
        private void BackupGUIStyle(string name)
        {
            GUIStyle value = new GUIStyle(GUI.skin.GetStyle(name));
            styleBackup.Add(name, value);
        }
        */

        /*
        private void RestoreGUIStyle(string name)
        {
            if (styleBackup.ContainsKey(name))
            {
                GUIStyle val = styleBackup[name];
                GUIStyle style = GUI.skin.GetStyle(name);
                style.normal.textColor = val.normal.textColor;
                style.alignment = val.alignment;
                style.wordWrap = val.wordWrap;
            }
        }
        */

        public static VMDAnimationController vMDAnimationController;
        public static VMDAnimationController vMDAnimationControllerSub;
        public static bool isFavorites = false;

        protected Vector2 scrollPosition;

        private string FavoritesName = string.Empty;

        float timeShiftMin = -60;
        float timeShiftMax = 60;
        float timeShiftNow = 0;

        private void DrawVMDAnimationArea()
        {
            //EnsureResourceLoaded();
            //GUI.skin.GetStyle("Button");
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            #region Favorites

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Favorites", gui[100,25]))
            {
                isFavorites = !isFavorites;
            }
            if (GUILayout.Button("add", gui[50f, 25f]))
            {
                SongMotionUtill.Set(
                    FavoritesName
                    , oggFilename
                    //, VMDAnimationMgr.Instance.controllers.Where(x=> CharacterMgrPatch.maids.Contains(x.maid) ).Select(x => x.lastLoadedVMD).ToArray()
                    , VMDAnimationMgr.Instance.maidcontrollers.Values.Select(x => x.lastLoadedVMD).ToArray()
                    );
               // MyLog.LogMessage("add", VMDAnimationMgr.Instance.controllers.Count, CharacterMgrPatch.maids.Count, VMDAnimationMgr.Instance.controllers.Where(x => CharacterMgrPatch.maids.Contains(x.maid)).Count() );
                MyLog.LogMessage("add", VMDAnimationMgr.Instance.maidcontrollers.Count, CharacterMgrPatch.maids.Count);
            }
            FavoritesName = GUILayout.TextField(FavoritesName, gui[350, 25]);
            if (GUILayout.Button("reload", gui[50, 25]))
            {
                SongMotionUtill.Deserialize();
            }
            if (GUILayout.Button("chack", gui[50, 25]))
            {
                MyLog.LogMessage("chack", VMDAnimationMgr.Instance.maidcontrollers.Count);
                MyLog.LogMessage("chack", CharacterMgrPatch.maids.Count);
            }
            GUILayout.EndHorizontal();
                      

            if (isFavorites)
            {               

                foreach (var item in SongMotionUtill.GetList())
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(item.Key, gui[550f, 25f]))
                    {
                        //VMDAnimationMgr.Instance.ControllerInstallAll();
                        VMDAnimationMgr.Instance.ClearAll();
                        if (item.Value.Motions.Count>0)
                        {
                            //var v = VMDAnimationMgr.Instance.maidcontrollers.ToList();
                            //var v=VMDAnimationMgr.Instance.controllers.Where(x => CharacterMgrPatch.maids.Contains(x.maid)).ToList();
                            var v = CharacterMgrPatch.maids;
                            for (int i = 0; i < item.Value.Motions.Count && i < v.Count; i++)
                            {
                                vMDAnimationControllerSub = VMDAnimationController.Install(v[i]);
                                vMDAnimationControllerSub.VMDAnimEnabled = true;
                                vMDAnimationControllerSub.LoadVMDAnimation(item.Value.Motions[i]);
                            }
                            //vMDAnimationController = VMDAnimationController.Install(focusChara);
                            lastFilename = vMDAnimationController.lastLoadedVMD ;
                        }
                        oggFilename = item.Value.Song;
                        AudioManager.Load(oggFilename, vMDAnimationController.Loop);
                        
                        //for (int i = 0; i < item.Value.Motions.Count && i < v.Count; i++)
                        //{
                        //    vMDAnimationController.Play();
                        //}
                        VMDAnimationMgr.Instance.PlayAll();
                        AudioManager.Play();
                        
                        isFavorites = false;
                        this.gameObject.SetActive(false);
                    }
                    if (GUILayout.Button("Del", gui[50f, 25f]))
                    {
                        SongMotionUtill.Del(item.Key);
                    }
                    GUILayout.EndHorizontal();
                }

               
            }

            #endregion

            #region OGG ===========================================

            GUILayout.BeginHorizontal( );

            GUILayout.Label("OGG", gui[50, 25]);

            if (GUILayout.Button("load", gui[50, 25]))
            {
                if (vMDAnimationController != null)
                {
                    AudioManager.Load(oggFilename, vMDAnimationController.Loop);
                }
                else
                {
                    AudioManager.Load(oggFilename, true);
                }
            }
            if (AudioManager.isPlay())
            {
                if (GUILayout.Button("Pause", (GUILayoutOption[])new GUILayoutOption[2] { GUILayout.Width(50f), GUILayout.Height(25f) }))
                {
                    AudioManager.Pause();
                }
            }
            else
            {
                if (GUILayout.Button("play", (GUILayoutOption[])new GUILayoutOption[2] { GUILayout.Width(50f), GUILayout.Height(25f) }))
                {
                    AudioManager.Play();

                }
            }
            if (oggFilename == null)
            {
                oggFilename = string.Empty;
            }
            oggFilename = GUILayout.TextField(oggFilename, gui[350, 25]);
            if (GUILayout.Button("...", gui[30, 25]))
            {                
                dialog.Filter = "OGG files (*.ogg)|*.ogg|WAV files (*.wav)|*.wav";

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    // 음악은 상대경로가 안먹힌다?
                    oggFilename = MyUtill.EvaluateRelativePath(Environment.CurrentDirectory, dialog.FileName);
                    if (vMDAnimationController != null)
                    {
                        AudioManager.Load(oggFilename, vMDAnimationController.Loop);
                    }
                    else
                    {
                        AudioManager.Load(oggFilename, true);
                    }
                    AudioManager.Play();
                }
            }
            GUILayout.EndHorizontal();

            #endregion =================================



            if (focusChara == null)
            {
                GUILayout.BeginHorizontal( );
                GUILayout.Label("Character not selected.", (GUILayoutOption[])new GUILayoutOption[1]
                {
                    GUILayout.Width(200f)
                });
                //GUILayout.Space(50f);
                if (GUILayout.Button("Close", gui[50f, 25f]))
                {
                    this.gameObject.SetActive(false);
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                vMDAnimationController = VMDAnimationController.Install(focusChara);
                if (!(vMDAnimationController == null) && focusChara != null)
                {
                    GUILayout.BeginVertical();
                    if (vMDAnimationController != lastController)
                    {
                        lastFilename = vMDAnimationController.lastLoadedVMD;
                        lastController = vMDAnimationController;
                        timeShiftNow = vMDAnimationController.timeShiftNow;
                    }
                    if (lastFilename == null)
                    {
                        lastFilename = "";
                    }

                    #region Maid Select

                    GUILayout.BeginHorizontal( );

                    if (GUILayout.Button("<", gui[20f, 25f]))
                    {
                        focusChara = FindPrevNextMaid(false);
                    }
                    if (GUILayout.Button(">", gui[20f, 25f]))
                    {
                        focusChara = FindPrevNextMaid(true);
                    }
                    if (GUILayout.Button(vMDAnimationController.VMDAnimEnabled ? "On" : "Off", gui[50f, 25f]))
                    {
                        vMDAnimationController.VMDAnimEnabled = !vMDAnimationController.VMDAnimEnabled;
                        //isFavorites = vMDAnimationController.VMDAnimEnabled;
                    }
                    GUILayout.Label((focusChara.status.fullNameJpStyle), (GUILayoutOption[])new GUILayoutOption[1]
                    {
                        GUILayout.Width(200f)
                    });
                    /*
                    if (vMDAnimationController.VMDAnimEnabled)
                    {
                        GUILayout.Space(50f);
                        if (vMDAnimationController.lastLoadedVMD != null && File.Exists(vMDAnimationController.lastLoadedVMD))
                        {
                            GUILayout.Label(Path.GetFileNameWithoutExtension(vMDAnimationController.lastLoadedVMD),  );
                        }
                    }
                    */
                    GUILayout.Space(50f);
                    if (GUILayout.Button("Close", gui[50f, 25f]))
                    {
                        this.gameObject.SetActive(false);
                    }
                    //GUILayout.Space(50f);

                    GUILayout.EndHorizontal();

                    #endregion

                    if (vMDAnimationController.VMDAnimEnabled)
                    {
                        #region VMD =====================================================================

                        GUILayout.BeginHorizontal( );
                        GUILayout.Label("VMD", (GUILayoutOption[])new GUILayoutOption[2] { GUILayout.Width(50f), GUILayout.Height(25f) });
                        if (GUILayout.Button("Load", (GUILayoutOption[])new GUILayoutOption[2] { GUILayout.Width(50f), GUILayout.Height(25f) }))
                        {
                            vMDAnimationController.LoadVMDAnimation(lastFilename, true);
                        }
                        if (GUILayout.Button("Reload", gui[50f, 25f]))
                        {
                            vMDAnimationController.ReloadVMDAnimation();
                            lastFilename = vMDAnimationController.lastLoadedVMD;
                        }
                        if (lastFilename == null)
                        {
                            lastFilename = string.Empty;
                        }
                        lastFilename = GUILayout.TextField(lastFilename, gui[350f, 25f]);
                        if (GUILayout.Button("...", gui[30f, 25f]))
                        {
                            //System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
                            dialog.Filter = "VMD files (*.vmd)|*.vmd";

                            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                lastFilename = MyUtill.EvaluateRelativePath(Environment.CurrentDirectory, dialog.FileName);
                                vMDAnimationController.LoadVMDAnimation(lastFilename);                                
                                vMDAnimationController.Play();
                            }

                        }
                        if (GUILayout.Button("All apply", gui[60, 25f]))
                        {
                            VMDAnimationMgr.Instance.ClearAll();
                            foreach (var item in  CharacterMgrPatch.maids)
                            {
                                vMDAnimationControllerSub = VMDAnimationController.Install(item);
                                vMDAnimationControllerSub.VMDAnimEnabled = true;
                                vMDAnimationControllerSub.LoadVMDAnimation(lastFilename);
                            }
                            //vMDAnimationController = VMDAnimationController.Install(focusChara);
                            VMDAnimationMgr.Instance.PlayAll();
                        }
                        GUILayout.EndHorizontal();

                        #endregion

                        #region play

                        GUILayout.BeginHorizontal( );
                        GUILayout.Label("(Player)", gui[50f, 25f]);
                        if (GUILayout.Button("Play", gui[50f, 25f]))
                        {
                            vMDAnimationController.Play();
                            AudioManager.Play(vMDAnimationController.Loop);
                            this.gameObject.SetActive(false);
                        }
                        if (GUILayout.Button("Pause", gui[50f, 25f]))
                        {
                            vMDAnimationController.Pause();
                            AudioManager.Pause(vMDAnimationController.Loop);
                        }
                        if (GUILayout.Button("Stop", gui[50f, 25f]))
                        {
                            vMDAnimationController.Stop();
                            AudioManager.Stop();
                        }
                        GUILayout.Space(30f);
                        GUILayout.Label("(All)", gui[GUILayoutOptionUtill.Type.Width, 50f]);
                        if (GUILayout.Button("Play", gui[50f, 25f]))
                        {
                            VMDAnimationMgr.Instance.PlayAll();
                            AudioManager.Play();
                            this.gameObject.SetActive(false);
                        }
                        if (GUILayout.Button("Pause", gui[50f, 25f]))
                        {
                            VMDAnimationMgr.Instance.PauseAll();
                            AudioManager.Pause(vMDAnimationController.Loop);

                        }
                        if (GUILayout.Button("Stop", gui[50f, 25f]))
                        {
                            VMDAnimationMgr.Instance.StopAll();
                            AudioManager.Stop();
                        }
                        GUILayout.EndHorizontal();

                        #endregion

                        #region option

                        #region time shift

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Sync Anm to BGM", gui[GUILayoutOptionUtill.Type.Width, 120f]);
                        if (GUILayout.Button(vMDAnimationController.SyncToBGM ? "On" : "Off", gui[GUILayoutOptionUtill.Type.Width, 40f]))
                        {
                            vMDAnimationController.SyncToBGM = !vMDAnimationController.SyncToBGM;
                        }
                        /*GUILayout.Space(30f);
						GUILayout.Label("Sync BGM to Anm", (GUILayoutOption[])new GUILayoutOption[1]
						{
								GUILayout.Width(120f)
						});
						if (GUILayout.Button(vMDAnimationController.SyncToAnim ? "On" : "Off", (GUILayoutOption[])new GUILayoutOption[1]
						{
								GUILayout.Width(40f)
						}))
						{
							vMDAnimationController.SyncToAnim = !vMDAnimationController.SyncToAnim;
						}*/
                        
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();



                        GUILayout.Label("time shift");
                        
                        if (!float.TryParse(GUILayout.TextField(timeShiftNow.ToString(), gui[GUILayoutOptionUtill.Type.Width, 40]),out timeShiftNow))
                            timeShiftNow = 0;

                        GUILayout.Label("min");
                        
                        if (!float.TryParse(GUILayout.TextField(timeShiftMin.ToString(), gui[GUILayoutOptionUtill.Type.Width, 40]), out timeShiftMin))
                            timeShiftMin = -60;

                        timeShiftNow= GUILayout.HorizontalSlider(timeShiftNow, timeShiftMin, timeShiftMax, gui[GUILayoutOptionUtill.Type.Width, 80]);
                        
                        if (!float.TryParse(GUILayout.TextField(timeShiftMax.ToString(), gui[GUILayoutOptionUtill.Type.Width, 40]), out timeShiftMax))
                            timeShiftMax = 60;

                        GUILayout.Label("max");

                        if (GUI.changed)
                        {
                            vMDAnimationController.timeShiftNow = timeShiftNow;

                        }
                        if (GUILayout.Button("All apply", gui[60, 25f]))
                        {
                            foreach (var item in CharacterMgrPatch.maids)
                            {
                                vMDAnimationControllerSub = VMDAnimationController.Install(item);
                                vMDAnimationControllerSub.timeShiftNow = timeShiftNow;
                            }
                        }
                        GUILayout.FlexibleSpace();
                        //vMDAnimationController = VMDAnimationController.Install(focusChara);

                        GUILayout.EndHorizontal(); 

                        #endregion



                        GUILayout.BeginHorizontal( );
                        vMDAnimationController.speed = AddSliderWithText("vmdAnimSpeed", "Speed", vMDAnimationController.speed, 5f);
                        GUILayout.EndHorizontal();


                        GUILayout.BeginHorizontal( );
                        GUILayout.Label("Loop", gui[GUILayoutOptionUtill.Type.Width, 40f]);
                        if (GUILayout.Button(vMDAnimationController.Loop ? "On" : "Off", gui[GUILayoutOptionUtill.Type.Width, 40f]))
                        {
                            vMDAnimationController.Loop = !vMDAnimationController.Loop;
                            AudioManager.SetLoop(vMDAnimationController.Loop);
                        }
                        GUILayout.Space(20f);
                        GUILayout.Label("Face", gui[GUILayoutOptionUtill.Type.Width, 40f]);
                        if (GUILayout.Button(vMDAnimationController.faceAnimeEnabled ? "On" : "Off", gui[GUILayoutOptionUtill.Type.Width, 40f]))
                        {
                            vMDAnimationController.faceAnimeEnabled = !vMDAnimationController.faceAnimeEnabled;
                        }
                        GUILayout.Space(10f);
                        GUILayout.Label("IK (foot)", gui[GUILayoutOptionUtill.Type.Width, 60f]);
                        if (GUILayout.Button(vMDAnimationController.enableIK ? "On" : "Off", gui[GUILayoutOptionUtill.Type.Width, 40f]))
                        {
                            vMDAnimationController.enableIK = !vMDAnimationController.enableIK;
                        }
                        GUILayout.Space(10f);
                        if (vMDAnimationController.enableIK)
                        {
                            GUILayout.Label("IK (toe)", gui[GUILayoutOptionUtill.Type.Width, 60f]);
                            if (GUILayout.Button(vMDAnimationController.IKWeight.disableToeIK ? "Off" : "On", gui[GUILayoutOptionUtill.Type.Width, 40f]))
                            {
                                vMDAnimationController.IKWeight.disableToeIK = !vMDAnimationController.IKWeight.disableToeIK;
                            }
                        }
                        /*GUILayout.Space(10f);
						GUILayout.Label("IK(Head)", (GUILayoutOption[])new GUILayoutOption[1]
						{
								GUILayout.Width(60f)
						});
						if (GUILayout.Button(vMDAnimationController.enableHeadRotate ? "On" : "Off", (GUILayoutOption[])new GUILayoutOption[1]
						{
								GUILayout.Width(40f)
						}))
						{
							vMDAnimationController.enableHeadRotate = !vMDAnimationController.enableHeadRotate;
						}*/
                        GUILayout.EndHorizontal();







                        if (vMDAnimationController.enableIK)
                        {
                            GUILayout.BeginHorizontal( );
                            vMDAnimationController.IKWeight.footIKPosWeight = AddSliderWithText("vmdIKFootPosWeight", "IK Weight(pos)", vMDAnimationController.IKWeight.footIKPosWeight, 1f);
                            GUILayout.EndHorizontal();
                            GUILayout.BeginHorizontal( );
                            vMDAnimationController.IKWeight.footIKRotWeight = AddSliderWithText("vmdIKFootRotWeight", "IK Weight(rot)", vMDAnimationController.IKWeight.footIKRotWeight, 1f);
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.BeginHorizontal( );
                        GUILayout.Label("Config: (needs Reload): ", gui[GUILayoutOptionUtill.Type.Width, 150f]);
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal( );
                        float num = AddSliderWithText("vmdCenterYPos", "(PMD)Center pos(y)", vMDAnimationController.centerBasePos.y, 15f);
                        if (num != vMDAnimationController.centerBasePos.y)
                        {
                            vMDAnimationController.centerBasePos.y = num;
                            //vMDAnimationController.centerBasePos = new Vector3(0f, num, 0f);
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal( );
                        float num2 = AddSliderWithTextFixedScale("(PMD)Hip pos(y)", vMDAnimationController.hipPositionAdjust.y, 1f, 6f);
                        if (num2 != vMDAnimationController.hipPositionAdjust.y)
                        {
                            vMDAnimationController.hipPositionAdjust.y = num2;
                            //vMDAnimationController.hipPositionAdjust = new Vector3(0f, num2, 0f);
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal( );
                        float num3 = AddSliderWithText("vmdModelScale", "Model Scale", vMDAnimationController.quickAdjust.ScaleModel, 2f);
                        if (num3 != vMDAnimationController.quickAdjust.ScaleModel)
                        {
                            vMDAnimationController.quickAdjust.ScaleModel = num3;
                        }
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal( );
                        vMDAnimationController.quickAdjust.Shoulder = AddSliderWithTextFixedScale("Shoulder Tilt", vMDAnimationController.quickAdjust.Shoulder, -10f, 40f);
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal( );
                        vMDAnimationController.quickAdjust.ArmUp = AddSliderWithTextFixedScale("Upper Arm Tilt", vMDAnimationController.quickAdjust.ArmUp, -10f, 40f);
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal( );
                        vMDAnimationController.quickAdjust.ArmLow = AddSliderWithTextFixedScale("Lower Arm Tilt", vMDAnimationController.quickAdjust.ArmLow, -10f, 40f);
                        GUILayout.EndHorizontal();
                        //GUILayout.Label("Other Config", (GUILayoutOption[])new GUILayoutOption[1]
                        //{
                        //	GUILayout.Width(150f)
                        //});
                        //GUILayout.BeginHorizontal( );
                        //vMDAnimationController.BgmVolume = AddSliderWithTextFixedScale("BGM volume", vMDAnimationController.BgmVolume, 0f, 1f);
                        //GUILayout.EndHorizontal();

                        #endregion
                    }

                    GUILayout.EndVertical();
                }
            }

            GUILayout.EndScrollView();
        }

        /*
		private void EnsureResourceLoaded()
		{
			if (m_fileImage == null)
			{
				m_fileImage = new Texture2D(32, 32, (TextureFormat)5, false);
				//m_fileImage.LoadImage(VMDResources.file_icon);
				m_fileImage.Apply();
				m_directoryImage = new Texture2D(32, 32, (TextureFormat)5, false);
				//m_directoryImage.LoadImage(VMDResources.folder_icon);
				m_directoryImage.Apply();
			}
		}
		*/
        /*
		protected void FileSelectedCallback(string path)
		{
			m_fileBrowser = null;
			lastFilename = path;
		}
		
		protected void FileSelectedCallbackOgg(string path)
		{
			m_fileBrowser = null;
			oggFilename = path;
		}
		*/

        public float AddSliderGeneral(string prop, string label, float value, float defaultMin, float defaultMax, bool fixedScale, bool useText)
        {
            
            GUILayout.Label(label, gui[GUILayoutOptionUtill.Type.Width, sliderLabelWidth]);
            GUILayout.Space(5f);
            float num;
            float num2;
            if (fixedScale)
            {
                num = defaultMin;
                num2 = defaultMax;
            }
            else
            {
                num = defaultMin;
                num2 = GetSliderMax(prop, defaultMax);
            }
            float result = GUILayout.HorizontalSlider(value, num, num2, gui[GUILayoutOptionUtill.Type.Width, sliderLabelWidth]);
            GUILayout.Space(5f);
            if (useText)
            {
                string text = value.ToString("F4");
                string text2 = GUILayout.TextField(text, gui[GUILayoutOptionUtill.Type.Width, valueLabelWidth]);
                if (text2 != text)
                {
                    float.TryParse(text2, out result);
                    //try
                    //{
                    //    result = float.Parse(text2);
                    //}
                    //catch (Exception)
                    //{
                    //}
                }
            }
            else
            {
                GUILayout.Label(value.ToString("F4"), gui[GUILayoutOptionUtill.Type.Width, valueLabelWidth]);
            }
            GUILayout.Space(5f);
            if (!fixedScale)
            {
                if (GUILayout.Button("0-1", gui[50,25]))
                {
                    SetSliderMax(prop, 1f);
                }
                if (GUILayout.Button("0-10", gui[50, 25]))
                {
                    SetSliderMax(prop, 10f);
                }
                if (GUILayout.Button("x2", gui[50, 25]))
                {
                    SetSliderMax(prop, GetSliderMax(prop, 1f) * 2f);
                }
            }
            return result;
        }

        public float AddSliderWithLabel(string prop, string label, float value, float defaultMax)
        {
            return AddSliderGeneral(prop, label, value, 0f, defaultMax, false, false);
        }

        public float AddSliderWithText(string prop, string label, float value, float defaultMax)
        {
            return AddSliderGeneral(prop, label, value, 0f, defaultMax, false, true);
        }

        public float AddSliderWithLabelFixedScale(string label, float value, float min, float max)
        {
            return AddSliderGeneral("", label, value, min, max, true, false);
        }

        public float AddSliderWithTextFixedScale(string label, float value, float min, float max)
        {
            return AddSliderGeneral("", label, value, min, max, true, true);
        }

        public float GetSliderMax(string key, float defaultMax)
        {
            if (sliderMax.ContainsKey(key))
            {
                return sliderMax[key];
            }
            return defaultMax;
        }

        public void SetSliderMax(string key, float value)
        {
            sliderMax[key] = value;
        }


    }
}
