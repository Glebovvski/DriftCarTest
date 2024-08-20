using UnityEngine;

public class DriftCounter : MonoBehaviour
{
    private const float TimeBetweenCounterUpdate = 0.1f;
    [SerializeField] private CarController car;
    [SerializeField] private DriftVisauliser driftVisauliser;
    
    private int driftCount;
    private float lastDriftCountUpdate;

    public int DriftCount => driftCount;
    
    private void Start()
    {
        driftCount = 0;
    }

    private void Update()
    {
        if (!car.IsDrifting)
            return;

        if (Time.time - lastDriftCountUpdate < TimeBetweenCounterUpdate)
            return;
        
        driftCount++;
        lastDriftCountUpdate = Time.time;
        driftVisauliser.UpdateDriftCounter(driftCount);
    }

}
