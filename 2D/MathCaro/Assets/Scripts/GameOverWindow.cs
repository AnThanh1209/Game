using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverWindow : MonoBehaviour
{
    public Text winnerName;
    public Button RestartButton;
    public Button QuitButton;

    public void Awake()
    {
        RestartButton.onClick.AddListener(RestartGame);
        QuitButton.onClick.AddListener(QuitGame);
    }

    public void SetName(string player)
    {
        winnerName.text = player + " Wins!";
        if (player == "Red")
        {
            winnerName.color = Color.red;
        }
        else if (player == "Blue")
        {
            winnerName.color = Color.blue;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MathCaro");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
