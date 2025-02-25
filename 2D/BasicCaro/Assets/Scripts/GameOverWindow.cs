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

    public void SetName(string s)
    {
        winnerName.text = s + " Wins!";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("BasicCaro");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
