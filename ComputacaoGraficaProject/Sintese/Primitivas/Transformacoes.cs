using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using ComputacaoGraficaProject.View;
using ComputacaoGraficaProject.Sintese.Primitivas;

namespace ComputacaoGraficaProject.Sintese
{
    public class Transformacoes
    {
        private Bitmap imagem;
        private Ponto ponto;

        public Transformacoes()
        {
            ponto = new Ponto();
            imagem = new Bitmap(Referencias.sizeImageX, Referencias.sizeImageY);
        }

        public void method_Translacao(int translacao_X, int translacao_Y)
        {
            // Matriz da translacao
            List<double[]> matrizTranslacao = new List<double[]>();
            matrizTranslacao.Add(new double[] { translacao_X, translacao_Y });

            transformar_soma(matrizTranslacao);
        }

        public void method_Escala(int ampliacao)
        {
            // Matriz do Escala
            List<double[]> matrizEscala = new List<double[]>();
            matrizEscala.Add(new double[] { ampliacao, 0 });
            matrizEscala.Add(new double[] { 0, ampliacao });

            // matrizEscala.Add(new double[] { ampliacao, 0, 0 });
            // matrizEscala.Add(new double[] { 0, ampliacao, 0 });
            // matrizEscala.Add(new double[] { 0, 0, 1 });

            transformar_multiplicacao(matrizEscala);
        }

        public void method_Rotacao(int angulo)
        {
            int translacaoX = Referencias.listaRetas[0][0];
            int translacaoY = Referencias.listaRetas[0][0];

            if (translacaoX == 0 && translacaoY == 0)
            {
                double anguloRadianos = Math.PI * angulo / 180;

                // Matriz do Rotacao
                List<double[]> matrizRotacao = new List<double[]>();
                matrizRotacao.Add(new double[] { Math.Cos(anguloRadianos), -Math.Sin(anguloRadianos) });
                matrizRotacao.Add(new double[] { Math.Sin(anguloRadianos), Math.Cos(anguloRadianos) });

                transformar_multiplicacao(matrizRotacao);
            }
            else
            {
                List<int[]> transformacoes = new List<int[]>();
                transformacoes.Add(new int[] { 1, -translacaoX, -translacaoY });
                transformacoes.Add(new int[] { 3, angulo });
                transformacoes.Add(new int[] { 1, translacaoX, translacaoY });

                conjuntoDeTransformacoes(transformacoes);
            }
        }

        public void method_Reflexao(int tipo)
        {
            // Se tipo = 1, rotaciona em X.
            // Se tipo = 2, rotaciona em Y.
            // Se tipo = 3, rotaciona em X e Y.

            List<double[]> matrizCisalhamento = new List<double[]>();

            if (tipo == 1)
            {
                matrizCisalhamento.Add(new double[] { 1, 0 });
                matrizCisalhamento.Add(new double[] { 0, -1 });
            }
            else if (tipo == 2)
            {
                matrizCisalhamento.Add(new double[] { -1, 0 });
                matrizCisalhamento.Add(new double[] { 0, 1 });
            }
            else if (tipo == 3)
            {
                matrizCisalhamento.Add(new double[] { -1, 0 });
                matrizCisalhamento.Add(new double[] { 0, -1 });
            }

            transformar_multiplicacao(matrizCisalhamento);
        }

        public void method_Cisalhamento(int cisalhamento_X, int cisalhamento_Y)
        {
            Console.WriteLine("X = " + cisalhamento_X);
            Console.WriteLine("Y = " + cisalhamento_Y);
            // Matriz do cisalhamento
            List<double[]> matrizCisalhamento = new List<double[]>();
            matrizCisalhamento.Add(new double[] { 1+(cisalhamento_X*cisalhamento_Y), cisalhamento_X });
            matrizCisalhamento.Add(new double[] { cisalhamento_Y, 1 });

            transformar_multiplicacao(matrizCisalhamento);
        }

        /* Transformação do tipo multiplicação. */
        private void transformar_multiplicacao(List<double[]> matrizTransformacao)
        {
            // Calcular a multiplicação de matrizes. //

            List<int[]> matrizGerada = new List<int[]>();

            for (int i = 0; i < Referencias.listaRetas.Count; i++)
            {
                int coordenada_X = (int)((matrizTransformacao[0][0] * Referencias.listaRetas[i][0]) + (matrizTransformacao[0][1] * Referencias.listaRetas[i][1]));
                int coordenada_Y = (int)((matrizTransformacao[1][0] * Referencias.listaRetas[i][0]) + (matrizTransformacao[1][1] * Referencias.listaRetas[i][1]));
                int[] coordenadas = new int[] { coordenada_X, coordenada_Y };
                matrizGerada.Add(coordenadas);
            }

            Referencias.listaRetas = matrizGerada;
        }

        /* Transformação do tipo soma. */
        private void transformar_soma(List<double[]> matrizTransformacao)
        {
            List<int[]> matrizGerada = new List<int[]>();

            for (int i = 0; i < Referencias.listaRetas.Count; i++)
            {
                int coordenada_X = (int)(Referencias.listaRetas[i][0] + matrizTransformacao[0][0]);
                int coordenada_Y = (int)(Referencias.listaRetas[i][1] + matrizTransformacao[0][1]);
                int[] coordenadas = new int[] { coordenada_X, coordenada_Y };
                matrizGerada.Add(coordenadas);
            }

            Referencias.listaRetas = matrizGerada;
        }
        
        /* Percorre o conjunto de transformações e realiza todas em sequênca. */
        public void conjuntoDeTransformacoes(List<int[]> transformacoes)
        {
            // Explicação:
            // O array de números inteiros indica:
            // Posição 0: O número que indica qual será a transformação (1, 2, 3, 4 ou 5).
            // Posição n: Os parâmetros solicitados de acordo com a transformação.

            for (int i=0; i < transformacoes.Count; i++)
            {
                if (transformacoes[i][0] == 1)
                {
                    method_Translacao(transformacoes[i][1], transformacoes[i][2]);
                }
                else if (transformacoes[i][0] == 2)
                {
                    method_Escala(transformacoes[i][1]);
                }
                else if (transformacoes[i][0] == 3)
                {
                    method_Rotacao(transformacoes[i][1]);
                }
                else if (transformacoes[i][0] == 4)
                {
                    method_Reflexao(transformacoes[i][1]);
                }
                else if (transformacoes[i][0] == 5)
                {
                    method_Cisalhamento(transformacoes[i][1], transformacoes[i][2]);
                }
            }

            atualizarInterface();
        }

        /* Atualiza a lista de coordenadas e a imagem na tela */
        private void atualizarInterface()
        {
            Retas retas = new Retas();
            retas.desenharRetas_PontoMedio(Referencias.listaRetas);
        }
    }
}
