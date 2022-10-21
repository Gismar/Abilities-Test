namespace Abilities_Test
{
    public class NonStackingBuff: IDisposable, IBuff<BuffData>
    {
        public int Difference = 0;

        public BuffData Data { get; set; }
        public int TicksLeft { get; set; }
        public Ability<BuffAbilityData> Source { get; set; }

        public void Remove(Character target)
        {
            target[Data.Modifying].ChangeValueBy(Data.Attribute, -Data.Value);
        }
        public void Apply(Character target)
        {
            int value = Difference > 0 ? Difference : Data.Value;

            target[Data.Modifying].ChangeValueBy(Data.Attribute, value);
            TicksLeft = Data.Duration;
            target.AddBuff(this);
        }

        public bool IsWeaker(BuffData data, out int difference)
        {
            difference = data.Value - Data.Value;
            return Data.Value < data.Value;
        }

        /// <returns>True if it needs to be removed</returns>
        public bool Tick()
        {
            TicksLeft--;
            if (TicksLeft <= 0)
                return true;
            return false;
        }

        public void Dispose()
        {
            Console.WriteLine($"{Data} is being Destroyed");
            GC.SuppressFinalize(this);
        }
    }
}
