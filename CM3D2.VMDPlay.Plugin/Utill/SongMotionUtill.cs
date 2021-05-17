using COM3D2.Lilly.Plugin;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CM3D2.VMDPlay.Plugin.Utill
{
    class SongMotionUtill
    {
        /*
        public class RootObject
        {
            [JsonProperty("songMotionDic")]
            public SongMotionDic songMotionDic { get; set; }
        }

        public class SongMotionDic
        {
            [JsonProperty("list")]
            public Dictionary<string, SongMotion> list { get; set; }
        }
        */
        public class SongMotion
        {
            [JsonProperty("Key")]
            public string Key { get; set ; }

            [JsonProperty("Song")]
            public string Song { get; set; }

            [JsonProperty("Motions")]
            public List<string> Motions { get; set; }

            public SongMotion()
            {
                this.Key =  string.Empty;
                this.Song =  string.Empty;
                this.Motions = new List<string>();
            }

            public SongMotion(List<string> motions = null)
            {
                this.Motions = motions ?? new List<string>();
            }

            public SongMotion(string key, List<string> motions = null)
            {
                this.Key = key ?? string.Empty;
                this.Motions = motions ?? new List<string>();
            }
            public SongMotion(string key, string song, List<string> motions = null)
            {
                this.Key = key ?? string.Empty;
                this.Song = song ?? string.Empty;
                this.Motions = motions ?? new List<string>();
            }

            public string this[int motion_index]
            {
                get
                {
                    return Motions[motion_index];
                }
            }
        }

        //public static List<SongMotion> list = new List<SongMotion>();
        public static Dictionary<string, SongMotion> list;

        public static string path;// CM3D2VMDPlugin.Instance.Config.ConfigFilePath + @"\CM3D2.VMDPlay.Plugin.json";
        //public static SongMotionList List;
       // public static RootObject rootObject;

        public static void Serialize()
        {
            MyLog.LogMessage("SongMotionDic", "Serialize", path);
            //File.WriteAllText(path, JsonConvert.SerializeObject(rootObject, Formatting.Indented));
            File.WriteAllText(path, JsonConvert.SerializeObject(list, Formatting.Indented));
        }

        public static void Deserialize()
        {
            MyLog.LogMessage("SongMotionDic", "Deserialize", File.Exists(path));
            if (File.Exists(path))
            {
                //rootObject = JsonConvert.DeserializeObject<RootObject>(File.ReadAllText(path));
                //list = JsonConvert.DeserializeObject<List<SongMotion>>(File.ReadAllText(path));
                list = JsonConvert.DeserializeObject<Dictionary<string, SongMotion>>(File.ReadAllText(path));
                //list = rootObject.songMotionDic.list;
            }
            else
            {

                //list.Add( new SongMotion("song path1", "Favorites name1", new List<string> { "motion path1", "motion path2" }));
                //list.Add( new SongMotion("song path2","Favorites name2", new List<string> { "motion path1", "motion path2" }));

                //                rootObject =new RootObject();
                //                rootObject.songMotionDic = new SongMotionDic();
                //                rootObject.songMotionDic.list = list = new Dictionary<string, SongMotion>();
                list = new Dictionary<string, SongMotion>();
                list.Add("Favorites name1", new SongMotion("song path1", new List<string> { "motion path1", "motion path2" }));
                list.Add("Favorites name2", new SongMotion("song path2", new List<string> { "motion path1", "motion path2" }));
                
                Serialize();
            }
        }
        /*
        public static void Set(string name, string song = null, params string[] motion)
        {
            MyLog.LogMessage("SongMotionDic", "Set", name, song);
            if (!List.list.ContainsKey(name))
            {
                List.list.Add(name, new SongMotion());
            }
            List.list[name].song = song;
            if (motion != null && motion.Length > 0)
            {
                List.list[name].motions.AddRange(motion);
            }
            Serialize();
        }

        public static void Det(string name)
        {
            MyLog.LogMessage("SongMotionDic", "Det", name);
            List.list.Remove(name);
            Serialize();
        }
        */

    }
}
