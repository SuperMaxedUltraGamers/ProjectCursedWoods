using UnityEngine;

namespace CursedWoods.Data
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Data/Audio")]
    public class AudioContainer : ScriptableObject
    {
        public enum PlayerSFX
        {
            Footstep = 0,
            Interact,
            SwordSwoosh,
            Fireball,
            IceRay,
            MagicBeam,
            Shockwave,
            TakeDamage,
            Death
        }

        public enum SkeletonSFX
        {
            Alert = 0,
            Melee,
            TakeDamage,
            Death
        }

        public enum MiscSFX
        {
            GateOpen
        }

        public enum Music
        {
            Menu,
            Ambient,
            Combat
        }

        [System.Serializable]
        public class PlayerSFXItem
        {
            public PlayerSFX Effect;
            public AudioClip Clip;
        }

        [System.Serializable]
        public class SkeletonSFXItem
        {
            public SkeletonSFX Effect;
            public AudioClip Clip;
        }

        [System.Serializable]
        public class MiscSFXItem
        {
            public MiscSFX Effect;
            public AudioClip Clip;
        }

        [System.Serializable]
        public class MusicItem
        {
            public Music music;
            public AudioClip Clip;
        }

        [SerializeField]
        private PlayerSFXItem[] playerSounds = new PlayerSFXItem[0];
        [SerializeField]
        private SkeletonSFXItem[] skeletonSounds = new SkeletonSFXItem[0];
        [SerializeField]
        private MiscSFXItem[] miscSounds = new MiscSFXItem[0];

        [SerializeField]
        private MusicItem[] musicClips = new MusicItem[0];

        public AudioClip GetSoundClip(PlayerSFX effect)
        {
            foreach (PlayerSFXItem item in playerSounds)
            {
                if (item.Effect == effect)
                {
                    return item.Clip;
                }
            }

            return null;
        }

        public AudioClip GetSoundClip(SkeletonSFX effect)
        {
            foreach (SkeletonSFXItem item in skeletonSounds)
            {
                if (item.Effect == effect)
                {
                    return item.Clip;
                }
            }

            return null;
        }

        public AudioClip GetSoundClip(MiscSFX effect)
        {
            foreach(MiscSFXItem item in miscSounds)
            {
                if (item.Effect == effect)
                {
                    return item.Clip;
                }
            }

            return null;
        }

        public AudioClip GetMusicClip(Music music)
        {
            foreach (MusicItem item in musicClips)
            {
                if (item.music == music)
                {
                    return item.Clip;
                }
            }

            return null;
        }

    }
}