using System.Collections;
using UnityEngine;

public class EnvironmentController : MonoBehaviour          // ��������� �� 2 �����������, Environment ������� � ��������� ����� ������� ����� ������������ ��� 2 ����������� � ���� �������� ProjectileController?
{
    private const int InitialNumberOfPlatforms = 6;
    private readonly Vector2 StartCoordinatesGround = new Vector2(13f, -4.5f);

    private Environment[] _prefabsGround;
    private Environment[] _prefabsWall;
    private ObjectPool<Environment> _platformPool;
    private ObjectPool<Environment> _wallPool;

    private float[] _startCoordinatesX = { 13f, 7f, 1f, -5f, -11f, -17f };
    private float _speedGround;

    private void Awake()
    {
        _prefabsGround = Resources.LoadAll<Environment>("Prefabs/Grounds");
        _prefabsWall = Resources.LoadAll<Environment>("Prefabs/Walls");
        _platformPool = new ObjectPool<Environment>(_prefabsGround, CreateEnvironment, EnablePlatform, DisablePlatform);
        _wallPool = new ObjectPool<Environment>(_prefabsWall, CreateEnvironment, EnableWall, DisableWall);
        _speedGround = 5f;
    }

    private void Start()
    {
        InitialCreationPlatforms();
        StartCoroutine(WallSpawn());
    }

    private void OnDisable()
    {
        StopCoroutine(WallSpawn());
    }

    private void InitialCreationPlatforms()
    {
        for (int i = 0; i < InitialNumberOfPlatforms; i++)
        {
            var platform = _platformPool.Get();
            platform.Outdated += OnPlatformOutdated;
            platform.Mover.SetStartData(new Vector2(_startCoordinatesX[i], StartCoordinatesGround.y), _speedGround);
            platform.gameObject.SetActive(true);
        }
    }

    private Environment CreateEnvironment(Environment prefab)       // � ��������
    {
        var item = Instantiate<Environment>(prefab);
        item.transform.SetParent(transform);

        return item;
    }
    private void EnablePlatform(Environment environment)            // ��������������� � � ��������
    {
        environment.gameObject.SetActive(true);
        environment.Outdated += OnPlatformOutdated;
    }

    private void DisablePlatform(Environment environment)           // ��������������� � � ��������
    {
        environment.Outdated -= OnPlatformOutdated;
        environment.gameObject.SetActive(false);
        environment.transform.position = Vector3.zero;
    }

    private void OnPlatformOutdated(Environment environment)    // OnChanged - � ��������
    {
        _platformPool.Return(environment);
        _platformPool.Get().Mover.SetStartData(StartCoordinatesGround, _speedGround);
    }

    private void EnableWall(Environment environment)            // ��������������� � � ��������
    {
        environment.gameObject.SetActive(true);
        environment.Outdated += OnWallOutdated;
    }

    private void DisableWall(Environment environment)           // ��������������� � � ��������
    {
        environment.Outdated -= OnWallOutdated;
        environment.gameObject.SetActive(false);
        environment.transform.position = Vector3.zero;
    }

    private void OnWallOutdated(Environment environment)    // OnChanged - � ��������
    {
        _wallPool.Return(environment);
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
                Vector2 newPosition = new Vector2(StartCoordinatesGround.x, Random.Range(MinHeight, StartCoordinatesGround.y));
                _wallPool.Get().Mover.SetStartData(newPosition, _speedGround);
                timePassed = Random.Range(MinDelay, MaxDelay);
            }

            yield return null;
            timer += Time.deltaTime;
        }
    }
}
