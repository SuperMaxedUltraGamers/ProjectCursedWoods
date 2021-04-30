using UnityEngine;

namespace CursedWoods
{
    public class Footsteps : MonoBehaviour
    {
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void PlayWalkingFootstep()
        {
            int sfxRandomiser = Random.Range(0, 3);
            switch (sfxRandomiser)
            {
                case 0:
                    // TODO: play some footstep
                    break;
                case 1:
                    // TODO: play some other footstep
                    break;
                case 2:
                    // TODO: play some even other footstep
                    break;
            }
        }

        private void PlayRunningFootstep()
        {
            /*
            if (!footstepAudioSource.isPlaying)
            {
                Settings.Instance.Audio.PlayEffect(footstepAudioSource, Data.AudioContainer.PlayerSFX.Footstep);
            }
            */
            int sfxRandomiser = Random.Range(0, 3);
            switch (sfxRandomiser)
            {
                case 0:
                    // TODO: play some footstep
                    //Settings.Instance.Audio.PlayEffect(footstepAudioSource, Data.AudioContainer.PlayerSFX.Footstep);
                    break;
                case 1:
                    // TODO: play some other footstep
                    break;
                case 2:
                    // TODO: play some even other footstep
                    break;
            }
        }
    }
}