using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public GameObject gameOverWindow;
    private Transform canvas;
    private Board board;

    public int row;
    public int column;
    public Sprite[] blueSprites;
    public Sprite[] redSprites;

    public Image image;
    public InputField inputField;

    private void Awake()
    {
        image = GetComponent<Image>();
        inputField = GetComponentInChildren<InputField>();

        inputField.onValueChanged.AddListener(ValidateInput);
    }

    private void Start()
    {
        board = FindFirstObjectByType<Board>();
        canvas = FindFirstObjectByType<Canvas>().transform;
    }
    public void ValidateInput(string input)
    {
        if ((!int.TryParse(input, out int choosenNumber)) || (choosenNumber < 1) || (choosenNumber > 5) || (board.CheckNumberCounter(choosenNumber) == false))
        {
            inputField.text = "";
            return;
        }

        ShowSprite(choosenNumber, board.currentTurn);

        if (board.CheckSum(row, column, choosenNumber))
        {
            GameObject window = Instantiate(gameOverWindow, canvas);
            window.GetComponent<GameOverWindow>().SetName(board.currentTurn);
        }
        else
        {
            board.UpdateCurrentTurn();
        }

        inputField.image.enabled = false;
        inputField.text = "";
        inputField.interactable = false;
    }

    private void ShowSprite(int number, string turn)
    {
        if (turn == "Blue")
        {
            image.sprite = blueSprites[number - 1];
        }
        else
        {
            image.sprite = redSprites[number - 1];
        }
    }


}
