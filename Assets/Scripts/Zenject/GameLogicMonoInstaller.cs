
using UnityEngine;
using Zenject;

public class GameLogicMonoInstaller : MonoInstaller
{
    public GameManager GameManager;
    public TileState[] States;

    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromInstance(GameManager).AsSingle();
        Container.Bind<TileState[]>().FromInstance(States).AsSingle();
    }
}
