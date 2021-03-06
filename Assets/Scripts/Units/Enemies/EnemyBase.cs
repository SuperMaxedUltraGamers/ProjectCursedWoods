using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CursedWoods
{
    public class EnemyBase : UnitBase
    {
        protected override void Die()
        {
            Destroy(gameObject);
        }
    }
}