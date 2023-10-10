using System.Collections.Generic;
using System.Linq;

public class GameState
{
    public static GameState Instance;
    public static GameBaseState CurrentState { get; private set; }
    public static List<GameBaseState> States { get; private set; }

    public void Init()
    {
        States = new List<GameBaseState>
        {
            new MenuState(),
            new PlayState()
        };

        CurrentState = States[0];
        Instance = this;
    }

    public static bool CheckState<T>() where T : GameBaseState
    {
        var state = States.FirstOrDefault(s => s is T);

        if (state == CurrentState)
            return true;
        else return false;
    }

    public static void SwitchState<T>() where T : GameBaseState
    {
        var state = States.FirstOrDefault(s => s is T);

        state.Start();
        CurrentState.Stop();
        CurrentState = state;
    }
}
