using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : BaseObject
{
    private const float MaxLifeDistance = 15f;

    private ProjectileController _projectileController;
    private Coroutine _shooting;

    private float _bulletSpeed;
    private float _minReloading;
    private float _maxReloading;

    public Action<CharacterController> Disabled;

    public override Mover Mover
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
    }

    private void OnEnable()
    {
        _shooting = StartCoroutine(Shooting());
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, Vector2.zero) > MaxLifeDistance)
            Disabled?.Invoke(this);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Disabled?.Invoke(this);
    }

    private IEnumerator Shooting()
    {
        bool isShoot = true;

        while (isShoot)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(_minReloading, _maxReloading));

            var bullet = _projectileController.GetAmmo() as Bullet;
            float newPosX = transform.position.x - GetComponent<CircleCollider2D>().radius - bullet.Collider.size.x;
            Vector2 startBulletPosition = new Vector2(newPosX, transform.position.y);
            bullet.Mover.SetStartData(startBulletPosition, Quaternion.identity, _bulletSpeed);
        }
    }
}
