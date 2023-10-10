using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Liderboard : BaseBehaviour
{
    [SerializeField]
    private List<TextMeshProUGUI> _leadersUI = new List<TextMeshProUGUI>();

    [SerializeField]
    private IOnLeaderBoard[] _leaders;

    private EnemySpawner _enemySpawner;
    private List<IOnLeaderBoard> _onLeaderBoard;
    private Player _player;

    public void Init(EnemySpawner enemySpawner, Player player)
    {
        _enemySpawner = enemySpawner;
        _player = player;
    }

    public void CraeateLeaderList()
    {
        _leaders = new IOnLeaderBoard[_leadersUI.Count];
        _onLeaderBoard = new List<IOnLeaderBoard>();

        foreach (var leader in _enemySpawner.Enemies.ToList())
        {
            _onLeaderBoard.Add(leader);
        }

        _onLeaderBoard.Add(_player);
    }

    public override void OnTick()
    {
        if (_leaders != null)
            FillLeaders();
    }

    private void FillLeaders()
    {
        for (int i = 0; i < _leaders.Length; i++)
        {
            _leaders[i] = null;
        }

        for (int i = 0; i < _leaders.Length; i++)
        {
            for (int k = 0; k < _onLeaderBoard.Count; k++)
            {
                IOnLeaderBoard leader = _onLeaderBoard[k];

                if (_leaders[i] == null)
                {
                    if (_leaders.Contains(leader))
                        continue;
                    else
                        _leaders[i] = leader;
                }
                else
                {
                    if (_leaders.Contains(leader))
                        continue;

                    if (leader.GetSize() >= _leaders[i].GetSize())
                    {
                        _leaders[i] = leader;
                    }
                }
            }
        }

        for (int i = 0; i < _leadersUI.Count; i++)
        {
            if (_leaders[i] != null)
                _leadersUI[i].text = $"{i+1}.{_leaders[i].GetName()} {_leaders[i].GetSize()}";
        }
    }
}
