using UnityEngine;

public class GameOverPanel : BasePanel
{
    [field: SerializeField]
    public PlayerSize PlayerSize { get; private set; }

    [field: SerializeField]
    public PlayerPlace PlayerPlace { get; private set; }
}
