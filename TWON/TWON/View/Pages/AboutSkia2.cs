using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaSharpFormsDemos
{
	public class AboutSkia2 : ContentPage
	{
		public AboutSkia2 ()
		{
			SKCanvasView canvasView = new SKCanvasView();
			canvasView.PaintSurface += OnCanvasViewPaintSurface;
			
			Content = canvasView;
		}
	
		//look up how to wrap a canvas view inside a scroll view so that the screen can be srolable.
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
				canvas.DrawCircle(info.Width / 6, info.Height / 5, 70, paint);

				paint.Style = SKPaintStyle.Stroke;
				paint.Color = SKColors.Blue;
				canvas.DrawCircle(info.Width / 6, info.Height / 5, 70, paint);



			SKPaint paint1 = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = Color.White.ToSKColor(),
				StrokeWidth = 15
			};
			canvas.DrawCircle(info.Width / 6, info.Height / 2, 70, paint1);

			paint1.Style = SKPaintStyle.Stroke;
			paint1.Color = SKColors.Black;
		
			canvas.DrawCircle(info.Width / 6, info.Height / 2, 70, paint1);


			SKPaint paint2 = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = Color.White.ToSKColor(),
				StrokeWidth = 15
			};
			//canvas.DrawCircle(info.Width / 6, info.Height / 2, 70, paint2);
			SKRect rect1 = SKRect.Create(250, 50, 500, 150);
			SKRoundRect rect2 = new SKRoundRect(rect1, 50, 50);
			canvas.DrawRoundRect(rect2, paint2);
		
			paint2.Style = SKPaintStyle.Stroke;
			paint2.Color = SKColors.Black;
			

			canvas.DrawRoundRect(rect2,  paint2);


			SKPaint paint3 = new SKPaint
			{
				Style = SKPaintStyle.Stroke,
				Color = Color.White.ToSKColor(),
				StrokeWidth = 15
			};
			//canvas.DrawCircle(info.Width / 6, info.Height / 2, 70, paint2);
			SKRect rect3 = SKRect.Create(250, 200, 500, 150);
			SKRoundRect rect4= new SKRoundRect(rect3, 50, 50);
			canvas.DrawRoundRect(rect4, paint3);

			paint3.Style = SKPaintStyle.Stroke;
			paint3.Color = SKColors.Blue;


			canvas.DrawRoundRect(rect4, paint3);


			canvas.DrawOval(info.Width / 2, info.Height / 2,
						   0.45f * info.Width, 0.45f * info.Height,
						   paint);

		}
	}
	}

