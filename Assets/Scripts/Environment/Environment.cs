using System;
using Unity.VisualScripting;
using UnityEngine;

public class Environment : MonoBehaviour
{
    private const float MaxLifeDistance = 18f;

    private Mover _mover;

    public Action<Environment> Outdated;

    public Mover Mover => _mover;

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
