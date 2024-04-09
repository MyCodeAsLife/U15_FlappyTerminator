using System.Collections;
using UnityEngine;

public class EnvironmentController : BaseController
{
    private const int InitialNumberOfPlatforms = 6;
    private readonly Vector2 InitialCoordinatesObstacle = new Vector2(15f, -4.5f);

    private Environment[] _prefabsGround;
    private Environment[] _prefabsWall;
    private Environment[] _prefabsCloud;
    private ObjectPool<BaseObject> _platformPool;
    private ObjectPool<BaseObject> _wallPool;
    private ObjectPool<BaseObject> _cloudPool;
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
        _platformPool = new ObjectPool<BaseObject>(_prefabsGround, base.Create, EnablePlatform, DisablePlatform);
        _wallPool = new ObjectPool<BaseObject>(_prefabsWall, base.Create, EnableWall, DisableWall);
        _cloudPool = new ObjectPool<BaseObject>(_prefabsCloud, base.Create, EnableCloud, DisableCloud);

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

        _wallPool.ReturnAll();
        _cloudPool.ReturnAll();
    }

    private void InitialCreationPlatforms()
    {
        for (int i = 0; i < InitialNumberOfPlatforms; i++)
        {
            var platform = _platformPool.Get() as Environment;
            platform.Outdated += OnPlatformOutdated;
            platform.Mover.SetStartData(new Vector2(_startCoordinatesX[i], InitialCoordinatesObstacle.y), transform.rotation, _speedGround);
            platform.gameObject.SetActive(true);
        }
    }

    private void EnablePlatform(BaseObject environment)
    {
        base.Enable(environment);
        (environment as Environment).Outdated += OnPlatformOutdated;
    }

    private void DisablePlatform(BaseObject environment)
    {
        (environment as Environment).Outdated -= OnPlatformOutdated;
        base.Disable(environment);
    }

    private void OnPlatformOutdated(BaseObject environment)
    {
        _platformPool.Return(environment);
        (_platformPool.Get() as Environment).Mover.SetStartData(InitialCoordinatesObstacle, transform.rotation, _speedGround);
    }

    private void EnableWall(BaseObject environment)
    {
        base.Enable(environment);
        (environment as Environment).Outdated += OnWallOutdated;
    }

    private void DisableWall(BaseObject environment)
    {
        (environment as Environment).Outdated -= OnWallOutdated;
        base.Disable(environment);
    }

    private void OnWallOutdated(BaseObject environment)
    {
        _wallPool.Return(environment);
    }

    private void EnableCloud(BaseObject environment)
    {
        base.Enable(environment);
        (environment as Environment).Outdated += OnCloudOutdated;
    }

    private void DisableCloud(BaseObject environment)
    {
        (environment as Environment).Outdated -= OnCloudOutdated;
        base.Disable(environment);
    }

    private void OnCloudOutdated(BaseObject environment)
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
            var cloud = _cloudPool.Get() as Environment;
            cloud.Mover.SetStartData(newPosition, Quaternion.identity, Random.Range(_minSpeedEnvironment, _maxSpeedEnvironment));
            cloud.transform.localScale = Vector3.one * Random.Range(1, MaxHeight);

            yield return new WaitForSeconds(Random.Range(MinDelay, MaxDelay));
        }
    }
}