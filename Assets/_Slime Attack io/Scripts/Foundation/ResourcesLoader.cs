using UnityEngine;

public class ResourcesLoader : MonoBehaviour
{
    public T GetResource<T>(string name) where T : MonoBehaviour
    {
        return Instantiate(Resources.Load<T>(name));
    }

    public ScriptableObject GetSO(string name)
    {
        return Resources.Load<ScriptableObject>(name);
    }
}
