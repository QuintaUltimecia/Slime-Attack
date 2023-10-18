using System.Collections.Generic;
using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
    public static List<BaseBehaviour> _updates = new List<BaseBehaviour>(10001);
    public static List<BaseBehaviour> _lateUpdates = new List<BaseBehaviour>(10001);
    public static List<BaseBehaviour> _fixedUpdates = new List<BaseBehaviour>(10001);

    public void Tick() => OnTick();
    public virtual void OnTick() { }

    public void LateTick() => OnLateTick();
    public virtual void OnLateTick() { }

    public void FixedTick() => OnFixedTick();
    public virtual void OnFixedTick() { }

    public virtual void OnEnable() => _updates.Add(this);
    public virtual void OnDisable() => _updates.Remove(this);
    public virtual void OnDestroy() => _updates.Remove(this);
}