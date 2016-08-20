using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ComputacaoGraficaProject.View;

namespace ComputacaoGraficaProject.Sintese
{
    public class Ponto
    {
        int tamanhoImagemX, tamanhoImagemY;

        /** 
         * Coordenadas do mundo para NDCs:
         * ndcX = (X - Xmin) / (Xmax - Xmin)
         * ndcY = (-Y - Ymin) / (Ymax - Ymin)
         * 
         * NDCs para dispositivos:
         * dcX = round(ndcX * (ndh - 1))
         * dcY = round(ndcY * (ndh - 1))
         * */
         
        public Ponto()
        {
            tamanhoImagemX = Referencias.sizeImageX;
            tamanhoImagemY = Referencias.sizeImageY;
        }

        public int X_ParaDispositivo(int x)
        {
            return Math.Abs(x - tamanhoImagemX);
        }

        public int Y_ParaDispositivo(int y)
        {
            return Math.Abs(y - tamanhoImagemY);
        }

        public int X_ParaMundo(int x)
        {
            return x - (tamanhoImagemX / 2);
        }

        public int Y_ParaMundo(int y)
        {
            return y - (tamanhoImagemY / 2);
        }

        public int X_MundoParaDispositivo(double x)
        {
            return (int)(x + (tamanhoImagemX / 2));
        }

        public int Y_MundoParaDispositivo(double y)
        {
            return (int)(tamanhoImagemY - (y + (tamanhoImagemY / 2)));
        }

        public double NormalizacaoX(int x)
        {
            return (double)x / tamanhoImagemX;
        }

        public double NormalizacaoY(int y)
        {
            return (double)y / tamanhoImagemY;
        }

        private Bitmap imagem;

        public void plotarPixel(double X, double Y, Color corDoPixel, Bitmap imagem)
        {
            this.imagem = imagem;

            int X_Dispositivo = X_MundoParaDispositivo(X);
            int Y_Dispositivo = Y_MundoParaDispositivo(Y);

            //Console.WriteLine(" X = " + X + " Y = " + Y);
            //Console.WriteLine(" X = " + X_Dispositivo + " Y = " + Y_Dispositivo);
            //Console.WriteLine("");

            imagem.SetPixel(X_Dispositivo, Y_Dispositivo, corDoPixel);

        }

        public Bitmap getImage()
        {
            return imagem;
        }
    }
}