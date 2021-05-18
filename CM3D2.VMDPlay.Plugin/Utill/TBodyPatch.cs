using CM3D2.VMDPlay.Plugin.Utill;
using COM3D2.Lilly.Plugin.Utill;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM3D2.Lilly.Plugin
{
    class TBodyPatch// : AwakeUtill
    {
        //TBody

        /// <summary>
        /// public void LoadBody_R(string f_strModelFileName, Maid f_maid)
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="f"></param>
        [HarmonyPostfix, HarmonyPatch(typeof(TBody), "LoadBody_R")]
        public static void LoadBody_R(string f_strModelFileName, Maid f_maid)
        {
            MyLog.LogMessage("TBody.LoadBody_R"
                , MyUtill.GetMaidFullName(f_maid)
                , f_strModelFileName
                );
            MyLog.LogMessage("LoadBody_R", f_maid.body0 == null, (f_maid.body0).m_Bones == null, (f_maid.body0).Face == null, !f_maid.body0.isLoadedBody);
            MyLog.LogMessage("LoadBody_R", f_maid.IsCrcBody, f_maid.boMAN, MaidControlleUtill.Count);
            try
            {
                if (!f_maid.boMAN)
                {
                    MaidControlleUtill.GetVMDAC(f_maid);
                }
            }
            catch (Exception e)
            {
                MyLog.LogError(e);
            }
            

        }
    }
}
