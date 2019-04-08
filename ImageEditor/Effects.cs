﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging.Filters;

namespace ImageEditor
{
    public static class Effects
    {
        public static Bitmap ToSepia(this Bitmap bitmap)
        {       
            var sepiaFilter = new Sepia();
            return sepiaFilter.Apply(AForge.Imaging.Image.Clone(bitmap, PixelFormat.Format24bppRgb));
        }

        public static Bitmap ToPixalation(this Bitmap bitmap)
        {
            var pixellateFilter = new Pixellate();
            return pixellateFilter.Apply(AForge.Imaging.Image.Clone(bitmap,PixelFormat.Format24bppRgb));
        }

        public static Bitmap resize(this Bitmap bitmap, int width, int height)
        {
            AForge.Imaging.Filters.ResizeBicubic filter = new AForge.Imaging.Filters.ResizeBicubic(width, height);
            return filter.Apply(AForge.Imaging.Image.Clone(bitmap, PixelFormat.Format24bppRgb));
        }

        public static Bitmap rotate(this Bitmap bitmap, int degrees)
        {
            AForge.Imaging.Filters.RotateBicubic filter = new AForge.Imaging.Filters.RotateBicubic(degrees);
            return filter.Apply(AForge.Imaging.Image.Clone(bitmap, PixelFormat.Format24bppRgb));
        }

        public static Bitmap ToBlackAndWhite(this Bitmap bitmap)
        {
            AForge.Imaging.Filters.Grayscale filter = Grayscale.CommonAlgorithms.Y;
            return filter.Apply(AForge.Imaging.Image.Clone(bitmap, PixelFormat.Format24bppRgb));
        }

        public static Bitmap gaussianBlur(this Bitmap bitmap, int blur)
        {
            AForge.Imaging.Filters.GaussianBlur filter = new AForge.Imaging.Filters.GaussianBlur(Convert.ToDouble(blur), 9);
            return filter.Apply(AForge.Imaging.Image.Clone(bitmap, PixelFormat.Format24bppRgb));
        }

        public static Bitmap ToJitter(this Bitmap bitmap)
        {
            AForge.Imaging.Filters.Jitter filter = new AForge.Imaging.Filters.Jitter();
            return filter.Apply(AForge.Imaging.Image.Clone(bitmap, PixelFormat.Format24bppRgb));
        }

        public static Bitmap ToInvert(this Bitmap bitmap)
        {
            AForge.Imaging.Filters.Invert filter = new AForge.Imaging.Filters.Invert();
            return filter.Apply(AForge.Imaging.Image.Clone(bitmap, PixelFormat.Format24bppRgb));
        }
    }
}