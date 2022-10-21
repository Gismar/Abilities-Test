namespace Abilities_Test
{
    public class Buff : IDisposable, IBuff<BuffData>
    {
        public BuffData Data { get; set; }
        public int TicksLeft { get; set; }
        public Ability<BuffAbilityData> Source { get; set; }

        public void Remove(Character target)
        {
            target[Data.Modifying].ChangeValueBy(Data.Attribute, -Data.Value);
        }
        public void Apply(Character target)
        {
            target[Data.Modifying].ChangeValueBy(Data.Attribute, Data.Value);

            TicksLeft = Data.Duration;
            target.AddBuff(this);
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
