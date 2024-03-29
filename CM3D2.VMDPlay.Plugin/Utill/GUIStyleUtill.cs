﻿using System;
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

        private static Dictionary<string, GUIStyle> gs = new Dictionary<string, GUIStyle>();

        public GUIStyle this[string key]
        {
            get
            {
                if (!gs.ContainsKey(key))
                {
                    return null;
                }
                return gs[key];
            }
        }

        public GUIStyle this[float Width, float Height]
        {
            get
            {
                string key = Width + "," + Height;
                if (!gs.ContainsKey(key))
                {
                    GUIStyle GUIStyle = new GUIStyle();
                    GUIStyle.fixedWidth = Width;
                    GUIStyle.fixedHeight = Height;
                    gs.Add(key, GUIStyle);
                }
                return gs[key];
            }
        }
        
        public GUIStyle this[Type type, float value]
        {
            get
            {
                string key = type + "," + value;
                if (!gs.ContainsKey(key))
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
                            gs.Add(key, GUIStyle);
                }
                return gs[key];
            }
        }

        public enum Type
        {
            Width
            , Height
        }
    }
}
