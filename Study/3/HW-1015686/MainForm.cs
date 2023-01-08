using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetPlan
{
    public partial class MainForm : Form
    {
        private List<Point> PointList = new List<Point>();
        private List<PointContainer> PointContainerList = new List<PointContainer>();
        private int dim;

        public MainForm()
        {
            InitializeComponent();
        }

        private void panelShow_MouseDown(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            AddPoint(p);   // Добавление новой точки
            ShowPoints();  // Показ всех точек
            dim = PointList.Count;            
        }
        /// <summary>
        /// Добавление новой точки в список
        /// </summary>
        /// <param name="p">Переменная типа Point</param>
        private void AddPoint(Point p)
        {
            PointList.Add(p);
        }
        
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            AddPointInfo();
        }
        /// <summary>
        /// Добавление информации
        /// </summary>
        private void AddPointInfo()
        {
            PointContainer pointContainer = new PointContainer();
            PointContainerList.Add(pointContainer);            
            pointContainer.Parent = panelPoints;
            pointContainer.newEdgeBeg.Minimum = 0;
            pointContainer.newEdgeBeg.Maximum = dim-1;
            pointContainer.newEdgeEnd.Minimum = 0;
            pointContainer.newEdgeEnd.Maximum = dim-1;
            ShowPointsInfo();
        }
        /// <summary>
        /// Показ информации о дугах
        /// Номер начала, конца, длина
        /// </summary>
        private void ShowPointsInfo()
        {
            int num = 1;
            foreach (var p in PointContainerList)
            {
                p.Text = num.ToString();
                p.Visible = true;
                p.Location = new Point(10, 3 + (p.Height) * (num - 1));
                num++;
            }
        }
        /// <summary>
        /// Показ всех точек на графике
        /// </summary>
        private void ShowPoints()
        {
            Graphics g = panelShow.CreateGraphics();
            // Create pen.
            Pen blackPen = new Pen(Color.Black, 3);

            // Create size of ellipse.            
            int width = 2;
            int height = 2;
            foreach (var p in PointList)
            {
                // Draw ellipse to screen.
                g.DrawEllipse(blackPen, p.X, p.Y, width, height);
            }            
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            ShowPicture();
            buttonAdd.Enabled = true;
        }
        /// <summary>
        /// Показ всего графа - точки и дуги
        /// </summary>
        private void ShowPicture()
        {
            Graphics g = panelShow.CreateGraphics();
            panelShow.Refresh(); // Очистка изображения
            //Clear(Color.Transparent);  //clear all

            // Create pen.
            Pen blackPen = new Pen(Color.Black, 3);  // Перо - чёрное, толстое

            // Create size of ellipse.            
            int width = 2; int height = 2;
            Brush br = new SolidBrush(Color.Green);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            int i = 0;
            foreach (var p in PointList)
            {
                // Draw ellipse to screen.
                g.DrawEllipse(blackPen, p.X, p.Y, width, height);  //  Вершины графа
                // Номера вершин
                g.DrawString(i.ToString(), Font, br, p.X+1, p.Y+1); // Координаты размещения текста
                i++;
            }
            
            blackPen = new Pen(Color.Black, 1);  // Перо - чёрное, тонкое
            foreach (var p in PointContainerList)
            {
                int pb = (int)p.newEdgeBeg.Value;  // Точка начальная
                int pe = (int)p.newEdgeEnd.Value;  // Точка конечная
                //  Ребро между ними
                g.DrawLine(blackPen, PointList[pb].X, PointList[pb].Y, PointList[pe].X, PointList[pe].Y);
            }                
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {
            PerfomCalc();
        }
        /// <summary>
        /// Расчёты и показ результатов
        /// </summary>
        private void PerfomCalc()
        {           
            List<Edge> lEdge = new List<Edge>();
            string stringMess = "Оптимальное значение  ";

            //  Создаём новый список, преобразуя в значения нужных типов
            lEdge = PointContainerList.Select(p => new Edge
                { vBeg = (int)p.newEdgeBeg.Value, vEnd = (int)p.newEdgeEnd.Value, Val = double.Parse(p.newEdgeWeight.Text) }).ToList();            
            
            Calc calc = new Calc(PointList.Count);
            calc.Start = (int)numericUpDownStart.Value; // - 1;
            calc.End = (int)numericUpDownEnd.Value; // - 1;

            double[] val = calc.GetData(lEdge);
            double retVal = val[calc.End];
            if (retVal == double.MaxValue)
            {
                stringMess = "Нет пути";
                textBoxResult.Text = stringMess;
                MessageBox.Show(stringMess);
                return;
            }
            var Path = calc.Path;
            // Показ графа - заново
            ShowPicture();
            // Прорисовка пути
            ShowPath(Path);
            // Надпись о результате
            textBoxResult.Text = stringMess + retVal.ToString("F5");
        }
        /// <summary>
        /// Показ оптимального маршрута
        /// </summary>
        /// <param name="Path"></param>
        private void ShowPath(List<int> Path)
        {
            Graphics g = panelShow.CreateGraphics();
            // Create pen.
            Pen blackPen = new Pen(Color.Red, 2);  // Перо - красное, потолще
            
            var startNode = Path.First();  // Начальная точка
            Path.Remove(startNode);  // Убираем стартовую из маршрута

            // Прорисовка нового маршрута
            foreach (var node in Path)
            {
                g.DrawLine(blackPen, PointList[startNode].X, PointList[startNode].Y, PointList[node].X, PointList[node].Y);
                startNode = node;
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
