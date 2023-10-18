using Facebook.Unity;
using System;
using UnityEngine;

public class EntryPoint
{
    private Player _player;
    private MainCamera _mainCamera;
    private MainCanvas _mainCanvas;
    private World _world;
    private GameMenu _menu;
    private Lightning _lightning;

    private ResourcesLoader _resourcesLoader;
    private GameFeaturesModule _featuresModule;

    private float _timeLevel;

    public EntryPoint(ResourcesLoader resourcesLoader)
    {
        _resourcesLoader = resourcesLoader;

        LoadResources();
        Init();
        Subs();

        FB.Init(onInitComplete: () => { FB.ActivateApp(); Start(); });
    }

    private void LoadResources()
    {
        ResourcesList resourcesList = new ResourcesList();
        _featuresModule = new GameFeaturesModule((GameFeaturesSO)_resourcesLoader.GetSO(resourcesList.GameFeature));

        _player = _resourcesLoader.GetResource<Player>(resourcesList.Player);
        _mainCamera = _resourcesLoader.GetResource<MainCamera>(resourcesList.MainCamera);
        _mainCanvas = _resourcesLoader.GetResource<MainCanvas>(resourcesList.MainCanvas);
        _world = _resourcesLoader.GetResource<World>(resourcesList.World);
        _menu = _resourcesLoader.GetResource<GameMenu>(resourcesList.Menu);
        _lightning = _resourcesLoader.GetResource<Lightning>(resourcesList.Lightning);
    }

    private void Init()
    {
        GameState gameState = new GameState();
        gameState.Init();
        GameState.Instance.Init();

        _mainCanvas.Init();
        _world.Init(_featuresModule);
        _mainCamera.Init(_player.transform);
        _player.Init(_mainCanvas.GetInternalPanel<GamePanel>().JoyStick, _mainCamera.Camera, _mainCanvas, _featuresModule);
        _world.EnemySpawner.Init(_world.DecorContainer, _mainCamera.Camera, _mainCanvas, _featuresModule);
        _mainCanvas.GetInternalPanel<CustomizePanel>().GetInternalPanel<ColorizedPanel>().Init(_player.Colorized);
        _mainCanvas.GetInternalPanel<CustomizePanel>().GetInternalPanel<HeadPanel>().Init(_player.Accessories, _player.Wallet);
        _mainCanvas.GetInternalPanel<CustomizePanel>().GetInternalPanel<MaskPanel>().Init(_player.Accessories, _player.Wallet);
        _mainCanvas.GetInternalPanel<GamePanel>().Liderboard.Init(_world.EnemySpawner, _player);
        _mainCanvas.GetInternalPanel<GamePanel>().Timer.Init(120);
    }

    private void Subs()
    {
        _mainCanvas.GetInternalPanel<MenuPanel>().GetInternalButton<StartButton>()
            .OnClick += () => StartGame();

        _mainCanvas.GetInternalPanel<GameOverPanel>().GetInternalButton<MenuButton>()
            .OnClick += () => BackToMenu();

        _mainCanvas.GetInternalPanel<MenuPanel>().GetInternalButton<CustomizeButton>()
            .OnClick += () => _mainCanvas.ShowPanel<CustomizePanel>();

        _mainCanvas.GetInternalPanel<CustomizePanel>().GetInternalPanel<MaskPanel>()
            .OnBuy += () => Save();

        _mainCanvas.GetInternalPanel<CustomizePanel>().GetInternalPanel<HeadPanel>()
            .OnBuy += () => Save();

        _mainCanvas.GetInternalPanel<CustomizePanel>().GetInternalPanel<MaskPanel>().GetInternalButton<SelectableButton>()
            .OnClick += () => Save();

        _mainCanvas.GetInternalPanel<CustomizePanel>().GetInternalPanel<HeadPanel>().GetInternalButton<SelectableButton>()
            .OnClick += () => Save();

        _world.EnemySpawner
            .OnRemoveEnemies += () => GameOver();

        _player.Decor
            .OnBeforeAbsorbe += () => GameOver();

        _player.Colorized.OnColorChanged += (value) => Save();

        _mainCanvas.GetInternalPanel<GamePanel>().Timer.OnEndTimer += () =>
        {
            GameOver();
            _world.EnemySpawner.StopEnemies();
        };
    }

    private void Start()
    {
        _mainCanvas.ShowPanel<MenuPanel>();

        _mainCamera.CameraMovement.IsMoveToTarget = false;
        _mainCamera.CameraMovement.SetPosition(_menu.CameraPoint.position);
        _mainCamera.CameraMovement.SetRotation(_menu.CameraPoint.rotation);

        _player.Disable();
        _player.Movement.SetPosition(_menu.PlayerPoint.position);

        _world.EnemySpawner.SpawnEnemies();

        _mainCanvas.GetInternalPanel<GamePanel>().Liderboard.CraeateLeaderList();

        _world.DecorContainer.AddDecor(_player.Decor);
        _lightning.SetMenuRotation();

        Application.targetFrameRate = 1000;

        Load();

        _mainCanvas.GetInternalPanel<CustomizePanel>().GetInternalPanel<ColorizedPanel>().StartColor();

        string key = "startgame";
        int valueStart = PlayerPrefs.GetInt(key);
        valueStart++;
        PlayerPrefs.SetInt(key, valueStart);

        string json = '{' + $" \"count\": \"{valueStart}\", \"days since reg\": \"{GetDays()}\"" + '}';
        //AppMetrica.Instance.ReportEvent("game_start", json);

        FB.LogAppEvent("game_start", valueStart);
    }

    private void StartGame()
    {
        GameState.SwitchState<PlayState>();

        _mainCanvas.ShowPanel<GamePanel>();

        _mainCamera.CameraMovement.IsMoveToTarget = true;
        _mainCamera.CameraMovement.SetRotation(UnityEngine.Quaternion.Euler(65, 0, 0));

        _player.Movement.SetPosition(_world.GetPlayerSpawnPoint());
        _player.Enable();
        _mainCamera.CameraMovement.SetPositionToTarget();

        _mainCanvas.GetInternalPanel<GamePanel>().JoyStick.Disable();
        _mainCanvas.GetInternalPanel<GamePanel>().Timer.StartTimer();

        _lightning.SetGameRotation();
        _world.EnemySpawner.EnableEnemies();

        string key = "levelstart";
        int levelStart = PlayerPrefs.GetInt(key);
        levelStart++;
        PlayerPrefs.SetInt(key, levelStart);

        //AppMetrica.Instance.SendEventsBuffer();
        //string json = '{' + $" \"level\": \"{_canvas.GamePanel.LevelManager.Count}\", \"days since reg\": \"{GetDays()}\" " + '}';
        //AppMetrica.Instance.ReportEvent("level_start", json);

        FB.LogAppEvent("level_start", levelStart);

        DateTime time = DateTime.Now;
        _timeLevel = time.Second;
    }

    private void GameOver()
    {
        _mainCamera.CameraMovement.IsMoveToTarget = false;
        _mainCanvas.ShowPanel<GameOverPanel>();
        _mainCanvas.GetInternalPanel<GamePanel>().Timer.StopTimer();
        _mainCanvas.GetInternalPanel<GameOverPanel>().PlayerPlace.SetPlace(_mainCanvas.GetInternalPanel<GamePanel>().Liderboard.PlayerPlace);
        _mainCanvas.GetInternalPanel<GameOverPanel>().PlayerSize.SetSize(_mainCanvas.GetInternalPanel<GamePanel>().Liderboard.PlayerSize);
        Save();

        DateTime time = DateTime.Now;
        _timeLevel = time.Second - _timeLevel;

        //AppMetrica.Instance.SendEventsBuffer();
        //string json = '{' + $" \"level\": \"{_canvas.GamePanel.LevelManager.Count}\", \"time_spent\": \"{_timeLevel}\", \"days since reg\": \"{GetDays()}\" " + '}';
        //AppMetrica.Instance.ReportEvent("level_complete", json);

        string key = "levelstart";
        int levelStart = PlayerPrefs.GetInt(key);

        FB.LogAppEvent("level_complete", levelStart);
    }

    private void BackToMenu()
    {
        GameState.SwitchState<MenuState>();

        _world.EnemySpawner.Restart();
        _world.DecorContainer.Restart();

        _player.Disable();
        _player.Decor.ResetTransform();
        _player.Decor.DisableAnimation();
        _player.Movement.SetPosition(_menu.PlayerPoint.position);

        _mainCanvas.ShowPanel<MenuPanel>();

        _mainCamera.CameraMovement.SetPosition(_menu.CameraPoint.position);
        _mainCamera.CameraMovement.SetRotation(_menu.CameraPoint.rotation);

        _lightning.SetMenuRotation();
    }

    private void Save()
    {
        SaveSystem saveSystem = new SaveSystem(false);
        PlayerData playerData = new PlayerData();

        playerData.Wallet = _player.Wallet.Value;
        playerData.Color = _player.Colorized.GetColor();
        playerData.CurrentMask = _player.Accessories.CurrentMask;
        playerData.CurrentHead = _player.Accessories.CurrentHead;

        playerData.Masks = new MaskData[_player.Accessories.MasksPurchased.Length];
        for (int i = 0; i < playerData.Masks.Length; i++)
        {
            playerData.Masks[i] = new MaskData();
            playerData.Masks[i].ID = i;
            playerData.Masks[i].IsPurchased = _player.Accessories.MasksPurchased[i];
        }

        playerData.Heads = new HeadData[_player.Accessories.HeadsPurchased.Length];
        for (int i = 0; i < playerData.Heads.Length; i++)
        {
            playerData.Heads[i] = new HeadData();
            playerData.Heads[i].ID = i;
            playerData.Heads[i].IsPurchased = _player.Accessories.HeadsPurchased[i];
        }

        saveSystem.Save(playerData);

        Debug.Log("Save Data");
    }

    private void Load()
    {
        SaveSystem saveSystem = new SaveSystem(false);
        var playerData = saveSystem.Load<PlayerData>();

        if (playerData != null)
        {
            _player.Wallet.AddWallet(playerData.Wallet);

            for (int i = 0; i < playerData.Masks.Length; i++)
                _player.Accessories.MasksPurchased[i] = playerData.Masks[i].IsPurchased;

            for (int i = 0; i < playerData.Heads.Length; i++)
                _player.Accessories.HeadsPurchased[i] = playerData.Heads[i].IsPurchased;

            _player.Accessories.ActiveMasks(ref playerData.CurrentMask);
            _player.Accessories.ApplyMask();

            _player.Accessories.ActiveHeads(ref playerData.CurrentHead);
            _player.Accessories.ApplyHead();

            _player.Colorized.RecolorR(playerData.Color.r);
            _player.Colorized.RecolorG(playerData.Color.g);
            _player.Colorized.RecolorB(playerData.Color.b);

            Debug.Log("Load Data");
        }
        else
        {
            _player.Colorized.RandomRecolor();

            Debug.Log("Create Data");
        }
    }

    private int GetDays()
    {
        DateSaver.SaveDate();
        DateTime dateTime = DateSaver.LoadDate();
        DateTime dateTimeNow = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        return (dateTimeNow - dateTime).Days;
    }
}