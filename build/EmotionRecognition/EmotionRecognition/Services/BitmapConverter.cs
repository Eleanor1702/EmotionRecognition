using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EmotionRecognition.Services
{

	//This class with a static method to convert Bitmap objects produced by VideoStream to BitmapSource
	class BitmapConverter {
		public static BitmapSource ConvertToBitmapSource(Bitmap bitPic){
			if (bitPic == null){
				throw new ArgumentNullException("bitmap");
			}

			var rect = new Rectangle(0, 0, bitPic.Width, bitPic.Height);

			var bitmapData = bitPic.LockBits(rect, ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			try {
				var size = (rect.Width * rect.Height) * 4;

				return BitmapSource.Create(
					bitPic.Width,
					bitPic.Height,
					bitPic.HorizontalResolution,
					bitPic.VerticalResolution,
					PixelFormats.Bgra32,
					null,
					bitmapData.Scan0,
					size,
					bitmapData.Stride);
			} finally {
				bitPic.UnlockBits(bitmapData);
			}
		}
    }
}
