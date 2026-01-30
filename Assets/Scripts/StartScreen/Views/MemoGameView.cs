using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class MemoGameView : UIBehaviour
{
    public Button ReStartButton;
    public Button ResetTurnedCardsButton;
    public List<CardView> Cards;
    public Transform CardsParent;
    public TextMeshProUGUI CountDownText;
    public TextMeshProUGUI MovesText;
    public GameObject WinEffect;
}
