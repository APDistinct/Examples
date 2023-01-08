using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetPlan
{
    public class Edge
    {
        public int vBeg { get; set; }  // Номер начальной вершины
        public int vEnd { get; set; }  // Номер конечной вершины
        public double Val { get; set; } // Вес ребра
    }
    public class Calc
    {        
        private double[,] A; // Матрица весов графа
        private int[] prev; // Массив предшественников
        public List<int> Path { get; set; } = new List<int>();   // Путь от начала к концу
        public int Start { get; set; }  // Начальная вершина пути
        public int End { get; set; }  // Конечная вершина пути
        public int dimA { get; set; }  // Количество вершин
        
        public Calc(int dim)
        {
            dimA = dim;
            A = new double[dimA, dimA];
            prev = new int[dimA];
        }
        
        /// <summary>
        /// Получение параметров сети, формирование матрицы, расчёт
        /// </summary>
        /// <param name="Coeff">Параметры графа</param>
        public double[] GetData(List<Edge> Coeff)
        {
            double d = 0; // double.MaxValue / dimA;
            //for (int i = 0; i < dimA; ++i) 
            //{
            //    A[i, i] = 0;  // Вес дуги от вершины к ней же
            //}
            for (int i = 0; i < dimA; ++i)  
            {
                for (int j = 0; i < dimA; ++i)  
                {
                    A[i, j] = d;  // Все отсутствующие рёбра - с нулевым --максимальным-- весом
                }
            }
            // Заполнение матрицы коэффициентов

            foreach (var c in Coeff)
            {
                A[c.vBeg, c.vEnd] = c.Val;
                A[c.vEnd, c.vBeg] = c.Val; // Предполагаем симметричность матрицы
            }            

            //SaveA("ap.csv");  // Промежуточное сохранение матрицы коэффициентов - проверка
            // Создание объекта для решения задачи
            double[] rett = new double[dimA];
            DStand dStand = new DStand();
            // Получаем пути ко всем вершинам графа
            rett = dStand.Dijkstra(A, Start, dimA, prev);
            // Получаем путь из начала в конец
            GetPath();
            return rett;
        }
        /// <summary>
        /// Построение пути из начальной в конечную вершину
        /// </summary>
        void GetPath()
        {
            Path.Clear();
            int j = End;        // Номер вершины, в которую попадаем
            Path.Add(End);
            //  Идём от конца к началу
            do
            {
                j = prev[j];
                Path.Add(j);
            } while (j != Start);            
            Path.Reverse(); // Переворот - от начала к концу
        }
        /// <summary>
        /// Сохранение матрицы коэффициентов в текстовый файл
        /// </summary>
        /// <param name="Fname">Имя файла</param>
        void SaveA(string Fname)
        {
            List<string> list = new List<string>();
            string s = "";
            for (int i = 0; i < dimA; ++i)
            {
                s = "";
                for (int j = 0; j < dimA; ++j)
                {
                    s += A[i, j].ToString() + " ; ";
                }
                list.Add(s);
            }
            System.IO.File.WriteAllLines(Fname, list);
        }
    }
}
