using System;
using Unity.VisualScripting;
using UnityEngine;

public class Environment : MonoBehaviour
{
    private const float EndPosX = -18f;

    private Mover _mover;

    public Action<Environment> Outdated;

    public Mover Mover => _mover;

    private void Awake()
    {
        _mover = this.AddComponent<Mover>();
    }

    private void Update()
    {
        if (transform.position.x <= EndPosX)
            Outdated?.Invoke(this);
    }
}
