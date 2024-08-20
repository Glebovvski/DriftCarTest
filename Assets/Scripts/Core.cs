using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    [SerializeField] private HUD hud;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private CarController car;
    [SerializeField] private DriftCounter driftCounter;

    private void Awake()
    {
        car.SetIsControllable(true);
        driftCounter.Init(car);
        gameTimer.Init(null);

        Subscribe();
    }

    private void UpdateGameTimerUI(float value)
    {
        hud.UpdateGamePlayTimer(value);
    }

    private void UpdateDriftUI(int value)
    {
        hud.UpdateDriftCounter(value);
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        driftCounter.OnUpdateDriftCounter += UpdateDriftUI;
        gameTimer.OnUpdateGameTimer += UpdateGameTimerUI;
        gameTimer.OnGameplayEnd += EndGamePlay;
    }

    private void EndGamePlay()
    {
        car.SetIsControllable(false);
    }

    private void Unsubscribe()
    {
        driftCounter.OnUpdateDriftCounter -= UpdateDriftUI;
        gameTimer.OnUpdateGameTimer -= UpdateGameTimerUI;
    }
}
