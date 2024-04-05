using UnityEngine;

public class ProjectileController : MonoBehaviour           // Создать общий класс родитель с EnvironmentController
{
    private ObjectPool<Bullet> _ammoPool;
    private Bullet[] _prefabsBullet;

    private void Awake()
    {
        _prefabsBullet = Resources.LoadAll<Bullet>("");
        _ammoPool = new ObjectPool<Bullet>(_prefabsBullet, CreateBullet, EnableBullet, DisableBullet);
    }

    public Bullet GetAmmo() => _ammoPool.Get();

    private Bullet CreateBullet(Bullet prefab)          // В родитель
    {
        var item = Instantiate<Bullet>(prefab);
        item.transform.SetParent(transform);

        return item;
    }

    private void EnableBullet(Bullet bullet)            // Переопределение и в родитель
    {
        bullet.gameObject.SetActive(true);
        bullet.Hited += OnChanged;
    }

    private void DisableBullet(Bullet bullet)            // Переопределение и в родитель
    {
        bullet.Hited -= OnChanged;
        bullet.gameObject.SetActive(false);
        bullet.transform.position = Vector3.zero;
    }

    private void OnChanged(Bullet bullet)                            // OnChanged - переопределение и в родитель
    {
        _ammoPool.Return(bullet);
    }
}
