using System;
using System.Windows.Forms;

namespace Colors2Cubase
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	internal sealed class Program
	{
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();			
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new UIForm());
		}
		
		private static void testMain(string[] args)
		{
			// 4294901760 = 255,0,0
			// 4278255104 = 0,255,0
			// 4278190334 = 0,0,255
			// 4278190080 = 0,0,0
			// 4294836478 = 255.255.0
			// 4278255358 = 0,255,255
			// 4294901502 = 255,255,255
			// 4294836478 = 255,0,255
			

			// https://code.google.com/p/tesla-engine/source/browse/trunk/Source/Tesla/Math/Color.cs?r=224
			Console.WriteLine("Black 0,0,0 = {0}", CubaseColor2Hex(4278190080));
			
			Console.WriteLine("White 255,255,255 = {0}", CubaseColor2Hex(4294901502));

			Console.WriteLine("Red 255,0,0 = {0}", CubaseColor2Hex(4294901760));
			
			Console.WriteLine("Green 0,255,0 = {0}", CubaseColor2Hex(4278255104));
			
			Console.WriteLine("Blue 0,0,255 = {0}", CubaseColor2Hex(4278190334));
			
			uint i = 4278190080;
			Console.WriteLine("4278190080 in hex 0x{0:X}", i);
			
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
			
			// 0x00000000 becomes 4278190080 when you set a pixel to black.
			// What calculation do I have to perform to turn non alpha hex color into a 1 alpha pixel32 color?
			// 4278190080 in base 16 is FF000000
			// ((R<<16) | (G<<8) | (B))
			
			//var color:uint = _buffer.getPixel32(x, y);
			//trace("0x" + color.toString(16)); // hexadecimal is base 16
		}
		
		private static string CubaseColor2Hex(long num) {

			num = num - 4278190080; // 4278190080 in base 16 is FF000000
			int b = 0;
			int g = 0;
			int r = 0;
			
			while (num > 65536) {
				b = b + 1;
				num = num - 65536;
			}
			while (num > 256) {
				g = g + 1;
				num = num - 256;
			}
			while (num > 1) {
				r = r + 1;
				num = num - 1;
			}
			return String.Format("RGB: {0}, {1}, {2}", r, g, b);
			//return Color3.new(r,g,b)
		}
	}
}