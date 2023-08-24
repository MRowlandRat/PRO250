using ASP_Minesweeper.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP_Minesweeper.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        static int BOMB;
        public static int x_choice, y_choice, SIZE, level;
        public char[,] board;
        bool[,] choice_board;
        char[,] game_board;
        bool loss = false;
        bool win = false;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Start(string x, string y, string board, string game, string choice)
        {
            this.board = Newtonsoft.Json.JsonConvert.DeserializeObject<char[,]>(board);
            this.game_board = Newtonsoft.Json.JsonConvert.DeserializeObject<char[,]>(game);
            this.choice_board = Newtonsoft.Json.JsonConvert.DeserializeObject<bool[,]>(choice);
            int inputx;
            int inputy;

            try
            {
                inputx = int.Parse(x);
                inputy = int.Parse(y);
            }
            catch
            {
                ViewBag.board = this.board;
                ViewBag.gameBoard = this.game_board;
                ViewBag.choiceBoard = this.choice_board;
                return View();
            }
            if (inputx < 1 || inputx > this.board.GetLength(0) || inputy < 1 || inputy > this.board.GetLength(0))
            {
                ViewBag.board = this.board;
                ViewBag.gameBoard = this.game_board;
                ViewBag.choiceBoard = this.choice_board;
                return View();
            }
            else
            {
                inputx -= 1;
                inputy -= 1;
            }


            string res = CheckGame(inputx, inputy);
            if (res == "lost")
            {
                ViewBag.state = "lost";
                ViewBag.board = this.game_board;
                return View();

            }
            else if (res == "won")
            {
                ViewBag.state = "won";
                ViewBag.board = this.game_board;
            }
            MakeBoard();

            ViewBag.board = this.board;
            ViewBag.gameBoard = this.game_board;
            ViewBag.choiceBoard = this.choice_board;
            return View();
        }

        [HttpGet]
        public IActionResult Start(string difficulty)
        {
            ViewBag.difficulty = difficulty;
            switch (difficulty)
            {
                case "easy":
                    SIZE = 5;
                    BOMB = 5;
                    break;
                case "med":
                    SIZE = 8;
                    BOMB = 15;
                    break;
                case "hard":
                    SIZE = 10;
                    BOMB = 25;
                    break;
                default:
                    SIZE = 15;
                    BOMB = 8;
                    break;

            }
            Setup();
            MakeBoard();
            ViewBag.board = this.board;
            ViewBag.gameBoard = this.game_board;
            ViewBag.choiceBoard = this.choice_board;


            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void MakeBoard()
        {
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {

                    //print bomb if lose
                    if (this.game_board[i, j] == 'B' && loss)
                    {
                        this.board[i, j] = 'B';
                    }
                    // print "?" 
                    else if ((this.game_board[i, j] == 'B' && !loss) || (this.game_board[i, j] == ' ' && this.choice_board[i, j] == false) || (this.game_board[i, j] != 'B' && this.game_board[i, j] != ' ' && this.choice_board[i, j] == false))
                    {
                        this.board[i, j] = '?';
                    }
                    // print 1,2,3 if selected
                    else if (this.game_board[i, j] != 'B' && this.choice_board[i, j] == true)
                    {
                        this.board[i, j] = this.game_board[i, j];
                    }

                }
            }
        }

        public void Setup()
        {
            this.choice_board = new bool[SIZE, SIZE];
            this.game_board = new char[SIZE, SIZE];
            this.board = new char[SIZE, SIZE];
            SetBombs(ref game_board, BOMB, ref choice_board);
        }

        public static void SetBombs(ref char[,] board_char, int number, ref bool[,] board_bool)
        {

            Random rand = new Random();
            int x, y;
            while (number > 0)
            {
                do
                {
                    x = rand.Next(0, SIZE);
                    y = rand.Next(0, SIZE);
                    //Console.WriteLine($"{x},{y}");
                } while (board_char[x, y] == 'B');
                board_char[x, y] = 'B';
                number--;
            }
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (board_char[i, j] != 'B')
                    {
                        board_char[i, j] = ' ';
                    }
                    board_bool[i, j] = false;
                }
            }
            int num_bomb = 0;
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (board_char[i, j] != 'B')
                    {
                        if (i > 0 && board_char[i - 1, j] == 'B') // North neighbor
                        {
                            num_bomb++;
                        }
                        if (i > 0 && j < SIZE - 1 && board_char[i - 1, j + 1] == 'B') // East North neighbor
                        {
                            num_bomb++;
                        }

                        if (i > 0 && j > 0 && board_char[i - 1, j - 1] == 'B') // West North neighbor
                        {
                            num_bomb++;
                        }

                        if (j > 0 && board_char[i, j - 1] == 'B') // West neighbor
                        {
                            num_bomb++;
                        }

                        if (j < SIZE - 1 && board_char[i, j + 1] == 'B') // East neighbor
                        {
                            num_bomb++;
                        }

                        if (i < SIZE - 1 && j < SIZE - 1 && board_char[i + 1, j + 1] == 'B') // South neighbor
                        {
                            num_bomb++;
                        }

                        if (i < SIZE - 1 && j > 0 && board_char[i + 1, j - 1] == 'B') // West South neighbor
                        {
                            num_bomb++;
                        }
                        if (i < SIZE - 1 && board_char[i + 1, j] == 'B') // South
                        {
                            num_bomb++;
                        }

                    }
                    if (num_bomb > 0)
                    {
                        string str = num_bomb.ToString();
                        char[] temp = str.ToCharArray();
                        board_char[i, j] = temp[0];

                    }
                    num_bomb = 0;


                }
            }
        }

        public string CheckGame(int x, int y)
        {
            if (this.game_board[x, y] == 'B')
            {
                return "lost";

            }
            else
            {
                FindEmpty(x, y);
            }

            if (CheckWin())
            {
                return "won";
            }
            return null;
        }

        bool CheckWin()
        {
            int cnt = 0;
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (this.choice_board[i, j] == true)
                    {
                        cnt++;
                    }
                }
            }
            if (cnt + BOMB == (SIZE * SIZE))
            {
                return true;
            }
            return false;
        }

        public void FindEmpty(int x, int y)
        {
            if (this.choice_board[x, y] == false)
            {
                if (this.game_board[x, y] != 'B')
                {
                    this.choice_board[x, y] = true;
                }
                if (x > 0)
                {
                    if (this.game_board[x - 1, y] != 'B')
                    {
                        this.choice_board[x - 1, y] = true;

                    }
                }

                if (x > 0 && y < SIZE - 1)
                {
                    if (this.game_board[x - 1, y + 1] != 'B')
                    {
                        this.choice_board[x - 1, y + 1] = true;

                    }
                }

                if (x > 0 && y > 0)
                {
                    if (this.game_board[x - 1, y - 1] != 'B')
                    {
                        this.choice_board[x - 1, y - 1] = true;

                    }
                }

                if (y > 0)
                {
                    if (this.game_board[x, y - 1] != 'B')
                    {
                        this.choice_board[x, y - 1] = true;

                    }
                }

                if (y < SIZE - 1)
                {
                    if (this.game_board[x, y + 1] != 'B')
                    {
                        this.choice_board[x, y + 1] = true;

                    }
                }

                if (x < SIZE - 1 && y < SIZE - 1)
                {
                    if (this.game_board[x + 1, y + 1] != 'B')
                    {
                        this.choice_board[x + 1, y + 1] = true;

                    }
                }

                if (x < SIZE - 1 && y > 0)
                {
                    if (this.game_board[x + 1, y - 1] != 'B')
                    {
                        this.choice_board[x + 1, y - 1] = true;

                    }
                }


            }
        }
    }
}