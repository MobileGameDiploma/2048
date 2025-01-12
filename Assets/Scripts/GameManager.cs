using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    private TileBoard _board;
    private CanvasGroup _gameOverScreen;
    [Inject(Id = "SocreText")] private TextMeshProUGUI _scoreText;
    [Inject(Id = "HighScoreText")] private TextMeshProUGUI _highScoreText;
    private ObjectPool _objectPool;

    private int score;

    [Inject]
    public void Construct(TileBoard board, CanvasGroup gameOverScreen, ObjectPool objectPool)
    {
        _board = board;
        _gameOverScreen = gameOverScreen;
        _objectPool = objectPool;
    }
    
    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        _objectPool.Reset();
        
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
        _gameOverScreen.DOFade(1f, .5f).SetDelay(1f);
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", Int32.Parse(_scoreText.text));
        _highScoreText.text = _scoreText.text;
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
