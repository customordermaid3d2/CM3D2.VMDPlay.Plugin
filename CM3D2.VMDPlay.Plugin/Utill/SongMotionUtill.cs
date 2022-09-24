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
        public struct motionAndTime
        {
            public string motion;
            public float time;
            public float[] adjustParams;

            public motionAndTime(string motion, float time, float[] adjust = null, int count=0)
            {
                this.motion = motion??string.Empty;
                this.time = time;

                if(adjust != null && count > 0) {
                    this.adjustParams = new float[count];
                    for(int i=0;i<count;++i)
                        this.adjustParams[i] = adjust[i];
                }
                else this.adjustParams = null;
            }
        }

        public class SongMotion
        {
            [JsonProperty("Song")]
            //public string Song { get=> song; set=> song= MyUtill. EvaluateRelativePath(Environment.CurrentDirectory, value); }
            public string Song { get; set; }

            //private string song;
            //  foreach (string f_strFileName in Directory.GetFiles(Path.Combine(GameMain.Instance.SerializeStorageManager.StoreDirectoryPath, "Preset"), "*.preset", SearchOption.AllDirectories))

            /// <summary>
            /// 이전 세이브 호환용. 사용하지 말것
            /// </summary>
            [Obsolete("이전 세이브 호환용. 사용하지 말것.")]
            [JsonProperty("Motions",NullValueHandling =NullValueHandling.Ignore)]
            public List<string> Motions
            {
                set {
                    Motions2.Clear();
                    Motions2.AddRange(value.Select(x=> new motionAndTime(x,0) ) );
                }
            }

            [JsonProperty("Motions2", NullValueHandling = NullValueHandling.Ignore)]
            public List<motionAndTime> Motions2 { get; set; }



            public SongMotion()
            {
                this.Song = string.Empty;
                this.Motions2 = new List<motionAndTime>();
            }

            public SongMotion(List<motionAndTime> MotionsTime = null)
            {
                this.Motions2 = MotionsTime ?? new List<motionAndTime>();
            }

            public SongMotion(string Song, List<motionAndTime> MotionsTime = null) : this( MotionsTime)
            {
                this.Song = Song ?? string.Empty;
            }


            public motionAndTime this[int motion_index]
            {
                get
                {
                    return Motions2[motion_index];
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

                Set("Favorites name1", "song path1",new motionAndTime ( "motion path1" ,0 ));
                Set("Favorites name2", "song path2",new motionAndTime ( "motion path1" ,0 ));
                Set("Favorites name2", "song path2",new motionAndTime ( "motion path1" ,0 ));
                Set("Favorites name2", "song path2",new motionAndTime ( "motion path1" ,0 ));
                Set("Favorites name2", "song path2",new motionAndTime ( "motion path1" ,0 ));
                Set("Favorites name2", "song path2",new motionAndTime ( "motion path1" ,0 ));

                Serialize();
            }
        }

        public static void Set(string name, string song = null,params motionAndTime[] motion )
        {
            MyLog.LogMessage("SongMotionDic", "Set", name, song);
            if (!list.ContainsKey(name))
            {
                list.Add(name, new SongMotion());
            }
            list[name].Song = song;
            if (motion != null && motion.Length > 0)
            {
                list[name].Motions2.Clear();
                list[name].Motions2.AddRange(motion);
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
            if (list.TryGetValue(name, out SongMotion songMotion))
            {
                return songMotion;
            }
            return null;
        }
        /*
        */

    }
}
