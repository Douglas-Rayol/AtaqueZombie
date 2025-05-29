using UnityEngine;

public class MovimentoJogador : MonoBehaviour
{
    [SerializeField] private float _velocidadeMovimento = 2.6f;
    [SerializeField] private Transform _pontoDeVerificacao;
    [SerializeField] private LayerMask _camadaDeColisao;

    private GerenciadorDeArmas _gerenciadorDeArmas;
    private Transform _cameraPrincipal;
    private CharacterController _characterController;
    private bool _estaNoChao;
    private float _velocidadeVertical;
    private bool _estaCorrendo;
    private float _nivelStamina;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraPrincipal = Camera.main.transform;
        _characterController = GetComponent<CharacterController>();
        _gerenciadorDeArmas = GetComponent<GerenciadorDeArmas>();
    }

    // Update is called once per frame
    void Update()
    {
        AplicarGravidade();
        ProcessarMovimento();
        AtualizarStamina();
    }

    private void AplicarGravidade()
    {
        _estaNoChao = Physics.CheckSphere(_pontoDeVerificacao.position, 0.3f, _camadaDeColisao);

        if (Input.GetKeyDown(KeyCode.Space) && _estaNoChao)
        {
            _velocidadeVertical = 4.5f;
        }

        if(!_estaNoChao || _velocidadeVertical > Physics.gravity.y)
        {
            _velocidadeVertical += Physics.gravity.y * Time.deltaTime;
        }

        _characterController.Move(new Vector3(0, _velocidadeVertical, 0) * Time.deltaTime);
    }

    private void ProcessarMovimento()
    {
        float _movimentoH = Input.GetAxis("Horizontal");
        float _movimentoV = Input.GetAxis("Vertical");

        _estaCorrendo = Input.GetKey(KeyCode.LeftShift);

        Vector3 _direcaoMovimento = new Vector3(_movimentoH, 0, _movimentoV);
        _direcaoMovimento = _cameraPrincipal.TransformDirection(_direcaoMovimento).normalized;
        _direcaoMovimento.y = 0;

        float _velocidadeAtual = _estaCorrendo && _nivelStamina > 0f ? _velocidadeMovimento * 2f : _velocidadeMovimento;

        if (_estaCorrendo && _nivelStamina > 0f)
        {
            _nivelStamina -= Time.deltaTime;
            _nivelStamina = Mathf.Max(0f, _nivelStamina);
        }

        _gerenciadorDeArmas.GetArmaAtual()._anim.SetBool("Mover", _direcaoMovimento != Vector3.zero);
        _gerenciadorDeArmas.GetArmaAtual()._anim.SetBool("Correr", _estaCorrendo && _nivelStamina > 0f);

        _characterController.Move(_direcaoMovimento * Time.deltaTime * _velocidadeAtual);
    }

    private void AtualizarStamina()
    {
        if(!_estaCorrendo && _nivelStamina < 2f)
        {
            _nivelStamina += Time.deltaTime;
        }

        InterfaceDeUsuario._Instance.AtualizarStamina(_nivelStamina / 2f);
    }

    public bool EstaCorrendo()
    {
        return _estaCorrendo && _nivelStamina > 0;
    }
}
