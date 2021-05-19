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
        public class SongMotion
        {
            [JsonProperty("Song")]
            //public string Song { get=> song; set=> song= MyUtill. EvaluateRelativePath(Environment.CurrentDirectory, value); }
            public string Song { get; set; }

            //private string song;
            //  foreach (string f_strFileName in Directory.GetFiles(Path.Combine(GameMain.Instance.SerializeStorageManager.StoreDirectoryPath, "Preset"), "*.preset", SearchOption.AllDirectories))

            [JsonProperty("Motions")]
            public List<string> Motions { get; set; }



            public SongMotion()
            {
                this.Song =  string.Empty;
                this.Motions = new List<string>();
            }

            public SongMotion(List<string> motions = null)
            {
                this.Motions = motions ?? new List<string>();
            }

            public SongMotion(string key, List<string> motions = null)
            {
                this.Song = string.Empty;
                this.Motions = motions ?? new List<string>();
            }
            public SongMotion(string key, string song, List<string> motions = null)
            {
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

        private static Dictionary<string, SongMotion> list;

        public static string path;

        public static void Serialize()
        {
            MyLog.LogMessage("SongMotionDic", "Serialize", path);
            File.WriteAllText(path, JsonConvert.SerializeObject(list, Formatting.Indented));
        }

        public static void Deserialize()
        {
            MyLog.LogMessage("SongMotionDic", "Deserialize", File.Exists(path));
            if (File.Exists(path))
            {
                list = JsonConvert.DeserializeObject<Dictionary<string, SongMotion>>(File.ReadAllText(path));
            }
            else
            {
                list = new Dictionary<string, SongMotion>();

                Set("Favorites name1", "song path1", "motion path1", "motion path2" );
                Set("Favorites name2", "song path2", "motion path1", "motion path2" );
                Set("Favorites name2", "song path2", "motion path1", "motion path2" );
                Set("Favorites name2", "song path2", "motion path1", "motion path2" );
                Set("Favorites name2", "song path2", "motion path1", "motion path2" );
                Set("Favorites name2", "song path2", "motion path1", "motion path2" );
                
                Serialize();
            }
        }

        public static void Set(string name, string song = null, params string[] motion)
        {
            MyLog.LogMessage("SongMotionDic", "Set", name, song);
            if (!list.ContainsKey(name))
            {
                list.Add(name, new SongMotion());
            }
            list[name].Song = song;
            if (motion != null && motion.Length > 0)
            {
                list[name].Motions.Clear();
                list[name].Motions.AddRange(motion);
            }
            Serialize();
        }

        public static void Del(string name)
        {
            MyLog.LogMessage("SongMotionDic", "Det", name);
            if (list.ContainsKey(name))
            {
                list.Remove(name);
                Serialize();
            }
        }

        public static Dictionary<string, SongMotion> GetList()
        {
            return list;
        }

        public static SongMotion Get(string name)
        {
            MyLog.LogMessage("SongMotionDic", "Get", name);
            //list.ContainsKey(name);
            if (list.TryGetValue(name,out SongMotion songMotion))
            {
                return songMotion;
            }
            return null;
        }
        /*
        */

    }
}
