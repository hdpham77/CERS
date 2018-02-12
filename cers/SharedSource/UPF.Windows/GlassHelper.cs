using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interop;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace UPF.Windows
{
	public static class GlassHelper {
		[StructLayout(LayoutKind.Sequential)]
		struct MARGINS {
			public MARGINS(Thickness t) {
				Left = (int)t.Left;
				Right = (int)t.Right;
				Top = (int)t.Top;
				Bottom = (int)t.Bottom;
			}

			public int Left;
			public int Right;
			public int Top;
			public int Bottom;
		}

		[DllImport("dwmapi.dll", PreserveSig = false)]
		private static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		private static extern bool DwmIsCompositionEnabled();

		public static bool PlatformSupported {
			get { return Environment.OSVersion.Version.Major >= 6; }
		}

		public static bool CompositionEnabled {
			get { return PlatformSupported && DwmIsCompositionEnabled(); }
		}

		public static void ExtendGlassFrame(Window window) {
			ExtendGlassFrame(window, Brushes.White);
		}

		public static void ExtendGlassFrame(Window window, Brush nonGlassBackground) {
			ExtendGlassFrame(window, new Thickness(-1), nonGlassBackground);
		}

		public static void ExtendGlassFrame(Window window, Thickness thickness) {
			ExtendGlassFrame(window, thickness, Brushes.White);
		}

		public static void ExtendGlassFrame(Window window, Thickness thickness, Brush nonGlassBackground) {
			if (PlatformSupported) {
				if (CompositionEnabled) {
					IntPtr hwnd = new WindowInteropHelper(window).Handle;
					if (hwnd == IntPtr.Zero) {
						throw new InvalidOperationException("The Window must be shown before extending glass.");
					}

					window.Background = Brushes.Transparent;
					HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = Colors.Transparent;

					MARGINS margins = new MARGINS(thickness);
					DwmExtendFrameIntoClientArea(hwnd, ref margins);
				}
				else {
					window.Background = nonGlassBackground;
				}
			}
			else {
				window.Background = nonGlassBackground;
			}
		}

		private static void ExtendGlassFrame(Window window, Border border) {
			if (CompositionEnabled) {
				// Obtain the window handle for WPF application
				IntPtr mainWindowPtr = new WindowInteropHelper(window).Handle;
				HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
				mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);

				// Get System Dpi
				System.Drawing.Graphics desktop = System.Drawing.Graphics.FromHwnd(mainWindowPtr);
				float DesktopDpiX = desktop.DpiX;
				float DesktopDpiY = desktop.DpiY;

				// Set Margins
				MARGINS margins = new MARGINS();

				// Extend glass frame into client area
				// Note that the default desktop Dpi is 96dpi. The  margins are
				// adjusted for the system Dpi.
				margins.Left = Convert.ToInt32(5 * (DesktopDpiX / 96));
				margins.Right = Convert.ToInt32(5 * (DesktopDpiX / 96));
				margins.Top = Convert.ToInt32(((int)border.Height + 5) * (DesktopDpiX / 96));
				margins.Bottom = Convert.ToInt32(5 * (DesktopDpiX / 96));

				int hr = DwmExtendFrameIntoClientArea(mainWindowSrc.Handle, ref margins);
				//
				if (hr < 0) {
					//if glass is not enabled.
					//DwmExtendFrameIntoClientArea Failed
				}
			}
		}
	}
}
