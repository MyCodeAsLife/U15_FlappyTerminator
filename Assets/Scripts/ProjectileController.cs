using UnityEngine;

public class ProjectileController : BaseController
{
    private ObjectPool<BaseObject> _ammoPool;
    private Bullet[] _prefabsBullet;

    private void Awake()
    {
        _prefabsBullet = Resources.LoadAll<Bullet>("");
        _ammoPool = new ObjectPool<BaseObject>(_prefabsBullet, base.Create, Enable, Disable);
    }

    private void OnDisable()
    {
        _ammoPool.ReturnAll();
    }

    public BaseObject GetAmmo() => _ammoPool.Get();

    protected override void Enable(BaseObject environment)
    {
        base.Enable(environment);
        (environment as Bullet).Disabled += OnChange;
    }

    protected override void Disable(BaseObject environment)
    {
        (environment as Bullet).Disabled -= OnChange;
        base.Disable(environment);
    }

    private void OnChange(BaseObject environment)
    {
        _ammoPool.Return(environment);
    }
}
