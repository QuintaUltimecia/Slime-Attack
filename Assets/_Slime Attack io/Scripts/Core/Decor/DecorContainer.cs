using UnityEngine;
using System.Collections.Generic;

public sealed class DecorContainer : MonoBehaviour
{
    private List<Decor> _decors = new List<Decor>();
    private List<Building> _building = new List<Building>();

    public void Restart()
    {
        foreach (Building decor in _building)
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
        float distance = size * 2f;
        Decor value = null;

        for (int i = 0; i < _decors.Count; i++)
        {
            Decor decor = _decors[i];

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

            for (int i = 0; i < _decors.Count; i++)
            {
                Decor decor = _decors[i];

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
            _building.Add(decor);
        }
    }
}
