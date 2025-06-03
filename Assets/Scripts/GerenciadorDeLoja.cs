using UnityEngine;

public class GerenciadorDeLoja : MonoBehaviour
{
    private bool _estaNaAreaDeCompra;
    private bool _lojaEstaAberta;

    [SerializeField] private GameObject _tooltipAbrirLoja;
    [SerializeField] private GameObject _lojaUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Loja"))
        {
            _estaNaAreaDeCompra = true;
            _tooltipAbrirLoja.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Loja"))
        {
            _estaNaAreaDeCompra = false;
            _tooltipAbrirLoja.SetActive(false);
            Fecharloja();
        }
    }

    private void Update()
    {
        if (_estaNaAreaDeCompra && Input.GetKeyDown(KeyCode.F))
        {
            _lojaEstaAberta = !_lojaEstaAberta;

            if(_lojaEstaAberta)
            {
                AbrirLoja();
            }
            else
            {
                Fecharloja();
            }
        }
    }

    private void AbrirLoja()
    {
        Cursor.lockState = CursorLockMode.None;
        _lojaUI.SetActive(true);
        _tooltipAbrirLoja.SetActive(false);

        Jogador.Instance.PausarJogador();
    }

    public void Fecharloja()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _lojaUI.SetActive(false);

        Jogador.Instance.RetormarJoagdor();
    }
}
