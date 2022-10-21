namespace Abilities_Test
{
    public partial class Character: IDisposable
    {
        public string Name;
        public int CurrentHealth = 0;

        private readonly Dictionary<Ability<BuffAbilityData>, List<IBuff<BuffData>>> _buffs;
        public List<IAbility> Abilities { get; private set; }

        public Character(string name, Dictionary<Attribute, int> baseAttributes)
        {
            SetAttributes(baseAttributes);
            Abilities = new List<IAbility>();
            Name = name;
            _buffs = new();
            CurrentHealth = (int)MathF.Ceiling(GetTotalAttribute(Attribute.MaxHealth));
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= (int)MathF.Floor(damage * (200 / (GetTotalAttribute(Attribute.Defense) + 100)));
            if (CurrentHealth <= 0)
                Die();
        }

        public void TakeTrueDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
                Die();
        }

        public int GetAttackRange(out bool isCrit)
        {
            Random random = new Random();
            float range = random.Next(50, 101) / 100f;
            isCrit = random.Next(0, 101) <= GetTotalAttribute(Attribute.CritRate);
            float wepMult = 2;
            float damage = 100 + (wepMult * GetTotalAttribute(Attribute.Damage)) + GetTotalAttribute(Attribute.Stat);
            float levelBon = 5 * 5;
            float total = range * (damage + levelBon) * (isCrit ? 2 + GetTotalAttribute(Attribute.CritDamage) / 100 : 1);
            return (int)MathF.Round(total);
        }

        public void AddBuff(IBuff<BuffData> buff)
        {
            if (_buffs.ContainsKey(buff.Source))
                _buffs[buff.Source].Add(buff);
            else
                _buffs.Add(buff.Source, new List<IBuff<BuffData>>() { buff });
        }

        public void AddAbility(IAbility ability)
        {
            ability.Caster = this;
            Abilities.Add(ability);
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

        public void DisplayBuffs() => _buffs.Keys.ToList().ForEach(b => b.Display());

        public void Die()
        {
            Console.WriteLine($"{Name} has died");
        }
    }
}
