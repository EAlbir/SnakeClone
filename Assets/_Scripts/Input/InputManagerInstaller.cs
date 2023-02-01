using UnityEngine;
using Zenject;

public class InputManagerInstaller : MonoInstaller
{
    [SerializeField] InputManager.SwipeSettings swipeSettings;

    public override void InstallBindings()
    {
        Container.Bind<InputManager>().AsSingle().WithArguments(swipeSettings).NonLazy();
    }
}