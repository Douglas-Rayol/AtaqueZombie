using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class InterfaceDeUsuario : MonoBehaviour
{
    public static InterfaceDeUsuario _Instance;

    [SerializeField] private Slider _stamiinaSlidar;

    [SerializeField] private TMP_Text _municaoText;

    [SerializeField] private Slider _barraDeVidaSlider;

    [SerializeField] private TMP_Text _pontosText;

    [SerializeField] private Image _miraImage;

    [SerializeField] private TMP_Text _ondaAtualText;
    [SerializeField] private TMP_Text _tempoRestanteProximaOndaText;

    [SerializeField] private GameObject _gameoverPanel;
    [SerializeField] private TMP_Text _OndaText;
    [SerializeField] private TMP_Text _MonstrosText;

    [SerializeField] private Volume _danoVolume;
    [SerializeField] private AudioSource _danoAudioSource;
    [SerializeField] private AudioSource _respiracaoAudioSouce;

    private Coroutine _danoVolumeCoroutine;

    [SerializeField] private Animator _headshotAnim;
    [SerializeField] private AudioSource _headshotAudioSouce;

    [SerializeField] private TMP_Text _pontosRecebidosText;
    [SerializeField] private Animator _pontosRecebidosAnim;

    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void AtualizarStamina(float stamina)
    {
        _stamiinaSlidar.value = stamina;
        _stamiinaSlidar.gameObject.SetActive(stamina < 0.99f);
    }

    public void AtualizarMunicao(int municaoAtual, int municaoNoInventario)
    {
        _municaoText.text = municaoAtual + "/" + municaoNoInventario;
    }

    public void AtualizarBarraDeVida(int _vidaAtual, int _vidaMaxima)
    {
        _barraDeVidaSlider.maxValue = _vidaMaxima;
        _barraDeVidaSlider.value = _vidaAtual;
    }

    public void AtualizarPontos(int _variacao, int saldoAtual)
    {
        _pontosText.text = "Pontos: " + saldoAtual;

        if(_variacao > 0)
        {
            _pontosRecebidosText.text = "+" + _variacao;
            _pontosRecebidosAnim.SetTrigger("Pontos");
        }
    }

    public void ExibirMira(bool exibirMira)
    {
        _miraImage.enabled = exibirMira;
    }

    public void AtualizarondaAtual(int ondaAtual)
    {
        _ondaAtualText.text = "onda " + ondaAtual;
        _OndaText.text = "Ondas: " + ondaAtual;
    }

    public void AtualizarTempoRestante(float tempo)
    {
        _tempoRestanteProximaOndaText.text = tempo.ToString("00.0");
    }

    public void ExibirGameover()
    {
        _gameoverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        _MonstrosText.text = "Monstros Derrotados: " + Jogador.Instance.GetMonstrosDerrotados();
    }

    private IEnumerator DanoVolumeCoroutine()
    {
        _danoAudioSource.Play();
        _respiracaoAudioSouce.Play();

        while (_danoVolume.weight < 1)
        {
            _danoVolume.weight += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2);

        while (_danoVolume.weight > 0)
        {
            _danoVolume.weight -= Time.deltaTime;
            yield return null;
        }

        _danoAudioSource.Stop();
        _respiracaoAudioSouce.Stop();
    }

    public void AtivarEfeitoDeDano()
    {
        if(_danoVolumeCoroutine != null)
        {
            StopCoroutine(_danoVolumeCoroutine);
        }

        _danoVolumeCoroutine = StartCoroutine(DanoVolumeCoroutine());
    }

    public void ExecutarHeadshot()
    {
        _headshotAnim.SetTrigger("Headshot");
        _headshotAudioSouce.PlayOneShot(_headshotAudioSouce.clip);
    }
}
