using System;


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
        state = null;
        state = LoadState?.Invoke(Name, typeof(State));
    }

    public event Action<object, string> SaveState;
    public event Func<string, Type, object> LoadState;

    public struct State
    {
        public int Score;
        public int BestScore;
    }
}