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
        static bool pauseMenu = false;
        static bool mainGame = false;
        static string[] mainMenuOptions = { "START", "DIFFICULTY", "EXIT" };
        static string[] difficultyOptions = { "EASY", "MEDIUM", "IMPOSSIBLE", "EXIT" };
        static string[] pauseMenuOptions = { "RESUME", "EXIT" };

        // Inicializacia hlavneho fieldu, pouzite aj pri prepisovani -> ked exitneme hru
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
            // nejake premenne
            Console.CursorVisible = false;
            InitializeField();
            Piece currentPiece = Piece.GetRandom();
            int currentX = width / 2 - 1;
            int currentY = 0;
            bool finished = false;
            Menu mainMenu_menu = new Menu(mainMenuOptions, false);
            Menu diffMenu = new Menu(difficultyOptions, true);
            Menu pauseMenu_menu = new Menu(pauseMenuOptions, false);

            /*
             * Hlavny loop hry
             * Cez if pozera ktora cast hry ma byt nacitana
             * Hybanie pomocou while -> urobene kvoli tomu ako funguju inputy do konzole - ak by sa to robilo cez if, kazdy input by sa zapisal do nejakeho queue
             * a postupne by sa vykonavali, takto sa beru inputy iba vtedy, ked realne drzime nejaku klavesu
             */
            while (true)
            {
                if (mainMenu)
                {
                    if(!mainMenu_menu.Finished)
                    {
                        mainMenu_menu.DrawMenu(consoleX, consoleY, height);
                        mainMenu_menu.Finished = true;
                    }
                    while (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.UpArrow)
                        {
                            mainMenu_menu.Selected--;
                            mainMenu_menu.CheckSelect();
                            mainMenu_menu.DrawMenu(consoleX, consoleY, height);
                        }
                        if (key == ConsoleKey.DownArrow)
                        {
                            mainMenu_menu.Selected++;
                            mainMenu_menu.CheckSelect();
                            mainMenu_menu.DrawMenu(consoleX, consoleY, height);
                        }
                        if (key == ConsoleKey.Enter)
                        {
                            if (mainMenu_menu.Selected == 0)
                            {
                                mainMenu = false;
                                mainMenu_menu.ReturnToDefault();
                                mainGame = true;
                            }
                            else if (mainMenu_menu.Selected == 1)
                            {
                                mainMenu = false;
                                mainMenu_menu.ReturnToDefault();
                                difficultyMenu = true;
                            }
                            else if (mainMenu_menu.Selected == 2)
                            {
                                Console.Clear();
                                return;
                            }
                        }
                    }
                }
                if (difficultyMenu)
                {
                    if(!diffMenu.Finished)
                    {
                        diffMenu.DrawMenu(consoleX,consoleY,height);
                        diffMenu.Finished = true;
                    }
                    while (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.UpArrow)
                        {
                            diffMenu.Selected--;
                            diffMenu.CheckSelect();
                            diffMenu.DrawMenu(consoleX, consoleY, height);
                        }
                        if (key == ConsoleKey.DownArrow)
                        {
                            diffMenu.Selected++;
                            diffMenu.CheckSelect();
                            diffMenu.DrawMenu(consoleX, consoleY, height);
                        }
                        if (key == ConsoleKey.Enter)
                        {
                            if (diffMenu.Selected == 0)
                            {
                                speed = 200;
                                diffMenu.Diffucilty = 0;
                                diffMenu.DrawMenu(consoleX, consoleY, height);
                            }
                            else if (diffMenu.Selected == 1)
                            {
                                speed = 125;
                                diffMenu.Diffucilty = 1;
                                diffMenu.DrawMenu(consoleX, consoleY, height);
                            }
                            else if (diffMenu.Selected == 2)
                            {
                                speed = 50;
                                diffMenu.Diffucilty = 2;
                                diffMenu.DrawMenu(consoleX, consoleY, height);
                            }
                            else if (diffMenu.Selected == 3)
                            {
                                difficultyMenu = false;
                                diffMenu.ReturnToDefault();
                                Console.Clear();
                                mainMenu = true;
                            }
                        }
                    }
                }
                if (pauseMenu)
                {
                    if (!pauseMenu_menu.Finished)
                    {
                        Console.Clear();
                        pauseMenu_menu.DrawMenu(consoleX, consoleY, height);
                        pauseMenu_menu.Finished = true;
                    }
                    while (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.UpArrow)
                        {
                            pauseMenu_menu.Selected--;
                            pauseMenu_menu.CheckSelect();
                            pauseMenu_menu.DrawMenu(consoleX, consoleY, height);
                        }
                        if (key == ConsoleKey.DownArrow)
                        {
                            pauseMenu_menu.Selected++;
                            pauseMenu_menu.CheckSelect();
                            pauseMenu_menu.DrawMenu(consoleX, consoleY, height);
                        }
                        if (key == ConsoleKey.Enter)
                        {
                            if (pauseMenu_menu.Selected == 0)
                            {
                                pauseMenu = false;
                                pauseMenu_menu.ReturnToDefault();
                                mainGame = true;
                            }
                            else if (pauseMenu_menu.Selected == 1)
                            {
                                pauseMenu = false;
                                pauseMenu_menu.ReturnToDefault();
                                InitializeField();
                                currentPiece = Piece.GetRandom();
                                currentX = width / 2 - 1;
                                currentY = 0;
                                score = 0;
                                mainMenu = true;
                            }
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
                            InitializeField();
                            currentPiece = Piece.GetRandom();
                            currentX = width / 2 - 1;
                            currentY = 0;
                            score = 0;
                            finished = false;
                            mainMenu = true;
                        }
                    }
                }
            }
        }

        /*
         * Vypisovanie herneho pola
         * Robene cez prepisovanie, nie cez clearovanie -> hra je viac smooth, az tak vela neblika
         * Dosiahnute pomocou hybania kurzoru
         */
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

        /*
         * Pozeranie mozneho movu
         * Ak je move mimo fieldu hry alebo prechadza cez dalsi block pocita sa ako nevalidny
         */
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
        /*
         * Ukladanie kociek
         * pretoze je robeny cez 2 polia musi sa kocka ulozit do hlavneho herneho pola
         */
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

        /*
         * Hybanie kociek
         * Robene cez referencie aby sa posizie mohli nacitat a rovno zapisovat
         * dx, dy - direction, je zadany priamo pri pozerani inputov pre jednoduhsiu implementaciu
         */
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

        // Rotovanie kociek, ak nie je validny (bol by mimo pole alebo uz by tam bola nejaka kocka), vrati sa do povodneho stavu
        static void Rotate(Piece piece, ref int posX, ref int posY)
        {
            piece.Rotate();
            if (!IsValidMove(piece, posX, posY))
            {
                piece.RotateBack();
            }
        }

        /*
         * Pozeranie ci je plna line
         * V podstate sa pozera cely field ci v lineach nie su diery (' ')
         * Ak niesu pripocita sa 100 bodov, line sa clearne a cely field sa od line ktora bola clearnuta posunie o policko dole
         */
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

    // classa pre vytvaranie menucok
    class Menu
    {
        // options ktore sa zobrazia v menu
        public string[] Options { get; private set; }
        // ktory option je vybrany
        public int Selected { get; set; }
        // vybrana difficulty (pre DIFFICULTY menu)
        public int Diffucilty { get; set; }
        // pre lahsie a krajsie vypisovanie (aby sa stale neopakovalo)
        public bool Finished { get; set; }
        // pre zistenie ci je menu DIFFICULTY alebo vsetky ostatne
        private bool Type { get; }

        public Menu(string[] options, bool type)
        {
            this.Options = options;
            this.Selected = 0;
            this.Diffucilty = 0;
            this.Finished = false;
            this.Type = type;
        }

        // vypisovanie samotneho menucka
        public void DrawMenu(int consoleX, int consoleY, int height)
        {
            for (int i = 0; i < Options.Length; i++)
            {
                Console.SetCursorPosition(consoleX, consoleY + height / 2 - Options.Length + i);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(consoleX, consoleY + height / 2 - Options.Length + i);
                if (i == Selected)
                {
                    Console.Write("-> ");
                }
                if(this.Type == true && i < this.Options.Length - 1)
                {
                    if(this.Diffucilty == i)
                    {
                        Console.Write("|#| " + Options[i]);
                    }
                    else
                    {
                        Console.Write("| | " + Options[i]);
                    }
                }
                else
                {
                    Console.Write(Options[i]);
                }

            }
        }
        // pozeranie ci nie je vybrany option mimo rozsahu
        public void CheckSelect()
        {
            if (this.Selected < 0)
            {
                this.Selected = this.Options.Length; 
            }
            if (this.Selected > this.Options.Length - 1)
            {
                this.Selected = 0;
            }
        }
        // vratenie menucka do povodneho stavu
        public void ReturnToDefault()
        {
            this.Selected = 0;
            this.Finished = false;
        }
    }
}