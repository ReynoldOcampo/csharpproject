using System;

public enum CellState //define cell on the gameboard
{
    Empty;
    PlayerOne;
    PlayerTwo;
}

public class GameBoard //gameboard
{
    private const int Rows = 6
    private const int Cols = 7
    private CellState[,] board;
}

public GameBoard()
{
    GameBoard = new CellState[Rows + 1, Cols + 1];
    ClearBoard();
}