using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  //it is a instance which is used to acces the script any where
    public int helixScore = 0;   //Score
    private int helixHighScore = 0;  //HighScore


    //GameScore
    public TextMeshProUGUI currentScoreText;

    //GameOver
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject gameOverUI;
    public GameObject leaderboard;
    public AudioManger AudioManger;
    public GameObject loadingScrene;

    public GameObject ball;
    public GameObject helix;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        helixScore = 0;
        helixHighScore = PlayerPrefs.GetInt("HighScore", helixHighScore);
        scoreText.text = $"{helixScore}";
        leaderboard.SetActive(false);
    }
    public void AddScore(bool isScored = false)  //adding score by passing parameters
    {
        if (isScored)
        {
            helixScore += 5;
            AudioManger.SFX(AudioManger.scoreAdd);
        }
        else
        {
            helixScore += 0;
        }
        currentScoreText.text = $"{helixScore}";
        if (helixScore >= helixHighScore)
        {
            AudioManger.SFX(AudioManger.highScore);
            helixHighScore = helixScore;
            PlayerPrefs.SetInt("HighScore", helixHighScore);
            PlayerPrefs.Save();
        }
    }
    public void GameOver()   //gameover menu
    {
        PlayfabManager.LeaderBoardInstance.PostHighScore(helixHighScore);
        AudioManger.SFX(AudioManger.gameOver);
        gameOverUI.SetActive(true);
        scoreText.text = $"{helixScore}";
        highScoreText.text = $"{helixHighScore}";
    }
    public void ReStart()  //restart button
    {
        ball.SetActive(false);
        helix.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnLeadearButtonClicked()
    {
        leaderboard.SetActive(true);
        PlayfabManager.LeaderBoardInstance.GetHighScores(100); // load once here
    }

    public void RefreshBotton()
    {
        PlayfabManager.LeaderBoardInstance.GetHighScores(100);
    }
    public void ButtonClick()
    {
        AudioManger.SFX(AudioManger.button);
    }
}
