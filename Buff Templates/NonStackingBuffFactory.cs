using System.Xml.Linq;

namespace Abilities_Test
{
    public class NonStackingBuffFactory<TBuff> where TBuff : NonStackingBuff, new()
    {
        public static NonStackingBuffFactory<TBuff> Instance { get; } = new();


        /// <summary>
        /// Creates a buff with data
        /// </summary>
        /// <returns>False if it failed and null</returns>
        public bool TryCreateBuff(TBuff old, BuffData data, out TBuff? buff)
        {
            if (data == default)
            {
                buff = null;
                return false;
            }

            if (old == null)
            {
                buff = new TBuff { Data = data};
                return true;
            }
            else if (old.IsWeaker(data, out int difference))
            {
                buff = new TBuff { Data = data, Difference = difference};
                return true;
            }

            buff = null;
            return false;
        }
    }
}
