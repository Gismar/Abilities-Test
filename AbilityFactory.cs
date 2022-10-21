using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Abilities_Test
{

    public class AbilityFactory<TAbility, TData> where TAbility : Ability<TData>, new()
    {
        private readonly static AbilityFactory<TAbility, TData> _instance = new();
        public static AbilityFactory<TAbility, TData> Instance => _instance;

        public bool TryCreateAbility(TData data, out TAbility? ability)
        {
            if (data == null)
            {
                ability = default;
                return false;
            }

            ability = new TAbility { Data = data };
            return true;
        }
    }
    public interface IAbility 
    {
        public Character Caster { get; set; }

        public void Cast(Character target);
        public void Display();
    }

    public abstract class Ability<TData> : IAbility
    {
        public TData Data { get; set; }
        public Character Caster { get; set; }

        public abstract void Cast(Character target);
        public abstract void Display();
    }
}
