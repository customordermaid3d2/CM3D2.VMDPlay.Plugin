using System;
using System.Text;

namespace HexDump
{
	internal class Utils
	{
		public static string HexDump(byte[] bytes, int bytesPerLine = 16)
		{
			if (bytes == null)
			{
				return "<null>";
			}
			int num = bytes.Length;
			char[] array = "0123456789ABCDEF".ToCharArray();
			int num2 = 11;
			int num3 = num2 + bytesPerLine * 3 + (bytesPerLine - 1) / 8 + 2;
			int num4 = num3 + bytesPerLine + Environment.NewLine.Length;
			char[] array2 = (new string(' ', num4 - Environment.NewLine.Length) + Environment.NewLine).ToCharArray();
			StringBuilder stringBuilder = new StringBuilder((num + bytesPerLine - 1) / bytesPerLine * num4);
			for (int i = 0; i < num; i += bytesPerLine)
			{
				array2[0] = array[(i >> 28) & 0xF];
				array2[1] = array[(i >> 24) & 0xF];
				array2[2] = array[(i >> 20) & 0xF];
				array2[3] = array[(i >> 16) & 0xF];
				array2[4] = array[(i >> 12) & 0xF];
				array2[5] = array[(i >> 8) & 0xF];
				array2[6] = array[(i >> 4) & 0xF];
				array2[7] = array[i & 0xF];
				int num5 = num2;
				int num6 = num3;
				for (int j = 0; j < bytesPerLine; j++)
				{
					if (j > 0 && (j & 7) == 0)
					{
						num5++;
					}
					if (i + j >= num)
					{
						array2[num5] = ' ';
						array2[num5 + 1] = ' ';
						array2[num6] = ' ';
					}
					else
					{
						byte b = bytes[i + j];
						array2[num5] = array[(b >> 4) & 0xF];
						array2[num5 + 1] = array[b & 0xF];
						array2[num6] = (char)((b < 32) ? 45 : ((int)b));
					}
					num5 += 3;
					num6++;
				}
				stringBuilder.Append(array2);
			}
			return stringBuilder.ToString();
		}
	}
}
