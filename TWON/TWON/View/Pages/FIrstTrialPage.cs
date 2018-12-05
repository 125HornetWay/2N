using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace TWON.View.Pages
{
	public class FIrstTrialPage : ContentPage
	{
		public FIrstTrialPage ()
		{
			SKCanvasView canvasView = new SKCanvasView();
			canvasView.PaintSurface += OnCanvasViewPaintSurface;
			Content = canvasView;
		}

		void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
		{
			SKImageInfo info = args.Info;
			SKSurface surface = args.Surface;
			SKCanvas canvas = surface.Canvas;

			canvas.Clear();

			SKPaint paint = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = Color.White.ToSKColor(),
				StrokeWidth = 15
			};
			canvas.DrawCircle(info.Width / 4, info.Height / 4, 100, paint);

			paint.Style = SKPaintStyle.Stroke;
			paint.Color = SKColors.Blue;
			//canvas.DrawCircle(info.Width / 2, info.Height / 2, 100, paint);
		}
	}
}
