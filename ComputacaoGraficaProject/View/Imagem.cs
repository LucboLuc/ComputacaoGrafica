using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Media.Imaging;
using ComputacaoGraficaProject.View;

namespace ComputacaoGraficaProject.View
{
    class Imagem
    {
        private Bitmap imagem;

        public Imagem()
        {
            imagem = new Bitmap(Referencias.sizeImageX, Referencias.sizeImageY);

            using (Graphics graph = Graphics.FromImage(imagem))
            {
                Rectangle ImageSize = new Rectangle(0, 0, 1024, 1024);
                graph.FillRectangle(Brushes.White, ImageSize);
            }

            // Desenhar as abscissas.
            desenharAbscissas();

            // Atualiza a imagem.
            atualizarImagem();
        }

        // Desenhar as abscissas.
        private void desenharAbscissas()
        {
            int x = Convert.ToInt32(imagem.Width / 2);
            int y = Convert.ToInt32(imagem.Height / 2);

            for (int i = 0; i < imagem.Width; i++)
            {
                imagem.SetPixel(i, y, Color.Pink);
            }

            for (int j = 0; j < imagem.Height; j++)
            {
                imagem.SetPixel(x, j, Color.Pink);
            }
        }

        public void atualizarImagem()
        {
            BitmapSource bitmapSource = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    imagem.GetHbitmap(),
                    IntPtr.Zero,
                    Int32Rect.Empty,
            BitmapSizeOptions.FromEmptyOptions());

            Referencias.imageDrawAbscissas.ImageSource = bitmapSource;
        }
    }
}
