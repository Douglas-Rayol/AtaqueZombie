using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class GerenciadorDeArmas : MonoBehaviour
{
    [SerializeField] private List<Arma> _armasDisponiveis;
    [SerializeField] private Arma _armaPrimaria;
    [SerializeField] private Arma _armaSecundaria;
    [SerializeField] private GameObject _efeitoImpactoDeTiro;
    [SerializeField] private GameObject _efeitoDeSangue;
    [SerializeField] private LayerMask _tiroLayerMask;
    [SerializeField] private CinemachinePanTilt _panTilt;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private Animator _armaOffsetAnim;


    private bool _trocaArma;
    private float _tempoRecoil;
    private Transform _cameraPrincipal;
    private float _tempoProximolTiro;
    private MovimentoJogador _movimentoJogador;
    private Coroutine _recarragarCoroutine;
    private bool _recarregando;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraPrincipal = Camera.main.transform;
        _armaPrimaria.gameObject.SetActive(true);
        _armaSecundaria?.gameObject.SetActive(false);

        _movimentoJogador = GetComponent<MovimentoJogador>();

        AtualizarInterfaceArma(_armaPrimaria);
    }

    // Update is called once per frame
    void Update()
    {
        if (_recarregando || _trocaArma) return;

        Arma armaAtual = GetArmaAtual();
        Recarregar(armaAtual);
        Atirar(armaAtual);
        AplicarRecoil(armaAtual);
        Mirar(armaAtual);
        TrocarArma();
    }

    public Arma GetArmaAtual()
    {
        return _armaPrimaria.gameObject.activeSelf ? _armaPrimaria : _armaSecundaria;
    }

    private void Atirar(Arma armaAtual)
    {
        if(Input.GetButton("Fire1") && Time.time >= _tempoProximolTiro && armaAtual._municaoAtual > 0 && !_movimentoJogador.EstaCorrendo())
        {
            _impulseSource.GenerateImpulse();
            _tempoProximolTiro = Time.time + 1f / armaAtual._tirosPorSegundo;

            armaAtual.RealizarDisparo();

            RaycastHit hit;
            if(Physics.Raycast(_cameraPrincipal.position, _cameraPrincipal.forward, out hit, 1000, _tiroLayerMask, QueryTriggerInteraction.Ignore))
            {
                var parteDoCorpoInimigo = hit.transform.GetComponent<ParteDoCorpo>();
                if (parteDoCorpoInimigo)
                {
                    Instantiate(_efeitoDeSangue, hit.point, Quaternion.LookRotation(hit.normal));

                    var vidaInimigo = parteDoCorpoInimigo.transform.root.GetComponent<VidaInimigo>();

                    int dano = armaAtual.GetDano(hit.distance, parteDoCorpoInimigo.nivelDeDano);

                    bool inimigomorto = vidaInimigo.ReduzirVida(dano);

                    if (inimigomorto)
                    {
                        Jogador.Instance.NovoMonstroDerrotados();

                        if(parteDoCorpoInimigo.nivelDeDano == NivelDeDano.ALTO)
                        {
                            Jogador.Instance.AdicionarPontos(vidaInimigo.GetPontosDerrota() * 2);
                        }
                        else
                        {
                            Jogador.Instance.AdicionarPontos(vidaInimigo.GetPontosDerrota());
                        }
                    }
                }
                else
                {
                    Instantiate(_efeitoImpactoDeTiro, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
            _tempoRecoil = 0.2f;
            armaAtual.ProximoRecoil();

            AtualizarInterfaceArma(armaAtual);
        }
    }

    private void Recarregar(Arma armaAtual)
    {
        if((Input.GetKeyDown(KeyCode.R) || armaAtual._municaoAtual <= 0) && armaAtual._municaoNoInventario > 0)
        {
            CancelarRecarga();
            _recarragarCoroutine = StartCoroutine(ExecutarRecarga(armaAtual));
        }
    }

    private void CancelarRecarga()
    {
        if(_recarragarCoroutine != null)
        {
            StopCoroutine(_recarragarCoroutine);
        }
        _recarregando = false;

        InterfaceDeUsuario._Instance.ExibirMira(true);
    }
 
    private IEnumerator ExecutarRecarga(Arma armaAtual)
    {
        _recarregando = true;
        armaAtual._anim.SetTrigger("Recarregar");
        armaAtual._anim.SetBool("Mirar", false);

        if(armaAtual._modelodaArma == ModeloDaArma.SHOTGUN)
        {
            int _balasParaRecarregar = Mathf.Min(armaAtual._capacidadeDoPente, armaAtual._municaoNoInventario) - armaAtual._municaoAtual;
            yield return new WaitForSeconds(armaAtual._tempoDelayRecarregar);

            for(int i = 0; i < _balasParaRecarregar; i++)
            {
                if(i == _balasParaRecarregar - 1)
                {
                    armaAtual._anim.SetTrigger("FimRecarregar");
                }
                armaAtual.RecarregarArma(1);
                yield return new WaitForSeconds(armaAtual._tempoDelayRecarregar);
                AtualizarInterfaceArma(armaAtual);
            }
        }
        else
        {
            yield return new WaitForSeconds(armaAtual._tempoDelayRecarregar);
            int _balasParaRecarregar = Mathf.Min(armaAtual._capacidadeDoPente, armaAtual._municaoNoInventario) - armaAtual._municaoAtual;
            armaAtual.RecarregarArma(_balasParaRecarregar);
        }

        AtualizarInterfaceArma(armaAtual);
        _recarregando = false;
    }

    private void AplicarRecoil(Arma armaAtual)
    {
        if (_tempoRecoil <= 0f) return;
        _tempoRecoil -= Time.deltaTime;
        Vector2 recoilAtual = armaAtual.ObterRecoilAtual();
        _panTilt.PanAxis.Value += recoilAtual.x * Time.deltaTime;
        _panTilt.TiltAxis.Value += recoilAtual.y * Time.deltaTime;
    }

    private void Mirar(Arma armaAtual)
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            armaAtual.AlterarMira();
        }
    }

    private void TrocarArma()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(AlterarArma(_armaPrimaria));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartCoroutine(AlterarArma(_armaSecundaria));
        }
    }

    private IEnumerator AlterarArma(Arma novaArma)
    {
        Arma armaAtual = GetArmaAtual();

        if (armaAtual == novaArma) yield break;

        _trocaArma = true;
        CancelarRecarga();
        _armaOffsetAnim.SetBool("TrocaDeArma", false);
        yield return new WaitForSeconds(0.5f);

        _armaPrimaria.gameObject.SetActive(false);
        _armaSecundaria.gameObject.SetActive(false);

        novaArma.gameObject.SetActive(true);

        _armaOffsetAnim.SetBool("TrocaDeArma", true);
        yield return new WaitForSeconds(0.5f);

        if(_armaPrimaria != novaArma)
        {
            _armaSecundaria = novaArma;
        }
        AtualizarInterfaceArma(armaAtual);
        _trocaArma = false;
    }

    private void AtualizarInterfaceArma(Arma armaAtual)
    {
        if(armaAtual != null)
        {
            InterfaceDeUsuario._Instance.AtualizarMunicao(armaAtual._municaoAtual, armaAtual._municaoNoInventario);
        }
    }

    public void EquiparNovaArma(ModeloDaArma modeloDaArma)
    {
        foreach(var arma in _armasDisponiveis)
        {
            if(arma._modelodaArma == modeloDaArma)
            {
                StartCoroutine(AlterarArma(arma));
                break;
            }
        }
    }

    public void EquiparMunicao(ModeloDaArma modeloDaArma)
    {
        foreach(var arma in _armasDisponiveis)
        {
            arma.CarregarInventario();
            AtualizarInterfaceArma(GetArmaAtual());
        }
    }
}