using System;
using System.Drawing;
using System.Threading;

namespace BitmapSorter
{
   class BitmapLine
   {
      public int Index { get; private set; }

      public Color[] Pixels { get; private set; }

      public BitmapLine(int index, Color[] pixels)
      {
         Index = index;
         Pixels = pixels;
      }
   }

   class ScrambledBitmap
   {
      private static readonly Random rand = new Random();
      private readonly object _syncObj = new object();

      public bool IsSorted { get; private set; }

      public BitmapLine[] Lines { get; set; }

      public ScrambledBitmap(Bitmap image)
      {
         Lines = new BitmapLine[image.Height];
         for (int row = 0; row < image.Height; row++)
         {
            Lines[row] = new BitmapLine(row, new Color[image.Width]);
            for (int col = 0; col < image.Width; col++)
            {
               Lines[row].Pixels[col] = image.GetPixel(col, row);
            }
         }
         IsSorted = true;
      }

      public void Scramble()
      {
         for (int row = 0; row < Lines.Length; row++)
         {
            int targetRow = rand.Next(0, Lines.Length);
            SwapLines(row, targetRow);
         }
         IsSorted = false;
      }

      private void SwapLines(int i, int j)
      {
         BitmapLine temp = Lines[j];
         Lines[j] = Lines[i];
         Lines[i] = temp;
      }

      public void Sort()
      {
         bool changeMade = true;
         while (changeMade)
         {
            lock (_syncObj)
            {
               changeMade = false;
               for (int i = 0; i < Lines.Length - 1; i++)
               {
                  if (Lines[i].Index > Lines[i + 1].Index)
                  {
                     changeMade = true;
                     SwapLines(i, i + 1);
                  }
               }
            }
            Thread.Sleep(50);
         }
         IsSorted = true;
      }

      public Bitmap ToBitmap()
      {
         Bitmap bmp = new Bitmap(Lines[0].Pixels.Length, Lines.Length);
         lock (_syncObj)
         {
            for (int row = 0; row < bmp.Height; row++)
            {
               for (int col = 0; col < bmp.Width; col++)
               {
                  bmp.SetPixel(col, row, Lines[row].Pixels[col]);
               }
            }
         }
         return bmp;
      }
   }
}
