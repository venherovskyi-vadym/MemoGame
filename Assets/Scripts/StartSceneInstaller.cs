using UnityEngine;
using Zenject;

public class StartSceneInstaller : MonoInstaller
{
    [SerializeField] StartScreenView _startScreenView;

    public override void InstallBindings()
    {
        base.InstallBindings();
        Container.BindViewPresenter<StartScreenView, StartScreenPresenter>(_startScreenView);
    }
}