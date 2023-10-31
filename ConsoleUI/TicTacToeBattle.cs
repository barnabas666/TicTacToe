using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI;

internal class TicTacToeBattle
{
    private bool isWinner = false;

    private Player player1;
    private Player player2;
    private Player actualPlayer; // actual player pointing to player1 or player2 through game.

    private int actualCoordinateX; // coordinate X[row] of last added battle stone
    private int actualCoordinateY; // coordinate Y[column] of last added battle stone

    private int fieldDimension; // dimension of battle field, we should put here at least 3
    private int winConditionNumber; // number of stones in row needed for win
    
    private int[,] battleField; // Battle Field, 0 == free field, 1 == player1, 2 == player2    
    private string[] battleStones; // Battle Stones, 0 == ' ', 1 == 'x', 2 == 'o'

    public TicTacToeBattle(int fieldDimension, Player player1, Player player2)
    {
        this.fieldDimension = fieldDimension;
        this.player1 = player1;
        this.player2 = player2;
        actualPlayer = player1;
        battleField = new int[fieldDimension, fieldDimension];
        battleStones = new string[] { " ", player1.BattleStone, player2.BattleStone };
        winConditionNumber = (fieldDimension > 5) ? 5 : fieldDimension;        
    }

    public void Play()
    {
        PrintBattleField();

        while (!isWinner)
        {
            EnterBattleStone(actualPlayer);

            PrintBattleField();            

            if (CheckWinner())
            {
                Console.WriteLine($"And the winner is MC Hammer... ehm player {actualPlayer.Name}!");
                break;
                isWinner = true;
            }

            if (CheckTie())
            {
                Console.WriteLine($"And the winner is MC Hammer... ehm both players win, its tie...");
                break;
                isWinner = true;
            }

            actualPlayer = (actualPlayer == player1) ? player2 : player1; // switching players
        }
    }

    private void PrintBattleField()
    {
        Console.Clear();
        // First row with horizontal coordinates 1 2 3 4 5 ...
        Console.Write("  ");
        for (int i = 0; i < battleField.GetLength(0); i++)
        {
            Console.Write(i + 1);
        }
        Console.WriteLine();

        for (int i = 0; i < battleField.GetLength(1); i++)
        {
            // Vertical coordinates at start of each row
            Console.Write("{0} ", i + 1);
            for (int j = 0; j < battleField.GetLength(0); j++)
            {
                int value = battleField[i, j];
                // Print battle field with player's corresponding battle stone (battleField and battleStones same index)
                Console.Write(battleStones[value]); 
            }
            Console.WriteLine();
        }
    }

    private void EnterBattleStone(Player actualPlayer)
    {
        bool isAvailable = false; // we check if field is free or not

        while (!isAvailable)
        {
            Console.WriteLine("Enter X[row] coordinate: ");
            while (!int.TryParse(Console.ReadLine(), out actualCoordinateX) || actualCoordinateX < 1 || actualCoordinateX > fieldDimension)
            {
                Console.WriteLine("Invalid coordinate X. Enter X[row] coordinate: ");
            }

            Console.WriteLine("Enter Y[column] coordinate: ");
            while (!int.TryParse(Console.ReadLine(), out actualCoordinateY) || actualCoordinateY < 1 || actualCoordinateY > fieldDimension)
            {
                Console.WriteLine("Invalid coordinate Y. Enter Y[column] coordinate: ");
            }

            if (battleField[actualCoordinateX - 1, actualCoordinateY - 1] == 0) // coordinate 1 is at index 0 thats why -1
            {
                battleField[actualCoordinateX - 1, actualCoordinateY - 1] = (actualPlayer == player1) ? 1 : 2;
                isAvailable = true;
            }
            else
            {
                Console.WriteLine("Invalid coordinates [X,Y]. This place is already taken.");
            }
        }
    }

    /// <summary>
    /// We could easily search whole battle field but I dont want to use dumb algorithm. So we search only row,
    /// column and 2 diagonals corresponding our actual (last entered) coordinates and just for actual player.
    /// For better imagination use paper with squares, for understanding diagonals u will need it :o)
    /// </summary>
    /// <returns></returns>
    private bool CheckWinner()
    {
        bool output = false;
        int counter = 0; // counting stones in row/column/diagonal
        int winNumber = (actualPlayer == player1) ? 1 : 2; // Number we are looking for (1 for player1, 2 for player2).

        // Checking left/right direction from last added coordinates. Only column index is changing.
        for (int i = actualCoordinateX - 1, j = 0; j < fieldDimension; j++) // coordinate 1 is at index 0
        {
            if (battleField[i, j] == winNumber) // If there is value corresponding to actual player value.
            {
                counter++;
                if (counter == winConditionNumber) // We have winner!
                    return true;
            }
            else
                counter = 0; // If there is different value, we reset counter.
        }
        counter = 0;

        // Checking up/down direction from last added coordinates. Only row index is changing.
        for (int j = actualCoordinateY - 1, i = 0; i < fieldDimension; i++) // coordinate 1 is at index 0
        {
            if (battleField[i, j] == winNumber) // If there is value corresponding to actual player value.
            {
                counter++;
                if (counter == winConditionNumber) // We have winner!
                    return true;
            }
            else
                counter = 0; // If there is different value, we reset counter.           
        }
        counter = 0;

        // borderNumber tells us how far we are from upper/left border, we go to closer one => Min.
        // If we have coordinates [5,3] we will start at index [2,0] and go diagonaly down
        // If we have coordinates [4,5] we will start at index [0,1] and go diagonaly down
        int borderNumber = Math.Min(actualCoordinateX, actualCoordinateY);
        int startRow = actualCoordinateX - borderNumber;
        int startCol = actualCoordinateY - borderNumber;

        // Checking upperLeft/downRight diagonal from last added coordinates.
        for (int i = startRow, j = startCol; i < fieldDimension && j < fieldDimension; i++, j++)
        {
            if (battleField[i, j] == winNumber) // If there is value corresponding to actual player value.
            {
                counter++;
                if (counter == winConditionNumber) // We have winner!
                    return true;
            }
            else
                counter = 0; // If there is different value, we reset counter.   
        }
        counter = 0;

        // borderNumber tells us how far we are from upper/right border, we go to closer one => Min.
        // If we have field dimension 5 and coordinates [2,3] we will start at index [0,3] and go diagonaly down
        // If we have field dimension 5 and coordinates [5,3] we will start at index [2,4] and go diagonaly down
        borderNumber = Math.Min(actualCoordinateX - 1, (fieldDimension - 1) - (actualCoordinateY - 1));
        startRow = (actualCoordinateX - 1) - borderNumber;
        startCol = (actualCoordinateY - 1) + borderNumber; // here we dont go to 0 but towards fieldDimension (5 here)

        // Checking upperRight/downLeft diagonal from last added coordinates.
        for (int i = startRow, j = startCol; i < fieldDimension && j > 0; i++, j--)
        {
            if (battleField[i, j] == winNumber) // If there is value corresponding to actual player value.
            {
                counter++;
                if (counter == winConditionNumber) // We have winner!
                    return true;
            }
            else
                counter = 0; // If there is different value, we reset counter.   
        }

        return output;
    }

    /// <summary>
    /// We check here if there is at least one field with value 0.
    /// If there is no value 0 than all fields are occupied, its game over.
    /// </summary>
    /// <returns></returns>
    private bool CheckTie()
    {
        for (int i = 0; i < battleField.GetLength(1); i++)
        {
            for (int j = 0; j < battleField.GetLength(0); j++)
            {
                if (battleField[i, j] == 0)
                    return false;
            }
        }

        return true;
    }
}