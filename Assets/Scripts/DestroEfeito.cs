using UnityEngine;

public class DestroEfeito : MonoBehaviour
{
    [SerializeField] private float _tempoDeVida;

    void Start()
    {
        Destroy(gameObject, _tempoDeVida);
    }
}
