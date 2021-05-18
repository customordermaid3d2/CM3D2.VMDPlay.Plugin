using COM3D2.Lilly.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CM3D2.VMDPlay.Plugin.VMDAnimationController;

namespace CM3D2.VMDPlay.Plugin.Utill
{
    public class MaidControlleUtill
    {
        //static List<VMDAnimationController> controllers = new List<VMDAnimationController>();
        static Dictionary<Maid, VMDAnimationController> maidControllers = new Dictionary<Maid, VMDAnimationController>();
        
        static int index=0;
        //static Maid maid;

        /// <summary>
        /// 활성화 대상만
        /// </summary>
        public static List<Maid> Maids { get; } = new List<Maid>();

        /// <summary>
        /// 활성화 대상만
        /// </summary>
        public static List<VMDAnimationController> Controllers { 
            get
            {
                return maidControllers.Values.ToList();
            }  
        }         

        public static int Count { get=> Maids.Count; }

        public static VMDAnimationController VMDAnimationController;

        public static string LastLoadedVMD { get => Controllers[index].lastLoadedVMD; set => Controllers[index].lastLoadedVMD = value; }

        public static Maid Maid { get; private set; } = null;

        internal static void Test()
        {
            foreach (var item in Maids)
            {
                MyLog.LogMessage("Test.Maids1", MyUtill.GetMaidFullName(item));
            }
            foreach (var item in Controllers)
            {
                MyLog.LogMessage("Test.Maids2", MyUtill.GetMaidFullName(item.maid));
            }
            foreach (var item in maidControllers)
            {
                MyLog.LogMessage("Test.Maids3", MyUtill.GetMaidFullName(item.Key));
                MyLog.LogMessage("Test.Maids4", MyUtill.GetMaidFullName(item.Value.maid));
            }
        }

        public unsafe static VMDAnimationController Install(Maid maid)
        {
            VMDAnimationController vMDAnimationController = maid.gameObject.GetComponent<VMDAnimationController>();
            if (vMDAnimationController == null)
            {
                if (maid.body0 == null || (maid.body0).m_Bones == null || (maid.body0).Face == null || !maid.body0.isLoadedBody)
                {
                    return null;
                }
                vMDAnimationController = maid.gameObject.AddComponent<VMDAnimationController>();
                vMDAnimationController.Init(maid);
                //VMDAnimationMgr.Instance.controllers.Add(vMDAnimationController);
                (maid.body0).m_Bones.gameObject.AddComponent<DestroyListener>().controller = vMDAnimationController;
            }
            return vMDAnimationController;
        }

        public static VMDAnimationController GetVMDAC(Maid maid)
        {
            Maid = maid;
            VMDAnimationController vMDAnimationController = maid.gameObject.GetComponent<VMDAnimationController>();
            if (vMDAnimationController == null)
            {
                //if (maid.body0 == null || (maid.body0).m_Bones == null || (maid.body0).Face == null || !maid.body0.isLoadedBody)
                //{
                //    return null;
                //}
                vMDAnimationController = maid.gameObject.AddComponent<VMDAnimationController>();
                vMDAnimationController.Init(maid);
                if (maidControllers.ContainsKey(maid))
                {
                    maidControllers[maid] = vMDAnimationController;
                }
                else
                {
                    maidControllers.Add(maid, vMDAnimationController);
                }
                //VMDAnimationMgr.Instance.controllers.Add(vMDAnimationController);
                (maid.body0).m_Bones.gameObject.AddComponent<DestroyListener>().controller = vMDAnimationController;
            }
            Maids.Add(maid);
            return vMDAnimationController;
        }

        public static void Remove(Maid maid)
        {
            if (Maids.Contains(maid))
            {
                Maids.Remove(maid);
            }
            if (maidControllers.ContainsKey(maid))
            {
                if (Controllers.Contains(maidControllers[maid]))
                {
                    Controllers.Remove(maidControllers[maid]);
                }
            }
            //Count = Maids.Count;
            if (Count==0)
            {
                MaidControlleUtill.Maid = null;
                //VMDAnimationController = null;
                //return;
            }
            if (MaidControlleUtill.Maid == maid)
            {
                PrevNextMaid();
            }
            MyLog.LogMessage("Remove", maidControllers.Count, Controllers.Count, Maids.Count, index, VMDAnimationController != null);
        }
       

        /// <summary>
        /// 최적화 완료?
        /// CharacterMgrPatch.cs로 옮겨서 구현하자
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        public static Maid PrevNextMaid(bool next=true)
        {
            if (Count>0)
            {
                index += (next ? 1 : (-1));
                index = (index + Count) % Count;
                //VMDAnimationController = maidControllers[Maid];
                MyLog.LogMessage("PrevNextMaid", maidControllers.Count, Controllers.Count, Maids.Count, index, VMDAnimationController != null);
                return Maid = Maids[index];
            }
            //VMDAnimationController = null;            
            MyLog.LogMessage("PrevNextMaid", maidControllers.Count, Controllers.Count, Maids.Count, index, VMDAnimationController != null);
            return Maid = null;
        }

    }
}
