using UnityEngine;

public class ArmaSway : MonoBehaviour
{
    [SerializeField] private float _intensidade = 0.03f;
    [SerializeField] private float _suavidade = 5.0f;

    private Vector3 _posicaoInicial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _posicaoInicial = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * _intensidade;
        float mouseY = Input.GetAxis("Mouse Y") * _intensidade;

        Vector3 posicaoAlvo = new Vector3(-mouseX, -mouseY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, _posicaoInicial + posicaoAlvo, Time.deltaTime * _suavidade);
    }
}
