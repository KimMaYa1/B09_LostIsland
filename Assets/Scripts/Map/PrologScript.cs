using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologScript : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private bool _isEnterCollision=false;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip_Thunder;
    [SerializeField] private AudioClip _audioClip_Bomb;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _audioSource.clip = _audioClip_Thunder;
        _audioSource.Play();
        _audioSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        RunorDive();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Rock") 
        {
            _audioSource.Stop();
            _audioSource.clip = _audioClip_Bomb;
            _audioSource.Play();
            _audioSource.loop = false;
            _isEnterCollision = true;
            Invoke("End", 1f);
        }
    }

    private void RunorDive() 
    {
        if (!_isEnterCollision)
        {
            _rigidbody.velocity = new Vector3(0, 0, -20);
        }
        else
        {
           transform.Rotate(new Vector3(1f, 0, 0));
        }
    }

    void End() 
    {
        LoadingSceneController.LoadScene("SampleScene");
    }
}
