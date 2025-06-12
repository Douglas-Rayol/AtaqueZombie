using UnityEngine;

public class Arma : MonoBehaviour
{
    public int _tirosPorSegundo;
    public int _capacidadeDoPente;
    public int _municaoNoInventario;
    public int _quantidadeMaximaDeMunicaoNoInveentario;
    public int _municaoAtual;

    public ParticleSystem _efeitoDisparo;

    [SerializeField] private Vector2[] _padraoDeRecoil;
    private int _indiceRecoil;

    public int _danoBaixo;
    public int _danoMedio;
    public int _danoAlto;
    public int _distanciaParaDanoMaximo;

    [Range(0, 1)]
    public float _multiplicadorDanoReduzindo;

    public ModeloDaArma _modelodaArma;
    public Animator _anim;

    public float _tempoDelayRecarregar;

    private void Awake()
    {
        _municaoAtual = _capacidadeDoPente;
        _anim = GetComponent<Animator>();
    }

    public Vector2 ObterRecoilAtual()
    {
        return _padraoDeRecoil[_indiceRecoil];
    }

    public void ProximoRecoil()
    {
        _indiceRecoil++;
        if(_indiceRecoil >= _padraoDeRecoil.Length)
        {
            _indiceRecoil = 0;
        }
    }

    public void RealizarDisparo()
    {
        _municaoAtual--;
        _anim.SetTrigger("Atirar");
        _efeitoDisparo.Play();
    }

    public void RecarregarArma(int quantidade)
    {
        _municaoAtual += quantidade;
        _municaoNoInventario -= quantidade;
    }

    public void AlterarMira()
    {
        bool miraAtiva = _anim.GetBool("Mirar");
        _anim.SetBool("Mirar", !miraAtiva);

        InterfaceDeUsuario._Instance.ExibirMira(miraAtiva);
    }

    public void CarregarInventario()
    {
        _municaoNoInventario = _quantidadeMaximaDeMunicaoNoInveentario;
    }

    public int GetDano(float distancia, NivelDeDano nivelDeDano)
    {
        int dano = 0;
        switch (nivelDeDano)
        {
            case NivelDeDano.BAIXO:
                dano = _danoBaixo;
                break;
            case NivelDeDano.MEDIO:
                dano = _danoMedio;
                break;
            case NivelDeDano.ALTO:
                dano = _danoAlto;
                break;
        }

        if(distancia > _distanciaParaDanoMaximo)
        {
            dano = (int)(dano * _multiplicadorDanoReduzindo);
        }

        return dano;
    }
}

public enum ModeloDaArma
{
    PISTOLA,
    SHOTGUN,
    M4A1,
    SMG,
    AK47,
    LMG,
}
