using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TimeSlowdown : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _slowdownFactor = 0.3f; // Во сколько раз замедляем время
    [SerializeField] private float _slowdownDuration = 2f; // Длительность замедления
    [SerializeField] private float _restoreSpeed = 2f; // Скорость восстановления времени

    private CancellationTokenSource _cts;

    private void OnDestroy()
    {
        _cts?.Cancel();
    }
    
    public async UniTask ApplySlowdown()
    {
        _cts?.Cancel(); 
        _cts = new CancellationTokenSource();
        
        try
        {
            Time.timeScale = _slowdownFactor;
            Time.fixedDeltaTime = 0.02f * Time.timeScale; 

            // Плавное восстановление времени
            float elapsed = 0f;
            while (elapsed < _slowdownDuration)
            {
                elapsed += Time.unscaledDeltaTime; 
                float progress = elapsed / _slowdownDuration;
                
                Time.timeScale = Mathf.Lerp(_slowdownFactor, 1f, progress * _restoreSpeed);
                Time.fixedDeltaTime = 0.02f * Time.timeScale;
                
                await UniTask.Yield(_cts.Token);
            }
            
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
        catch (OperationCanceledException)
        {
           
        }
    }
}
