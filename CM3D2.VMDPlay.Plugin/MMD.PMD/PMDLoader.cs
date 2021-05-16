using System.IO;
using UnityEngine;

namespace MMD.PMD
{
	public class PMDLoader
	{
		public static PMDFormat Load(BinaryReader bin, GameObject caller, string path)
		{
			return new PMDFormat(bin, caller, path);
		}
	}
}
