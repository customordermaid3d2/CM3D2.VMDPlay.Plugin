using COM3D2.Lilly.Plugin.Utill;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Yotogis;

namespace COM3D2.Lilly.Plugin
{
    /// <summary>
    /// 캐릭터 설정 관련
    /// </summary>
    public static class CharacterMgrPatch // 이름은 마음대로 지어도 되긴 한데 나같은 경우 정리를 위해서 해킹 대상 클래스 이름에다가 접미사를 붇임
    {
        public static Dictionary<int, Maid> maidList = new Dictionary<int, Maid>();
        public static List<Maid> maids = new List<Maid>();

        // private void SetActive(Maid f_maid, int f_nActiveSlotNo, bool f_bMan)
        [HarmonyPatch(typeof(CharacterMgr), "SetActive")]
        [HarmonyPostfix]
        public static void SetActive(Maid f_maid, int f_nActiveSlotNo, bool f_bMan)
        {
            if (!f_bMan)
            {
                maidList.Add(f_nActiveSlotNo, f_maid);
                maids.Add( f_maid);
            }
        }

        // public void Deactivate(int f_nActiveSlotNo, bool f_bMan)
        [HarmonyPatch(typeof(CharacterMgr), "Deactivate")]
        [HarmonyPrefix]
        public static void Deactivate(int f_nActiveSlotNo, bool f_bMan)
        {
            if (!f_bMan)
            {
                maids.Remove(maidList[f_nActiveSlotNo]);
                maidList.Remove(f_nActiveSlotNo);
            }
        }       
    }
}
