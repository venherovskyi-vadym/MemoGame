using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class MemoGamePresenter : UiPresenterBase<MemoGameView>
{
    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private readonly SignalBus _signalBus;
    private readonly CardsStorage _cardsStorage;
    private readonly Dictionary<int,int> _countByIds = new Dictionary<int, int>();
    private readonly List<int> _collectedIds = new List<int>();
    private readonly List<int> _usedIds = new List<int>();
    private int _difficulty;

    public MemoGamePresenter(MemoGameView view, SignalBus signalBus, CardsStorage cardsStorage) : base(view)
    {
        _cardsStorage = cardsStorage;
        _signalBus = signalBus;
    }

    public override void Initialize()
    {
        base.Initialize();
        InitialDisable();
        _signalBus.GetStream<LaunchGameSignal>().Subscribe(LaunchGame).AddTo(_disposables);
        View.ReStartButton.onClick.AsObservable().Subscribe(RelaunchGame).AddTo(_disposables);
    }

    public override void Dispose()
    {
        base.Dispose();
        _disposables.Dispose();
    }

    private void RelaunchGame(Unit _)
    {
        LaunchGame(_difficulty);
    }

    private void LaunchGame(LaunchGameSignal signal)
    {
        if(signal.Difficulty < 2 || signal.Difficulty > 8)
            return;

        LaunchGame(signal.Difficulty);

        Show();
        _signalBus.Fire(new GameLaunchedSignal());
    }

    private void LaunchGame(int difficulty)
    {
        _difficulty = difficulty;
        var groupSize = difficulty;
        var groupsCount = View.Cards.Count / groupSize;
        var usableCards = groupsCount * groupSize;
        _usedIds.Clear();
        _countByIds.Clear();

        for (int i = 0; i < _cardsStorage.Count; i++)
            _usedIds.Add(i);

        for (int i = 0; i < groupsCount; i++)
        {
            var id = _usedIds[Random.Range(0, _usedIds.Count)];
            _usedIds.Remove(id);
            _countByIds.Add(id, groupSize);
        }

        _usedIds.Clear();
        _usedIds.AddRange(_countByIds.Keys);

        for (int i = 0; i < View.Cards.Count; i++)
        {
            var usablecard = i < usableCards;
            View.Cards[i].gameObject.SetActive(usablecard);

            if(!usablecard)
                break;
            var idIndex = Random.Range(0, _usedIds.Count);
            var id = _usedIds[idIndex];
            View.Cards[i].Image.sprite = _cardsStorage.GetSprite(id);
            View.Cards[i].Id = id;
            _countByIds[id] -= 1;

            if(_countByIds[id] == 0)
            {
                _usedIds.Remove(id);
                _countByIds.Remove(id);
            }
        }
    }
}
