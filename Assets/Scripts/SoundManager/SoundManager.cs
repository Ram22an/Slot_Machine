using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource Background,RollSound;
    private void Start()
    {
        Background.Play();
        Background.volume = 0.3f;
        PlayButton.HandlePulled +=PlayRollSound;
    }
    public void PlayRollSound()
    {
        RollSound.Play();
    }

}
