using System;
using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public class SpellCaster : MonoBehaviour
    {
        public ISpell CurrentSpell
        {
            get;
            private set;
        }

        private bool isInSpellMenu = false;

        //private ISpell[] allSpells;
        //private Dictionary<Spells, ISpell> spellKeyValuePairs = new Dictionary<Spells, ISpell>();

        // TODO: get rid of these and make some sort dictionary tat holds the spells,
        //       problem is that you need to also attach the projectile etc. prefabs that the spells, maybe create data type for that?
        private SpellFireBall spellFireBall;
        private SpellIceRay spellIceRay;
        private SpellMagicBeam spellMagicBeam;
        private SpellShockwave spellShockwave;

        private void Awake()
        {
            spellFireBall = GetComponent<SpellFireBall>();
            spellIceRay = GetComponent<SpellIceRay>();
            spellMagicBeam = GetComponent<SpellMagicBeam>();
            spellShockwave = GetComponent<SpellShockwave>();
            //SpellFireBall spellFireBall = gameObject.AddComponent<SpellFireBall>();
            //or maybe this
            //ISpell spellFireBall = gameObject.AddComponent<SpellFireBall>();
            //spellKeyValuePairs.Add(Spells.Fireball, spellFireBall);
        }

        private void Start()
        {
            /*
            int spellAmount = Enum.GetNames(typeof(Spells)).Length;
            allSpells = new ISpell[spellAmount];
            for (int i=0; i<spellAmount; i++)
            {

            }
            */

            CurrentSpell = spellFireBall;
        }

        private void Update()
        {
            if (Input.GetAxisRaw(CharController.OPEN_SPELLMENU) > 0f)
            {
                CharController.IgnoreCameraControl = true;
                isInSpellMenu = true;
                Vector2 inputDir = new Vector2(Input.GetAxisRaw(CharController.HORIZONTAL_RS), Input.GetAxisRaw(CharController.VERTICAL_RS));
                //print(inputDir);
                if (inputDir.x >= 0f && inputDir.y >= 0f)
                {
                    print("Fireball selected");
                    CurrentSpell = spellFireBall;
                }
                else if (inputDir.x >= 0f && inputDir.y <= 0f)
                {
                    print("IceRaySingle selected");
                    CurrentSpell = spellIceRay;
                }
                else if (inputDir.x <= 0f && inputDir.y <= 0f)
                {
                    print("MagicBeam selected");
                    CurrentSpell = spellMagicBeam;
                }
                else if (inputDir.x <= 0f && inputDir.y >= 0f)
                {
                    print("Shockwave selected");
                    CurrentSpell = spellShockwave;
                }
            }
            else
            {
                CharController.IgnoreCameraControl = false;
                isInSpellMenu = false;
            }
        }

        public void CastSpell()
        {
            if (!CurrentSpell.IsCasting && !CurrentSpell.IsInCoolDown && !isInSpellMenu)
            {
                CurrentSpell.CastSpell();
            }
        }
    }
}