using System;
using Unity.VisualScripting;
using UnityEngine;

public class Environment : BaseObject
{
    private const float MaxLifeDistance = 18f;

    public Action<Environment> Outdated;

    public override Mover Mover => _mover;

    private void Awake()
    {
        _mover = this.AddComponent<Mover>();
    }

    private void LateUpdate()
    {
        if (Vector2.Distance(Vector2.zero, transform.position) > MaxLifeDistance)
            Outdated?.Invoke(this);
    }
}
