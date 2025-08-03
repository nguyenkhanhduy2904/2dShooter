using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public interface IDamageable
    {
        public void TakeDmg(int dmg, bool _isCrit);
        public void DealDmg(IDamageable target, int dmg, bool isCrit);

        public void ShowDamage(string text, bool isCrit = false);

        public IEnumerator ChangeColor(Color color, float time);
    }
}
