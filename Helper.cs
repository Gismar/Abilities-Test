using Newtonsoft.Json.Linq;

namespace Abilities_Test
{
    public static class Helper
    {
        public static string LevelDisplay(int level) => level switch
        {
            43 => "■■■",
            42 => "■■○",
            41 => "■○○",
            33 => "○○○",
            32 => "○○×",
            31 => "○××",
            23 => "×××",
            22 => "×× ",
            21 => "×  ",
            _ => $"Lv. {level}"
        };

        public static string DisplayBuff(BuffData data) => data.Modifying switch
        {
            Modifying.Base => $"Base {data.Attribute} {(Math.Sign(data.Value) > 0 ? "+" : "")}{data.Value}",
            Modifying.Bonus => $"{(Math.Sign(data.Value) > 0 ? "+" : "")}{data.Value} {data.Attribute}",
            Modifying.Multiplicative => $"{(Math.Sign(data.Value) > 0 ? "+" : "")}{data.Value}% {data.Attribute}",
            Modifying.Limiter => $"{data.Value} {data.Attribute} limited to {data.Value - 1}",
            _ => "???"
        };

        public static BuffAbility? CreateNonStackingBuffAbility(int level, string name, JToken json)
        {
            var data = new List<IBuff<BuffData>>();
            foreach (JToken value in json["BuffData"])
            {
                BuffData temp = new(
                    Attribute: Enum.Parse<Attribute>((string)value["Attribute"]),
                    Modifying: Enum.Parse<Modifying>((string)value["Modifying"]),
                    Value: (int)value["Value"],
                    Duration: (int)value["Duration"]
                );

                if (NonStackingBuffFactory<NonStackingBuff>.Instance.TryCreateBuff(null, temp, out NonStackingBuff? buff))
                    data.Add(buff);
            }

            BuffAbilityData nsBuffAbilityData = new(name, level, data.ToArray());
            AbilityFactory<BuffAbility, BuffAbilityData>.Instance.TryCreateAbility(nsBuffAbilityData, out BuffAbility? ability);
            return ability;
        }
        public static BuffAbility? CreateBuffAbility(int level, string name, JToken json)
        {
            var data = new List<IBuff<BuffData>>();
            foreach (JToken value in json["BuffData"])
            {
                BuffData temp = new(
                    Attribute: Enum.Parse<Attribute>((string)value["Attribute"]),
                    Modifying: Enum.Parse<Modifying>((string)value["Modifying"]),
                    Value: (int)value["Value"],
                    Duration: (int)value["Duration"]
                );

                if (BuffFactory<Buff>.Instance.TryCreateBuff(temp, out Buff? buff))
                    data.Add(buff);
            }

            BuffAbilityData abilityData = new(name, level, data.ToArray());
            AbilityFactory<BuffAbility, BuffAbilityData>.Instance.TryCreateAbility(abilityData, out BuffAbility? ability);
            return ability;
        }
        public static AttackAbility? CreateAttackAbility(int level, string name, JToken json)
        {
            AttackAbilityData data = new(name, level, (int)json["Damage"], false);
            AbilityFactory<AttackAbility, AttackAbilityData>.Instance
                .TryCreateAbility(data, out AttackAbility? ability);
            return ability;
        }
    }
}
