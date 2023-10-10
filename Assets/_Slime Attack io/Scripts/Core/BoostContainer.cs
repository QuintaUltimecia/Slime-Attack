using UnityEngine;

public class BoostContainer : MonoBehaviour
{
    [SerializeField]
    private DecorContainer _decorContainer;

    public void Init(GameFeaturesModule gameFeatures)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            BaseBoost boost = transform.GetChild(i).GetComponent<BaseBoost>();
            boost.Init(gameFeatures);

            _decorContainer.AddDecor(boost);
        }
    }
}
