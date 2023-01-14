using UnityEngine;
using UnityEngine.UI;

public class MusicMuteScript : MonoBehaviour
{
    private bool _switcher;

    private AudioSource _aSource;
    private AudioSource _aSfx;

    private void Start()
    {
        _aSource = GameObject.Find("Video Player").GetComponent<AudioSource>();
        _aSfx = GameObject.Find("SFXController").GetComponent<AudioSource>();
        
        if (this.gameObject.GetComponentsInChildren<Image>()[1].enabled)
        {
            _switcher = true;
        }
        else
        {
            _switcher = false;
        }
    }

    public void MuteStatus()
    {

        if (_switcher)
        {
            this.gameObject.GetComponentsInChildren<Image>()[1].enabled = false;
            _aSource.mute = false;
            if (_aSfx.mute == false)
            {
                _aSfx.Play();
            }
            _switcher = !_switcher;
        }
        else
        {
            this.gameObject.GetComponentsInChildren<Image>()[1].enabled = true;
            _aSource.mute = true;
            if (_aSfx.mute == false)
            {
                _aSfx.Play();
            }
            _switcher = !_switcher;
        }

    }

}
