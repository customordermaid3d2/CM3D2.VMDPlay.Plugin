using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COM3D2.Lilly.Plugin
{
    class MyUtill
    {
		/// <summary>
		/// 닷넷 3.5에선 컴파일 안되서 직접 복사 수정
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="separator"></param>
		/// <param name="values"></param>
		/// <returns></returns>
		//public static string Join(string separator, params object[] values)
        //{
        //    return Join<object>(separator, values);
        //}
       

		public static string Join<T>(string separator, IEnumerable<T> values)
		{
			if (values == null)
			{
				return "";
			}
			if (separator == null)
			{
				separator = string.Empty;
			}
			string result;
			using (IEnumerator<T> enumerator = values.GetEnumerator())
			{
				if (!enumerator.MoveNext())
				{
					result = string.Empty;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (enumerator.Current != null)
					{
						T t = enumerator.Current;
						string text = t.ToString();
						if (text != null)
						{
							stringBuilder.Append(text);
						}
					}
					while (enumerator.MoveNext())
					{
						stringBuilder.Append(separator);
						if (enumerator.Current != null)
						{
							T t = enumerator.Current;
							string text2 = t.ToString();
							if (text2 != null)
							{
								stringBuilder.Append(text2);
							}
						}
					}
					result = stringBuilder.ToString();
				}
			}
			return result;
		}

        internal static string GetMaidFullName(Maid maid)
        {
            if (maid==null)
            {
                return "null";
            }
            //return maid.status.fullNameEnStyle+" , "+maid.status.heroineType;
            return Join(" , " 
                , new object[] {
                      maid.status.personal.replaceText
                    , maid.status.fullNameEnStyle
                    , maid.status.heroineType
                    , maid.status.contract
                }
            );

            /*
            StringBuilder s = new StringBuilder();
			if (maid.status != null)
			{
				s.Append(maid.status.fullNameEnStyle);
				//s.Append(maid.status.firstName);
				//s.Append(" , " + maid.status.lastName);
				//if (maid.status.personal != null)
				//{
				//	s.Append(" , " + maid.status.personal.id);
				//	s.Append(" , " + maid.status.personal.replaceText);
				//	s.Append(" , " + maid.status.personal.uniqueName);
				//	s.Append(" , " + maid.status.personal.drawName);
				//}
			}
			return s.ToString();
            */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodBase"></param>
        /// <returns></returns>
        public static string GetClassMethodName(System.Reflection.MethodBase methodBase)
        {
			return methodBase.ReflectedType.Name+"."+methodBase.Name+":" ;
        }

		/// <summary>
		/// 프리셋에서 불러온 메뉴들을 반납
		/// </summary>
		/// <param name="f_prest"></param>
		/// <returns></returns>
		public static MaidProp[] getMaidProp(CharacterMgr.Preset f_prest)
		{
			// f_prest.strFileName
			//
			MaidProp[] array;
			if (f_prest.ePreType == CharacterMgr.PresetType.Body)
			{
				array = (from mp in f_prest.listMprop
						 where (1 <= mp.idx && mp.idx <= 80) || (115 <= mp.idx && mp.idx <= 122)
						 select mp).ToArray<MaidProp>();
			}
			else if (f_prest.ePreType == CharacterMgr.PresetType.Wear)
			{
				array = (from mp in f_prest.listMprop
						 where 81 <= mp.idx && mp.idx <= 110
						 select mp).ToArray<MaidProp>();
			}
			else
			{
				array = (from mp in f_prest.listMprop
						 where (1 <= mp.idx && mp.idx <= 110) || (115 <= mp.idx && mp.idx <= 122)
						 select mp).ToArray<MaidProp>();
			}

			return array;
		}

        /// <summary>
        /// 출처: https://cacodemon.tistory.com/entry/랜덤-열거형값-얻기-Random-Enum [카코데몬의 잡동사니]
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T RandomEnum<T>()
        {
            Array values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args">제외할 목록</param>
        /// <returns></returns>
        public static T RandomEnum<T>(params T[] args)
        {
            //Array values = Enum.GetValues(typeof(T));
            List<T> lst = ((T[])Enum.GetValues(typeof(T))).ToList();
            for (int i = 0; i < args.Length; i++)
            {
                lst.Remove(args[i]);
            }            
            return lst[UnityEngine.Random.Range(0, lst.Count)];
            //return lst[new Random().Next(0, lst.Count)];
            //return (T)values.GetValue(new Random().Next(0, values.Length));
        }

		/// <summary>
		/// C:\Users\lilly\source\repos\HarmonyXTest\HarmonyXTest\bin\Debug        
		/// C:\Users\lilly\source\repos\HarmonyXTest\HarmonyXTest\bin\Debug\test
		/// test
		/// </summary>
		/// <param name="mainDirPath">제거할 경로 Environment.CurrentDirectory</param>
		/// <param name="absoluteFilePath">절대경로</param>
		/// <returns></returns>
		public static string EvaluateRelativePath(String mainDirPath, String absoluteFilePath)
		{
			// 입력값 검증
			if (String.IsNullOrEmpty(mainDirPath)) return absoluteFilePath; //new ArgumentNullException("mainDirPath 입력값이 null 또는 공백입니다.");
			if (String.IsNullOrEmpty(absoluteFilePath)) return absoluteFilePath; //throw new ArgumentNullException("absoluteFilePath 입력값이 null 또는 공백입니다.");
			if (Path.GetPathRoot(mainDirPath) != Path.GetPathRoot(absoluteFilePath)) return absoluteFilePath; //throw new ArgumentException("입력값의 루트가 다르므로 처리할 수 없습니다.");
			if (Path.IsPathRooted(mainDirPath) == false) return absoluteFilePath; //throw new ArgumentException("mainDirPath 이 절대경로가 아닙니다.");
			if (Path.IsPathRooted(absoluteFilePath) == false)  return absoluteFilePath;//throw new ArgumentException("absoluteFilePath 이 절대경로가 아닙니다.");
			

			// 입력값 보정, C:\test 일때 test가 파일인지 디렉토리인지 애매하다
			mainDirPath = mainDirPath.Trim();
			absoluteFilePath = absoluteFilePath.Trim();
			if (Directory.Exists(mainDirPath + Path.DirectorySeparatorChar)) mainDirPath = mainDirPath + Path.DirectorySeparatorChar;
			if (Directory.Exists(absoluteFilePath + Path.DirectorySeparatorChar)) absoluteFilePath = absoluteFilePath + Path.DirectorySeparatorChar;

			// 상대 경로 추출
			Uri mainDirUri = new Uri(mainDirPath);
			Uri absoluteFileUri = new Uri(absoluteFilePath);
			Uri relativeUri = mainDirUri.MakeRelativeUri(absoluteFileUri);
			String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

			// 리턴
			return relativePath.Replace('/', Path.DirectorySeparatorChar);
		}


	}
}
