using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{

    public static Transform[,] gameBoard = new Transform[10, 20];

    public static void PrintArray()
    {
        string arrayOutput = "";
        int iMax = gameBoard.GetLength(0) - 1;
        int jMax = gameBoard.GetLength(1) - 1;

        for (int j = jMax; j >= 0; j--)
        {
            for (int i = 0; i <= iMax; i++)
            {
                if (gameBoard[i, j] == null)
                {
                    arrayOutput += " ";
                }
                else
                {
                    // arrayOutput += "X";
                    arrayOutput += " ";
                }
            }

            arrayOutput += "\n";
        }

        var myArrayComp = GameObject.Find("MyArray").GetComponent<Text>();
        myArrayComp.text = arrayOutput;

    }

    public static int DeleteAllFullRows()
    {
        int multiplier = 0;
        for (int row = 0; row < 20; row++)
        {
            if (RowFull(row))
            {
                DeleteGBRow(row);
                multiplier++;
                Debug.Log("Row Cleared: " + row + "; Multiplier at :" + multiplier);
                row--;
            }

        }
        if (multiplier > 0)
        {
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.rowDelete);
        }
        // Debug.Log("------------------------------");
        return multiplier;
    }

    public static bool RowFull(int row)
    {
        for (int col = 0; col < 10; col++)
        {
            if (gameBoard[col, row] == null)
            {
                // Debug.Log("FALSE; Coord: " + col + " " + row);
                return false;
            }
        }
        return true;
    }

    public static void DeleteGBRow(int row)
    {
        for (int col = 0; col < 10; ++col)
        {
            Destroy(gameBoard[col, row].gameObject);
            gameBoard[col, row] = null;
        }

        row++;

        for (int j = row; j < 20; ++j)
        {
            for (int col = 0; col < 10; ++col)
            {
                if (gameBoard[col, j] != null)
                {
                    gameBoard[col, j - 1] = gameBoard[col, j];
                    gameBoard[col, j] = null;
                    gameBoard[col, j - 1].position += new Vector3(0, -1, 0);
                }

            }
        }
    }
}
