using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DifficultyStorage : ScriptableObject
{
    [SerializeField] private int _minTime;
    [SerializeField] private int _maxTime;
    
    public int GetTime(int difficulty, int minDifficulty, int maxDifficulty) => 
    (int)Mathf.Lerp(_minTime, _maxTime, Mathf.InverseLerp(maxDifficulty, minDifficulty, difficulty));
}
