
namespace Abilities_Test
{
    public readonly record struct BuffAbilityData(string Name, int Level, IBuff<BuffData>[] Buffs);
    public class BuffAbility : Ability<BuffAbilityData> //Does not need Caster Info
    {
        public override void Cast(Character target)
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
            string temp = $"{Caster.Name} casted {Helper.LevelDisplay(Data.Level)} {Data.Name}";
            if (Data.Buffs.Length == 1)
                Console.Write(temp);
            else
                Console.WriteLine(temp);
        }

        public override void Display()
        {
            Console.WriteLine($"{Caster.Name}'s {Helper.LevelDisplay(Data.Level)} {Data.Name}");
            foreach (IBuff<BuffData> buff in Data.Buffs)
                Console.WriteLine($" - {Helper.DisplayBuff(buff.Data)}: {buff.TicksLeft}s");
        }
    }
}
