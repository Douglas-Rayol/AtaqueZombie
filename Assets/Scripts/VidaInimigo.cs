using UnityEngine;
using UnityEngine.Events;

public class VidaInimigo : MonoBehaviour
{
    [SerializeField] private int _vida = 100;
    [SerializeField] private int _pontosDerrota;
    [SerializeField] private UnityEvent OnMorrer;

    public bool ReduzirVida(int valor)
    {
        if(_vida <= 0) return false;

        _vida -= valor;
        if(_vida <= 0)
        {
            OnMorrer.Invoke();
            return true;
        }
        return false;
    }

    public int GetPontosDerrota()
    {
        return _pontosDerrota;
    }
}
