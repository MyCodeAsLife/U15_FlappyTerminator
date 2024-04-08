using System.Collections;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    private Explosion _prefabExplosion;
    private CharacterController[] _prefabsCharacter;
    private ObjectPool<CharacterController> _charactersPool;
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
        _charactersPool = new ObjectPool<CharacterController>(_prefabsCharacter, CreateEnvironment, EnablePlatform, DisablePlatform);
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

    //public void Restart()
    //{
    //    StopCoroutine(CharactersSpawn());
    //    _charactersPool.ReturnAll();
    //    StartCoroutine(CharactersSpawn());
    //}

    private CharacterController CreateEnvironment(CharacterController prefab)       // В родитель
    {
        var item = Instantiate<CharacterController>(prefab);
        item.transform.SetParent(transform);

        return item;
    }
    private void EnablePlatform(CharacterController character)            // Переопределение и в родитель
    {
        character.gameObject.SetActive(true);
        character.Outdated += OnCharacterOutdated;
    }

    private void DisablePlatform(CharacterController character)           // Переопределение и в родитель
    {
        character.Outdated -= OnCharacterOutdated;
        character.gameObject.SetActive(false);
        character.transform.position = Vector3.zero;
    }

    private void OnCharacterOutdated(CharacterController character)         // Возврат врага в пулл + респавн нового?   // OnChanged - в родитель вместе с остальными контроллерами
    {
        StartCoroutine(Explode(character.transform.position));
        _charactersPool.Return(character);
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
        var effect = Instantiate(_prefabExplosion);
        effect.transform.position = position;

        yield return new WaitForSeconds(1.1f);              // Магическое дерьмо
        Destroy(effect.gameObject);
    }
}
