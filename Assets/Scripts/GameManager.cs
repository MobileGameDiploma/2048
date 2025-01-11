using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileBoard Board;
    public CanvasGroup GameOverScreen;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI HighScoreText;

    private int score;
    
    private void Start()
    {
        NewGame();
    }

    public void NewGame()
    {
        SetScore(0);
        HighScoreText.text = LoadHighScore().ToString();
        
        GameOverScreen.alpha = 0;
        GameOverScreen.interactable = false;
        GameOverScreen.gameObject.SetActive(false);
        
        Board.ClearBoard();
        Board.CreateTile();
        Board.CreateTile();
        Board.enabled = true;
    }

    public void GameOver()
    {
        Board.enabled = false;
        GameOverScreen.interactable = true;
        GameOverScreen.gameObject.SetActive(true);
        StartCoroutine(Fade(GameOverScreen, 1f, 1f));
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

    public void IncreaseScore(int points)
    {
        SetScore(score + points);
    }

    private void SetScore(int score)
    {
        this.score = score;
        ScoreText.text = score.ToString();
        SaveHighScore();
    }

    private void SaveHighScore()
    {
        if (score > LoadHighScore())
        {
            PlayerPrefs.SetInt("HighScore", score);
            HighScoreText.text = score.ToString();
        }
    }

    private int LoadHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }
}
