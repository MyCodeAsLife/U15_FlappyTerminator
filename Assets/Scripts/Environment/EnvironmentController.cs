using System.Collections;
using UnityEngine;

public class EnvironmentController : MonoBehaviour          // Разделить на 2 контроллера, Environment вынести в отдельный класс который будет использовать эти 2 контроллера и туда добавить ProjectileController?
{
    private const int InitialNumberOfPlatforms = 6;
    private readonly Vector2 InitialCoordinatesObstacle = new Vector2(13f, -4.5f);

    private Environment[] _prefabsGround;
    private Environment[] _prefabsWall;
    private Environment[] _prefabsCloud;
    private ObjectPool<Environment> _platformPool;
    private ObjectPool<Environment> _wallPool;
    private ObjectPool<Environment> _cloudPool;
    private Quaternion _initialWallRotation;

    private Coroutine _wallSpawn;
    private Coroutine _cloudSpawn;

    private float[] _startCoordinatesX = { 13f, 7f, 1f, -5f, -11f, -17f };
    private float _speedGround;
    private float _minSpeedEnvironment;
    private float _maxSpeedEnvironment;

    private void Awake()
    {
        _prefabsGround = Resources.LoadAll<Environment>("Prefabs/Grounds");
        _prefabsWall = Resources.LoadAll<Environment>("Prefabs/Walls");
        _prefabsCloud = Resources.LoadAll<Environment>("Prefabs/Clouds");
        _platformPool = new ObjectPool<Environment>(_prefabsGround, CreateEnvironment, EnablePlatform, DisablePlatform);
        _wallPool = new ObjectPool<Environment>(_prefabsWall, CreateEnvironment, EnableWall, DisableWall);
        _cloudPool = new ObjectPool<Environment>(_prefabsCloud, CreateEnvironment, EnableCloud, DisableCloud);            // Переименовать методы, нужен отдельный метод на деспаун и возвращение в пулл

        _initialWallRotation = _prefabsWall[0].transform.rotation;
        _speedGround = 5f;
        _minSpeedEnvironment = 1f;
        _maxSpeedEnvironment = 5f;

        InitialCreationPlatforms();
    }

    private void OnEnable()
    {
        _wallSpawn = StartCoroutine(WallSpawn());
        _cloudSpawn = StartCoroutine(CloudSpawn());
    }

    private void OnDisable()
    {
        if (_wallSpawn != null)
            StopCoroutine(_wallSpawn);

        if (_cloudSpawn != null)
            StopCoroutine(_cloudSpawn);

        //_platformPool.ReturnAll();
        _wallPool.ReturnAll();
        _cloudPool.ReturnAll();
    }

    //public void Restart()
    //{
    //    StopCoroutine(WallSpawn());
    //    StopCoroutine(CloudSpawn());
    //    _platformPool.ReturnAll();
    //    _wallPool.ReturnAll();
    //    _cloudPool.ReturnAll();

    //    InitialCreationPlatforms();
    //    StartCoroutine(WallSpawn());
    //    StartCoroutine(CloudSpawn());
    //}

    private void InitialCreationPlatforms()
    {
        for (int i = 0; i < InitialNumberOfPlatforms; i++)
        {
            var platform = _platformPool.Get();
            platform.Outdated += OnPlatformOutdated;
            platform.Mover.SetStartData(new Vector2(_startCoordinatesX[i], InitialCoordinatesObstacle.y), transform.rotation, _speedGround);
            platform.gameObject.SetActive(true);
        }
    }

    private Environment CreateEnvironment(Environment prefab)       // В родитель
    {
        var item = Instantiate<Environment>(prefab);
        item.transform.SetParent(transform);

        return item;
    }
    private void EnablePlatform(Environment environment)            // Переопределение и в родитель
    {
        environment.gameObject.SetActive(true);
        environment.Outdated += OnPlatformOutdated;
    }

    private void DisablePlatform(Environment environment)           // Переопределение и в родитель
    {
        environment.Outdated -= OnPlatformOutdated;
        environment.gameObject.SetActive(false);
        environment.transform.position = Vector3.zero;
    }

    private void OnPlatformOutdated(Environment environment)    // OnChanged - в родитель
    {
        _platformPool.Return(environment);
        _platformPool.Get().Mover.SetStartData(InitialCoordinatesObstacle, transform.rotation, _speedGround);
    }

    private void EnableWall(Environment environment)            // Переопределение и в родитель
    {
        environment.gameObject.SetActive(true);
        environment.Outdated += OnWallOutdated;
    }

    private void DisableWall(Environment environment)           // Переопределение и в родитель
    {
        environment.Outdated -= OnWallOutdated;
        environment.gameObject.SetActive(false);
        environment.transform.position = Vector3.zero;
    }

    private void OnWallOutdated(Environment environment)    // OnChanged - в родитель
    {
        _wallPool.Return(environment);
    }

    private void EnableCloud(Environment environment)            // Переопределение и в родитель
    {
        environment.gameObject.SetActive(true);
        environment.Outdated += OnCloudOutdated;
    }

    private void DisableCloud(Environment environment)           // Переопределение и в родитель
    {
        environment.Outdated -= OnCloudOutdated;
        environment.gameObject.SetActive(false);
        environment.transform.position = Vector3.zero;
    }

    private void OnCloudOutdated(Environment environment)    // OnChanged - в родитель
    {
        _cloudPool.Return(environment);
    }

    private IEnumerator WallSpawn()
    {
        const float MinDelay = 2f;
        const float MaxDelay = 5f;
        const float MinHeight = -8f;

        float timer = 0;
        float timePassed = 1f;
        bool isWork = true;

        while (isWork)
        {
            if (timer > timePassed)
            {
                timer = 0;
                Vector2 newPosition = new Vector2(InitialCoordinatesObstacle.x, Random.Range(MinHeight, InitialCoordinatesObstacle.y));
                _wallPool.Get().Mover.SetStartData(newPosition, _initialWallRotation, _speedGround);
                timePassed = Random.Range(MinDelay, MaxDelay);
            }

            yield return null;
            timer += Time.deltaTime;
        }
    }

    private IEnumerator CloudSpawn()
    {
        const float MinDelay = 0.5f;
        const float MaxDelay = 1f;
        const float MaxHeight = 5f;

        bool isWork = true;

        while (isWork)
        {
            Vector2 newPosition = new Vector2(InitialCoordinatesObstacle.x, Random.Range(0, MaxHeight));
            var cloud = _cloudPool.Get();
            cloud.Mover.SetStartData(newPosition, Quaternion.identity, Random.Range(_minSpeedEnvironment, _maxSpeedEnvironment));
            cloud.transform.localScale = Vector3.one * Random.Range(1, MaxHeight);

            yield return new WaitForSeconds(Random.Range(MinDelay, MaxDelay));
        }
    }
}
