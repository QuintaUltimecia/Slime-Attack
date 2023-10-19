using Facebook.Unity;
using System.Threading.Tasks;
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

        gameState
            .Init();

        GameState.Instance
            .Init();

        _mainCanvas
            .Init();

        _world
            .Init(_featuresModule);

        _mainCamera
            .Init(_player.transform);

        _player
            .Init(_mainCanvas.GetPanel<GamePanel>().JoyStick, _mainCamera.Camera, _mainCanvas, _featuresModule);

        _world.EnemySpawner
            .Init(_world.DecorContainer, _mainCamera.Camera, _mainCanvas, _featuresModule);

        _mainCanvas.GetPanel<ColorizedPanel>()
            .Init(_player.Colorized);

        _mainCanvas.GetPanel<HeadPanel>()
            .Init(_player.Accessories, _player.Wallet);

        _mainCanvas.GetPanel<MaskPanel>()
            .Init(_player.Accessories, _player.Wallet);

        _mainCanvas.GetPanel<GamePanel>().Liderboard
            .Init(_world.EnemySpawner, _player);

        _mainCanvas.GetPanel<MenuPanel>().BestSize
            .Init();

        _mainCanvas.GetPanel<GamePanel>().Timer
            .Init(_featuresModule.Timer);
    }

    private void Subs()
    {
        _mainCanvas.GetButton<StartButton>()
            .OnClick += () => StartGame();

        _mainCanvas.GetButton<MenuButton>()
            .OnClick += () => BackToMenu();

        _mainCanvas.GetButton<CustomizeButton>()
            .OnClick += () => _mainCanvas.ShowPanel<CustomizePanel>();

        _mainCanvas.GetPanel<MaskPanel>()
            .OnBuy += () => Save();

        _mainCanvas.GetPanel<HeadPanel>()
            .OnBuy += () => Save();

        _mainCanvas.GetButton<SelectableButton>()
            .OnClick += () => Save();

        _mainCanvas.GetButton<SelectableButton>()
            .OnClick += () => Save();

        _world.EnemySpawner
            .OnRemoveEnemies += () => GameOver();

        _player.Decor
            .OnBeforeAbsorbe += () => GameOver();

        _player.Colorized
            .OnColorChanged += (value) => Save();

        _mainCanvas.GetPanel<GamePanel>().Timer.OnEndTimer += () =>
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

        _mainCanvas.GetPanel<GamePanel>().Liderboard.CreateLeaderList();

        _world.DecorContainer.AddDecor(_player.Decor);
        _lightning.SetMenuRotation();

        Load();

        _mainCanvas.GetPanel<ColorizedPanel>().StartColor();

        Application.targetFrameRate = 1000;
        Analytics.Start();
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

        _mainCanvas.GetPanel<GamePanel>().JoyStick.Disable();
        _mainCanvas.GetPanel<GamePanel>().Timer.StartTimer();

        _lightning.SetGameRotation();
        _world.EnemySpawner.EnableEnemies();

        Analytics.StartGame();
    }

    private void GameOver()
    {
        _mainCamera.CameraMovement.IsMoveToTarget = false;
        _mainCanvas.ShowPanel<GameOverPanel>();
        _mainCanvas.GetPanel<GamePanel>().Timer.StopTimer();
        _mainCanvas.GetPanel<GameOverPanel>().PlayerPlace.SetPlace(_mainCanvas.GetPanel<GamePanel>().Liderboard.PlayerPlace);
        _mainCanvas.GetPanel<GameOverPanel>().PlayerSize.SetSize(_mainCanvas.GetPanel<GamePanel>().Liderboard.PlayerSize);
        Save();

        Analytics.GameOver();
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

    private async void Save()
    {
        await Task.Delay(1000);

        SaveSystem saveSystem = new SaveSystem(false);
        PlayerData playerData = new PlayerData();

        playerData.BestSize = _mainCanvas.GetPanel<MenuPanel>().BestSize.Value;

        if (_player.SlimeSize.Value > playerData.BestSize)
            playerData.BestSize = Mathf.RoundToInt(_player.Deformator.Value * 100);

        _mainCanvas.GetPanel<MenuPanel>().BestSize.SetBastSize(playerData.BestSize);

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

            _mainCanvas.GetPanel<MenuPanel>().BestSize.SetBastSize(playerData.BestSize);

            Debug.Log("Load Data");
        }
        else
        {
            _player.Colorized.RandomRecolor();
            _mainCanvas.GetPanel<MenuPanel>().BestSize.SetBastSize(0);

            Debug.Log("Create Data");
        }
    }
}