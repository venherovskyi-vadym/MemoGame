using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardsStorage : ScriptableObject
{
    [SerializeField] private List<Sprite> _cards;
    
    public int Count => _cards.Count;
    public Sprite GetSprite(int index) => _cards[index];
}
