using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Damage_Calc
{
    public struct Armor
    {
        public string Type;
        public int ArmorPoints;
        public int ArmorToughness;

        public Armor(string type, int armorPoints, int armorToughness)
        {
            Type = type;
            ArmorPoints = armorPoints;
            ArmorToughness = armorToughness;
        }
    }
}
