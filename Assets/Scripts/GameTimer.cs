using System;
using UnityEngine;

public interface IComponent
{
    void Init(MonoBehaviour behaviour);
}

public class GameTimer : MonoBehaviour,IComponent
{
    [SerializeField] private int gameplayTime = 2;

    private float timeLeft;

    public event Action OnGameplayEnd; 
    public event Action<float> OnUpdateGameTimer; 

    private void Awake()
    {
        timeLeft = 2 * 60;
    }

    private void Update()
    {
        if (timeLeft <= 0)
            return;
        timeLeft -= Time.deltaTime;
        OnUpdateGameTimer?.Invoke(timeLeft);
        if (timeLeft <= 0)
            OnGameplayEnd?.Invoke();
    }

    public void Init(MonoBehaviour behaviour)
    {
        
    }
}
