using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Linq;

namespace Abilities_Test
{
    public readonly record struct BuffData(Attribute Attribute, Modifying Modifying, int Value, int Duration);
    public enum Modifying
    {
        Base,
        Bonus,
        Multiplicative,
        Limiter
    }

    public class BuffFactory<TBuff> where TBuff : Buff, new()
    {
        public static BuffFactory<TBuff> Instance { get; } = new();

        /// <summary>
        /// Creates a buff with data
        /// </summary>
        /// <returns>False if it failed and null</returns>
        public bool TryCreateBuff(BuffData data, Character target, out TBuff? buff)
        {
            if (data == default || target == null)
            {
                buff = null;
                return false;
            }

            buff = new TBuff { Data = data };
            return true;
        }
    }

    public interface IBuff<TData>
    {
        public IAbility<BuffAbilityData> Source { get; set; }
        public TData Data { get; set; }
        public int TicksLeft { get; set; }
        public bool Tick();
        public void Apply(Character target);
        public void Remove(Character target);
    }
}
