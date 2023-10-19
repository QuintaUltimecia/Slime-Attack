public class GameFeaturesModule
{
    private GameFeaturesSO _gameFeaturesSO;

    public GameFeaturesModule(GameFeaturesSO gameFeatures)
    {
        _gameFeaturesSO = gameFeatures;

        BuildingRefreshTime = gameFeatures.BuildingRefreshTime;
        SlimeSpeed = gameFeatures.SlimeSpeed;
        Timer = gameFeatures.Timer;
    }

    public float BuildingRefreshTime { get; private set; }
    public float SlimeSpeed { get; private set; }
    public int Timer { get; private set; }

    public void DivideBuildingRefreshTime(int value)
    {
        BuildingRefreshTime = _gameFeaturesSO.BuildingRefreshTime / value;
    }
}
