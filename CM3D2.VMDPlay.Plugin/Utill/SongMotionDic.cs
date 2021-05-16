using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CM3D2.VMDPlay.Plugin.Utill
{
    class SongMotionDic
    {
        // 
        public static Dictionary<string, SongMotion> list = new Dictionary<string, SongMotion>();

        public static string json;
        public static string path;// CM3D2VMDPlugin.Instance.Config.ConfigFilePath + @"\CM3D2.VMDPlay.Plugin.json";

        public static void Serialize()
        {
            File.WriteAllText(CM3D2VMDPlugin.Instance.Config.ConfigFilePath + @"\CM3D2.VMDPlay.Plugin.json", JsonConvert.SerializeObject(list, Formatting.Indented));
        }
        
        public static void Deserialize()
        {           
            if (File.Exists(path))
            {
                list = JsonConvert.DeserializeObject<Dictionary<string, SongMotion>>(File.ReadAllText(CM3D2VMDPlugin.Instance.Config.ConfigFilePath+ @"\CM3D2.VMDPlay.Plugin.json"));
            }
        }

        public static void Set(string name,string song=null, params string[] motion)
        {
            if (!list.ContainsKey(name))
            {
                list.Add(name, new SongMotion());
            }
            list[name].song = song;
            if (motion!=null && motion.Length>0)
            {
                list[name].motions.AddRange(motion);
            }
            Serialize();
        }

        public static void Det(string name)
        {
            list.Remove(name);
            Serialize();
        }

        public class SongMotion
        {
            public string song = string.Empty;
            public List<string> motions = new List<string>();

            public string this[int motion_index]
            {
                get
                {
                    return motions[motion_index];
                }
            }
        }
    }
}
