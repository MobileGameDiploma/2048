
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GameLogicMonoInstaller : MonoInstaller
{
    [FormerlySerializedAs("GameManager")] public GameMachine gameMachine;
    public TileState[] States;

    public override void InstallBindings()
    {
        Container.Bind<GameMachine>().FromInstance(gameMachine).AsSingle();
        Container.Bind<TileState[]>().FromInstance(States).AsSingle();
    }
}
