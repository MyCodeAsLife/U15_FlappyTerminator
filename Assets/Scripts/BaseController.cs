using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected BaseObject Create(BaseObject prefab)
    {
        var item = Instantiate<BaseObject>(prefab);
        item.transform.SetParent(transform);

        return item;
    }
    protected virtual void Enable(BaseObject environment)
    {
        environment.gameObject.SetActive(true);
    }

    protected virtual void Disable(BaseObject environment)
    {
        environment.gameObject.SetActive(false);
        environment.transform.position = Vector3.zero;
    }
}
