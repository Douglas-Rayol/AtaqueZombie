using System.Diagnostics;
using UnityEngine;

public class Jogador : MonoBehaviour
{
    public static Jogador Instance;

    [SerializeField] private int _vidaMaxima = 100;
    [SerializeField] private int _pontos;
    [SerializeField] private GameObject _cameraCinemachine;

    private int _vidaAtual;
    private bool _estaMorto;
    private int _monstrosDerrotados;

    private MovimentoJogador _movimentoJogador;
    private GerenciadorDeArmas _gerenciadorDeArmas;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _vidaAtual = _vidaMaxima;
        AtualizarBarraDeVida();
        InterfaceDeUsuario._Instance.AtualizarPontos(0, _pontos);

        _movimentoJogador = GetComponent<MovimentoJogador>();
        _gerenciadorDeArmas = GetComponent<GerenciadorDeArmas>();
    }

    public void ReduzirVida(int valor)
    {
        if (_estaMorto) return;

        _vidaAtual -= valor;
        AtualizarBarraDeVida();

        if ( _vidaAtual <= 0)
        {
            Morrer();
        }
    }

    private void Morrer()
    {
        _estaMorto = true;
        Time.timeScale = 0;
        InterfaceDeUsuario._Instance.ExibirGameover();
    }

    private void AtualizarBarraDeVida()
    {
        InterfaceDeUsuario._Instance.AtualizarBarraDeVida(_vidaAtual, _vidaMaxima);
    }

    public void AdicionarPontos(int valor)
    {
        _pontos += valor;
        InterfaceDeUsuario._Instance.AtualizarPontos(valor, _pontos);
    }

    public void ReduzirPontos(int valor)
    {
        _pontos = Mathf.Max(0, _pontos - valor);
        InterfaceDeUsuario._Instance.AtualizarPontos(-valor, _pontos);
    }

    public void PausarJogador()
    {
        _movimentoJogador.enabled = false;
        _gerenciadorDeArmas.enabled = false;
        _cameraCinemachine.SetActive(false);
    }

    public void RetormarJoagdor()
    {
        _movimentoJogador.enabled = true;
        _gerenciadorDeArmas.enabled = true;
        _cameraCinemachine.SetActive(true);
    }

    public int GetPontos()
    {
        return _pontos;
    }

    public void RestaurarVida()
    {
        _vidaAtual = _vidaMaxima;
        AtualizarBarraDeVida();
    }

    public void NovoMonstroDerrotados()
    {
        _monstrosDerrotados++;
    }

    public int GetMonstrosDerrotados()
    {
        return _monstrosDerrotados;
    }
}
