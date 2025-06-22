using UnityEngine;
using UnityEngine.AI;

public class InimigoVoador : MonoBehaviour
{
    private Transform _jogador;
    private NavMeshAgent _navMshAgent;
    private Animator _animator;
    private float _tempoProximoAtaque;

    [SerializeField] private float _distanciaDeAtaque;
    [SerializeField] private float _intervaloEntreAtaque;

    [SerializeField] private GameObject _bolaAcida;
    [SerializeField] private Transform _pontoDeLancamento;

    
    private void Start()
    {
        _jogador = GameObject.FindGameObjectWithTag("Player").transform;
        _navMshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    
    private void Update()
    {
        float distanciaParajogador = Vector3.Distance(_jogador.position, transform.position);

        if(distanciaParajogador <= _distanciaDeAtaque)
        {
            _navMshAgent.velocity = Vector3.zero;

            if(Time.time > _tempoProximoAtaque)
            {
                Ataque();
            }
        }
        else
        {
            _navMshAgent.SetDestination(_jogador.position);
        }
    }

    private void Ataque()
    {
        _tempoProximoAtaque = Time.time + _intervaloEntreAtaque;

        _pontoDeLancamento.LookAt(_jogador);
        Instantiate(_bolaAcida, _pontoDeLancamento.position, _pontoDeLancamento.rotation);
    }

    public void Morrer()
    {
        enabled = false;
        _animator.SetTrigger("Morrer"); ;
        Destroy(gameObject, 0.6f);
    }
}
