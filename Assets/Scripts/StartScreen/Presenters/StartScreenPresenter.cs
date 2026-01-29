using UniRx;
using UnityEngine;
using Zenject;

public class StartScreenPresenter : UiPresenterBase<StartScreenView>
{
    private const string _playerNameKey = "PlayerName";
    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private readonly SignalBus _signalBus;

    public StartScreenPresenter(StartScreenView view, SignalBus signalBus) : base(view)
    {
        _signalBus = signalBus;
    }

    public override void Initialize()
    {
        base.Initialize();
        InitialEnable();
        _signalBus.GetStream<GameLaunchedSignal>().Subscribe(GameLaunched).AddTo(_disposables);
        View.PlayerNameProperty.InputField.text = LoadPlayerName();
        View.PlayerNameProperty.InputField.onSubmit.AsObservable().Subscribe(SubmitPlayerName).AddTo(_disposables);
        View.DifficultyProperty.Slider.onValueChanged.AsObservable().Subscribe(DifficultyChanged).AddTo(_disposables);
        View.StartButton.onClick.AsObservable().Subscribe(LaunchGame).AddTo(_disposables);
    }

    public override void Dispose()
    {
        base.Dispose();
        _disposables.Dispose();
    }

    private void SubmitPlayerName(string playerName)
    {
        Debug.Log($"player:{playerName}");
        SavePlayerName(playerName);
    }

    private void DifficultyChanged(float value)
    {
        View.DifficultyProperty.ValueText.text = ((int) value).ToString();
    }

    private void LaunchGame(Unit _)
    {
        SavePlayerName(View.PlayerNameProperty.InputField.text);
        _signalBus.Fire(new LaunchGameSignal((int)View.DifficultyProperty.Slider.value));
    }

    private void GameLaunched(GameLaunchedSignal signal)
    {
        Hide();
    }

    private string LoadPlayerName()
    {
        return PlayerPrefs.GetString(_playerNameKey);
    }

    private void SavePlayerName(string playerName)
    {
        PlayerPrefs.SetString(_playerNameKey, playerName);
    }
}
