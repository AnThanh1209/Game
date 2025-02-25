using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public GameObject gameOverWindow;
    private Transform canvas;
    public int row;
    public int column;

    private Board board;

    public Sprite xSprite;
    public Sprite oSprite;

    private Image image;
    private Button button;

    private void Awake()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        board = FindFirstObjectByType<Board>();
        canvas = FindFirstObjectByType<Canvas>().transform;
    }

    public void ChangeImage(string s)
    {
        if (s == "X")
        {
            image.sprite = xSprite;
        }
        else
        {
            image.sprite = oSprite;
        }
    }

    public void OnClick()
    {
        ChangeImage(board.currentTurn);
        if (board.Check(this.row, this.column)) 
        {
            GameObject window = Instantiate(gameOverWindow, canvas);
            window.GetComponent<GameOverWindow>().SetName(board.currentTurn);
        }

        if (board.currentTurn == "X")
        {
            board.currentTurn = "O";
        }
        else
        {
            board.currentTurn = "X";
        }
    }
}
