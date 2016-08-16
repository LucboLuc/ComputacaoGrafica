using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using ComputacaoGraficaProject.Business;

namespace ComputacaoGraficaProject.View
{
    public static class Referencias
    {
        public static Functions functions = new Functions();

        public static ImageBrush imagemDraw;
        public static ImageBrush imageDrawAbscissas;

        public static int sizeImageX;
        public static int sizeImageY;

        public static ListView listViewRetas;
        public static List<int[]> listaRetas;

        public static ListView listViewTransformacoes;
        public static List<int[]> listaTransformacoes;
    }
}
