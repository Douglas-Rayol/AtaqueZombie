using UnityEngine;
using UnityEngine.UI;

public class ArmaCard : MonoBehaviour
{
    [SerializeField] private Button _comprarArma;
    [SerializeField] private Button _comprarMunicao;

    [SerializeField] private int _valorDaArma;
    [SerializeField] private int _valorDaMunicao;

    [SerializeField] private ModeloDaArma _modelodaArma;

    [SerializeField] private GerenciadorDeArmas _gerenciadorDeArmas;
    [SerializeField] private GerenciadorDeLoja _gerenciadorDeLoja;

    private void OnEnable()
    {
        _comprarArma.interactable = Jogador.Instance.GetPontos() >= _valorDaArma;
        _comprarMunicao.interactable = Jogador.Instance.GetPontos() >= _valorDaMunicao;
    }

    public void ComprarArma()
    {
        if(Jogador.Instance.GetPontos() >= _valorDaArma)
        {
            Jogador.Instance.ReduzirPontos(_valorDaArma);
            _gerenciadorDeArmas.EquiparNovaArma(_modelodaArma);
            _gerenciadorDeLoja.Fecharloja();
        }
    }

    public void ComprarMunicao()
    {
        if (Jogador.Instance.GetPontos() >= _valorDaMunicao)
        {
            Jogador.Instance.ReduzirPontos(_valorDaMunicao);
            _gerenciadorDeArmas.EquiparMunicao(_modelodaArma);
            _gerenciadorDeLoja.Fecharloja();
        }
    }
}
