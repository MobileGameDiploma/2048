using System;
using TMPro;
using UnityEngine;


public class ScoreInfo : ISavable
{
    public int Score { get; set; }
    public int BestScore { get; set; }

    public string Name { get; set; }

    public ScoreInfo(string name)
    {
        Name = name;
    }

    public void Save()
    {
        State state = new State();
        
        state.Score = Score;
        state.BestScore = BestScore;
        
        if(SaveState != null)
            SaveState.Invoke(state, Name);
    }

    public void Load(out object state)
    {
        state = LoadState?.Invoke(Name, typeof(State));
    }

    public void LoadScore()
    {
        object state = new State();
        Load(out state);
        State resultState = (State)state;
        Score = resultState.Score;
        BestScore = resultState.BestScore;
    }

    public event Action<object, string> SaveState;
    public event Func<string, Type, object> LoadState;

    public struct State
    {
        public int Score { get; set; }
        public int BestScore { get; set; }

        public void ToConsole()
        {
            Debug.Log(Score);
            Debug.Log(BestScore);
        }
    }
}