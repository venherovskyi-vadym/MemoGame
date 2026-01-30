using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class MemoGamePresenter : UiPresenterBase<MemoGameView>
{
    private const int _minDifficulty = 2;
    private const int _maxDifficulty = 8;
    private readonly CompositeDisposable _disposables = new CompositeDisposable();
    private readonly SignalBus _signalBus;
    private readonly CardsStorage _cardsStorage;
    private readonly DifficultyStorage _difficultyStorage;
    private readonly Dictionary<int,int> _countByIds = new Dictionary<int, int>();
    private readonly List<int> _collectedIds = new List<int>();
    private readonly List<int> _usedIds = new List<int>();
    private readonly List<CardView> _turnedCards = new List<CardView>();
    private int _difficulty;
    private int _groupsCount;
    private int _turns;
    private float _startTime;
    private float _difficultyTime;

    public MemoGamePresenter(
        MemoGameView view, 
        SignalBus signalBus, 
        CardsStorage cardsStorage, 
        DifficultyStorage difficultyStorage) : base(view)
    {
        _cardsStorage = cardsStorage;
        _difficultyStorage = difficultyStorage;
        _signalBus = signalBus;        
    }

    public override void Initialize()
    {
        base.Initialize();
        InitialDisable();
        _signalBus.GetStream<LaunchGameSignal>().Subscribe(LaunchGame).AddTo(_disposables);
        View.ReStartButton.onClick.AsObservable().Subscribe(RelaunchGame).AddTo(_disposables);
        View.ResetTurnedCardsButton.onClick.AsObservable().Subscribe(ResetTurnedCards).AddTo(_disposables);
        
        foreach (var item in View.Cards)
        {
            item.Image.enabled = false;
            var view = item;
            item.Button.onClick.AsObservable().Subscribe((_)=>CardClicked(view)).AddTo(_disposables);
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        _disposables.Dispose();
    }

    public override void Tick()
    {
        base.Tick();

        if(_difficultyTime - (Time.time - _startTime) <= 0 || _collectedIds.Count == _groupsCount)
            View.ResetTurnedCardsButton.gameObject.SetActive(false);
        else
            View.CountDownText.text = ((int)(_difficultyTime -(Time.time - _startTime))).ToString();
    }

    private void RelaunchGame(Unit _)
    {
        _turns = 0;
        View.ResetTurnedCardsButton.gameObject.SetActive(false);
        LaunchGame(_difficulty);
    }

    private void LaunchGame(LaunchGameSignal signal)
    {
        if(signal.Difficulty < _minDifficulty || signal.Difficulty > _maxDifficulty)
            return;

        LaunchGame(signal.Difficulty);

        Show();
        _signalBus.Fire(new GameLaunchedSignal());
    }

    private void CardClicked(CardView view)
    {
        if(CardGroupCollected(view.Id))
            return;

        if(!FitTurnedCards(view.Id))
            View.ResetTurnedCardsButton.gameObject.SetActive(true);

        if(!CardTurned(view))
            _turns++;

        View.MovesText.text = _turns.ToString();
        TurnCard(view);
        CountCards(view.Id);
    }

    private void ResetTurnedCards(Unit _)
    {
        foreach (var item in _turnedCards)
            SetCardState(item, false);
        _turnedCards.Clear();
        View.ResetTurnedCardsButton.gameObject.SetActive(false);
    }

    private bool CardTurned(CardView view) => view.gameObject.activeInHierarchy && _turnedCards.Contains(view);
    private bool CardGroupCollected(int id) => _countByIds.ContainsKey(id) && _countByIds[id] >= _difficulty;
    private bool FitTurnedCards(int id) => _turnedCards.Count == 0 || _turnedCards.Find(f => f.Id != id) == null;

    private void TurnCard(CardView view)
    {
        var turned = CardTurned(view);
        SetCardState(view, !turned);

        if(turned)
            _turnedCards.Remove(view);
        else
            _turnedCards.Add(view);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="view"></param>
    /// <param name="state"> true for Front to activate</param>
    private void SetCardState(CardView view, bool state)
    {
        view.Back.SetActive(!state);
        view.Image.enabled = state;
    }

    private void CountCards(int id)
    {
        var idCount = 0;
        foreach (var item in _turnedCards)
        {
            if(item.Id == id)
                idCount++;
        }

        _countByIds[id] = idCount;

        if(CardGroupCollected(id))
            _turnedCards.RemoveAll(ra=>ra.Id == id);
    }

    private void LaunchGame(int difficulty)
    {
        _difficultyTime = _difficultyStorage.GetTime(difficulty, _minDifficulty, _maxDifficulty);
        _startTime = Time.time;
        _difficulty = difficulty;
        var groupSize = difficulty;
        _groupsCount = View.Cards.Count / groupSize;
        var usableCards = _groupsCount * groupSize;
        _usedIds.Clear();
        _countByIds.Clear();
        _collectedIds.Clear();
        _turnedCards.Clear();

        foreach (var item in View.Cards)
            SetCardState(item, false);

        for (int i = 0; i < _cardsStorage.Count; i++)
            _usedIds.Add(i);

        for (int i = 0; i < _groupsCount; i++)
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
                continue;
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
