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
    [Inject(Id = "SystemSetter")] private SystemSetter _systemSetter;
    private ObjectPool _objectPool;

    private ScoreInfo _score;

    [Inject]
    public void Construct(TileBoard board, CanvasGroup gameOverScreen, ObjectPool objectPool)
    {
        _score = new ScoreInfo("Score");
        _systemSetter.AddSavable(_score);
        _board = board;
        _gameOverScreen = gameOverScreen;
        _objectPool = objectPool;
    }
    
    private void Start()
    {
        _score.LoadScore();
        NewGame();
    }

    public void NewGame()
    {
        _objectPool.Reset();
        SetScore(0);
        _highScoreText.text = _score.BestScore.ToString();
        
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
        _score.BestScore = _score.Score;
        UpdateBestScoreText();
    }

    public void IncreaseScore(int points)
    {
        SetScore(_score.Score + points);
        UpdateHighScore();
    }

    private void SetScore(int score)
    {
        _score.Score = score;
        UpdateScoreText();
    }

    public void UpdateScoreText()
    {
        _scoreText.text = _score.Score.ToString();
    }
    
    public void UpdateBestScoreText()
    {
        _highScoreText.text = _score.BestScore.ToString();
    }

    private void UpdateHighScore()
    {
        if (_score.Score > _score.BestScore)
        {
            _score.BestScore = _score.Score;
            UpdateBestScoreText();
        }
    }

    private void OnApplicationQuit()
    {
        _score.Save();
    }
}
