using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private TileBoard _board;
    private CanvasGroup _gameOverScreen;
    [Inject(Id = "SocreText")] private TextMeshProUGUI _scoreText;
    [Inject(Id = "HighScoreText")] private TextMeshProUGUI _highScoreText;

    private int score;

    [Inject]
    public void Construct(TileBoard board, CanvasGroup gameOverScreen)
    {
        _board = board;
        _gameOverScreen = gameOverScreen;
    }
    
    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        SetScore(0);
        _highScoreText.text = LoadHighScore().ToString();
        
        _gameOverScreen.alpha = 0;
        _gameOverScreen.interactable = false;
        _gameOverScreen.gameObject.SetActive(false);
        
        _board.ClearBoard();
        _board.CreateTile();
        _board.CreateTile();
        _board.enabled = true;
    }

    public void GameOver()
    {
        _board.enabled = false;
        _gameOverScreen.interactable = true;
        _gameOverScreen.gameObject.SetActive(true);
        StartCoroutine(Fade(_gameOverScreen, 1f, 1f));
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float to, float delay)
    {
        yield return new WaitForSeconds(delay);
        float elapsedTime = 0;
        float duration = 0.5f;
        float from = canvasGroup.alpha;
        
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        canvasGroup.alpha = to;
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        _highScoreText.text = "0";
    }

    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }

    private void SetScore(int score)
    {
        this.score = score;
        _scoreText.text = score.ToString();
        SaveHighScore();
    }

    private void SaveHighScore()
    {
        if (score > LoadHighScore())
        {
            PlayerPrefs.SetInt("HighScore", score);
            _highScoreText.text = score.ToString();
        }
    }

    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }
    
}
