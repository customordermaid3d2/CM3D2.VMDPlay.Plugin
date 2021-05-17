using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CM3D2.VMDPlay.Plugin.GUIUtill
{
    public class GUILayoutOptionUtill
    { 

        private static GUILayoutOptionUtill instance;

        public static GUILayoutOptionUtill Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GUILayoutOptionUtill();
                }
                return instance;
            }
        }

        private static Dictionary<string, GUILayoutOption[]> gs = new Dictionary<string, GUILayoutOption[]>();

        private GUILayoutOptionUtill()
        {

        }


        public GUILayoutOption[] this[string key]
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

        public GUILayoutOption[] this[float Width, float Height]
        {
            get
            {
                string key = Width + "," + Height;
                if (!gs.ContainsKey(key))
                {
                    GUILayoutOption[] GUIStyle = new GUILayoutOption[]
                    {
                        GUILayout.Width(Width),
                        GUILayout.Height(Height)
                    };
                    gs.Add(key, GUIStyle);
                }
                return gs[key];
            }
        }
        
        public GUILayoutOption[] this[Type type, float value]
        {
            get
            {
                string key = type + "," + value;
                if (!gs.ContainsKey(key))
                {
                    GUILayoutOption[] GUIStyle = new GUILayoutOption[1];
                    switch (type)
                    {
                        case Type.Width:
                            GUIStyle[0] = GUILayout.Width(value);
                            break;
                        case Type.Height:
                            GUIStyle[0] = GUILayout.Height(value);
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
