namespace Abilities_Test
{
    public enum Attribute
    {
        Stat,
        Damage,
        MaxHealth,
        Defense,
        MoveSpeed,
        AttackSpeed,
        CritRate,
        CritDamage,
        Perception,
        Evasion
    }
    public class Attributes: IDisposable
    {
        private readonly Dictionary<Attribute, int> _attributes;

        public int this[Attribute type]
        {
            get => _attributes[type];
            private set => _attributes[type] = value;
        }

        public Attributes(int setAll)
        {
            _attributes = new(Enum.GetValues<Attribute>().Length);
            foreach (Attribute key in Enum.GetValues<Attribute>())
                _attributes.Add(key, setAll);
        }

        public Attributes(Dictionary<Attribute, int> dictionary)
        {
            _attributes = new(Enum.GetValues<Attribute>().Length);
            foreach (Attribute key in Enum.GetValues<Attribute>()) 
            {
                if (dictionary.TryGetValue(key, out var value))
                    _attributes.Add(key, value);
                else
                    _attributes.Add(key, -1);
            }
        }
	
	    public void ChangeValueBy (Attribute type, int value) => this[type] += value;
        public void Dispose()
        {
            _attributes.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
