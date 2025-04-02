using UnityEngine;
using Zenject;

public class SaveLoadSystemInstaller : MonoInstaller
{
    public SystemSetter SystemSetter;
    public override void InstallBindings()
    {
        Container.Bind<SystemSetter>().WithId("SystemSetter").FromInstance(SystemSetter).AsSingle(); 
    }
    
}
