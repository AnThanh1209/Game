using UnityEngine;
using UnityEngine.UI;

public class Pattern : MonoBehaviour
{
    public GameObject NumberPrefab;
    public Transform pattern; 
    public GridLayoutGroup gridLayout;
    public string playerTurn = "";

    private Image[,] numberImages;
    private Image backgroundImage;

    public Sprite[] playerSprites;
    public Sprite[] numberSprites;

    public void Awake()
    {
        backgroundImage = GetComponent<Image>();
    }

    public void Start()
    {
        gridLayout.constraintCount = 2;
        numberImages = new Image[5,2];

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                GameObject imageObj = Instantiate(NumberPrefab, pattern);
                numberImages[i, j] = imageObj.GetComponent<Image>();
                if (j == 0)
                {
                    numberImages[i, j].sprite = playerSprites[i];
                }
                else
                {
                    numberImages[i, j].sprite = numberSprites[4];
                }
            }
        }
    }

    public void UpdateRemainNumber(int number, int counter)
    {
        numberImages[number, 1].sprite = numberSprites[counter];
    }

    public void UpdateBackgroundColor(string currentTurn)
    {
        if (playerTurn == currentTurn)
        {
            backgroundImage.color = new Color(1f, 1f, 204/255f);
        }
        else
        {
            backgroundImage.color = new Color(245/245f, 245/245f, 245/245f);
        }
    }
}