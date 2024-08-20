using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

public class HUD : MonoBehaviour
{
    private const float BlipDuration = 1f;
    private const float BlipScale = 1.3f;
    
    [SerializeField] private TMP_Text driftCountText;
    [SerializeField] private TMP_Text gameplayTimerText;

    
    [SerializeField] private Color warningColor;
    private Sequence gameTimerSequence;
    private Sequence driftCountSequence;
    
    private bool gamePlayTimerBlip = false;

    [Inject] private DriftCounter _driftCounter;
    [Inject] private GameTimer _gameTimer;
    [Inject] private EndGamePopup endGamePopup;

    private void Awake()
    {
        Subscribe();
    }

    private void Subscribe()
    {
        _driftCounter.OnUpdateDriftCounter += UpdateDriftCounter;
        _gameTimer.OnUpdateGameTimer += UpdateGamePlayTimer;
    }

    public void UpdateDriftCounter(int value)
    {
        driftCountSequence.Kill(true);
        driftCountSequence = DOTween.Sequence();
        driftCountSequence.Join(driftCountText.transform.DOScale(BlipScale, BlipDuration));
        driftCountSequence.Append(driftCountText.transform.DOScale(1, BlipDuration));
        
        driftCountSequence.Play();
        driftCountText.text = value.ToString();
    }

    public void UpdateGamePlayTimer(float value)
    {
        int minutes = Mathf.FloorToInt(value / 60);
        int remainingSeconds = Mathf.FloorToInt(value % 60);

        if (minutes == 0 && remainingSeconds <= 10)
        {
            BlipTimer();
        }

        if (minutes == 0 && remainingSeconds <= 0)
        {
            gameTimerSequence.Kill();
        }
        gameplayTimerText.text = string.Format("{0:00}:{1:00}", minutes, remainingSeconds);
    }

    private void BlipTimer()
    {
        if(gamePlayTimerBlip)
            return;
        
        gameTimerSequence.Kill();
        gameTimerSequence = DOTween.Sequence();
        gameTimerSequence.Append(gameplayTimerText.transform.DOScale(BlipScale, BlipDuration));
        gameTimerSequence.Join(gameplayTimerText.DOColor(warningColor, BlipDuration));
        gameTimerSequence.SetLoops(-1, LoopType.Yoyo);
        gameTimerSequence.Play();
        
        gamePlayTimerBlip = true;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
    
    private void Unsubscribe()
    {
        _driftCounter.OnUpdateDriftCounter -= UpdateDriftCounter;
        _gameTimer.OnUpdateGameTimer -= UpdateGamePlayTimer;
    }
}


