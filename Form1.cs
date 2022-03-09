using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace simul_lab_3
{
    public partial class Form1 : Form
    {
        const int MaxGens = 10, MaxCells = 10;

        Dictionary<string, bool> Rules = new Dictionary<string, bool>();
        bool[,] CellularMartix;

        Bitmap bm;
        Graphics graph;
        Random rand = new Random();
        int LastGeneration;
        bool dt = false;


        public Form1()
        {
            InitializeComponent();
        }
        private void CreateFirstGeneration()
        {
            CellularMartix = new bool[MaxGens, MaxCells];
            LastGeneration = 0;
            CellularMartix[0, 2] = true;
            CellularMartix[0, 3] = true;
            CellularMartix[0, 7] = true;
        }

        private void InitCellGrid()
        {
            bm = new Bitmap(panel1.Width, panel1.Height);
            graph = Graphics.FromImage(bm);

            DrawLeftColumn();
            DrawGridTemplate();
        }

        private void DrawCells()
        {
            SolidBrush solidBrush = new SolidBrush(Color.Orange);

            for (int genNum = 0; genNum < MaxGens; genNum++)
            {
                for (int cellNum = 0; cellNum < MaxCells; cellNum++)
                {
                    if (CellularMartix[genNum, cellNum])
                    {
                        graph.FillRectangle(solidBrush, 30 + cellNum * 30, genNum * 30, 30, 30);
                    }
                }
            }
        }

        private void DrawLeftColumn()
        {
            graph.FillRectangle(new SolidBrush(Color.LightGray), new Rectangle(0, 0, 30, 300));

            Font font = new Font("Arial", 16);
            SolidBrush solidBrush = new SolidBrush(Color.Black);

            for (int i = 0; i < MaxGens; i++)
            {
                graph.DrawString(i.ToString(),
                    font, solidBrush, 6, i * 30 + 4);
            }
        }

        private void DrawGridTemplate()
        {
            Pen pen = new Pen(Color.Black);

            for (int i = 0; i < MaxGens + 1; i++)
            {
                graph.DrawLine(pen, 0, i * 30, 330, i * 30);
            }

            for (int i = 0; i < MaxCells + 2; i++)
            {
                graph.DrawLine(pen, i * 30, 0, i * 30, 300);
            }
        }

        private string IntToFullBinString(int input, int length)
        {
            string s = Convert.ToString(input, 2);
            while (s.Length < length) s = '0' + s;
            return s;
        }

        private void SetRules()
        {
            string rule = IntToFullBinString((int)edRule.Value, 8);

            for (int i = 0; i < 8; i++)
            {
                Rules[IntToFullBinString(i, 3)] = Convert.ToBoolean(Convert.ToInt32(rule.Substring(7 - i, 1)));
            }
        }

        private void CreateNextGeneration()
        {
            int prevGen = LastGeneration;
            LastGeneration++;
            for (int i = 0; i < MaxCells; i++)
            {
                string rule = (CellularMartix[prevGen, mod(i - 1, MaxCells - 1)] ? "1" : "0")
                    + (CellularMartix[prevGen, i] ? "1" : "0")
                    + (CellularMartix[prevGen, mod(i + 1, MaxCells - 1)] ? "1" : "0");

                CellularMartix[LastGeneration, i] = Rules[rule];
            }
        }

        private void bt_Rule_Click(object sender, EventArgs e)
        {
            if (dt)
            {
                timer1.Stop();
            }
            else
            {
                CreateFirstGeneration();

                graph.Clear(Color.White);
                SetRules();
                timer1.Start();
            }
            dt = !dt;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(bm, Point.Empty);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (LastGeneration >= MaxGens - 1)
            {
                timer1.Stop();
                return;
            }

            CreateNextGeneration();
            DrawLeftColumn();
            DrawCells();
            DrawGridTemplate();
            panel1.Invalidate();
        }
        private int mod(int x, int m)
        {
            return (x % m + m) % m;
        }

    }
}
