using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuConfig : MonoBehaviour
{
    [SerializeField] private GameObject _menuPause;
    [SerializeField] private CinemachineInputAxisController _axisController;

    [SerializeField] private Slider _sensibilidadeSlider;
    [SerializeField] private Slider _audioSlider;
    [SerializeField] private TMP_Dropdown _qualidadeDropdown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CarregarConfiguracoes();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && _menuPause)
        {
            _menuPause.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Jogador.Instance.PausarJogador();
        }
    }

    public void RetomarPartida()
    {
        _menuPause.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Jogador.Instance.RetormarJoagdor();
    }

    public void SalvarSensibilidade()
    {
        float sensibilidade = _sensibilidadeSlider.value;

        PlayerPrefs.SetFloat("Sensibilidade", sensibilidade);
        PlayerPrefs.Save();

        if (_axisController)
        {
            _axisController.Controllers[0].Input.LegacyGain = 120 * sensibilidade;
            _axisController.Controllers[1].Input.LegacyGain = -120 * sensibilidade;
        }
    }

    public void SalvarAudio()
    {
        float audio = _audioSlider.value;
        PlayerPrefs.SetFloat("Audio", audio);
        PlayerPrefs.Save();

        AudioListener.volume = audio;
    }

    public void SalvarQualidade()
    {
        int qualidadeIndex = _qualidadeDropdown.value;

        PlayerPrefs.SetInt("Qualidade", qualidadeIndex);
        PlayerPrefs.Save();

        QualitySettings.SetQualityLevel(qualidadeIndex);
    }

    public void SalvarConfiguracoes()
    {
        SalvarSensibilidade();
        SalvarAudio();
        SalvarQualidade();
    }


    public void CarregarConfiguracoes()
    {
        float sensibilidade = PlayerPrefs.GetFloat("Sensibilidade", 1.0f);
        float audio = PlayerPrefs.GetFloat("Audio", 1.0f);
        int qualidade = PlayerPrefs.GetInt ("Qualidade", 3);

        _audioSlider.value = audio;
        AudioListener.volume = audio;

        if (_axisController)
        {
            _axisController.Controllers[0].Input.LegacyGain = 120 * sensibilidade;
            _axisController.Controllers[1].Input.LegacyGain = -120 * sensibilidade;
        }

        _sensibilidadeSlider.value = sensibilidade;

        _qualidadeDropdown.value = qualidade;
        QualitySettings.SetQualityLevel(qualidade);
    }

    public void CarregarNovaCena(int indxCena)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(indxCena);
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }
}
