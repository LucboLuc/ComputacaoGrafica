using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ComputacaoGraficaProject.View;

namespace ComputacaoGraficaProject.Sintese
{
    class Circunferencia
    {
        private Bitmap imagem;
        private Ponto ponto;

        public Circunferencia(int raio)
        {
            ponto = new Ponto();
            imagem = new Bitmap(Referencias.sizeImageX, Referencias.sizeImageY);

            methodPontoMedio(raio);
            
            Referencias.functions.atualizarImagem(imagem);
        }

        /** 
         * Algoritmo para desenhar o Círculo com algoritmo do Ponto Médio:
         * 
         * IncE = (2 * x) + 3
         * IncSE = (2 * (x - y)) + 5
         * d = (1 - raio)
         * d += IncE || d += IncSE
         * */

        private void methodPontoMedio(int raio)
        {
            int x = 0;
            int y = raio;
            double d = ((double)1 - (double)raio);

            plotarPixel(x, y);

            while (y > x)
            {
                if (d < 0)
                {
                    double IncE = (2 * x) + 3;
                    d += IncE;
                }
                else
                {
                    double IncSE = (2 * (x - y)) + 5;
                    d += IncSE;
                    y--;
                }
                x++;
                plotarPixel(x, y);
            }
        }

        private void plotarPixel(int x, int y)
        {
            ponto.plotarPixel(x, y, Color.Blue, imagem);
            ponto.plotarPixel(y, x, Color.Blue, imagem);
            ponto.plotarPixel(y, -x, Color.Blue, imagem);
            ponto.plotarPixel(x, -y, Color.Blue, imagem);
            ponto.plotarPixel(-x, -y, Color.Blue, imagem);
            ponto.plotarPixel(-y, -x, Color.Blue, imagem);
            ponto.plotarPixel(-y, x, Color.Blue, imagem);
            ponto.plotarPixel(-x, y, Color.Blue, imagem);
        }
    }
}
