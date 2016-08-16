using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using ComputacaoGraficaProject.View;
using ComputacaoGraficaProject.Sintese;
using ComputacaoGraficaProject.Business;
using ComputacaoGraficaProject.Sintese.Primitivas;

namespace ComputacaoGraficaProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            //apagarCamposSintese();

            Referencias.imagemDraw = imagemDraw;
            Referencias.imageDrawAbscissas = imagemDrawAbscissas;

            Referencias.listaRetas = listaRetas;
            Referencias.listViewRetas = listViewRetas;

            Referencias.listaTransformacoes = listaTransformacoes;
            Referencias.listViewTransformacoes = listViewTransformacoes;
        }

        /* ***********************************************************************************
         * -------------------- INÍCIO DA PARTE DE SÍNTESE DE IMAGEM ------------------------
         *************************************************************************************/

        private Boolean imagemIniciada = false;
        private Ponto ponto;

        private int sizeImageX = 0;
        private int sizeImageY = 0;
        
        private void apagarCamposSintese()
        {
            X_Reta.Text = ""; Y_Reta.Text = "";
            raioCircunferencia.Text = "";

            listViewRetas.Items.Clear();
            listaRetas = new List<int[]>();

            listViewTransformacoes.Items.Clear();
            listaTransformacoes = new List<int[]>();
        }
        
        private Boolean validacaoCamposCircunferencia()
        {
            if (raioCircunferencia.Text.Equals(""))
            {
                MessageBox.Show("Preencha o tamanho do raio.");
                return false;
            }
            if (int.Parse(raioCircunferencia.Text) > sizeImageX / 2
                || int.Parse(raioCircunferencia.Text) > sizeImageY / 2)
            {
                MessageBox.Show("O raio está fora do intervalo da imagem.");
                return false;
            }
            return true;
        }

        private Boolean validacaoImagem()
        {
            if (!imagemIniciada)
            {
                sizeImageX = (int)imagemLabelAbscissas.ActualWidth;
                sizeImageY = (int)imagemLabelAbscissas.ActualHeight;

                Referencias.sizeImageX = sizeImageX;
                Referencias.sizeImageY = sizeImageY;

                imagemDraw.ImageSource = null;
                new Imagem();
                ponto = new Ponto();
                resolucaoTela.Content = "Resolução da Tela: " + sizeImageX + " x " + sizeImageY;
                imagemIniciada = true;
            }
            return true;
        }

        private void limparTela_Click(object sender, EventArgs e)
        {
            imagemIniciada = false;
            validacaoImagem();
            apagarCamposSintese();

            /* Teste - Cria um quadrado */
            Referencias.listaRetas.Add(new int[] { 0, 0 });
            Referencias.listaRetas.Add(new int[] { 0, 100 });
            Referencias.listaRetas.Add(new int[] { 100, 100 });
            Referencias.listaRetas.Add(new int[] { 100, 0 });
            Referencias.listaRetas.Add(new int[] { 0, 0 });
            Retas retas = new Retas();
            retas.desenharRetas_PontoMedio(Referencias.listaRetas);
        }

        private void imagemDraw_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Cross;
        }

        private void imagemDraw_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void imagemDraw_MouseMove(object sender, MouseEventArgs e)
        {
            if (imagemIniciada)
            {
                int xAux, yAux;
                double xNorm, yNorm;
                xAux = ponto.X_ParaMundo((int)e.GetPosition(imagemLabel).X);
                yAux = ponto.Y_ParaDispositivo((int)e.GetPosition(imagemLabel).Y);
                yAux = ponto.Y_ParaMundo(yAux);
                pontoCoordenadaMundo.Content = "Coordenadas de Mundo: " + xAux + "; " + yAux;

                xNorm = ponto.NormalizacaoX((int)e.GetPosition(imagemLabel).X);
                yNorm = ponto.Y_ParaDispositivo((int)e.GetPosition(imagemLabel).Y);
                yNorm = ponto.NormalizacaoY((int)yNorm);
                pontoNormalizacao.Content = "Normalização: " + String.Format("{0:0.000}", xNorm)
                                            + "; " + String.Format("{0:0.000}", yNorm);

                yAux = ponto.Y_ParaDispositivo((int)e.GetPosition(imagemLabel).Y);
                pontoCoordenadaDispositivo.Content = "Coordenadas de Dispositivo: " + 
                                                    (int)e.GetPosition(imagemLabel).X + "; " + yAux;
            }
        }

        private void imagemDraw_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*if (validacaoImagem())
            {
                int[] coordenadas = new int[] { (int)e.GetPosition(imagemLabel).X, (int)e.GetPosition(imagemLabel).Y };
                listaRetas.Add(coordenadas);

                listViewRetas.Items.Add(new Functions.ObjectReta { X = e.GetPosition(imagemLabel).X + "", Y = e.GetPosition(imagemLabel).Y + "" });
            }*/
        }

        /* ----------------- INÍCIO: PARTE DE DESENHO DA CIRCUNFERÊNCIA ------------------------ */

        private void desenharCircunferencia_Click(object sender, RoutedEventArgs e)
        {
            if (listaRetas.Count > 0)
            {
                new Circunferencia(int.Parse(raioCircunferencia.Text));
            }
            else
            {
                MessageBox.Show("Preencha os pontos.");
            }
        }

        /* ----------------- FINAL: PARTE DE DESENHO DA CIRCUNFERÊNCIA ------------------------- */

        /* ------------------- INÍCIO: PARTE DE DESENHO DA RETA ------------------------------- */

        private void desenharReta_Click(object sender, RoutedEventArgs e)
        {
            if (listaRetas.Count > 0)
            {
                Button button = sender as Button;

                Retas retas = new Retas();

                if (button == bDesenharRetaDDA) // Desenha a Reta por DDA
                {
                    retas.desenharRetas_DDA(listaRetas);
                }
                else if (button == bDesenharRetaPontoMedio) // Desenha a Reta por Ponto Médio
                {
                    retas.desenharRetas_PontoMedio(listaRetas);
                }
            }
            else
            {
                MessageBox.Show("Preencha os pontos.");
            }
        }
        
        private Boolean validacaoCamposReta()
        {
            if (X_Reta.Text.Equals("") || Y_Reta.Text.Equals(""))
            {
                MessageBox.Show("Preencha as coordenadas.");
                return false;
            }
            if (int.Parse(X_Reta.Text) > sizeImageX / 2
                || int.Parse(Y_Reta.Text) > sizeImageY / 2)
            {
                MessageBox.Show("As coordenadas estão fora do intervalo da imagem.");
                return false;
            }
            return true;
        }
        
        private List<int[]> listaRetas = new List<int[]>();

        private void adicionarCoordenadaReta_Click(object sender, RoutedEventArgs e)
        {
            if (validacaoImagem() && validacaoCamposReta())
            {
                int[] coordenadas = new int[] { int.Parse(X_Reta.Text), int.Parse(Y_Reta.Text) };
                listaRetas.Add(coordenadas);

                listViewRetas.Items.Add(new Functions.ObjectReta { X = X_Reta.Text, Y = Y_Reta.Text });
                X_Reta.Text = ""; Y_Reta.Text = "";
            }
        }

        /* ------------------- FINAL: PARTE DE DESENHO DA RETA ------------------------------- */
        /* ------------------- INÍCIO: PARTE DE TRANSFORMAÇÕES ------------------------------- */

        private List<int[]> listaTransformacoes = new List<int[]>();

        private void transformar_Click(object sender, RoutedEventArgs e)
        {
            if (validacaoImagem())
            {
                Transformacoes transformacoes = new Transformacoes();
                transformacoes.conjuntoDeTransformacoes(listaTransformacoes);
            }

            listViewTransformacoes.Items.Clear();
            listaTransformacoes = new List<int[]>();
        }

        // Evento que envia para a classe de transformação, o tipo de transformação escolhida pelo usuário.
        private void bTransformacao_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            int[] info = null;

            if (button == tTransladar)
            {
                info = new int[] { 1, int.Parse(X_Translacao.Text), int.Parse(Y_Translacao.Text) };
                listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Transladar("+ int.Parse(X_Translacao.Text)  + ", "+ int.Parse(Y_Translacao.Text) + ")" });
            }
            else if (button == tEscalonar)
            {
                info = new int[] { 2, int.Parse(ampliacaoEscala.Text) };
                listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Escalonar(" + ampliacaoEscala.Text + ")" });
            }
            else if (button == tRotacionar)
            {
                info = new int[] { 3, int.Parse(anguloRotacao.Text) };
                listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Rotacionar(" + anguloRotacao.Text + ")" });
            }
            else if (button == tRefletir_1)
            {
                info = new int[] { 4, 1 };
                listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Refletir em X" });
            }
            else if (button == tRefletir_2)
            {
                info = new int[] { 4, 2 };
                listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Refletir em Y" });
            }
            else if (button == tRefletir_3)
            {
                info = new int[] { 4, 3 };
                listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Refletir em X e Y" });
            }
            else if (button == tCisalhar)
            {
                info = new int[] { 5, int.Parse(X_Cisalhamento.Text), int.Parse(Y_Cisalhamento.Text) };
                listViewTransformacoes.Items.Add(new Functions.ObjectTransformacao { Transformacao = "Cisalhar(" + X_Cisalhamento.Text + ", " + Y_Cisalhamento.Text + ")" });
            }

            listaTransformacoes.Add(info);
        }

        /* ------------------- FINAL: PARTE DE TRANSFORMAÇÕES ------------------------------- */

        /* ***********************************************************************************
         * ---------------------- FIM DA PARTE DE SÍNTESE DE IMAGEM --------------------------
         *************************************************************************************/
    }
}
