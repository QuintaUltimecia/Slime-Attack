using System.Threading;
using System.Threading.Tasks;

public class MoveSpeed
{
    public float CurrentValue { get; private set; }
    private readonly float _defaultValue;

    public System.Action OnAfterUpSpeed;
    public System.Action OnBeforeUpSpeed;

    private CancellationTokenSource _cts;

    public MoveSpeed(float value)
    {
        _defaultValue = value;
        CurrentValue = _defaultValue;
    }

    public void StopMultiplier()
    {
        if (_cts != null)
        {
            _cts.Cancel();
        }
    }

    public async void StartMultiplier()
    {
        StopMultiplier();

        await Task.Delay(1);

        using (_cts = new CancellationTokenSource())
        {
            CurrentValue = _defaultValue * 2;
            OnBeforeUpSpeed?.Invoke();

            await Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(5000, _cts.Token);
                }
                catch
                {

                }
            });

            CurrentValue = _defaultValue;
            OnAfterUpSpeed?.Invoke();
            _cts = null;
        }
    }
}
