using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceDeUsuario : MonoBehaviour
{
    public static InterfaceDeUsuario _Instance;

    [SerializeField] private Slider _stamiinaSlidar;

    [SerializeField] private TMP_Text _municaoText;

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
}
