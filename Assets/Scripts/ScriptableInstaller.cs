using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ScriptableInstaller", menuName = "Installers/ScriptableInstaller")]
public class ScriptableInstaller : ScriptableObjectInstaller
{
    [SerializeField] private CardsStorage CardsStorage;

    public override void InstallBindings()
    {
        Container.Bind<CardsStorage>().FromScriptableObject(CardsStorage).AsSingle();
        InstallSignalBus();
        InstallSignals();
    }

    private void InstallSignalBus()
    {
        Container.BindInterfacesAndSelfTo<SignalBus>().AsSingle().CopyIntoAllSubContainers();
        Container.BindMemoryPool<SignalSubscription, SignalSubscription.Pool>();
        Container.BindFactory<SignalDeclarationBindInfo, SignalDeclaration, SignalDeclaration.Factory>();
    }

    private void InstallSignals()
    {
        Container.DeclareSignal<LaunchGameSignal>();
        Container.DeclareSignal<GameLaunchedSignal>();
    }
}
