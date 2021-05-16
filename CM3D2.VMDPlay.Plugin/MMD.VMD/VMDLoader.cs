using System.IO;

namespace MMD.VMD
{
	public class VMDLoader
	{
		public static VMDFormat Load(BinaryReader bin, string path, string clip_name)
		{
			return new VMDFormat(bin, path, clip_name);
		}
	}
}
