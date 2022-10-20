using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Abilities_Test
{
    public readonly record struct BuffAbilityData(string Name, int Level, IBuff<BuffData>[] Buffs);

    public class AbilityFactory<TAbility, TData> where TAbility : IAbility<TData>, new()
    {
        private readonly static AbilityFactory<TAbility, TData> _instance = new();
        public static AbilityFactory<TAbility,TData> Instance => _instance;

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

    public interface IAbility<TData>
    {
        public TData Data { get; set; }

        public void Cast(Character target);
        public void Display();
    }

    public class BuffAbility : IAbility<BuffAbilityData>
    {
        public BuffAbilityData Data { get; set; }
        public void Cast(Character target)
        {
            DisplayName();
            foreach (IBuff<BuffData> buff in Data.Buffs)
            {
                buff.Source = this;
                buff.Apply(target);
                Console.WriteLine($" - {Helper.DisplayBuff(buff.Data)}");
            }
        }

        private void DisplayName()
        {
            string temp = $"Casted {Helper.LevelDisplay(Data.Level)} {Data.Name}";
            if (Data.Buffs.Length == 1)
                Console.Write(temp);
            else
                Console.WriteLine(temp);
        }

        public void Display()
        {
            DisplayName();
            foreach (IBuff<BuffData> buff in Data.Buffs)
                Console.WriteLine($" - {Helper.DisplayBuff(buff.Data)}: {buff.TicksLeft}s");
        }
    }
}
