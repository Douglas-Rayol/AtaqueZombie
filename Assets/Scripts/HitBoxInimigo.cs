using UnityEngine;

public class HitBoxInimigo : MonoBehaviour
{
    private bool _jogadorNaHitbox;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _jogadorNaHitbox = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _jogadorNaHitbox = false;
        }
    }

    public bool GetJoagadorNaHitbox()
    {
        return _jogadorNaHitbox;
    }
}
