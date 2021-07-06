using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public class CActor
    {
        public int X, Y;
        public int W, H;
        public int R, C;
        public Color clr;
        public int h,g=99999,f=99999;
        public int parent_r, parent_c;
        public String type;
        public Bitmap Im;
    }
    public class Apple
    {
        public int X, Y;
        public int W, H;
        public int R, C;
        public Bitmap Im;
    }
    public class Snake
    {
        public int X, Y;
        public int Xold, Yold;
        public int R, C;
        public int W, H;
        public Bitmap Im;
        public int Type = 0;
    }
    public class node
    {
        public node Parent;
        public List<node> Childs;
        public int R, C, H, PC, Cost, T;
        public String nodeType;
        public Boolean isClosed = false;
    }
    public partial class Form1 : Form
    {
        Bitmap off;
        const int N = 20;
        Random RR = new Random();
        Random RC = new Random();
        Apple Apple = new Apple();
        int CF = 1, Moved = 0;
        CActor[,] B = new CActor[N, N];
        CActor MyMove = new CActor();
        List<Snake> Snake = new List<Snake>();
        List<CActor> Moves = new List<CActor>();
        Stack<CActor> path = new Stack<CActor>();
        List<node> nodes = new List<node>();
        List<CActor> openList = new List<CActor>();
        List<CActor> closedList = new List<CActor>();
        Timer TT = new Timer();
        bool Right = false, Left = false, Down = false, Up = false;
        char LastMove, CurrentMove;

        public Form1()
        {
            this.WindowState = FormWindowState.Maximized;
            this.Load += new EventHandler(Form1_Load);
            this.Paint += new PaintEventHandler(Form1_Paint);
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
            this.MouseDown += new MouseEventHandler(Form1_MouseDown);
            TT.Tick += new EventHandler(TT_Tick);
            TT.Start();

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            int iC = e.X / B[0, 0].W;
            int iR = e.Y / B[0, 0].H;
            if(iC < N && iR < N)
            {
                //B[iR, iC].Im = new Bitmap("Metal.png");
                B[iR, iC].clr = Color.FromArgb(50,50,50);
                B[iR, iC].type = "C";
                B[iR, iC].h = 99;
                for(int i = 0; i < nodes.Count; i++)
                {
                    if(nodes[i].R == iR && nodes[i].C == iC)
                    {
                        //MessageBox.Show("[" + nodes[i].R + "," + nodes[i].C + "]" + " is Locked");
                        nodes.RemoveAt(i);

                    }
                }
            }
        }

        private void TT_Tick(object sender, EventArgs e)
        {
            MoveSnake();
            if (Right == true && Snake[0].C != N-1)
            {
                //CurrentMove = 'R';
                Snake[0].C++;
                Snake[0].X = B[Snake[0].R, Snake[0].C].X;
                //Snake[0].Im = new Bitmap("Robo.png");
                Right = false;
            }
            if (Left == true && Snake[0].C != 0)
            {
                //CurrentMove = 'L';
                Snake[0].C--;
                Snake[0].X = B[Snake[0].R, Snake[0].C].X;
                //Snake[0].Im = new Bitmap("Robo.png");
                Left = false;
            }
            if (Up == true && Snake[0].R != 0)
            {
                //CurrentMove = 'U';
                Snake[0].R--;
                Snake[0].Y = B[Snake[0].R, Snake[0].C].Y;
                //Snake[0].Im = new Bitmap("Robo.png");
                Up = false;
            }
            if (Down == true && Snake[0].R !=N-1)
            {
                //CurrentMove = 'D';
                Snake[0].R++;
                Snake[0].Y = B[Snake[0].R, Snake[0].C].Y;
                //Snake[0].Im = new Bitmap("Robo.png");
                Down = false;
            }
            /*if (CurrentMove == 'R' && LastMove == 'R')
            {
                Snake[1].Im = new Bitmap("4.png");
            }
            if (CurrentMove == 'R' && LastMove == 'U')
            {
                Snake[1].Im = new Bitmap("9.png");
            }
            if (CurrentMove == 'R' && LastMove == 'D')
            {
                Snake[1].Im = new Bitmap("7.png");
            }
            if (CurrentMove == 'L' && LastMove == 'L')
            {
                Snake[1].Im = new Bitmap("4.png");
            }
            if (CurrentMove == 'L' && LastMove == 'U')
            {
                Snake[1].Im = new Bitmap("8.png");
            }
            if (CurrentMove == 'L' && LastMove == 'D')
            {
                Snake[1].Im = new Bitmap("6.png");
            }
            if (CurrentMove == 'D' && LastMove == 'D')
            {
                Snake[1].Im = new Bitmap("5.png");
            }
            if (CurrentMove == 'D' && LastMove == 'R')
            {
                Snake[1].Im = new Bitmap("8.png");
            }
            if (CurrentMove == 'D' && LastMove == 'L')
            {
                Snake[1].Im = new Bitmap("9.png");
            }
            if (CurrentMove == 'U' && LastMove == 'U')
            {
                Snake[1].Im = new Bitmap("5.png");
            }
            if (CurrentMove == 'U' && LastMove == 'R')
            {
                Snake[1].Im = new Bitmap("6.png");
            }
            if (CurrentMove == 'U' && LastMove == 'L')
            {
                Snake[1].Im = new Bitmap("7.png");
            }*/
            if (Snake[0].R == Apple.R && Snake[0].C == Apple.C)
            {
                //UpdateSnakeBody();
                CreateApple();
            }
            DrawDubb(this.CreateGraphics());
            /*if (Right == true)
            {
                LastMove = 'R';
            }
            if (Left == true)
            {
                LastMove = 'L';
            }
            if (Up == true)
            {
                LastMove = 'U';
            }
            if (Down == true)
            {
                LastMove = 'D';
            }*/
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Right /*&& Left == false*/)
            {
                Right = true;
                Left = false;
                Up = false;
                Down = false;
            }
            if (e.KeyCode == Keys.Left /*&& Right == false*/)
            {
                Right = false;
                Left = true;
                Up = false;
                Down = false;
            }
            if (e.KeyCode == Keys.Up /*&& Down == false*/)
            {
                Right = false;
                Left = false;
                Up = true;
                Down = false;
            }
            if (e.KeyCode == Keys.Down /*&& Up == false*/)
            {
                Right = false;
                Left = false;
                Up = false;
                Down = true;
            }
            if(e.KeyCode == Keys.Enter)
            {
                AStarSearch();
                /*for(int i=0;i<nodes.Count;i++)
                {
                    MessageBox.Show("[" + nodes[i].R + "," + nodes[i].C + "] " + nodes[i].nodeType);
                }*/
            }
            Apple.Im = new Bitmap("Battery.png");
        }
        bool isValid(int row, int col)
        {
            // Returns true if row number and column number 
            // is in range and not closed
            if((row >= 0) && (row < N) && (col >= 0) && (col < N))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        void MoveSnake()
        {
            /*for (int i = Snake.Count - 1; i > 0; i--)
            {
                Snake[i].X = Snake[i - 1].X;
                Snake[i].Y = Snake[i - 1].Y;
            }
            for (int i = Snake.Count - 2; i > 1; i--)
            {
                Snake[i].Im = Snake[i - 1].Im;
            }*/
        }
        void GenerateValues()
        {
            int val = 0;
            int index = 0;
            for (int i = Apple.R; i < N; i++)
            {
                for (int j = Apple.C; j < N; j++)
                {
                    node state = new node();
                    state.R = i;
                    state.C = j;
                    state.H = val;
                    B[i, j].h = val;
                    state.PC = 1;
                    nodes.Add(state);
                    val++;
                }
                index++;
                val = index;
            }
            val = 1;
            index = 1;
            for (int i = Apple.R - 1; i > -1; i--)
            {
                for (int j = Apple.C; j < N; j++)
                {
                    node state = new node();
                    state.R = i;
                    state.C = j;
                    state.H = val;
                    B[i, j].h = val;
                    state.PC = 1;
                    nodes.Add(state);
                    val++;
                }
                index++;
                val = index;
            }
            val = 1;
            index = 1;
            for (int i = Apple.R; i < N; i++)
            {
                for (int j = Apple.C-1; j > -1; j--)
                {
                    node state = new node();
                    state.R = i;
                    state.C = j;
                    state.H = val;
                    B[i, j].h = val;
                    state.PC = 1;
                    nodes.Add(state);
                    val++;
                }
                index++;
                val = index;
            }
            val = 2;
            index = 2;
            for (int i = Apple.R-1; i > -1 ; i--)
            {
                for (int j = Apple.C-1; j > -1; j--)
                {
                    node state = new node();
                    state.R = i;
                    state.C = j;
                    state.H = val;
                    B[i, j].h = val;
                    state.PC = 1;
                    nodes.Add(state);
                    val++;
                }
                index++;
                val = index;
            }
            for (int r = 0; r < N;r++)
            {
                for(int c=0;c<N;c++)
                {
                    B[r,c].clr = Color.FromArgb(52, 207, 142);
                }
            }
        }
        void KillSnake()
        {
            for(int i=1;i<Snake.Count;i++)
            {
                if(Snake[0].R == Snake[i].R && Snake[0].C == Snake[i].C)
                {
                    MessageBox.Show("LOSE!");
                }
            }
        }
        void UpdateSnakeBody()
        {
            Snake pnn = new Snake();
            pnn.X = Snake[Snake.Count - 1].X;
            pnn.Y = Snake[Snake.Count - 1].Y;
            Snake[Snake.Count - 1].Im = new Bitmap("4.png");
            pnn.Im = new Bitmap("10.png");
            Snake.Add(pnn);

        }
        void CreateApple()
        {
            Apple.R = 19;
            Apple.C = 10;
            Apple.X = B[Apple.R, Apple.C].X;
            Apple.Y = B[Apple.R, Apple.C].Y;
            nodes.Clear();
            GenerateValues();
            DeclareTypes();
            /*for (int i = 0; i < nodes.Count; i++)
            {
                MessageBox.Show("[" + nodes[i].R + "," + nodes[i].C + "]" + " H = " + nodes[i].H);
            }*/
        }
        void DeclareTypes()
        {
            openList.Clear();
            for(int i=0; i<nodes.Count;i++)
            {
                nodes[i].nodeType = "";
                B[nodes[i].R, nodes[i].C].type = "";
                if(nodes[i].R == Apple.R && nodes[i].C == Apple.C)
                {
                    nodes[i].nodeType = "G";
                    B[nodes[i].R, nodes[i].C].type = "G";
                }
                if(nodes[i].R == Snake[0].R && nodes[i].C == Snake[0].C)
                {
                    nodes[i].nodeType = "S";
                    B[nodes[i].R, nodes[i].C].type = "S";
                    openList.Add(B[nodes[i].R, nodes[i].C]);
                }
            }
        }
        void MoveRobot()
        {
            int Action = 0;
            if(Snake[0].R == 0 && Snake[0].C == 0) //Upper Left Corner
            {
                int min = 9999;
                if(B[Snake[0].R + 1,Snake[0].C].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R + 1;
                    Move.C = Snake[0].C;
                    Moves.Add(Move);
                }
                if (B[Snake[0].R, Snake[0].C+1].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R;
                    Move.C = Snake[0].C + 1;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                for (int i = 0; i < Moves.Count; i++)
                {
                    if(Moves[i].h < min)
                    {
                        min = Moves[i].h;
                        MyMove = Moves[i];
                    }
                }
                Snake[0].R = MyMove.R;
                Snake[0].C = MyMove.C;
                Action = 1;
            }
            Moves.Clear();
            if (Snake[0].R == 0 && Snake[0].C == N && Action == 0) //Upper Right Corner
            {
                int min = 9999;
                if (B[Snake[0].R + 1, Snake[0].C].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R + 1;
                    Move.C = Snake[0].C;
                    Moves.Add(Move);
                }
                if (B[Snake[0].R, Snake[0].C - 1].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R;
                    Move.C = Snake[0].C-1;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                for (int i = 0; i < Moves.Count; i++)
                {
                    if (Moves[i].h < min)
                    {
                        min = Moves[i].h;
                        MyMove = Moves[i];
                    }
                }
                Snake[0].R = MyMove.R;
                Snake[0].C = MyMove.C;
                Action = 1;

            }
            Moves.Clear();
            if (Snake[0].R == N && Snake[0].C == 0 && Action == 0) //Lower Left Corner
            {
                int min = 9999;
                if (B[Snake[0].R - 1, Snake[0].C].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R - 1;
                    Move.C = Snake[0].C;
                    Moves.Add(Move);
                }
                if (B[Snake[0].R, Snake[0].C + 1].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R;
                    Move.C = Snake[0].C + 1;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                for (int i = 0; i < Moves.Count; i++)
                {
                    if (Moves[i].h < min)
                    {
                        min = Moves[i].h;
                        MyMove = Moves[i];
                    }
                }
                Snake[0].R = MyMove.R;
                Snake[0].C = MyMove.C;
                Action = 1;

            }
            Moves.Clear();
            if (Snake[0].R == N && Snake[0].C == N && Action == 0) //Lower Right Corner
            {
                int min = 9999;
                if (B[Snake[0].R - 1, Snake[0].C].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R - 1;
                    Move.C = Snake[0].C;
                    Moves.Add(Move);
                }
                if (B[Snake[0].R, Snake[0].C - 1].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R;
                    Move.C = Snake[0].C - 1;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                for (int i = 0; i < Moves.Count; i++)
                {
                    if (Moves[i].h < min)
                    {
                        min = Moves[i].h;
                        MyMove = Moves[i];
                    }
                }
                Snake[0].R = MyMove.R;
                Snake[0].C = MyMove.C;
                Action = 1;

            }
            Moves.Clear();
            if (Snake[0].R == 0 && Snake[0].C > 0 && Action == 0) //Upper Row
            {
                int min = 9999;
                if (B[Snake[0].R + 1, Snake[0].C].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R + 1;
                    Move.C = Snake[0].C;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                if (B[Snake[0].R, Snake[0].C - 1].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R;
                    Move.C = Snake[0].C - 1;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                if (B[Snake[0].R, Snake[0].C + 1].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R;
                    Move.C = Snake[0].C + 1;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                for (int i = 0; i < Moves.Count; i++)
                {
                    if (Moves[i].h < min)
                    {
                        min = Moves[i].h;
                        MyMove = Moves[i];
                    }
                }
                Snake[0].R = MyMove.R;
                Snake[0].C = MyMove.C;
                Action = 1;

            }
            Moves.Clear();
            if (Snake[0].R == N && Snake[0].C > 0 && Action == 0) //Lower Row
            {
                int min = 9999;
                if (B[Snake[0].R - 1, Snake[0].C].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R - 1;
                    Move.C = Snake[0].C;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                if (B[Snake[0].R, Snake[0].C - 1].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R;
                    Move.C = Snake[0].C - 1;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                if (B[Snake[0].R, Snake[0].C + 1].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R;
                    Move.C = Snake[0].C + 1;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                for (int i = 0; i < Moves.Count; i++)
                {
                    if (Moves[i].h < min)
                    {
                        min = Moves[i].h;
                        MyMove = Moves[i];
                    }
                }
                Snake[0].R = MyMove.R;
                Snake[0].C = MyMove.C;
                Action = 1;

            }
            Moves.Clear();
            if (Snake[0].R > 0 && Snake[0].C == 0 && Action == 0) //Left most Column
            {
                int min = 9999;
                if (B[Snake[0].R - 1, Snake[0].C].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R - 1;
                    Move.C = Snake[0].C;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                if (B[Snake[0].R + 1, Snake[0].C].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R + 1;
                    Move.C = Snake[0].C;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                if (B[Snake[0].R, Snake[0].C + 1].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R;
                    Move.C = Snake[0].C + 1;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                for (int i = 0; i < Moves.Count; i++)
                {
                    if (Moves[i].h < min)
                    {
                        min = Moves[i].h;
                        MyMove = Moves[i];
                    }
                }
                Snake[0].R = MyMove.R;
                Snake[0].C = MyMove.C;
                Action = 1;

            }
            Moves.Clear();
            if (Snake[0].R > 0 && Snake[0].C == N && Action == 0) //Right most Column
            {
                int min = 9999;
                if (B[Snake[0].R - 1, Snake[0].C].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R - 1;
                    Move.C = Snake[0].C;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                if (B[Snake[0].R + 1, Snake[0].C].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R + 1;
                    Move.C = Snake[0].C;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                if (B[Snake[0].R, Snake[0].C - 1].type != "C")
                {
                    CActor Move = new CActor();
                    Move.R = Snake[0].R;
                    Move.C = Snake[0].C - 1;
                    Move.h = B[Move.R, Move.C].h;
                    Moves.Add(Move);
                }
                for (int i = 0; i < Moves.Count; i++)
                {
                    if (Moves[i].h < min)
                    {
                        min = Moves[i].h;
                        MyMove = Moves[i];
                    }
                }
                Snake[0].R = MyMove.R;
                Snake[0].C = MyMove.C;
                Action = 1;
            }
            MessageBox.Show("Moved to " + Snake[0].R + "," + Snake[0].C);

        }
        void sortMyList()
        {
            for (int i = 0; i < openList.Count; i++)
            {
                CActor Temp;
                for (int j = i; j > 0; j--)
                {
                    if(openList[j].f < openList[j-1].f)
                    {
                        Temp = openList[j];
                        openList[j] = openList[j - 1];
                        openList[j - 1] = Temp;
                    }
                }
            }
            /*MessageBox.Show("Open list start");
            for (int i = 0; i < openList.Count; i++)
            {
                MessageBox.Show(openList[i].f.ToString());
            }
            MessageBox.Show("Open list end");*/

        }
        void AStarSearch()
        {
            int R, C;
            //Intialize Start
            B[Snake[0].R, Snake[0].C].h = 0;
            B[Snake[0].R, Snake[0].C].g = 0;
            B[Snake[0].R, Snake[0].C].f = 0;
            B[Snake[0].R, Snake[0].C].parent_r = Snake[0].R;
            B[Snake[0].R, Snake[0].C].parent_c = Snake[0].C;
            bool foundDest = false;
            openList.Add(B[Snake[0].R, Snake[0].C]);

            while (openList.Count != 0)
            {
                sortMyList();
                int gNew, hNew, fNew;
                R = openList[0].R;
                C = openList[0].C;
                closedList.Add(openList[0]);
                openList.RemoveAt(0);
                //North Successor
                if(isValid(R-1,C) == true)
                {
                    if(B[R-1, C].type == "G")
                    {
                        B[R - 1, C].parent_r = R;
                        B[R - 1, C].parent_c = C;
                        tracePath(R - 1, C);
                        foundDest = true;
                        break;
                    }
                    else if (isInClosed(R - 1, C) == false && B[R - 1, C].type != "C")
                    {
                        gNew = B[R, C].g + 1;
                        hNew = B[R - 1, C].h;
                        fNew = gNew + hNew;

                        if (B[R - 1, C].f == 99999 || B[R - 1, C].f > fNew)
                        {
                            openList.Add(B[R - 1, C]);
                            B[R - 1, C].f = fNew;
                            B[R - 1, C].g = gNew;
                            B[R - 1, C].h = hNew;
                            B[R - 1, C].parent_r = R;
                            B[R - 1, C].parent_c = C;
                        }
                    }
                }
                
                //South Successor
                if (isValid(R + 1, C) == true)
                {
                    if (B[R + 1, C].type == "G")
                    {
                        B[R + 1, C].parent_r = R;
                        B[R + 1, C].parent_c = C;
                        tracePath(R + 1, C);
                        foundDest = true;
                        break;
                    }
                    else if (isInClosed(R + 1, C) == false && B[R + 1, C].type != "C")
                    {
                        gNew = B[R, C].g + 1;
                        hNew = B[R + 1, C].h;
                        fNew = gNew + hNew;

                        if (B[R + 1, C].f == 99999 || B[R + 1, C].f > fNew)
                        {
                            openList.Add(B[R + 1, C]);
                            B[R + 1, C].f = fNew;
                            B[R + 1, C].g = gNew;
                            B[R + 1, C].h = hNew;
                            B[R + 1, C].parent_r = R;
                            B[R + 1, C].parent_c = C;

                        }
                    }
                }
                
                //East Successor
                if (isValid(R, C + 1) == true)
                {
                    if (B[R, C + 1].type == "G")
                    {
                        B[R, C + 1].parent_r = R;
                        B[R, C + 1].parent_c = C;

                        tracePath(R, C + 1);
                        foundDest = true;
                        break;
                    }
                    else if (isInClosed(R, C + 1) == false && B[R, C + 1].type != "C")
                    {

                        gNew = B[R, C].g + 1;
                        hNew = B[R, C + 1].h;
                        fNew = gNew + hNew;

                        if (B[R, C + 1].f == 99999 || B[R, C + 1].f > fNew)
                        {
                            openList.Add(B[R, C + 1]);
                            B[R, C + 1].f = fNew;
                            B[R, C + 1].g = gNew;
                            B[R, C + 1].h = hNew;
                            B[R, C + 1].parent_r = R;
                            B[R, C + 1].parent_c = C;

                        }
                    }
                }
                
                //West Successor
                if (isValid(R, C - 1) == true)
                {

                    if (B[R, C - 1].type == "G")
                    {
                        B[R, C - 1].parent_r = R;
                        B[R, C - 1].parent_c = C;
                        tracePath(R, C - 1);
                        foundDest = true;
                        break;
                    }
                    else if (isInClosed(R, C - 1) == false && B[R, C - 1].type != "C")
                    {
                        gNew = B[R, C].g + 1;
                        hNew = B[R, C - 1].h;
                        fNew = gNew + hNew; 

                        if (B[R, C - 1].f == 99999 || B[R, C - 1].f > fNew)
                        {
                            openList.Add(B[R, C - 1]);
                            B[R, C - 1].f = fNew;
                            B[R, C - 1].g = gNew;
                            B[R, C - 1].h = hNew;
                            B[R, C - 1].parent_r = R;
                            B[R, C - 1].parent_c = C;


                        }
                    }
                }
                /*for(int i=0;i<closedList.Count;i++)
                {
                    MessageBox.Show("[" + closedList[i].R + "," + closedList[i].C + "]");
                }
                MessageBox.Show("Bam");*/

            }
            if(foundDest == false)
            {
                MessageBox.Show("No path Found");
            }
        }
        void tracePath(int R, int C)
        {
            while (!(B[R, C].parent_r == R && B[R, C].parent_c == C))
            {
                path.Push(B[R, C]);
                int tempRow = B[R, C].parent_r;
                int tempCol = B[R, C].parent_c;
                R = tempRow;
                C = tempCol;
            }
            path.Push(B[R, C]);
            while(path.Count != 0)
            {
                CActor P = path.Peek();
                B[P.R, P.C].clr = Color.FromArgb(232, 184, 26);
                Snake[0].X = B[P.R, P.C].X;
                Snake[0].Y = B[P.R, P.C].Y;
                path.Pop();
                DrawDubb(this.CreateGraphics());
            }
        }
        bool isInClosed(int R,int C)
        {
            for(int i=0;i < closedList.Count;i++)
            {
                if(closedList[i].R == R && closedList[i].C == C)
                {
                    return true;
                }
            }
            return false;
        }
        void CreateSnake()
        {
            for (int i = 0; i < 1; i++)
            {
                Snake pnn = new Snake();
                if (i == 0)
                {
                    pnn.R = 1;
                    pnn.C = 10;
                    pnn.X = B[pnn.R, pnn.C].X;
                    pnn.Y = B[pnn.R, pnn.C].Y;
                    pnn.Im = new Bitmap("Robo.png");
                }
                /*if (i == 1)
                {
                    pnn.R = 7;
                    pnn.C = 1;
                    pnn.X = B[pnn.R, pnn.C].X;
                    pnn.Y = B[pnn.R, pnn.C].Y;
                    pnn.Im = new Bitmap("4.png");
                }
                if (i == 2)
                {
                    pnn.R = 7;
                    pnn.C = 0;
                    pnn.X = B[pnn.R, pnn.C].X;
                    pnn.Y = B[pnn.R, pnn.C].Y;
                    pnn.Im = new Bitmap("10.png");
                }*/
                Snake.Add(pnn);
            }
        }
        void Create()
        {
            int ax = 0;
            int ay = 0;
            for (int r = 0; r < N; r++)
            {
                ax = 0;
                for (int c = 0; c < N; c++)
                {
                    CActor pnn = new CActor();
                    pnn.X = ax;
                    pnn.Y = ay;
                    pnn.W = this.ClientSize.Height / N;
                    pnn.H = this.ClientSize.Height / N;
                    pnn.R = r;
                    pnn.C = c;
                    pnn.clr = Color.FromArgb(52, 207, 142);
                    ax += pnn.W;
                    B[r, c] = pnn;
                }
                ay += this.ClientSize.Height / N;
            }
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawDubb(e.Graphics);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(ClientSize.Width, ClientSize.Height);
            Create();
            CreateSnake();
            CreateApple();
        }
        void DrawScene(Graphics g)
        {
            g.Clear(Color.Black);

            for (int r = 0; r < N; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    g.DrawRectangle(new Pen(Color.FromArgb(35, 125, 87), 4),
                                    B[r, c].X, B[r, c].Y, B[r, c].W, B[r, c].H);
                    g.FillRectangle(new SolidBrush(B[r, c].clr), B[r, c].X, B[r, c].Y, B[r, c].W, B[r, c].H);
                    FontFamily F = new FontFamily("Arial");
                    Font FF = new Font(F, 12, FontStyle.Regular);
                    //g.DrawImage(B[r, c].Im, B[r, c].X, B[r, c].Y, B[r, c].W, B[r, c].H);
                    g.DrawString(B[r,c].h.ToString() , FF, Brushes.White, B[r,c].X + 3, B[r,c].Y + 3);
                    g.DrawString(B[r,c].type , FF, Brushes.White, B[r,c].X, B[r,c].Y + 20);

                }

            }
            g.DrawImage(Apple.Im = new Bitmap("Battery1.png"), Apple.X, Apple.Y, B[0, 0].W, B[0, 0].H);
            for (int i = 0; i < Snake.Count; i++)
            {
  
                g.DrawImage(Snake[i].Im, Snake[i].X, Snake[i].Y, B[0, 0].W, B[0, 0].H);
            }
        }
        void DrawDubb(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            DrawScene(g2);
            g.DrawImage(off, 0, 0);
        }
    }
}
