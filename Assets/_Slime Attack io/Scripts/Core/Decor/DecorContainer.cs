using UnityEngine;
using System.Collections.Generic;
using System;

public sealed class DecorContainer : MonoBehaviour
{
    private List<Decor> _decors = new List<Decor>();

    public void Restart()
    {
        foreach (Decor decor in _decors)
            decor.Restart();
    }

    public void AddDecor(Decor decor)
    {
        _decors.Add(decor);
    }

    public Decor GetDecor()
    {
        foreach (var decor in _decors)
        {
            if (decor.IsAbsorbed == false)
                return decor;
        }

        return null;
    }

    public Decor GetDecorAfterPlayer(Transform target, float size, Decor origin)
    {
        foreach (var decor in _decors)
        {
            if (decor.IsAbsorbed == false && size >= decor.Features.PointForDeform && origin != decor)
                if (Vector3.Distance(target.position, decor.Transform.position) > size * 8f)
                    return decor;
        }

        return null;
    }

    public Decor GetDecorWithDistance(Transform target, float size, Decor origin)
    {
        float distance = size * 4f;
        Decor value = null;

        foreach (var decor in _decors)
        {
            if (decor.IsAbsorbed == false && size >= decor.Features.PointForDeform && origin != decor)
            {
                if (Vector3.Distance(target.position, decor.Transform.position) < distance)
                {
                    value = decor;

                    break;
                }
            }
        }

        if (value == null)
        {
            distance = float.MaxValue;

            foreach (var decor in _decors)
            {
                if (decor.IsAbsorbed == false && size >= decor.Features.PointForDeform && origin != decor)
                {
                    if (Vector3.Distance(target.position, decor.Transform.position) < distance)
                    {
                        distance = Vector3.Distance(target.position, decor.Transform.position);
                        value = decor;
                    }
                }
            }

            return value;
        }
        else
        {
            return value;
        }
    }

    public void Init(GameFeaturesModule gameFeatures)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Building decor = transform.GetChild(i).GetComponent<Building>();
            decor.Init(gameFeatures);

            _decors.Add(decor);
        }
    }
}
