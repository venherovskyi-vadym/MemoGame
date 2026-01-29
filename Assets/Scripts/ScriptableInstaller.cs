using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ScriptableInstaller", menuName = "Installers/ScriptableInstaller")]
public class ScriptableInstaller : ScriptableObjectInstaller
{
    [SerializeField] private CardsStorage _cardsStorage;
    [SerializeField] private DifficultyStorage _difficultyStorage;

    public override void InstallBindings()
    {
        Container.Bind<CardsStorage>().FromScriptableObject(_cardsStorage).AsSingle();
        Container.Bind<DifficultyStorage>().FromScriptableObject(_difficultyStorage).AsSingle();
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
