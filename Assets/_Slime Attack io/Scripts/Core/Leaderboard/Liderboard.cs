using System;
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
    private IOnLeaderBoard _player;

    public int PlayerSize { get; private set; }
    public int PlayerPlace { get; private set; }

    private bool _isInitialized = false;

    public void Init(EnemySpawner enemySpawner, Player player)
    {
        _enemySpawner = enemySpawner;
        _player = player;

        _isInitialized = true;
    }

    public override void OnEnable()
    {
        _lateUpdates.Add(this);
    }

    public override void OnDisable()
    {
        _lateUpdates.Remove(this);
    }

    public override void OnLateTick()
    {
        if (_leaders != null && _isInitialized == true)
            FillLeaders();
    }


    public void CreateLeaderList()
    {
        _onLeaderBoard = new List<IOnLeaderBoard>();
        foreach (var leader in _enemySpawner.Enemies.ToList())
            _onLeaderBoard.Add(leader);

        _onLeaderBoard.Add(_player);
        _leaders = new IOnLeaderBoard[_onLeaderBoard.Count];
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

        int index = Array.IndexOf(_leaders, _player);
        PlayerSize = _leaders[index].GetSize();
        PlayerPlace = index + 1;
    }
}
