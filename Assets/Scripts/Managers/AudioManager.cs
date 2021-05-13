using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using CursedWoods.Data;
using System.Collections;

namespace CursedWoods
{
    public class AudioManager : MonoBehaviour
    {
        private const float MUSIC_DEFAUL_FADE_SPEED = 1f;
        private const float MAX_SFX_PITCH = 1f;
        private const float MIN_SFX_PITCH = 0.95f;

        private float musicSourceOgVol;
        private AudioSource source;
        private AudioMixer mixer;
        private AudioContainer audioData;

        public enum SoundGroup
        {
            None = 0,
            Music,
            Effect
        }

        public readonly Dictionary<SoundGroup, string> soundGroupNames = new Dictionary<SoundGroup, string>()
        {
            {SoundGroup.Music, "MusicVolume"},
            {SoundGroup.Effect, "EffectVolume"}
        };

        public void InitAudioManager(AudioSource source, AudioMixer mixer, AudioContainer audioData)
        {
            this.source = source;
            this.mixer = mixer;
            this.audioData = audioData;
        }

        public float PlayEffect(AudioSource source, AudioContainer.PlayerSFX effect, float volMultiplier = 1f)
        {
            float length = 0f;
            AudioClip clip = audioData.GetSoundClip(effect);
            if (clip != null)
            {
                RandomizePitch(source);
                GetVolume(SoundGroup.Effect, out float mixerVol);
                source.PlayOneShot(clip, mixerVol * volMultiplier);
                length = clip.length;
            }

            return length;
        }

        public float PlayEffect(AudioSource source, AudioContainer.SkeletonSFX effect)
        {
            float length = 0f;
            AudioClip clip = audioData.GetSoundClip(effect);
            if (clip != null)
            {
                RandomizePitch(source);
                source.PlayOneShot(clip);
                length = clip.length;
            }

            return length;
        }

        public float PlayEffect(AudioSource source, AudioContainer.PosTreeSFX effect, float volMultiplier = 1f)
        {
            float length = 0f;
            AudioClip clip = audioData.GetSoundClip(effect);
            if (clip != null)
            {
                RandomizePitch(source);
                GetVolume(SoundGroup.Effect, out float mixerVol);
                source.PlayOneShot(clip, mixerVol * volMultiplier);
                length = clip.length;
            }

            return length;
        }

        public float PlayEffect(AudioSource source, AudioContainer.MushroomSFX effect, float volMultiplier = 1f)
        {
            float length = 0f;
            AudioClip clip = audioData.GetSoundClip(effect);
            if (clip != null)
            {
                RandomizePitch(source);
                GetVolume(SoundGroup.Effect, out float mixerVol);
                source.PlayOneShot(clip, mixerVol * volMultiplier);
                length = clip.length;
            }

            return length;
        }

        public float PlayEffect(AudioSource source, AudioContainer.TreeBossSFX effect, float volMultiplier = 1f)
        {
            float length = 0f;
            AudioClip clip = audioData.GetSoundClip(effect);
            if (clip != null)
            {
                RandomizePitch(source);
                GetVolume(SoundGroup.Effect, out float mixerVol);
                source.PlayOneShot(clip, mixerVol * volMultiplier);
                length = clip.length;
            }

            return length;
        }

        public float PlayEffect(AudioSource source, AudioContainer.FinalBossSFX effect, float volMultiplier = 1f)
        {
            float length = 0f;
            AudioClip clip = audioData.GetSoundClip(effect);
            if (clip != null)
            {
                RandomizePitch(source);
                GetVolume(SoundGroup.Effect, out float mixerVol);
                source.PlayOneShot(clip, mixerVol * volMultiplier);
                length = clip.length;
            }

            return length;
        }

        public float PlayEffect(AudioSource source, AudioContainer.MiscSFX effect, float volMultiplier = 1f)
        {
            float length = 0f;
            AudioClip clip = audioData.GetSoundClip(effect);
            if (clip != null)
            {
                RandomizePitch(source);
                GetVolume(SoundGroup.Effect, out float mixerVol);
                source.PlayOneShot(clip, mixerVol * volMultiplier);
                length = clip.length;
            }

            return length;
        }

        private void RandomizePitch(AudioSource source)
        {
            source.pitch = Random.Range(MIN_SFX_PITCH, MAX_SFX_PITCH);
        }

        public void PlayMusic(AudioContainer.Music music)
        {
            AudioClip musicClip = audioData.GetMusicClip(music);
            if (musicClip != null)
            {
                source.clip = musicClip;
                source.loop = true;
                source.Play();
            }
        }

        public void ChangeMusic(AudioContainer.Music music, float fadeSpeed = MUSIC_DEFAUL_FADE_SPEED)
        {
            StartCoroutine(FadeMusicOut(music, fadeSpeed));
        }

        public void StopMusic()
        {
            source.Stop();
        }

        public void PauseMusic()
        {
            if (source.isPlaying)
            {
                source.Pause();
            }
            else
            {
                ContinueMusic();
            }
        }

        public void ContinueMusic()
        {
            if (source.clip != null)
            {
                source.loop = true;
                source.UnPause();
            }
        }

        /// <summary>
		/// Sets the volume for a sound group
		/// </summary>
		/// <param name="sliderValue">Volume value between 0 and 1</param>
		public bool SetVolume(float sliderValue, SoundGroup group)
        {
            float maxVolume = -10f;
            float minVolume = -80f;
            float slope = (maxVolume - minVolume) / 1;
            float volume = slope * sliderValue + minVolume;
            bool wasVolumeSet = false;

            if (soundGroupNames.ContainsKey(group))
            {
                wasVolumeSet = mixer.SetFloat(soundGroupNames[group], volume);
            }

            return wasVolumeSet;
        }

        public bool GetVolume(SoundGroup group, out float volume)
        {
            float maxVolume = -10f;
            float minVolume = -80f;
            float slope = (maxVolume - minVolume) / 1;

            if (soundGroupNames.ContainsKey(group)
                && mixer.GetFloat(soundGroupNames[group], out volume))
            {
                volume = (volume - minVolume) / slope;
                return true;
            }

            volume = 0f;
            return false;
        }

        private IEnumerator FadeMusicOut(AudioContainer.Music music, float fadeSpeed)
        {
            musicSourceOgVol = source.volume;
            float fadingVol = musicSourceOgVol;
            while (fadingVol > 0)
            {
                fadingVol -= Time.deltaTime * fadeSpeed;
                source.volume = fadingVol;
                yield return null;
            }

            source.volume = musicSourceOgVol;
            PlayMusic(music);
        }
    }
}