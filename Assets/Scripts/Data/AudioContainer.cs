using UnityEngine;

namespace CursedWoods.Data
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Data/Audio")]
    public class AudioContainer : ScriptableObject
    {
        public enum PlayerSFX
        {
            Footstep = 0,
            SwordSwoosh,
            FireballLaunch,
            FireballHit,
            IceRay,
            MagicBeam,
            Shockwave,
            TakeDamage,
            Death,
            Dash
        }

        public enum SkeletonSFX
        {
            //Alert = 0,
            //Melee,
            TakeDamage = 0,
            Death
        }

        public enum PosTreeSFX
        {
            //Alert = 0,
            //Melee,
            RockHit = 0,
            TakeDamage,
            Death
        }

        public enum MushroomSFX
        {
            //Alert = 0,
            //Melee,
            PoisonCloud = 0,
            TakeDamage,
            Death
        }

        public enum TreeBossSFX
        {
            FrontAttack = 0,
            SideAttack,
            DropAttack,
            RootAttackStart,
            RootAttackRaise,
            TakeDamage,
            Death
        }

        public enum FinalBossSFX
        {
            ProjectileLaunch,
            ProjectileHit,
            Laser,
            MagicScythe,
            TakeDamage,
            Death
        }

        public enum MiscSFX
        {
            GateOpen = 0,
            DoorOpen,
            Locked,
            KeySpawn,
            KeyPickUp,
            HealthPickUp,
            GeneralPickUp,
            RunestoneInteract,
            ButtonPress
        }

        public enum Music
        {
            Menu,
            ForestAmbience,
            CastleAmbience,
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
        public class PosTreeSFXItem
        {
            public PosTreeSFX Effect;
            public AudioClip Clip;
        }

        [System.Serializable]
        public class MushroomSFXItem
        {
            public MushroomSFX Effect;
            public AudioClip Clip;
        }

        [System.Serializable]
        public class TreeBossSFXItem
        {
            public TreeBossSFX Effect;
            public AudioClip Clip;
        }

        [System.Serializable]
        public class FinalBossSFXItem
        {
            public FinalBossSFX Effect;
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
        private PosTreeSFXItem[] posTreeSounds = new PosTreeSFXItem[0];
        [SerializeField]
        private MushroomSFXItem[] mushroomSounds = new MushroomSFXItem[0];
        [SerializeField]
        private TreeBossSFXItem[] treeBossSounds = new TreeBossSFXItem[0];
        [SerializeField]
        private FinalBossSFXItem[] finalBossSounds = new FinalBossSFXItem[0];
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

        public AudioClip GetSoundClip(PosTreeSFX effect)
        {
            foreach (PosTreeSFXItem item in posTreeSounds)
            {
                if (item.Effect == effect)
                {
                    return item.Clip;
                }
            }

            return null;
        }

        public AudioClip GetSoundClip(MushroomSFX effect)
        {
            foreach (MushroomSFXItem item in mushroomSounds)
            {
                if (item.Effect == effect)
                {
                    return item.Clip;
                }
            }

            return null;
        }

        public AudioClip GetSoundClip(TreeBossSFX effect)
        {
            foreach (TreeBossSFXItem item in treeBossSounds)
            {
                if (item.Effect == effect)
                {
                    return item.Clip;
                }
            }

            return null;
        }

        public AudioClip GetSoundClip(FinalBossSFX effect)
        {
            foreach (FinalBossSFXItem item in finalBossSounds)
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