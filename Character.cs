namespace Abilities_Test
{
    public partial class Character: IDisposable
    {
        public string Name;
        public int CurrentHealth = 0;

        private readonly Dictionary<IAbility<BuffAbilityData>, List<IBuff<BuffData>>> _buffs;

        public Character(string name, Dictionary<Attribute, int> baseAttributes)
        {
            SetAttributes(baseAttributes);
            Name = name;
            _buffs = new();
            CurrentHealth = (int)MathF.Ceiling(GetTotalAttribute(Attribute.MaxHealth));
        }

        public void TakeDamage(int damage, bool isTrueDamage)
        {
            if (isTrueDamage)
            {
                CurrentHealth -= damage;
            }
            else
            {
                CurrentHealth -= (int)MathF.Floor(damage * (200 / (GetTotalAttribute(Attribute.Defense) + 100)));
            }
            if (CurrentHealth <= 0)
                Die();
        }

        public void AddBuff(IBuff<BuffData> buff)
        {
            if (_buffs.ContainsKey(buff.Source))
                _buffs[buff.Source].Add(buff);
            else
            {
                _buffs.Add(buff.Source, new List<IBuff<BuffData>>() { buff });
            }
        }

        public void TickBuffs()
        {
            foreach (var buff in _buffs.Keys)
            {
                for (int i = _buffs[buff].Count - 1; i >= 0; i--)
                {
                    if (_buffs[buff][i].Tick())
                    {
                        _buffs[buff][i].Remove(this);
                        _buffs[buff].RemoveAt(i);
                    }
                }

                if (_buffs[buff].Count == 0)
                {
                    _buffs.Remove(buff);
                    continue;
                }
            }
        }

        public void DisplayBuffs()
        {
            foreach (var buff in _buffs.Keys)
            {
                buff.Display();
            }
        }

        public void Die()
        {
            Console.WriteLine("Character has died");
        }
    }
}
