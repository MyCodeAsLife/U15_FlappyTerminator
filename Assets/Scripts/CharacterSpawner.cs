using System.Collections;
using UnityEngine;

public class CharacterSpawner : BaseController
{
    private Explosion _prefabExplosion;
    private CharacterController[] _prefabsCharacter;
    private ObjectPool<BaseObject> _charactersPool;
    private Coroutine _charactersSpawn;

    private float _minSpawnInterval;
    private float _maxSpawnInterval;
    private float _minStartPosY;
    private float _maxStartPosY;
    private float _startPosX;
    private float _characterSpeed;

    private Quaternion _initialCharacterRotation;

    private void Awake()
    {
        _prefabExplosion = Resources.Load<Explosion>("Prefabs/Explosion");
        _prefabsCharacter = Resources.LoadAll<CharacterController>("Prefabs/Characters");
        _charactersPool = new ObjectPool<BaseObject>(_prefabsCharacter, base.Create, Enable, Disable);
        _initialCharacterRotation = _prefabsCharacter[0].transform.rotation;

        _minSpawnInterval = 1.5f;
        _maxSpawnInterval = 5;
        _minStartPosY = -4f;
        _maxStartPosY = 5f;
        _startPosX = 13f;
        _characterSpeed = -2f;
    }

    private void OnEnable()
    {
        _charactersSpawn = StartCoroutine(CharactersSpawn());
    }

    private void OnDisable()
    {
        if (_charactersSpawn != null)
            StopCoroutine(_charactersSpawn);

        _charactersPool.ReturnAll();
    }

    protected override void Enable(BaseObject character)
    {
        base.Enable(character);
        (character as CharacterController).Disabled += OnChange;
    }

    protected override void Disable(BaseObject character)
    {
        (character as CharacterController).Disabled -= OnChange;
        base.Disable(character);
    }

    private void OnChange(BaseObject character)
    {
        StartCoroutine(Explode(character.transform.position));
        _charactersPool.Return(character as CharacterController);
    }

    private IEnumerator CharactersSpawn()
    {
        bool isWork = true;

        while (isWork)
        {
            yield return new WaitForSeconds(Random.Range(_minSpawnInterval, _maxSpawnInterval));

            Vector2 newPosition = new Vector2(_startPosX, Random.Range(_minStartPosY, _maxStartPosY));
            _charactersPool.Get().Mover.SetStartData(newPosition, _initialCharacterRotation, _characterSpeed);
        }
    }

    private IEnumerator Explode(Vector3 position)
    {
        const float Duration = 1.1f;
        var effect = Instantiate(_prefabExplosion);
        effect.transform.position = position;

        yield return new WaitForSeconds(Duration);
        Destroy(effect.gameObject);
    }
}
