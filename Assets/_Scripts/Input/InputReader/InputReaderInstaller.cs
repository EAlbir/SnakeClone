using UnityEngine;
using Zenject;

public class InputReaderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<InputReader>().AsSingle();
    }
}