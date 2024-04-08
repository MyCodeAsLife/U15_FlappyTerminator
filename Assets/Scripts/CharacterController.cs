using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private const float MaxLifeDistance = 15f;

    private Mover _mover;
    private ProjectileController _projectileController;
    private Coroutine _shooting;

    private float _bulletSpeed;
    private float _minReloading;
    private float _maxReloading;

    public Action<CharacterController> Outdated;

    public Mover Mover
    {
        get
        {
            CreateComponentMover();
            return _mover;
        }
    }

    private void Awake()
    {
        CreateComponentMover();
        _projectileController = GetComponentInParent<ProjectileController>();
        _bulletSpeed = 4f;
        _minReloading = 2f;
        _maxReloading = 4f;

        _mover.SetStartData(new Vector2(12f, 0f), transform.rotation, -2f);              // ћагические числа, начально задание данных перенести в спавнер
    }

    private void OnEnable()
    {
        _shooting = StartCoroutine(Shooting());
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, Vector2.zero) > MaxLifeDistance)
            Outdated?.Invoke(this);            // ƒобавить еще событие дл€ устаревани€ врага или изменить текущее?
    }

    private void OnDisable()
    {
        if (_shooting != null)
            StopCoroutine(_shooting);
    }

    private void CreateComponentMover()
    {
        if (_mover == null)
            _mover = this.AddComponent<Mover>();
    }

    private void OnTriggerEnter2D(Collider2D collision)     // ѕри столновении с чем либо   
    {
        Outdated?.Invoke(this);
    }

    private IEnumerator Shooting()
    {
        bool isShoot = true;

        while (isShoot)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(_minReloading, _maxReloading));

            var bullet = _projectileController.GetAmmo();
            float newPosX = transform.position.x - GetComponent<CircleCollider2D>().radius - bullet.Collider.size.x;
            Vector2 startBulletPosition = new Vector2(newPosX, transform.position.y);
            bullet.Mover.SetStartData(startBulletPosition, Quaternion.identity, _bulletSpeed);
        }
    }
}
