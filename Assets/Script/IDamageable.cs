using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script
{
    public interface IDamageable
    {
        public void TakeDmg(int dmg, bool _isCrit);
        public void DealDmg(IDamageable target);

        public void ShowDamage(string text, bool isCrit = false);
    }
}
