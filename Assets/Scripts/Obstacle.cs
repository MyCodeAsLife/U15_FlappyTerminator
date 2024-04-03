using System;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private readonly float EndPosX;

    private ObstacleMover _mover;

    public Action<Obstacle> Outdated;

    public Obstacle()
    {
        EndPosX = -17;
    }

    private void Awake()
    {
        float startPosX = 7f;
        float startPosY = -4.5f;
        float speed = 1;

        _mover = this.AddComponent<ObstacleMover>();
        _mover.SetStartData(startPosX, startPosY, speed);
    }

    private void Update()
    {
        if (transform.position.x <= EndPosX)
            Outdated?.Invoke(this);
    }
}
