using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Snake
{
    public partial class Form1 : Form
    {
        private PictureBox[] snake = new PictureBox[400];
        private int dirX, dirY;
        private int _width = 900;
        private int _height = 800;
        private int _sizeOfSides = 40;
        bool move = true;
        Random rnd = new Random();
        int appleX, appleY;
        int _point = 0;
        private int[] dirMasX = new int[400], dirMasY = new int[400];
        bool _escape;
        public Form1()
        {
            InitializeComponent();
            this.Width = _width;
            this.Height = _height + _sizeOfSides;
            dirX = 1;
            dirY = 0;
            dirMasX[0] = dirX;
            dirMasY[0] = dirY;
            generateMap();
            timer.Tick += new EventHandler(_update);
            timer.Interval = 200;
            timer.Start();
            Point_Box.Location = new Point(840, 20);
            this.KeyDown += new KeyEventHandler(OKR);
            appleX = rnd.Next(1,20);
            appleY = rnd.Next(1,20);
            Apple.Location = new Point(_sizeOfSides * appleX + 1, _sizeOfSides * appleY + 1);
            snake[0] = new PictureBox();
            snake[0].BackColor = Color.Aquamarine;
            snake[0].Size = new Size(_sizeOfSides-1, _sizeOfSides-1);
            snake[0].Location = new Point(1, 1);
            this.Controls.Add(snake[0]);
            _escape = false;
        }
        // Обработка движений, проигрыш и рандомайзер
        public void _update(Object myObject, EventArgs eventAgr)
        {   
            for(int i = _point; i >= 0; --i)
            {
                //Обработка движений
                if (move)
                {
                    snake[i].Location = new Point(snake[i].Location.X + dirMasX[i] * _sizeOfSides, snake[i].Location.Y + dirMasY[i] * _sizeOfSides);
                    dirMasX[i + 1] = dirMasX[i];
                    dirMasX[i] = dirX;
                    dirMasY[i + 1] = dirMasY[i];
                    dirMasY[i] = dirY;
                    // Огранечитель по X
                    if (snake[0].Location.X >= _height)
                    {
                        snake[0].Location = new Point(snake[0].Location.X - _sizeOfSides, snake[00].Location.Y);
                        move = false;
                        timer.Stop();
                    }
                    else if (snake[0].Location.X < 0)
                    {
                        snake[0].Location = new Point(snake[0].Location.X + _sizeOfSides, snake[0].Location.Y);
                        move = false;
                        timer.Stop();
                    }
                    // Ограничитель по Y
                    else if (snake[0].Location.Y >= _width - _sizeOfSides * 3)
                    {
                        snake[0].Location = new Point(snake[0].Location.X, snake[0].Location.Y - _sizeOfSides);
                        move = false;
                        timer.Stop();
                    }
                    else if (snake[0].Location.Y < 0)
                    {
                        snake[0].Location = new Point(snake[0].Location.X, snake[0].Location.Y + _sizeOfSides);
                        move = false;
                        timer.Stop();
                    }
                }
                // Рандомайзер,сбор Яблок, Рост змейки
                if(snake[0].Location == Apple.Location)
                {
                    appleX = rnd.Next(1, 20);
                    appleY = rnd.Next(1, 20);
                    Apple.Location = new Point(_sizeOfSides * appleX + 1, _sizeOfSides * appleY + 1);
                    ++_point;
                    Point_Box.Text = _point.ToString();
                    if(timer.Interval > 100) timer.Interval -= 5;
                    //Рост змейки
                    snake[_point] = new PictureBox();
                    snake[_point].Size = new Size(_sizeOfSides - 1, _sizeOfSides - 1); 
                    snake[_point].BackColor = Color.Green;
                    dirMasX[_point] = dirMasX[i];
                    dirMasX[i] = dirX;
                    dirMasY[_point] = dirMasY[i];
                    dirMasY[i] = dirY;
                    snake[_point].Location = new Point(snake[_point-1].Location.X + _sizeOfSides * - dirMasX[i], snake[_point-1].Location.Y + _sizeOfSides * - dirMasY[i]);
                    this.Controls.Add(snake[_point]);
                }
            }
            // Проигрыш
            CannibalLoss();
            if (timer.Enabled == false)
            {
                for(int i = _point; i > 0; --i) snake[i].BackColor = Color.Red;
                MessageBox.Show("Game Over");
            }
            
        }
        //Проигрыш при поеданиии себя
        public void CannibalLoss()
        {
            for(int i = _point; i > 0; --i)
            {
                if(snake[0].Location == snake[i].Location)
                {
                    timer.Stop();
                }
            }


            
            
        }
        // Обработка карты 20/20
        public void generateMap()
        {
            // Горизонтази
            for (int i = 0; i < _height / _sizeOfSides + 1; ++i)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point(0, _sizeOfSides * i);
                pic.Size = new Size(_height, 1);
                this.Controls.Add(pic);
            }
            // Вертикали
            for (int i = 0; i < _width / _sizeOfSides - 1; ++i)
            {
                PictureBox pic = new PictureBox();
                pic.BackColor = Color.Black;
                pic.Location = new Point(_sizeOfSides * i, 0);
                pic.Size = new Size(1, _width - 100);
                this.Controls.Add(pic);
            }
        }
        //Бинды клавиш
        public void OKR(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode.ToString())
            {
                case "A":
                    dirX = -1;
                    dirY = 0;
                    break;
                case "D":
                    dirX = 1;
                    dirY = 0;
                    break;
                case "S":
                    dirY = 1;
                    dirX = 0;
                    break;
                case "W":
                    dirY = -1;
                    dirX = 0;
                    break;
                case "Escape":
                    if (_escape)
                    {
                        _escape = false;
                        timer.Start();
                    }
                    else
                    {
                        timer.Stop();
                        _escape = true;
                    }
                    break;
            }
        }
    }
}
