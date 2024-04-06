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
