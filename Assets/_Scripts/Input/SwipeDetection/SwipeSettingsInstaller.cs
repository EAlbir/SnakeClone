using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "SwipeSettingsInstaller", menuName = "Installers/SwipeSettingsInstaller")]
public class SwipeSettingsInstaller : ScriptableObjectInstaller<SwipeSettingsInstaller>
{
    public SwipeSettings swipeSettings;

    public override void InstallBindings()
    {
        Container.BindInstances(swipeSettings);
    }
}