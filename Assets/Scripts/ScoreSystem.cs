using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public int CurrentScore { get; private set; }
    
    [Header("Score Settings")]
    [SerializeField] private int _minorHitScore = 10;
    [SerializeField] private int _normalHitScore = 25;
    [SerializeField] private int _criticalHitScore = 50;

    public event Action<int> OnScoreChanged;
    public event Action<DamageType> OnDamageType;

    public enum DamageType { Minor, Normal, Critical }

    public void AddDamageScore(float damageAmount)
    {
        DamageType type = GetDamageType(damageAmount);
        int scoreToAdd = type switch
        {
            DamageType.Minor => _minorHitScore,
            DamageType.Normal => _normalHitScore,
            DamageType.Critical => _criticalHitScore,
            _ => 0
        };

        CurrentScore += scoreToAdd;
        OnScoreChanged?.Invoke(CurrentScore);
        OnDamageType?.Invoke(type);
    }

    public DamageType GetDamageType(float damage)
    {
        if (damage >= 50f) return DamageType.Critical;
        if (damage >= 25f) return DamageType.Normal;
        return DamageType.Minor;
    }
}