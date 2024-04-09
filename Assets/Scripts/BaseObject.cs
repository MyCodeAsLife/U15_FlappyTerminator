using UnityEngine;

public abstract class BaseObject : MonoBehaviour
{
    protected Mover _mover;

    public virtual Mover Mover => _mover;
}
