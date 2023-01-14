using UnityEngine;
using UnityEngine.UI;

public class SFXMuteScript : MonoBehaviour
{
    private bool _switcher;

    private AudioSource _aSfx;

    private void Start()
    {
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
            _aSfx.mute = false;
            this.gameObject.GetComponentsInChildren<Image>()[1].enabled = false;
            _aSfx.Play();
            _switcher = !_switcher;
        }
        else
        {
            this.gameObject.GetComponentsInChildren<Image>()[1].enabled = true;
            _aSfx.mute = true;
            _switcher = !_switcher;
        }

    }
}
