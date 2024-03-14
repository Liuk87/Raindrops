using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    [SerializeField] private float _bpm;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Intervals[] _intervals;

    private Coroutine _intervalCoroutine;

    private void Start()
    {
        _intervalCoroutine = StartCoroutine(IntervalCheckCoroutine());
    }

    private IEnumerator IntervalCheckCoroutine()
    {
        while (true)
        {
            if (UIManager.Instance.GameState == UIManager.eGameState.InGame)
            {
                SampledTimeIntervals();
            }
            yield return null; // Wait for the next frame
        }
    }

    private void SampledTimeIntervals()
    {
        foreach (Intervals interval in _intervals)
        {
            float sampledTime = (_audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetIntervalLength(_bpm)));
            interval.CheckForInterval(sampledTime);
        }
    }

    private void OnDisable()
    {
        if (_intervalCoroutine != null)
        {
            StopCoroutine(_intervalCoroutine);
            _intervalCoroutine = null;
        }
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float _steps;
    [SerializeField] private UnityEvent _trigger;
    private int _lastInterval;

    public float GetIntervalLength(float bpm)
    {
        return 60f / (bpm * _steps);
    }

    public void CheckForInterval(float interval)
    {
        if (Mathf.FloorToInt(interval) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(interval);
            _trigger.Invoke();
        }
    }
}
