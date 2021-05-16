using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin.GUIUtill
{
    public class GUIStyleUtill
    {
        private static GUIStyleUtill instance;

        public static GUIStyleUtill Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GUIStyleUtill();
                }
                return instance;
            }
        }

        private static Dictionary<string, GUIStyle> DGUIStyle = new Dictionary<string, GUIStyle>();

        public GUIStyle this[string key]
        {
            get
            {
                if (!DGUIStyle.ContainsKey(key))
                {
                    return null;
                }
                return DGUIStyle[key];
            }
        }

        public GUIStyle this[float Width, float Height]
        {
            get
            {
                string key = Width + "," + Height;
                if (!DGUIStyle.ContainsKey(key))
                {
                    GUIStyle GUIStyle = new GUIStyle();
                    GUIStyle.fixedWidth = Width;
                    GUIStyle.fixedHeight = Height;
                    DGUIStyle.Add(key, GUIStyle);
                }
                return DGUIStyle[key];
            }
        }
        
        public GUIStyle this[Type type, float value]
        {
            get
            {
                string key = type + "," + value;
                if (!DGUIStyle.ContainsKey(key))
                {
                    GUIStyle GUIStyle = new GUIStyle();
                    switch (type)
                    {
                        case Type.Width:
                            GUIStyle.fixedWidth = value;
                            break;
                        case Type.Height:
                            GUIStyle.fixedHeight = value;
                            break;
                        default:
                            break;
                    }
                            DGUIStyle.Add(key, GUIStyle);
                }
                return DGUIStyle[key];
            }
        }

        public enum Type
        {
            Width
            , Height
        }
    }
}
