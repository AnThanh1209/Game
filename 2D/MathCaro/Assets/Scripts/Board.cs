using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Board : MonoBehaviour
{
    public enum Parameter {
        playerNumber = 2,
        inputNumber = 5
    }
    public enum Axis
    {
        row = 0,
        column = 1,
        axis = 2
    }

    public struct CellData
    {
        public int number;
        public string player;
        public Cell cell;

        public CellData(int number, string player, Cell cell)
        {
            this.number = number;
            this.player = player;
            this.cell = cell;
        }
    }
    public struct CellRange
    {
        public int begin;
        public int end;

        public CellRange(int begin, int end)
        {
            this.begin = begin;
            this.end = end;
        }
    }

    public GameObject cellPrefab;
    public Transform board;
    public GridLayoutGroup gridLayout;
    public Pattern redPattern;
    public Pattern bluePattern;
    public string currentTurn = "";
    public int boardSize;
    public Text timerText;

    private int[,] numberCount;
    private int[] preCell;
    private CellData[,] boardData;
    private bool firstTurn;
    private float turnTime = 60f;
    private bool isGameOver = false;
    private Coroutine timerCoroutine;

    public void Awake()
    {
        //timerText = FindFirstObjectByType<Text>();
    }

    public void Start()
    {
        firstTurn = true;
        numberCount = new int[(int)Parameter.playerNumber, (int)Parameter.inputNumber];
        preCell = new int[(int)Axis.axis];
        boardData   = new CellData[boardSize + 1, boardSize + 1];
        gridLayout.constraintCount = boardSize;
        CreateBoard();
        redPattern.UpdateBackgroundColor(currentTurn);
        bluePattern.UpdateBackgroundColor(currentTurn);
        StartTurn();
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
                boardData[i, j] = new CellData(0, "None", cell);
            }
        }
        for (int i = 0; i < (int)Parameter.playerNumber; i++)
        {
            for (int j = 0; j < (int)Parameter.inputNumber; j++)
            {
                numberCount[i, j] = 0;
            }
        }
        for (int i = 0; i < (int)Axis.axis; i++)
        {
            preCell[i] = 0;
        }
    }

    public void UpdateCurrentTurn()
    {
        if (currentTurn == "Red")
        {
            currentTurn = "Blue";
        }
        else
        {
            currentTurn = "Red";
        }
        redPattern.UpdateBackgroundColor(currentTurn);
        bluePattern.UpdateBackgroundColor(currentTurn);
        StartTurn();
    }

    public void StartTurn()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        timerCoroutine = StartCoroutine(TurnTimer());
    }

    private IEnumerator TurnTimer()
    {
        turnTime = 60f;

        while (turnTime > 0)
        {
            timerText.text = Mathf.CeilToInt(turnTime).ToString();
            yield return new WaitForSeconds(1f);
            turnTime--;
        }

        UpdateCurrentTurn();
    }

    public bool CheckNumberCounter(int number)
    {
        if (isGameOver == true) {
            return false;
        }

        if (firstTurn == true && number > 3)
        {
            return false;
        }
        firstTurn = false;

        int player = (currentTurn == "Red") ? 0 : 1;
        if (numberCount[player, number - 1] >= 4)
        {
            return false;
        }

        numberCount[player, number - 1]++;

        if (currentTurn == "Red")
        {
            redPattern.UpdateRemainNumber(number - 1, 4 - numberCount[player, number - 1]);
        }
        else
        {
            bluePattern.UpdateRemainNumber(number - 1, 4 - numberCount[player, number - 1]);
        }
        return true;
    }

    public bool CheckSum(int row, int column, int number)
    {
        boardData[row, column].number = number;
        boardData[row, column].player = currentTurn;
        boardData[row, column].cell.GetComponent<Image>().color = new Color(1f, 1f, 204f / 255f);
        boardData[row, column].cell.GetComponentInChildren<InputField>().image.enabled = false;

        if (preCell[(int)Axis.row] != 0) {
            boardData[preCell[(int)Axis.row], preCell[(int)Axis.column]].cell.GetComponent<Image>().color = new Color(1f, 1f, 1f);
            boardData[preCell[(int)Axis.row], preCell[(int)Axis.column]].cell.GetComponentInChildren<InputField>().image.enabled = false;
        }
        preCell[(int)Axis.row] = row;
        preCell[(int)Axis.column] = column;

        int Sum = 0;
        CellRange rowRange = new CellRange(0, 0);
        CellRange columnRange = new CellRange(0, 0);

        //Check hang doc "|"
        for (int i = 1; i <= boardSize; i++) //xuong duoi
        {
            if (boardData[i, column].player == currentTurn)
            {
                Sum += boardData[i, column].number;
            }
            else if (boardData[i, column].player != "None")
            {
                Sum -= boardData[i, column].number;
            }
            else { continue; }

            if (rowRange.begin == 0)
            {
                rowRange.begin = i;
            }
            rowRange.end = i;
        }
        if (Sum == 16) {
            redPattern.UpdateBackgroundColor("None");
            bluePattern.UpdateBackgroundColor("None");
            for (int i = rowRange.begin; i <= rowRange.end; i++)
            {
                boardData[i, column].cell.GetComponent<Image>().color = new Color(1f, 1f, 204f/255f);
                boardData[i, column].cell.GetComponentInChildren<InputField>().image.enabled = false;
            }
            isGameOver = true;
            return true; 
        }
        
        //Check hang ngang "-"
        Sum = 0;
        columnRange = new CellRange(0, 0);
        for (int i = 1; i <= boardSize; i++) //sang phai
        {
            if (boardData[row, i].player == currentTurn)
            {
                Sum += boardData[row, i].number;
            }
            else if (boardData[row, i].player != "None")
            {
                Sum -= boardData[row, i].number;
            }
            else { continue; }

            if (columnRange.begin == 0)
            {
                columnRange.begin = i;
            }
            columnRange.end = i;
        }
        if (Sum == 16)
        {
            redPattern.UpdateBackgroundColor("None");
            bluePattern.UpdateBackgroundColor("None");
            for (int i = columnRange.begin; i <= columnRange.end; i++)
            {
                boardData[row, i].cell.GetComponent<Image>().color = new Color(1f, 1f, 204f / 255f);
                boardData[row, i].cell.GetComponentInChildren<InputField>().image.enabled = false;
            }
            isGameOver = true;
            return true;
        }

        //Check hang cheo "\"
        Sum = 0;
        rowRange = new CellRange(0, 0);
        columnRange = new CellRange(0, 0);
        for (int i = row; i > 0; i--) //cheo tren trai
        {
            int newColumn = column - (row - i);
            if (newColumn <= 0) break;

            if (boardData[i, newColumn].player == currentTurn)
            {
                Sum += boardData[i, newColumn].number;
            }
            else if (boardData[i, newColumn].player != "None")
            {
                Sum -= boardData[i, newColumn].number;
            }
            else { continue; }

            if (rowRange.end == 0)
            {
                rowRange.end = i;
                columnRange.end = newColumn;
            }
            rowRange.begin = i;
            columnRange.begin = newColumn;
        }
        for (int i = row + 1; i <= boardSize; i++) //cheo duoi phai
        {
            int newColumn = column + (i - row);
            if (newColumn > boardSize) { break; }

            if (boardData[i, newColumn].player == currentTurn)
            {
                Sum += boardData[i, newColumn].number;
            }
            else if (boardData[i, newColumn].player != "None")
            {
                Sum -= boardData[i, newColumn].number;
            }
            else { continue; }

            if (rowRange.begin == 0)
            {
                rowRange.begin = i;
                columnRange.begin = newColumn;
            }
            rowRange.end = i;
            columnRange.end = newColumn;
        }

        if (Sum == 16)
        {
            redPattern.UpdateBackgroundColor("None");
            bluePattern.UpdateBackgroundColor("None");
            for (int i = rowRange.begin; i <= rowRange.end; i++)
            {
                boardData[i, columnRange.begin + (i - rowRange.begin)].cell.GetComponent<Image>().color = new Color(1f, 1f, 204f / 255f);
                boardData[i, columnRange.begin + (i - rowRange.begin)].cell.GetComponentInChildren<InputField>().image.enabled = false;
            }
            isGameOver = true;
            return true;
        }

        //Check hang cheo "/"
        Sum = 0;
        rowRange = new CellRange(0, 0);
        columnRange = new CellRange(0, 0);
        for (int i = row; i > 0; i--) //cheo tren phai
        {
            int newColumn = column + (row - i);
            if (newColumn > boardSize) break;

            if (boardData[i, newColumn].player == currentTurn)
            {
                Sum += boardData[i, newColumn].number;
            }
            else if (boardData[i, newColumn].player != "None")
            {
                Sum -= boardData[i, newColumn].number;
            }
            else { continue; }

            if (rowRange.end == 0)
            {
                rowRange.end = i;
                columnRange.end = newColumn;
            }
            rowRange.begin = i;
            columnRange.begin = newColumn;
        }
        for (int i = row + 1; i <= boardSize; i++) //cheo duoi trai
        {
            int newColumn = column - (i - row);
            if (newColumn <= 0) { break; }

            if (boardData[i, newColumn].player == currentTurn)
            {
                Sum += boardData[i, newColumn].number;
            }
            else if (boardData[i, newColumn].player != "None")
            {
                Sum -= boardData[i, newColumn].number;
            }
            else { continue; }

            if (rowRange.begin == 0)
            {
                rowRange.begin = i;
                columnRange.begin = newColumn;
            }
            rowRange.end = i;
            columnRange.end = newColumn;
        }

        if (Sum == 16)
        {
            redPattern.UpdateBackgroundColor("None");
            bluePattern.UpdateBackgroundColor("None");
            for (int i = rowRange.begin; i <= rowRange.end; i++)
            {
                boardData[i, columnRange.begin - (i - rowRange.begin)].cell.GetComponent<Image>().color = new Color(1f, 1f, 204f / 255f);
                boardData[i, columnRange.begin - (i - rowRange.begin)].cell.GetComponentInChildren<InputField>().image.enabled = false;
            }
            isGameOver = true;
            return true;
        }

        return false;
    }
}
