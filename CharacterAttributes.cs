namespace Abilities_Test
{
    public partial class Character
    {
        public Dictionary<Modifying, Attributes> CharacterAttributes;

        private void SetAttributes(Dictionary<Attribute, int> baseAttributes)
        {
            CharacterAttributes = new(4)
            {
                [Modifying.Base] = new Attributes(baseAttributes),
                [Modifying.Bonus] = new Attributes(0),
                [Modifying.Multiplicative] = new Attributes(100),
                [Modifying.Limiter] = new Attributes(-1)
            };
        }

        public float GetTotalAttribute(Attribute attribute)
        {
            float total = (this[Modifying.Base][attribute] 
                * (this[Modifying.Multiplicative][attribute] / 100f))
                + this[Modifying.Bonus][attribute];

            if (this[Modifying.Limiter][attribute] == -1)
                return total;

            return MathF.Min(total, this[Modifying.Limiter][attribute]);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this[Modifying.Base].Dispose();
            this[Modifying.Bonus].Dispose();
            this[Modifying.Multiplicative].Dispose();
            this[Modifying.Limiter].Dispose();
        }

        /// <summary>
        /// Shortcut for CharacterAttributes[Modifying Enum]
        /// </summary>
        /// <param name="modifying"></param>
        /// <returns>Base/Bonus/Multiplicative/Limiter Attributes </returns>
        public Attributes this[Modifying modifying]
        {
            get => CharacterAttributes[modifying];
        }
    }
}
