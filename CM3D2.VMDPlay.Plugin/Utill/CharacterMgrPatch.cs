using CM3D2.VMDPlay.Plugin;
using COM3D2.Lilly.Plugin;
using COM3D2.Lilly.Plugin.Utill;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Yotogis;

namespace COM3D2.VMDPlay.Plugin
{
    /// <summary>
    /// 캐릭터 설정 관련
    /// </summary>
    public static class CharacterMgrPatch // 이름은 마음대로 지어도 되긴 한데 나같은 경우 정리를 위해서 해킹 대상 클래스 이름에다가 접미사를 붇임
    {
        // public static Dictionary<int, Maid> maidList = new Dictionary<int, Maid>();
        public static List<Maid> maids = new List<Maid>();        

        // private void SetActive(Maid f_maid, int f_nActiveSlotNo, bool f_bMan)
        [HarmonyPatch(typeof(Maid), "Visible", MethodType.Setter)]
        [HarmonyPrefix]
        public static void Visible(Maid __instance, bool value)
        {
            MyLog.LogMessage("Visible", MyUtill.GetMaidFullName( __instance), value, __instance.IsCrcBody, __instance.boMAN);
            if (value)
            {
                maids.Add(__instance);
                CM3D2VMDGUI.vMDAnimationController = VMDAnimationController.Install(__instance);
            }
            else
            {
                maids.Remove(__instance);
                var vMDAnimationController = VMDAnimationController.Install(__instance);
                vMDAnimationController.Stop();
                VMDAnimationMgr.Instance.controllers.Remove(vMDAnimationController);
            }
        }
            
        /*
        // private void SetActive(Maid f_maid, int f_nActiveSlotNo, bool f_bMan)
        [HarmonyPatch(typeof(CharacterMgr), "SetActive")]
        [HarmonyPrefix]
        public static void SetActive(Maid f_maid, int f_nActiveSlotNo, bool f_bMan)
        {
            
            if (!f_bMan)
            {                
                //CM3D2VMDGUI.vMDAnimationController = VMDAnimationController.Install(f_maid);                
                if (maidList.ContainsKey(f_nActiveSlotNo))
                {
                    if (maidList[f_nActiveSlotNo] == f_maid)
                    {
                        return;
                    }
                    if (maidList[f_nActiveSlotNo] != null)
                    {
                        if (maids.Contains(maidList[f_nActiveSlotNo]))
                        {
                            maids.Remove(maidList[f_nActiveSlotNo]);
                        }
                        maidList.Remove(f_nActiveSlotNo);
                    }
                }
                if (maids.Contains(f_maid))
                {
                    foreach (var item in maidList)
                    {
                        if (item.Value== f_maid)
                        {
                            maidList.Remove(item.Key);
                            break;
                        }
                    }
                }
                else
                {
                    maids.Add( f_maid);
                }
                maidList.Add(f_nActiveSlotNo, f_maid);
            }
            MyLog.LogMessage("CharacterMgrPatch.SetActive", MyUtill.GetMaidFullName(f_maid), f_nActiveSlotNo, f_bMan, maidList.Count, maids.Count, VMDAnimationMgr.Instance.controllers.Count);
        }

        // public void Deactivate(int f_nActiveSlotNo, bool f_bMan)
        [HarmonyPatch(typeof(CharacterMgr), "Deactivate")]
        [HarmonyPrefix]
        public static void Deactivate(int f_nActiveSlotNo, bool f_bMan)
        {
            if (!f_bMan)
            {
                if (maidList.ContainsKey(f_nActiveSlotNo))
                {
                    VMDAnimationMgr.Instance.controllers.Remove(VMDAnimationController.Install(maidList[f_nActiveSlotNo]));
                    if (maids.Contains(maidList[f_nActiveSlotNo]))
                    {
                        maids.Remove(maidList[f_nActiveSlotNo]);
                    }
                    maidList.Remove(f_nActiveSlotNo);
                }
            }
            MyLog.LogMessage("CharacterMgrPatch.Deactivate",f_nActiveSlotNo, f_bMan, maidList.Count, maids.Count, VMDAnimationMgr.Instance.controllers.Count);
        }       

        */
    }
}
