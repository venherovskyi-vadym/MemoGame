using UniRx;
using UnityEngine;

public class StartScreenPresenter : UiPresenterBase<StartScreenView>
{
private CompositeDisposable disposables = new CompositeDisposable();

    public StartScreenPresenter(StartScreenView view) : base(view)
    {
    }

    public override void Initialize()
    {
        base.Initialize();
        View.PlayerNameProperty.InputField.onSubmit.AsObservable().Subscribe(SubmitPlayerName).AddTo(disposables);
        View.DifficultyProperty.Slider.onValueChanged.AsObservable().Subscribe(DifficultyChanged).AddTo(disposables);
    }

    private void SubmitPlayerName(string playerName)
    {
        Debug.Log($"player:{playerName}");
    }

    private void DifficultyChanged(float value)
    {
        Debug.Log($"diff:{value}");
    }
}
