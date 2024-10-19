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
        static bool isGameOver = false;

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

            while (!isGameOver)
            {
                while (Console.KeyAvailable)
                { 
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.LeftArrow) MovePiece(currentPiece, ref currentX, ref currentY, -1, 0);
                    if (key == ConsoleKey.RightArrow) MovePiece(currentPiece, ref currentX, ref currentY, 1, 0);
                    if (key == ConsoleKey.DownArrow) MovePiece(currentPiece, ref currentX, ref currentY, 0, 1);
                    if (key == ConsoleKey.UpArrow) Rotate(currentPiece, ref currentX, ref currentY);
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
                        isGameOver = true;
                    }
                }

                Draw(currentPiece, currentX, currentY);
                Thread.Sleep(200);
            }

            Console.Clear();
            Console.SetCursorPosition(width / 2 - 4, height / 2);
            Console.WriteLine("GAME OVER!");
            Console.SetCursorPosition(width / 2 - 4, height / 2 + 1);
            Console.WriteLine($"Score: {score}");
        }

        static void Draw(Piece currentPiece, int posX, int posY)
        {
            Console.SetCursorPosition(0, 0);

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
                for (int x = 0; x < width; x++)
                {
                    Console.Write(tempField[y, x]);
                }
                Console.WriteLine();
            }

            Console.SetCursorPosition(0, height);
            Console.WriteLine($"Score: {score}");
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
    }

    class Piece
    {
        public char[,] Shape { get; private set; }
        public int Size { get; private set; }

        private char[,] originalShape;

        public Piece(char[,] shape)
        {
            this.Shape = shape;
            this.originalShape = (char[,])shape.Clone();
            this.Size = shape.GetLength(0);
        }

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

        public static Piece GetRandom()
        {
            Random rand = new Random();
            switch (rand.Next(0, 6))
            {
                case 0:
                    return new Piece(new char[,] {
                    { 'X', 'X', ' ' },
                    { 'X', 'X', ' ' },
                    { ' ', ' ', ' ' }
                });
                case 1:
                    return new Piece(new char[,] {
                    { ' ', 'X', ' ' },
                    { 'X', 'X', 'X' },
                    { ' ', ' ', ' ' }
                });
                case 2:
                    return new Piece(new char[,] {
                    { 'X', 'X' },
                    { 'X', 'X' }
                });
                case 3:
                    return new Piece(new char[,] {
                    { 'X', ' ', ' ' },
                    { 'X', 'X', 'X' },
                    { ' ', ' ', ' ' }
                });
                case 4:
                    return new Piece(new char[,] {
                    { ' ', ' ', 'X' },
                    { 'X', 'X', 'X' },
                    { ' ', ' ', ' ' }
                });
                case 5:
                    return new Piece(new char[,] {
                    { 'X', 'X', 'X', 'X' },
                    { ' ', ' ', ' ', ' ' },
                    { ' ', ' ', ' ', ' ' },
                    { ' ', ' ', ' ', ' ' }
                });
                default:return null;
            }
        }
    }
}
