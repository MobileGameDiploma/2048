
using TMPro;
using UnityEngine;
using Zenject;

public class UIMonoInstaller : MonoInstaller
{
    public TileBoard Board;
    public CanvasGroup GameOverScreen;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI HighScoreText;

    public override void InstallBindings()
    {
        Container.Bind<TileBoard>().FromInstance(Board).AsSingle();
        Container.Bind<CanvasGroup>().FromInstance(GameOverScreen).AsSingle();
        Container.Bind<TextMeshProUGUI>().WithId("SocreText").FromInstance(ScoreText);
        Container.Bind<TextMeshProUGUI>().WithId("HighScoreText").FromInstance(HighScoreText);
    }
}
