using UnityEngine;
using Zenject;

public class PrefabsMonoInstaller : MonoInstaller
{
    public Tile TilePrefab;

    public override void InstallBindings()
    {
        Container.Bind<Tile>().FromInstance(TilePrefab).AsSingle();
    }
}
