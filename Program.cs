using System;
using System.Threading;

namespace Tetris
{
    class Program
    {
        static int width = 10;
        static int height = 20;
        static int score = 0;
        static char[,] field = new char[height, width];
        static int consoleX = Console.WindowWidth / 2 - width / 2 - 1;
        static int consoleY = Console.WindowHeight / 2 - height / 2 - 1;
        static int speed = 200;
        static bool isGameOver = false;
        static bool mainMenu = true;
        static bool difficultyMenu = false;
        static bool pauseMenu = false; // TODO
        static bool mainGame = false;
        static string[] mainMenuOptions = { "START", "DIFFICULTY", "EXIT" };
        static string[] difficultyOptions = { "EASY", "MEDIUM", "IMPOSSIBLE", "EXIT" };
        static string[] pauseMenuOptions = { "RESUME", "EXIT" };
        

        /*
         * PODLA CASU SCORE BOARD ALE NEVIEM CI TO STIHNEM DO PIATKU MUSI PRIST NEJAKY SPEED DEVELOPMENT XDDXXDXDXDXDDDDDDD (uz mi sibe z toho)
         */

        static void InitializeField()
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    field[y, x] = ' ';
                }
            }
        }

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            InitializeField();
            Piece currentPiece = Piece.GetRandom();
            int currentX = width / 2 - 1;
            int currentY = 0;
            int selectedDifficulty = 0; // TODO
            int selected = 0;
            bool finished = false;

            while (true)
            {
                if (mainMenu)
                {
                    if(!finished)
                    {
                        DrawMenu(mainMenuOptions, selected);
                        finished = true;
                    }
                    while (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.UpArrow)
                        {
                            selected--;
                            if (selected < 0)
                            {
                                selected = mainMenuOptions.Length - 1;
                            }
                            DrawMenu(mainMenuOptions, selected);
                        }
                        if (key == ConsoleKey.DownArrow)
                        {
                            selected++;
                            if (selected > mainMenuOptions.Length - 1)
                            {
                                selected = 0;
                            }
                            DrawMenu(mainMenuOptions, selected);
                        }
                        if (key == ConsoleKey.Enter)
                        {
                            if (selected == 0)
                            {
                                mainMenu = false;
                                ReturnToDefault(ref selected, ref finished);
                                mainGame = true;
                            }
                            else if (selected == 1)
                            {
                                mainMenu = false;
                                ReturnToDefault(ref selected, ref finished);
                                difficultyMenu = true;
                            }
                            else if (selected == 2)
                            {
                                Console.Clear();
                                return;
                            }
                        }
                    }
                }

                if (difficultyMenu)
                {
                    if(!finished)
                    {
                        DrawMenu(difficultyOptions, selected);
                        finished = true;
                    }
                    while (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.UpArrow)
                        {
                            selected--;
                            if (selected < 0)
                            {
                                selected = difficultyOptions.Length - 1;
                            }
                            DrawMenu(difficultyOptions, selected);
                        }
                        if (key == ConsoleKey.DownArrow)
                        {
                            selected++;
                            if (selected > difficultyOptions.Length - 1)
                            {
                                selected = 0;
                            }
                            DrawMenu(difficultyOptions, selected);
                        }
                        if (key == ConsoleKey.Enter)
                        {
                            if (selected == 0)
                            {
                                speed = 200;
                            }
                            else if (selected == 1)
                            {
                                speed = 125;
                            }
                            else if (selected == 2)
                            {
                                speed = 50;
                            }
                            else if (selected == 3)
                            {
                                difficultyMenu = false;
                                ReturnToDefault(ref selected, ref finished);
                                Console.Clear();
                                mainMenu = true;
                            }
                        }
                    }
                }

                if (pauseMenu)
                {
                    Console.Clear();
                    while (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.Enter)
                        {
                            pauseMenu = false;
                            ReturnToDefault(ref selected, ref finished);
                            mainGame = true;
                        }
                    }
                }

                if (mainGame)
                {
                    while (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.LeftArrow) MovePiece(currentPiece, ref currentX, ref currentY, -1, 0);
                        if (key == ConsoleKey.RightArrow) MovePiece(currentPiece, ref currentX, ref currentY, 1, 0);
                        if (key == ConsoleKey.DownArrow) MovePiece(currentPiece, ref currentX, ref currentY, 0, 1);
                        if (key == ConsoleKey.UpArrow) Rotate(currentPiece, ref currentX, ref currentY);
                        if (key == ConsoleKey.P)
                        {
                            mainGame = false;
                            pauseMenu = true;
                        }
                    }

                    if (!MovePiece(currentPiece, ref currentX, ref currentY, 0, 1))
                    {
                        PlacePiece(currentPiece, currentX, currentY);
                        CheckLines();
                        currentPiece = Piece.GetRandom();
                        currentX = width / 2 - 1;
                        currentY = 0;

                        if (!IsValidMove(currentPiece, currentX, currentY))
                        {
                            mainGame = false;
                            isGameOver = true;
                        }
                    }

                    Draw(currentPiece, currentX, currentY);
                    Thread.Sleep(speed);
                }

                if (isGameOver)
                {
                    if(!finished)
                    {
                        Console.Clear();
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 4, Console.WindowHeight / 2);
                        Console.WriteLine("GAME OVER!");
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 4, Console.WindowHeight / 2 + 1);
                        Console.WriteLine($"Score: {score}");
                        finished = true;
                    }
                    while (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.Enter)
                        {
                            isGameOver = false;
                            Console.Clear();
                            ReturnToDefault(ref selected, ref finished);
                            mainMenu = true;
                        }
                    }
                }
            }
        }


        static void ReturnToDefault (ref int selected, ref bool finished)
        {
            selected = 0;
            finished = false;
        }

        static void Draw(Piece currentPiece, int posX, int posY)
        {
            Console.SetCursorPosition(consoleX, consoleY);
            Console.Write("+" + new string('-', width) + "+");

            char[,] tempField = (char[,])field.Clone();
            for (int y = 0; y < currentPiece.Size; y++)
            {
                for (int x = 0; x < currentPiece.Size; x++)
                {
                    if (currentPiece.Shape[y, x] != ' ')
                    {
                        int drawX = posX + x;
                        int drawY = posY + y;
                        if (drawX >= 0 && drawX < width && drawY >= 0 && drawY < height)
                        {
                            tempField[drawY, drawX] = currentPiece.Shape[y, x];
                        }
                    }
                }
            }
            for (int y = 0; y < height; y++)
            {
                Console.SetCursorPosition(consoleX, consoleY + y + 1);
                Console.Write("|");
                for (int x = 0; x < width; x++)
                {
                    Console.Write(tempField[y, x]);
                }
                Console.Write("|");
            }
            Console.SetCursorPosition(consoleX, consoleY + height + 1);
            Console.Write("+" + new string('-', width) + "+");
            Console.SetCursorPosition(consoleX, consoleY + height + 3);
            Console.Write($"Score: {score}");
        }

        static bool IsValidMove(Piece piece, int posX, int posY)
        {
            for (int y = 0; y < piece.Size; y++)
            {
                for (int x = 0; x < piece.Size; x++)
                {
                    if (piece.Shape[y, x] != ' ')
                    {
                        int newX = posX + x;
                        int newY = posY + y;
                        if (newX < 0 || newX >= width || newY < 0 || newY >= height || field[newY, newX] != ' ')
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        static void PlacePiece(Piece piece, int posX, int posY)
        {
            for (int y = 0; y < piece.Size; y++)
            {
                for (int x = 0; x < piece.Size; x++)
                {
                    if (piece.Shape[y, x] != ' ')
                    {
                        field[posY + y, posX + x] = piece.Shape[y, x];
                    }
                }
            }
        }

        static bool MovePiece(Piece piece, ref int posX, ref int posY, int dx, int dy)
        {
            int newX = posX + dx;
            int newY = posY + dy;

            if (IsValidMove(piece, newX, newY))
            {
                posX = newX;
                posY = newY;
                return true;
            }

            return false;
        }

        static void Rotate(Piece piece, ref int posX, ref int posY)
        {
            piece.Rotate();
            if (!IsValidMove(piece, posX, posY))
            {
                piece.RotateBack();
            }
        }

        static void CheckLines()
        {
            for (int y = 0; y < height; y++)
            {
                bool fullLine = true;
                for (int x = 0; x < width; x++)
                {
                    if (field[y, x] == ' ')
                    {
                        fullLine = false;
                        break;
                    }
                }

                if (fullLine)
                {
                    score += 100;
                    for (int lineY = y; lineY > 0; lineY--)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            field[lineY, x] = field[lineY - 1, x];
                        }
                    }

                    for (int x = 0; x < width; x++)
                    {
                        field[0, x] = ' ';
                    }
                }
            }
        }

        static void DrawMenu(string[] options, int selected)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.SetCursorPosition(consoleX, consoleY + height / 2 - options.Length + i);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(consoleX, consoleY + height / 2 - options.Length + i);
                if (i == selected)
                {
                    Console.Write("-> " + options[i]);
                }
                else
                {
                    Console.Write(options[i]);
                }

            }
        }
    }
    /*
     * klasa pre padajuce blocky,
     * nikdy som nevedel ze sa volaju 'tetromino' loliky lol
     */
    class Piece
    {
        // aky tvar ma kusok
        public char[,] Shape { get; private set; }
        // velkost block pre vykreslovanie
        public int Size { get; private set; }
        // originalny tvar kusku pre pripad nudze 
        private char[,] originalShape;

        public Piece(char[,] shape)
        {
            this.Shape = shape;
            this.originalShape = (char[,])shape.Clone();
            this.Size = shape.GetLength(0);
        }

        // rotovanie kusku
        public void Rotate()
        {
            char[,] newShape = new char[Size, Size];
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    newShape[x, Size - 1 - y] = Shape[y, x];
                }
            }
            Shape = newShape;
        }

        public void RotateBack()
        {
            Shape = (char[,])originalShape.Clone();
        }

        // vypisane kusky z ktorych sa da vyberat + randomne vyberanie
        public static Piece GetRandom()
        {
            Random random = new Random();
            switch (random.Next(0, 5))
            {
                case 0:
                    return new Piece(new char[,] {
                    { ' ', 'X', ' ' },
                    { 'X', 'X', 'X' },
                    { ' ', ' ', ' ' }
                });
                case 1:
                    return new Piece(new char[,] {
                    { 'X', 'X' },
                    { 'X', 'X' }
                });
                case 2:
                    return new Piece(new char[,] {
                    { 'X', ' ', ' ' },
                    { 'X', 'X', 'X' },
                    { ' ', ' ', ' ' }
                });
                case 3:
                    return new Piece(new char[,] {
                    { ' ', ' ', 'X' },
                    { 'X', 'X', 'X' },
                    { ' ', ' ', ' ' }
                });
                case 4:
                    return new Piece(new char[,] {
                    { 'X', 'X', 'X', 'X' },
                    { ' ', ' ', ' ', ' ' },
                    { ' ', ' ', ' ', ' ' },
                    { ' ', ' ', ' ', ' ' }
                });
                default: return null;
            }
        }
    }
}

