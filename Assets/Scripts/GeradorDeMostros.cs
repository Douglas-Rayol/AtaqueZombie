using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class GeradorDeMostros : MonoBehaviour
{
    [Tooltip("Array de prefabs de monstros, ordenados por dificuldade")]
    [SerializeField] private GameObject[] _monstros;
    [SerializeField] private Transform[] _pontosDeSpawn;

    [SerializeField] private int _monstrosIniciaisPorOnda;
    //[SerializeField] private float _intervaloEntreOndas;

    private int _ondaAtual = 1;
    private float _tempoRestanteParaProximaOnda;

    private Transform _jogador;

    private void Start()
    {
        _jogador = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(GerarOndas());

        InterfaceDeUsuario._Instance.AtualizarTempoRestante(_tempoRestanteParaProximaOnda);
        InterfaceDeUsuario._Instance.AtualizarondaAtual(_ondaAtual);
    }

    private void Update()
    {
        _tempoRestanteParaProximaOnda -= Time.deltaTime;
        InterfaceDeUsuario._Instance.AtualizarTempoRestante(_tempoRestanteParaProximaOnda);    }

    private IEnumerator GerarOndas()
    {
        while (true)
        {
            Jogador.Instance.RestaurarVida();

            _tempoRestanteParaProximaOnda = 30 + 5 * _ondaAtual;
            int totalMonstros = Mathf.CeilToInt(_monstrosIniciaisPorOnda * Mathf.Log(_ondaAtual + 1));

            int subOndas = Mathf.CeilToInt(_tempoRestanteParaProximaOnda / 20);
            float intervaloSubOndas = _tempoRestanteParaProximaOnda / subOndas;

            for (int i = 0; i < subOndas; i++)
            {
                GerarMonstros(Mathf.CeilToInt(totalMonstros / subOndas));
                yield return new WaitForSeconds(intervaloSubOndas);
            }
            _ondaAtual++;
            InterfaceDeUsuario._Instance.AtualizarondaAtual(_ondaAtual);
        }
    }

    private void GerarMonstros(int quantidadeDeMonstros)
    {
        for (int i = 0; i < quantidadeDeMonstros; i++)
        {
            int maxindiceMonstro = Mathf.Min(_ondaAtual / 2, _monstros.Length);
            int indiceTipoDeMonstro = Random.Range(0, maxindiceMonstro);

            int indiceSpawn;
            do
            {
               indiceSpawn = Random.Range(0, _pontosDeSpawn.Length);
            }
            while (Vector3.Distance(_pontosDeSpawn[indiceSpawn].position, _jogador.position) < 15f);

            Transform pontoDeSpawn = _pontosDeSpawn[indiceSpawn];
            Instantiate(_monstros[indiceTipoDeMonstro], pontoDeSpawn.position, pontoDeSpawn.rotation);
        }
    }
}
