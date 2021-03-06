﻿using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public class ProjectileShockwave : ProjectileBase
    {
        /// <summary>
        /// Determinates how large of an area the shockwave covers at it's maximum scale.
        /// </summary>
        [SerializeField]
        private float targetScale = 10f;

        /// <summary>
        /// How fast the shockwave scales up.
        /// </summary>
        [SerializeField]
        private float scaleSpeed = 5f;
        private float startScaleSpeed = 5f;

        /// <summary>
        /// The starting scale of the shockwave.
        /// </summary>
        private float startScale = 0.1f;


        /// <summary>
        /// The current scale of the shockwave.
        /// </summary>
        private float currentScale = 0.1f;

        /// <summary>
        /// What is the acceptable difference between shockwave's current scale and targetscale while lerping.
        /// </summary>
        private float targetScaleOffAmount = 0.01f;

        private ParticleSystem particles;

        private List<Collider> hitColliders = new List<Collider>();

        protected override void Awake()
        {
            base.Awake();
            particles = GetComponentInChildren<ParticleSystem>();
        }

        private void Update()
        {
            Scale(Time.deltaTime);
        }

        public override void Activate(Vector3 pos, Quaternion rot)
        {
            base.Activate(pos, rot * Quaternion.Euler(90f, 0f, 0f));
            hitColliders.Clear();
            currentScale = startScale;
            targetScale = 10f;
            scaleSpeed = startScaleSpeed;
            isDecreasing = false;
            transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            particles.Play();
        }

        private bool isDecreasing = false;

        /// <summary>
        /// Scales the shockwave to it's targetscale and then deactivates it.
        /// </summary>
        /// <param name="deltaTime">Takes delta time, makes sure scaling is framerate independent.</param>
        private void Scale(float deltaTime)
        {
            if (!isDecreasing)
            {
                currentScale = Mathf.Lerp(currentScale, targetScale, scaleSpeed * deltaTime);
                transform.localScale = Vector3.one * currentScale;
                if (currentScale >= targetScale - targetScaleOffAmount)
                {
                    targetScale = 0f;
                    isDecreasing = true;
                }
            }
            else
            {
                scaleSpeed *= Time.deltaTime * 2f + 1f;
                currentScale = Mathf.Lerp(currentScale, targetScale, scaleSpeed / 2f * deltaTime);
                transform.localScale = Vector3.one * currentScale;

                if (currentScale <= 0f)
                {
                    Deactivate();
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            int otherLayer = other.gameObject.layer;
            if (otherLayer == GlobalVariables.ENEMY_LAYER)
            {

                if (!hitColliders.Contains(other))
                {
                    hitColliders.Add(other);

                    IHealth otherHealth = other.GetComponent<IHealth>();
                    if (otherHealth == null)
                    {
                        otherHealth = other.GetComponentInParent<IHealth>();
                    }

                    otherHealth.DecreaseHealth(DamageAmount, DamageType);
                }
            }
        }
    }
}