using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ScreenShare.WPF.Utils.Converters;

internal class BitmapToBitmapImage : IValueConverter
{
	[DllImport("gdi32.dll")]
	public static extern bool DeleteObject(IntPtr hObject);

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is not Bitmap btmp) { return null!; }

		BitmapImage bi = new BitmapImage();
		bi.BeginInit();
		MemoryStream ms = new MemoryStream();
		btmp.Save(ms, ImageFormat.Bmp);
		ms.Seek(0, SeekOrigin.Begin);
		bi.StreamSource = ms;
		bi.EndInit();
		return bi;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is not BitmapImage) { return null!; }

		// Convert BitmapImage to BitmapSource
		BitmapSource bitmapSource = (BitmapImage)value as BitmapSource;

		// Convert BitmapSource to Bitmap
		Bitmap bitmap = new Bitmap(
			bitmapSource.PixelWidth,
			bitmapSource.PixelHeight,
			PixelFormat.Format32bppPArgb);

		BitmapData data = bitmap.LockBits(
			new Rectangle(System.Drawing.Point.Empty, bitmap.Size),
			ImageLockMode.WriteOnly,
			PixelFormat.Format32bppPArgb);

		bitmapSource.CopyPixels(
			Int32Rect.Empty,
			data.Scan0,
			data.Height * data.Stride,
			data.Stride);

		bitmap.UnlockBits(data);

		return bitmap;
	}
}