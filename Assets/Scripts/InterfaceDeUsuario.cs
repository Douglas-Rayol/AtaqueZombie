using TMPro;
using UnityEngine;
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
}
