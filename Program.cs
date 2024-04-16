using System;
using System.Threading;

// Define the possible states of each cell on the game board
public enum CellState
{
    Empty,
    PlayerOne,
    PlayerTwo
}

// Define the game board class
public class GameBoard
{
    private const int Rows = 6; // Number of rows in the game board
    private const int Cols = 7; // Number of columns in the game board
    private CellState[,] board; // 2D array representing the game board

    // Constructor to initialize the game board
    public GameBoard()
    {
        board = new CellState[Rows + 1, Cols + 1]; // Add 1 to both dimensions to allow for 1-based indexing
        ClearBoard(); // Initialize the board with empty cells
    }

    // Constructor initializes the game board and clears it
    public GameBoard()
    {
        board = new CellState[Rows + 1, Cols + 1];
        ClearBoard();
    }

    // Method to clear the game board
    public void ClearBoard()
    {
        for (int row = 1; row <= Rows; row++)
        {
            for (int col = 1; col <= Cols; col++)
            {
                board[row, col] = CellState.Empty;
            }
        }
    }

    // Method to check if a move is valid
    public bool IsValidMove(int col)
    {
        return col >= 1 && col <= Cols && board[1, col] == CellState.Empty;
    }

    // Method to make a move on the board
    public bool MakeMove(int col, CellState player)
    {
        if (!IsValidMove(col))
            return false;

        int row;
        for (row = Rows; row >= 1; row--)
        {
            if (board[row, col] == CellState.Empty)
            {
                board[row, col] = player;
                return true;
            }
        }

        return false;
    }

    // Method to check if the board is full
    public bool IsFull()
    {
        for (int col = 1; col <= Cols; col++)
        {
            if (board[1, col] == CellState.Empty)
                return false;
        }
        return true;
    }

    // Method to check for a win condition
    public bool CheckWin(CellState player, out int[] winningCombination, out string winType)
    {
        winningCombination = null;
        winType = "";

        // Check for horizontal win
        for (int row = 1; row <= Rows; row++)
        {
            for (int col = 1; col <= Cols - 3; col++)
            {
                if (board[row, col] == player &&
                    board[row, col + 1] == player &&
                    board[row, col + 2] == player &&
                    board[row, col + 3] == player)
                {
                    winningCombination = new int[] { row, col, row, col + 1, row, col + 2, row, col + 3 };
                    winType = "horizontal";
                    return true;
                }
            }
        }

        // Check for vertical win
        for (int col = 1; col <= Cols; col++)
        {
            for (int row = 1; row <= Rows - 3; row++)
            {
                if (board[row, col] == player &&
                    board[row + 1, col] == player &&
                    board[row + 2, col] == player &&
                    board[row + 3, col] == player)
                {
                    winningCombination = new int[] { row, col, row + 1, col, row + 2, col, row + 3, col };
                    winType = "vertical";
                    return true;
                }
            }
        }

        // Check for diagonal win (down-right)
        for (int row = 1; row <= Rows - 3; row++)
        {
            for (int col = 1; col <= Cols - 3; col++)
            {
                if (board[row, col] == player &&
                    board[row + 1, col + 1] == player &&
                    board[row + 2, col + 2] == player &&
                    board[row + 3, col + 3] == player)
                {
                    winningCombination = new int[] { row, col, row + 1, col + 1, row + 2, col + 2, row + 3, col + 3 };
                    winType = "diagonal (down-right)";
                    return true;
                }
            }
        }

        // Check for diagonal win (down-left)
        for (int row = 1; row <= Rows - 3; row++)
        {
            for (int col = 4; col <= Cols; col++)
            {
                if (board[row, col] == player &&
                    board[row + 1, col - 1] == player &&
                    board[row + 2, col - 2] == player &&
                    board[row + 3, col - 3] == player)
                {
                    winningCombination = new int[] { row, col, row + 1, col - 1, row + 2, col - 2, row + 3, col - 3 };
                    winType = "diagonal (down-left)";
                    return true;
                }
            }
        }

        return false;
    }

    // Method to display the game board
    public void DisplayBoard()
    {
        for (int row = 1; row <= Rows; row++)
        {
            for (int col = 1; col <= Cols; col++)
            {
                switch (board[row, col])
                {
                    case CellState.Empty:
                        Console.Write("| ");
                        break;
                    case CellState.PlayerOne:
                        Console.Write("|O");
                        break;
                    case CellState.PlayerTwo:
                        Console.Write("|X");
                        break;
                }
            }
            Console.WriteLine("|");
        }
        Console.WriteLine("---------------");
        Console.WriteLine(" 1 2 3 4 5 6 7 ");
        Console.WriteLine("---------------");
    }
}

// Class representing the Connect Four game
public class ConnectFourGame
{
    private readonly GameBoard board;
    private readonly Player[] players;
    private int currentPlayerIndex;

    // Method to start and manage the game
    public void Play()
    {
        Console.WriteLine("Welcome to Connect Four!");

        while (true)
        {
            Console.Clear(); // Clear the console and display the game board
            board.DisplayBoard();
            int col;

            Thread.Sleep(1000);

            col = players[currentPlayerIndex].GetMove();// Get the current player's move

            while (!board.MakeMove(col, players[currentPlayerIndex].Token)) // Handle case where column is full
            {
                Console.WriteLine("Column is full. Please choose another column.");
                Thread.Sleep(1000);
                col = players[currentPlayerIndex].GetMove();
            }

            int[] winningCombination; // Check for win or draw
            string winType;
            if (board.CheckWin(players[currentPlayerIndex].Token, out winningCombination, out winType))
            {
                Console.Clear(); // Display win message
                board.DisplayBoard();
                Console.WriteLine($"{players[currentPlayerIndex].Name} wins by {winType}!");
                if (!PlayAgain()) break;// Ask if players want to play again
                else board.ClearBoard(); // Reset the board if they do
            }
            else if (board.IsFull())
            {
                Console.Clear();
                board.DisplayBoard();
                Console.WriteLine("It's a draw!");// Display draw message
                if (!PlayAgain()) break; // Ask if players want to play again
                else board.ClearBoard();
            }
            else
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % 2;// Switch to the next player
            }
        }
    }
    // Method to ask if players want to play again
    private bool PlayAgain()
    {
        Console.Write("Do you want to play again? (yes/no): ");
        string input = Console.ReadLine().Trim().ToLower();
        return input == "yes" || input == "y";
    }
}

// Main program class
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to Connect Four Game");
        Console.WriteLine("Click Enter to Start the Game");
        Console.ReadLine();

        // Get names of players
        Console.Write("Enter Player 1 Name: ");
        string player1Name = Console.ReadLine();

        Console.Write("Enter Player 2 Name: ");
        string player2Name = Console.ReadLine();

        // Create player objects
        Player player1 = new HumanPlayer(player1Name, CellState.PlayerOne);
        Player player2 = new HumanPlayer(player2Name, CellState.PlayerTwo);

        // Create game object and start the game
        ConnectFourGame game = new ConnectFourGame(player1, player2);
        game.Play();
    }
}

