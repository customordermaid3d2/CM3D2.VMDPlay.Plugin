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
        public static List<VMDAnimationController> Controllers { get; private set; } = new List<VMDAnimationController>();
        

        public static int Count { get; private set; } = 0;
        public static VMDAnimationController VMDAnimationController { get; private set; }
        public static string LastLoadedVMD { get => Controllers[index].lastLoadedVMD; set => Controllers[index].lastLoadedVMD = value; }

        static Maid maid=null;

        public static Maid Maid
        {
            get {
                return maid;
            }
            private set
            {
                if (maid != null && maidControllers.ContainsKey(value))
                {
                    VMDAnimationController= maidControllers[value];
                    maid = value;
                }
                else
                {
                    VMDAnimationController = null;
                    maid = null;
                }
            }
        }

        public static VMDAnimationController GetVMDAC(Maid maid)
        {
            MyLog.LogMessage("GetVMDAC", maid.body0 != null, (maid.body0).m_Bones != null, (maid.body0).Face != null, !maid.body0.isLoadedBody);

            if (maidControllers.ContainsKey(maid))
            {
                if (!Controllers.Contains(maidControllers[maid]))
                {
                    Controllers.Add(maidControllers[maid]);
                }
                VMDAnimationController= maidControllers[maid];
            }
            else
            {
                VMDAnimationController = new VMDAnimationController();
                VMDAnimationController.Init(maid);

                maidControllers.Add(maid, VMDAnimationController);
                Controllers.Add(VMDAnimationController); 
            }
            Maids.Add(maid);
            Count= Maids.Count;
            MaidControlleUtill.Maid = maid;

            MyLog.LogMessage("GetVMDAC", maidControllers.Count, Controllers.Count, Maids.Count, index);
            return VMDAnimationController;
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
            Count = Maids.Count;
            if (Count==0)
            {
                MaidControlleUtill.Maid = null;
                //VMDAnimationController = null;
            }
            if (MaidControlleUtill.Maid == maid)
            {
                PrevNextMaid();
            }
            MyLog.LogMessage("Remove", maidControllers.Count, Controllers.Count, Maids.Count, index);
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
                return Maid = Maids[index];
            }
            //VMDAnimationController = null;
            Maid = null;
            return null;
        }

    }
}
