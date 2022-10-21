using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abilities_Test
{
    public readonly record struct AttackAbilityData(string Name, int Level, int Damage, bool IsTrueDamage);
    public class AttackAbility : Ability<AttackAbilityData>
    {
        public override void Cast(Character target)
        {
            int damage = (int)MathF.Round(Data.Damage / 100f * Caster.GetAttackRange(out bool isCrit)); 

            Console.WriteLine(
                $"{Caster.Name}'s {Helper.LevelDisplay(Data.Level)} {Data.Name} dealt {damage} " +
                $"{(isCrit ? "Critical" : "")}" +
                $"{(Data.IsTrueDamage ? "True" : "")} Damage"
            );
            if (Data.IsTrueDamage)
                target.TakeTrueDamage(damage);
            else
                target.TakeDamage(damage);
        }

        public override void Display()
        {
            Console.WriteLine(
                $"{Helper.LevelDisplay(Data.Level)} {Data.Name} deals {Data.Damage}% " +
                $"{(Data.IsTrueDamage ? "True" : "")} Damage"
            );
        }
    }
}
