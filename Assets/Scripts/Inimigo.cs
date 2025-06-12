using UnityEngine;
using UnityEngine.AI;

public class Inimigo : MonoBehaviour
{
    private Transform _jogador;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;
    private float _tempoProximoAtaque;

    [SerializeField] private float _distanciaAtaque;
    [SerializeField] private float _intervaloEntreAtaque = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _jogador = GameObject.FindGameObjectWithTag("Player").transform;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanciaParaJogador = Vector3.Distance(_jogador.position, transform.position);

        if (distanciaParaJogador < _distanciaAtaque)
        {
            _navMeshAgent.velocity = Vector3.zero;

            if(Time.time > _tempoProximoAtaque)
            {
                Atacar();
            }
        }
        else
        {
            _navMeshAgent.SetDestination(_jogador.position);
        }
    }

    private void Atacar()
    {
        Vector3 direcaoParaJogador = (_jogador.position - transform.position).normalized;
        Quaternion rotacaoParaJogador = Quaternion.LookRotation(direcaoParaJogador);
        transform.rotation = rotacaoParaJogador;

        _animator.SetTrigger("Ataque");
        _tempoProximoAtaque = Time.time + _intervaloEntreAtaque;
    }

    public void Morrer()
    {
        enabled = false;
        _animator.SetTrigger("Morrer");
        Destroy(gameObject, 2f);
    }
}