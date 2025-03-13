using UnityEngine;

public class ExplosionDestroy : MonoBehaviour
{
    public void OnAnimationFinish()
    {
        Destroy(gameObject);
    }
}
