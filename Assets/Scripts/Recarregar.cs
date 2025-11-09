using UnityEngine;

public class Recarregar : MonoBehaviour
{
    [SerializeField] private AudioSource _recarregarAudioSource;

    [SerializeField] private AudioClip _recarregar1AudioClip;
    [SerializeField] private AudioClip _recarregar2AudioClip;
    [SerializeField] private AudioClip _recarregar3AudioClip;

    public void Recarregar1()
    {
        _recarregarAudioSource.PlayOneShot(_recarregar1AudioClip);
    }

    public void Recarregar2()
    {
        _recarregarAudioSource.PlayOneShot(_recarregar2AudioClip);
    }

    public void Recarregar3()
    {
        _recarregarAudioSource.PlayOneShot(_recarregar3AudioClip);
    }
}
