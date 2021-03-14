﻿using UnityEngine;
using UnityEngine.UI;

namespace CursedWoods.UI
{
    public class LevelUIManager : MonoBehaviour
    {
        [SerializeField]
        private SpellCaster playerSC;

        [SerializeField]
        private GameObject spellMenu;

        [SerializeField]
        private GameObject spellMenuPointer;
        private Image pointerImg;

        [SerializeField]
        private Image[] spellMenuSpellGraphics;
        private int previousGraphicIndex;

        private void Awake()
        {
            pointerImg = spellMenuPointer.GetComponent<Image>();
        }

        private void OnEnable()
        {
            playerSC.SpellMenuTransIn += SpellMenuTransIn;
            playerSC.SpellMenuTransOut += SpellMenuTransOut;
            playerSC.SelectionMoved += UpdateSpellMenu;
        }

        private void OnDisable()
        {
            playerSC.SpellMenuTransIn -= SpellMenuTransIn;
            playerSC.SpellMenuTransOut -= SpellMenuTransOut;
            playerSC.SelectionMoved -= UpdateSpellMenu;
        }

        private void SpellMenuTransIn(float transparency)
        {
            spellMenu.SetActive(true);

            for (int i = 0; i < spellMenuSpellGraphics.Length; i++)
            {
                if (i == previousGraphicIndex && previousGraphicIndex != 0)
                {
                    SetImageAlpha(spellMenuSpellGraphics[i], transparency* 2f);
                }
                else
                {
                    SetImageAlpha(spellMenuSpellGraphics[i], transparency);
                }
            }

            SetImageAlpha(pointerImg, transparency * 2f);
        }

        private void SpellMenuTransOut(float transparency)
        {
            if (transparency < 0.01f)
            {
                spellMenu.SetActive(false);
            }
            else
            {
                foreach (Image image in spellMenuSpellGraphics)
                {
                    SetImageAlpha(image, transparency);
                }

                SetImageAlpha(pointerImg, transparency);
            }
        }

        private void UpdateSpellMenu(Vector2 inputDir, int spellMenuGraphicIndex)
        {
            // Rotation of selector.
            float rot = Mathf.Atan2(-inputDir.x, inputDir.y);
            spellMenuPointer.transform.rotation = Quaternion.Euler(0f, 0f, rot * Mathf.Rad2Deg);

            if (spellMenuGraphicIndex != previousGraphicIndex)
            {
                SetImageAlpha(spellMenuSpellGraphics[previousGraphicIndex], 0.5f);
            }

            SetImageAlpha(spellMenuSpellGraphics[spellMenuGraphicIndex], 1f);
            previousGraphicIndex = spellMenuGraphicIndex;
        }

        private void SetImageAlpha(Image img, float alpha)
        {
            Color tempColor = img.color;
            tempColor.a = alpha;
            img.color = tempColor;
        }
    }
}