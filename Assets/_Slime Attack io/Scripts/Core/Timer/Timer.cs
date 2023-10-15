using TMPro;
using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(TextMeshProUGUI))]
public class Timer : BaseBehaviour
{
    public int Value { get; private set; } = 60;
    private TextMeshProUGUI _text;

    private Coroutine _timerRoutine;

    public event Action OnEndTimer;

    public void Init(int value)
    {
        Value = value;
        _text = GetComponent<TextMeshProUGUI>();
        _text.text = Value.ToString();
    }

    public void StartTimer()
    {
        _timerRoutine = StartCoroutine(TimerRoutine());
    }

    public void StopTimer()
    {
        if (_timerRoutine != null)
        {
            StopCoroutine(_timerRoutine);
            _timerRoutine = null;
        }
    }

    private IEnumerator TimerRoutine()
    {
        int value = Value;

        while (value != 0)
        {
            value--;
            _text.text = value.ToString();
            yield return new WaitForSeconds(1);
        }

        OnEndTimer?.Invoke();

        _timerRoutine = null;
    }
}
