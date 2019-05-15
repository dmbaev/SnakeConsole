using System;
using System.Collections.Generic;
using System.Timers;

namespace Snake_console_NetCore
{
    class Program
    {
        static int _rows = 10;
        static int _columns = 10;
        static bool NoGameOver;
        static bool GameRestart = false;
        static Timer _gTimer;
        static ConsoleKey _lastMove;
        static Random rnd = new Random();
        static int _rndX;
        static int _rndY;
        static int _Score;
        static int _lineStartMenu;
        static double _Interval = 1000;
        static List<_coordinates> _Snake = new List<_coordinates>();
        private  class _coordinates {
            public int _x {get; set;}
            public int _y { get; set;}
        }
        static void Main(string[] args)
        {
            do
            {
                StartGame();
            } while (GameRestart);
        }

        static void StartGame()
        {
            _Snake.Clear();
            _Snake.Add(new _coordinates {_x=1, _y=1 });
            _lastMove = ConsoleKey.RightArrow;
            Console.Clear();
            Console.CursorVisible = false;
            GameRestart = false;
            NoGameOver = true;
            _Score = -1;
            _Interval = 1000;
            _lineStartMenu = (int)(Math.Round((decimal)(_columns/2)) -3);

            _gTimer = new Timer(_Interval);
            _gTimer.Elapsed += OnTimedEvent;
            _gTimer.Start();

            draw();
            draw_O();
            AddBlock();

            do
            {
                _Move(Console.ReadKey().Key);
         

            } while (NoGameOver);
            _gTimer.Stop();
        }

        static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            _Move(_lastMove);
        }

        static void _Move(ConsoleKey _key)
        {
            if (!(_key == ConsoleKey.UpArrow || _key == ConsoleKey.DownArrow || _key == ConsoleKey.RightArrow || _key == ConsoleKey.LeftArrow || _key == ConsoleKey.R))
            {
                Console.SetCursorPosition(0, 0);
                Console.Write(" ");
                Console.SetCursorPosition(0, 0);
                return;
            }

                _gTimer.Stop();
            var _xZero_old = _Snake[0]._x;
            var _yZero_old = _Snake[0]._y;
            if (_key == ConsoleKey.UpArrow)
            {
                _Snake[0]._y--;
                if (_Snake.Count > 1)
                {
                    Console.SetCursorPosition(_Snake[_Snake.Count - 1]._x, _Snake[_Snake.Count - 1]._y);
                    Console.Write(" ");
                    for (int i = _Snake.Count - 1; i > 0; i--)
                    {
                        _Snake[i]._y = _Snake[i - 1]._y;
                        _Snake[i]._x = _Snake[i - 1]._x;
                    }
                    _Snake[1]._y = _yZero_old;
                }
            }
            else if (_key == ConsoleKey.DownArrow)
            {
                _Snake[0]._y++;
                if (_Snake.Count > 1)
                {
                    Console.SetCursorPosition(_Snake[_Snake.Count - 1]._x, _Snake[_Snake.Count - 1]._y);
                    Console.Write(" ");
                    for (int i = _Snake.Count - 1; i > 0; i--)
                    {
                        _Snake[i]._y = _Snake[i - 1]._y;
                        _Snake[i]._x = _Snake[i - 1]._x;
                    }
                    _Snake[1]._y = _yZero_old;
                }
            }
            else if (_key == ConsoleKey.RightArrow)
            {
                _Snake[0]._x++;

                if (_Snake.Count>1)
                {
                    Console.SetCursorPosition(_Snake[_Snake.Count - 1]._x, _Snake[_Snake.Count - 1]._y);
                    Console.Write(" ");
                    for (int i = _Snake.Count - 1; i > 0; i--)
                    {
                        _Snake[i]._x = _Snake[i - 1]._x;
                        _Snake[i]._y = _Snake[i - 1]._y;
                    }
                    _Snake[1]._x = _xZero_old;
                }
               
            }
            else if (_key == ConsoleKey.LeftArrow)
            {
                _Snake[0]._x--;
                if (_Snake.Count > 1)
                {
                    Console.SetCursorPosition(_Snake[_Snake.Count - 1]._x, _Snake[_Snake.Count - 1]._y);
                    Console.Write(" ");
                    for (int i = _Snake.Count - 1; i > 0; i--)
                    {
                        _Snake[i]._x = _Snake[i - 1]._x;
                        _Snake[i]._y = _Snake[i - 1]._y;
                    }
                    _Snake[1]._x = _xZero_old;
                }

            }
            else if (_key == ConsoleKey.R)
            {
                GameRestart = true;
                NoGameOver = false;
            }

            if (_key == ConsoleKey.UpArrow || _key == ConsoleKey.DownArrow || _key == ConsoleKey.RightArrow || _key == ConsoleKey.LeftArrow)
            {
                _lastMove = _key;
            }

            if (_rndX == _Snake[0]._x && _rndY == _Snake[0]._y && NoGameOver)
            {
                AddBlock();

                _Snake.Add(new _coordinates {_x = _xZero_old, _y = _yZero_old });

            }

            //end game--------------------
            if (GameOver())
            {
                NoGameOver = false;
                Console.Clear();
                Console.Write("Game Over! Score: " + _Score);
                Console.ReadKey();
            }

            if (WinGame())
            {
                NoGameOver = false;
                Console.Clear();
                Console.Write("You won! Score: " + _Score);
                Console.ReadKey();
            }
            //end game--------------------

            if ((_xZero_old != _Snake[0]._x || _yZero_old != _Snake[0]._y) && NoGameOver)
            {
                Console.SetCursorPosition(_xZero_old, _yZero_old);
                Console.Write(" ");
                draw_O();
                Console.SetCursorPosition(0, 0);
            }
            _gTimer.Start();
        }

        static bool GameOver()
        {
            if (_Snake[0]._x == 0 || _Snake[0]._x == _columns + 1 || _Snake[0]._y == _rows + 1 || _Snake[0]._y == 0)
            {
                return true;
            }

            var _bach = _Snake.FindAll(f => f._x == _Snake[0]._x & f._y == _Snake[0]._y);
            if (_bach.Count>1)
            {
                return true;
            }

            return false;
        }

        static bool WinGame()
        {
            if (_Interval<1)
            {
                return true;
            }
            return false;
        }

        static void draw()
        {
            var _average = Math.Round((decimal)_rows/2);
            draw_start_stop();

            Console.Write("\n");
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _columns; j++)
                {
                    
                    if (j == 0)
                    {
                        Console.Write($"|"); //: '\0').ToString() + 
                    }

                    Console.Write($" ");
                    if (j == _columns - 1)
                    {
                        Console.Write($"|");
                    }
                }
                Console.Write("\n");
            }
            draw_start_stop();

            draw_menu();
        }

        static void AddScore()
        {
            _Score++;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(_columns + 12, _lineStartMenu);
            Console.Write(_Score);
            Console.ResetColor();
            if (Math.Round((decimal)(_Score/3))== _Score/3 && _Score >0)
            {
                _Interval = _Interval - 15;
                
                if (_Interval > 0)
                {
                    _gTimer.Interval = _Interval;
                }
                else{
                    NoGameOver = false;
                }
            }
        }

        static void draw_O()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(_Snake[0]._x, _Snake[0]._y);
            Console.Write("O");
            for (int i = 1; i < _Snake.Count; i++)
            {
                Console.SetCursorPosition(_Snake[i]._x, _Snake[i]._y);
                Console.Write("o");
            }
            
            Console.ResetColor();
        }

        static void AddBlock()
        {
           
            do
            {
                _rndX = rnd.Next(1, _columns - 1);
                _rndY = rnd.Next(1, _rows - 1);
            } while (_rndX == _Snake[0]._x && _rndY == _Snake[0]._y && _Snake.FindAll(f=>f._x ==_rndX & f._y==_rndY).Count>0);
           
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(_rndX, _rndY);
            Console.Write("x");
            Console.ResetColor();

            AddScore();
        }

        static void draw_start_stop()
        {
            for (int i = 0; i < _columns+1; i++)
            {
                if (i == 0)
                {
                    Console.Write(" ");
                }
                else
                {
                    Console.Write("-");
                }
            }
        }

        static void draw_menu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(_columns + 5, _lineStartMenu);
            Console.Write($"Score: 0");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(_columns + 5, _lineStartMenu+2);
            Console.Write($"R-restart");
            Console.ResetColor();
        }
    }
}
