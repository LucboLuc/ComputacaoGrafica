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
        private List<List<double[]>> transformacoes = new List<List<double[]>>();

        public Transformacoes()
        {
            ponto = new Ponto();
            imagem = new Bitmap(Referencias.sizeImageX, Referencias.sizeImageY);
        }

        public void method_Translacao(double translacao_X, double translacao_Y)
        {
            // Matriz da translacao
            List<double[]> matrizTranslacao = new List<double[]>();
            matrizTranslacao.Add(new double[] {1, 0, translacao_X});
            matrizTranslacao.Add(new double[] { 0, 1, translacao_Y });
            matrizTranslacao.Add(new double[] { 0, 0, 1 });

            transformacoes.Add(matrizTranslacao);
        }

        public void method_Escala(double ampliacao)
        {
            double translacaoX = Referencias.listaRetas[0][0];
            double translacaoY = Referencias.listaRetas[0][1];

            method_Translacao(translacaoX, translacaoY);

            // Matriz do Escala
            List<double[]> matrizEscala = new List<double[]>();
            matrizEscala.Add(new double[] { ampliacao, 0, 0});
            matrizEscala.Add(new double[] { 0, ampliacao, 0});
            matrizEscala.Add(new double[] { 0, 0, 1});

            transformacoes.Add(matrizEscala);

            method_Translacao(-translacaoX, -translacaoY);
        }

        public void method_Rotacao(double angulo)
        {
            double translacaoX = Referencias.listaRetas[0][0];
            double translacaoY = Referencias.listaRetas[0][1];

            method_Translacao(translacaoX, translacaoY);

            double anguloRadianos = Math.PI * angulo / 180;

            // Matriz do Rotacao
            List<double[]> matrizRotacao = new List<double[]>();
            matrizRotacao.Add(new double[] { Math.Cos(anguloRadianos), -Math.Sin(anguloRadianos), 0});
            matrizRotacao.Add(new double[] { Math.Sin(anguloRadianos), Math.Cos(anguloRadianos), 0});
            matrizRotacao.Add(new double[] { 0, 0, 1 });

            transformacoes.Add(matrizRotacao);

            method_Translacao(-translacaoX, -translacaoY);
        }

        public void method_Reflexao(double tipo)
        {
            // Se tipo = 1, rotaciona em X.
            // Se tipo = 2, rotaciona em Y.
            // Se tipo = 3, rotaciona em X e Y.

            List<double[]> matrizCisalhamento = new List<double[]>();

            if (tipo == 1)
            {
                matrizCisalhamento.Add(new double[] { 1, 0, 0 });
                matrizCisalhamento.Add(new double[] { 0, -1, 0 });
                matrizCisalhamento.Add(new double[] { 0, 0, 1 });
            }
            else if (tipo == 2)
            {
                matrizCisalhamento.Add(new double[] { -1, 0, 0 });
                matrizCisalhamento.Add(new double[] { 0, 1, 0 });
                matrizCisalhamento.Add(new double[] { 0, 0, 1 });
            }
            else if (tipo == 3)
            {
                matrizCisalhamento.Add(new double[] { -1, 0, 0 });
                matrizCisalhamento.Add(new double[] { 0, -1, 0 });
                matrizCisalhamento.Add(new double[] { 0, 0, 1 });
            }

            transformacoes.Add(matrizCisalhamento);
        }

        public void method_Cisalhamento(double cisalhamento_X, double cisalhamento_Y)
        {
            double translacaoX = Referencias.listaRetas[0][0];
            double translacaoY = Referencias.listaRetas[0][1];

            method_Translacao(translacaoX, translacaoY);

            // Matriz do cisalhamento
            List<double[]> matrizCisalhamento = new List<double[]>();
            matrizCisalhamento.Add(new double[] { 1+(cisalhamento_X*cisalhamento_Y), cisalhamento_X, 0 });
            matrizCisalhamento.Add(new double[] { cisalhamento_Y, 1, 0 });
            matrizCisalhamento.Add(new double[] { 0, 0, 1 });

            transformacoes.Add(matrizCisalhamento);

            method_Translacao(-translacaoX, -translacaoY);
        }

        /* Transformação do tipo multiplicação. */
        private void transformar_multiplicacao(List<double[]> matrizTransformacao)
        {
            // Calcular a multiplicação de matrizes. //

            List<double[]> matrizGerada = new List<double[]>();

            for (int i = 0; i < Referencias.listaRetas.Count; i++)
            {
                double coordenada_X = (int)((matrizTransformacao[0][0] * Referencias.listaRetas[i][0]) + (matrizTransformacao[0][1] * Referencias.listaRetas[i][1]) + (matrizTransformacao[0][2] * 1));
                double coordenada_Y = (int)((matrizTransformacao[1][0] * Referencias.listaRetas[i][0]) + (matrizTransformacao[1][1] * Referencias.listaRetas[i][1]) + (matrizTransformacao[1][2] * 1));
                double[] coordenadas = new double[] { coordenada_X, coordenada_Y };
                matrizGerada.Add(coordenadas);
            }

            Referencias.listaRetas = matrizGerada;
        }

        /* Transformação do tipo soma. */
        private void transformar_soma(List<double[]> matrizTransformacao)
        {
            List<double[]> matrizGerada = new List<double[]>();

            for (int i = 0; i < Referencias.listaRetas.Count; i++)
            {
                int coordenada_X = (int)(Referencias.listaRetas[i][0] + matrizTransformacao[0][0]);
                int coordenada_Y = (int)(Referencias.listaRetas[i][1] + matrizTransformacao[0][1]);
                double[] coordenadas = new double[] { coordenada_X, coordenada_Y };
                matrizGerada.Add(coordenadas);
            }

            Referencias.listaRetas = matrizGerada;
        }
        
        private List<double[]> calcular_matriz_composta()
        {
            while(transformacoes.Count > 1)
            {
                List<double[]> matrizResulParcial = new List<double[]>();

                for (int i = 0; i < transformacoes[0].Count; i++)
                {
                    double posicao1 = 0, posicao2 = 0, posicao3 = 0;
                    for (int j = 0; j < transformacoes[0].Count; j++)
                    {
                        posicao1 = posicao1 + (transformacoes[0][i][j] * transformacoes[1][j][0]);
                        posicao2 = posicao2 + (transformacoes[0][i][j] * transformacoes[1][j][1]);                       
                        posicao3 = posicao3 + (transformacoes[0][i][j] * transformacoes[1][j][2]);
                    }
                    matrizResulParcial.Add(new double[]{ posicao1,posicao2,posicao3});
                }
                transformacoes.RemoveAt(0);
                transformacoes.RemoveAt(0);
                transformacoes.Insert(0, matrizResulParcial);
            }
            return transformacoes[0];
        }

        private void imprimirMatriz(List<double[]> matriz)
        {
            for(int i = 0; i < matriz.Count; i++)
            {
                for(int j = 0; j < matriz.Count; j++)
                {
                    Console.WriteLine(matriz[i][j] + "   ");
                }
                Console.WriteLine("a");
            }
        }

        /* Percorre o conjunto de transformações e realiza todas em sequênca. */
        public void conjuntoDeTransformacoes(List<double[]> transformacoes)
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

            transformar_multiplicacao(calcular_matriz_composta());
            transformacoes.Clear();

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
