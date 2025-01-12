using UnityEngine;
using Zenject;

public class PrefabsMonoInstaller : MonoInstaller
{
    public Tile TilePrefab;
    public GameObject ObjectPoolParent;

    public override void InstallBindings()
    {
        Container.Bind<GameObject>().FromInstance(ObjectPoolParent).AsSingle();
        Container.Bind<Tile>().FromInstance(TilePrefab).AsSingle();
        Container.Bind<ObjectPool>().AsSingle().WithArguments(TilePrefab, ObjectPoolParent);
    }
}
