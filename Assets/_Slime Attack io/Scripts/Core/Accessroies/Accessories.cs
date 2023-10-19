using UnityEngine;
using System.Collections.Generic;

public class Accessories : MonoBehaviour
{
    [SerializeField]
    private Transform _masksContainer;

    [SerializeField]
    private Transform _headsContainer;

    private List<Accessory> _masks = new List<Accessory>();
    public bool[] MasksPurchased { get; private set; }
    private List<Accessory> _heads = new List<Accessory>();
    public bool[] HeadsPurchased;

    private int _lastMask;
    private int _lastHead;

    private int _currentMask = 0;
    private int _currentHead = 0;

    public int CurrentHead { get { return _currentHead; } }
    public int CurrentMask { get { return _currentMask; } }

    public void Init()
    {
        for (int i = 0; i < _masksContainer.childCount; i++)
            _masks.Add(_masksContainer.GetChild(i).GetComponent<Accessory>());

        for (int i = 0; i < _headsContainer.childCount; i++)
            _heads.Add(_headsContainer.GetChild(i).GetComponent<Accessory>());

        HeadsPurchased = new bool[_heads.Count];
        HeadsPurchased[0] = true;
        MasksPurchased = new bool[_masks.Count];
        MasksPurchased[0] = true;

        ActiveMasks(ref _currentMask);
        ActiveHeads(ref _currentHead);

        Load();
    }

    public void ActiveMasks(ref int index)
    {
        foreach (var item in _masks)
            item.Enabled(false);

        if (index > _masks.Count - 1)
            index = 0;
        else if (index < 0)
            index = _heads.Count - 1;

        _lastMask = index;
        _masks[_lastMask].Enabled(true);
    }

    public void ActiveHeads(ref int index)
    {
        foreach (var item in _heads)
            item.Enabled(false);

        if (index > _heads.Count - 1)
            index = 0;
        else if (index < 0)
            index = _heads.Count - 1;

        _lastHead = index;
        _heads[_lastHead].Enabled(true);
    }

    public void ApplyHead()
    {
        _currentHead = _lastHead;
    }

    public void ApplyMask()
    {
        _currentMask = _lastMask;
    }

    public void Back()
    {
        ActiveMasks(ref _currentMask);
        ActiveHeads(ref _currentHead);
    }

    public AccessorySO GetHeadFeatures()
    {
        return _heads[_lastHead].AccessoryFeatures;
    }

    public AccessorySO GetMaskFeatures()
    {
        return _masks[_lastMask].AccessoryFeatures;
    }

    public bool GetHeadPurchased()
    {
        return HeadsPurchased[_lastHead];
    }

    public void SetHeadPurchased()
    {
        HeadsPurchased[_lastHead] = true;
    }

    public bool GetMaskPurchased()
    {
        return MasksPurchased[_lastMask];
    }

    public void SetMaskPurchased()
    {
        MasksPurchased[_lastMask] = true;
    }

    public bool CurrentHeadIsLast()
    {
        return _currentHead == _lastHead;
    }

    public bool CurrentMasksIsLast()
    {
        return _currentMask == _lastMask;
    }

    private void Load()
    {
        SaveSystem saveSystem = new SaveSystem(false); // SAVES
        PlayerData playerData = saveSystem.Load<PlayerData>();

        if (playerData != null)
        {
            for (int i = 0; i < playerData.Masks.Length; i++)
                MasksPurchased[i] = playerData.Masks[i].IsPurchased;

            for (int i = 0; i < playerData.Heads.Length; i++)
                HeadsPurchased[i] = playerData.Heads[i].IsPurchased;
        }
    }
}