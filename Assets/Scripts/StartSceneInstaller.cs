using UnityEngine;
using Zenject;

public class StartSceneInstaller : MonoInstaller
{
    [SerializeField] StartScreenView _startScreenView;
    [SerializeField] MemoGameView _memoGameView;

    public override void InstallBindings()
    {
        Container.BindViewPresenter<StartScreenView, StartScreenPresenter>(_startScreenView);
        Container.BindViewPresenter<MemoGameView, MemoGamePresenter>(_memoGameView);
    }
}