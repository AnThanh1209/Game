using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public GameObject cellPrefab;

    public Transform board;
    public GridLayoutGroup gridLayout;

    public string currentTurn = "X";
    public int boardSize;
    private string[,] matrix;

    public void Awake()
    {
        
    }

    public void Start()
    {
        matrix = new string[boardSize + 1, boardSize + 1];
        gridLayout.constraintCount = boardSize;
        CreateBoard();
    }

    private void CreateBoard()
    {
        for (int i = 1; i <= boardSize; i++)
        {
            for (int j = 1; j <= boardSize; j++)
            {
                GameObject cellTransform = Instantiate(cellPrefab, board);
                Cell cell = cellTransform.GetComponent<Cell>();
                cell.row = i;
                cell.column = j;
                matrix[i, j] = "";
            }
        }
    }
    public bool Check(int row, int column)
    {
        matrix[row, column] = currentTurn;

        //Check hang doc "|"
        int count = 0;
        for (int i = row - 1; i >= 1; i--) //len tren
        {
            if (matrix[i, column] == currentTurn)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        for (int i = row; i <= boardSize; i++) //xuong duoi
        {
            if (matrix[i, column] == currentTurn)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        if (count >= 5)
        {
            return true;
        }

        //Check hang ngang "-"
        count = 0;
        for (int i = column - 1; i >= 1; i--) //sang trai
        {
            if (matrix[row, i] == currentTurn)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        for (int i = column; i <= boardSize; i++) //sang phai
        {
            if (matrix[row, i] == currentTurn)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        if (count >= 5)
        {
            return true;
        }

        //Check hang cheo "\"
        count = 0;
        for (int i = column - 1; i >= 1; i--) //cheo trai tren
        {
            if (matrix[row - (column - i), i] == currentTurn)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        for (int i = column; i <= boardSize; i++) //cheo phai duoi
        {
            if (matrix[row + (i - column), i] == currentTurn)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        if (count >= 5)
        {
            return true;
        }

        //Check hang cheo "/"
        count = 0;
        for (int i = column; i <= boardSize; i++) //cheo phai tren
        {
            if (matrix[row - (i - column), i] == currentTurn)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        for (int i = column - 1; i >= 1; i--) //cheo trai duoi
        {
            if (matrix[row + (column - i), i] == currentTurn)
            {
                count++;
            }
            else
            {
                break;
            }
        }
        if (count >= 5)
        {
            return true;
        }

        return false;
    }
}
