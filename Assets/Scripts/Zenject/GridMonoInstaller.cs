using UnityEngine;
using Zenject;

public class GridMonoInstaller : MonoInstaller
{
    public TileGrid Grid;
    public TileRow[] Rows;
    public TileCell[] Cells;
    
    public override void InstallBindings()
    {
        Container.Bind<TileGrid>().FromInstance(Grid).AsSingle();
        Container.Bind<TileRow[]>().FromInstance(Rows).AsSingle();
        Container.Bind<TileCell[]>().FromInstance(Cells).AsSingle();
    }
}
