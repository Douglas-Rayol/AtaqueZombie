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
}
