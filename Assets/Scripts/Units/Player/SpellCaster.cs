using System;
using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public class SpellCaster : MonoBehaviour
    {
        //private ISpell[] allSpells;
        //private Dictionary<Spells, ISpell> spellKeyValuePairs = new Dictionary<Spells, ISpell>();

        // TODO: get rid of these and make some sort dictionary that holds the spells,
        //       problem is that you need to also attach the projectile etc. prefabs that the spells, maybe create data type for that?
        private SpellFireBall spellFireBall;
        private SpellIceRay spellIceRay;
        private SpellMagicBeam spellMagicBeam;
        private SpellShockwave spellShockwave;

        private float menuFadeTransparency;
        private float fadeSpeed = 20f;
        private int spellGraphicIndex;

        public event Action<float> SpellMenuTransIn;
        public event Action<float> SpellMenuTransOut;
        public event Action<Vector2, int> SelectionMoved;
        public event Action<Spells, float> SpellCasted;

        public ISpell CurrentSpell
        {
            get;
            private set;
        }

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
            if (!GameMan.Instance.CharController.IgnoreControl)
            {
                if (Input.GetAxisRaw(GlobalVariables.OPEN_SPELLMENU) > 0f && !CurrentSpell.IsCasting)
                {
                    if (GameMan.Instance.PlayerManager.IsSpellCastUnlocked)
                    {
                        if (menuFadeTransparency < 0.5f)
                        {
                            menuFadeTransparency = Mathf.Lerp(menuFadeTransparency, 0.5f, Time.deltaTime * fadeSpeed);
                            if (SpellMenuTransIn != null)
                            {
                                SpellMenuTransIn(menuFadeTransparency);
                            }
                        }

                        GameMan.Instance.CharController.IgnoreCameraControl = true;
                        GameMan.Instance.CharController.IsInSpellMenu = true;
                        Time.timeScale = 0.5f;

                        Vector2 inputDir = new Vector2(Input.GetAxisRaw(GlobalVariables.HORIZONTAL_RS), Input.GetAxisRaw(GlobalVariables.VERTICAL_RS));

                        //print(inputDir.magnitude);
                        if (inputDir.magnitude > 0.4f)
                        {
                            if (inputDir.x >= 0f && inputDir.y >= 0f)
                            {
                                if (GameMan.Instance.PlayerManager.GetSpellLockStatus(Spells.Fireball))
                                {
                                    CurrentSpell = spellFireBall;
                                    spellGraphicIndex = 1;
                                }
                            }
                            else if (inputDir.x >= 0f && inputDir.y <= 0f)
                            {
                                if (GameMan.Instance.PlayerManager.GetSpellLockStatus(Spells.Shockwave))
                                {
                                    CurrentSpell = spellShockwave;
                                    spellGraphicIndex = 2;
                                }
                            }
                            else if (inputDir.x <= 0f && inputDir.y <= 0f)
                            {
                                if (GameMan.Instance.PlayerManager.GetSpellLockStatus(Spells.IceRay))
                                {
                                    CurrentSpell = spellIceRay;
                                    spellGraphicIndex = 3;
                                }
                            }
                            else if (inputDir.x <= 0f && inputDir.y >= 0f)
                            {
                                if (GameMan.Instance.PlayerManager.GetSpellLockStatus(Spells.MagicBeam))
                                {
                                    CurrentSpell = spellMagicBeam;
                                    spellGraphicIndex = 4;
                                }
                            }

                            if (SelectionMoved != null)
                            {
                                SelectionMoved(inputDir, spellGraphicIndex);
                            }
                        }
                        else if (CharController.hasMouseMoved)
                        {
                            Vector2 mousePos = CharController.mousePos;
                            float halfWidth = Screen.width * 0.5f;
                            float halfHeight = Screen.height * 0.5f;
                            if (mousePos.x >= halfWidth && mousePos.y >= halfHeight)
                            {
                                if (GameMan.Instance.PlayerManager.GetSpellLockStatus(Spells.Fireball))
                                {
                                    CurrentSpell = spellFireBall;
                                    spellGraphicIndex = 1;
                                    inputDir = new Vector2(1f, 1f);
                                }
                            }
                            else if (mousePos.x >= halfWidth && mousePos.y <= halfHeight)
                            {
                                if (GameMan.Instance.PlayerManager.GetSpellLockStatus(Spells.Shockwave))
                                {
                                    CurrentSpell = spellShockwave;
                                    spellGraphicIndex = 2;
                                    inputDir = new Vector2(1f, -1f);
                                }
                            }
                            else if (mousePos.x <= halfWidth && mousePos.y <= halfHeight)
                            {
                                if (GameMan.Instance.PlayerManager.GetSpellLockStatus(Spells.IceRay))
                                {
                                    CurrentSpell = spellIceRay;
                                    spellGraphicIndex = 3;
                                    inputDir = new Vector2(-1f, -1f);
                                }
                            }
                            else if (mousePos.x <= halfWidth && mousePos.y >= halfHeight)
                            {
                                if (GameMan.Instance.PlayerManager.GetSpellLockStatus(Spells.MagicBeam))
                                {
                                    CurrentSpell = spellMagicBeam;
                                    spellGraphicIndex = 4;
                                    inputDir = new Vector2(-1f, 1f);
                                }
                            }

                            if (SelectionMoved != null)
                            {
                                SelectionMoved(inputDir, spellGraphicIndex);
                            }
                        }
                    }
                }
                else
                {
                    if (menuFadeTransparency > 0.01f)
                    {
                        menuFadeTransparency = Mathf.Lerp(menuFadeTransparency, 0f, Time.deltaTime * fadeSpeed);
                        if (SpellMenuTransOut != null)
                        {
                            SpellMenuTransOut(menuFadeTransparency);
                        }
                    }

                    GameMan.Instance.CharController.IgnoreCameraControl = false;
                    GameMan.Instance.CharController.IsInSpellMenu = false;
                    Time.timeScale = 1f;
                }
            }
        }

        public void CastSpell()
        {
            CharController controller = GameMan.Instance.CharController;
            if (!CurrentSpell.IsCasting && !CurrentSpell.IsInCoolDown && !controller.IsInSpellMenu)
            {
                CurrentSpell.CastSpell();
                if (SpellCasted != null)
                {
                    float reactivateTime = CurrentSpell.CoolDownTime + CurrentSpell.CastTime;
                    if (reactivateTime > 0f)
                    {
                        SpellCasted(CurrentSpell.SpellType, reactivateTime);
                    }
                }
            }
        }
    }
}