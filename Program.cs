using Abilities_Test;
using System.Threading.Tasks;
using Attribute = Abilities_Test.Attribute;

var character = new Character("Player", new() { [Attribute.Stat] = 250});

CastSimpleBuff("Very Powerful Multiplying Stat Buff", 10, Attribute.Stat, Modifying.Multiplicative, -100, 11);
CastSimpleBuff("Very Powerful Bonus Stat Buff", 10, Attribute.Stat, Modifying.Bonus, -100, 9);
CastSimpleBuff("Very Powerful Base Stat Buff", 10, Attribute.Stat, Modifying.Base, -100, 7);
CastSimpleBuff("Very Powerful Limiting Stat Buff", 10, Attribute.Stat, Modifying.Limiter, 999+1, 4);

Console.WriteLine($"\n{character.Name}'s Stat: {character.GetTotalAttribute(Attribute.Stat):n}");
character.DisplayBuffs();
character.TickBuffs();
Console.WriteLine();

CastComplexBuff("Super Very Powerful Multiplying Stat Buff", 43, Attribute.Stat, Modifying.Multiplicative, 1000, 5);

void CastSimpleBuff(string name, int level, Attribute attribute, Modifying modifying, int strength, int duration)
{
    var buffData = new BuffData(attribute, modifying, strength, duration);
    bool result = BuffFactory<Buff>
    .Instance
    .TryCreateBuff(buffData, character, out Buff? temp);

    if (result)
    {
        var data = new BuffAbilityData(name, level, new IBuff<BuffData>[] { temp });
        result = AbilityFactory<BuffAbility, BuffAbilityData>
            .Instance
            .TryCreateAbility(data, out BuffAbility? ability);

        if (result)
        {
            ability?.Cast(character);
            ability?.Display();
        }
    }
}

void CastComplexBuff(string name, int level, Attribute attribute, Modifying modifying, int strength, int duration)
{
    var buffData = new BuffData(attribute, modifying, strength, duration);
    bool result = true;
    IBuff<BuffData>[] buffs = new IBuff<BuffData>[10];
    for (int i = 0; i < 10; i++)
    {
        result &= BuffFactory<Buff>
        .Instance
        .TryCreateBuff(buffData, character, out Buff? temp);
        buffs[i] = temp;
    }

    if (result)
    {
        var data = new BuffAbilityData(name, level, buffs);
        result = AbilityFactory<BuffAbility, BuffAbilityData>
            .Instance
            .TryCreateAbility(data, out BuffAbility? ability);

        if (result)
        {
            ability?.Cast(character);
            ability?.Display();
        }
    }
}


Console.WriteLine($"\n{character.Name}'s Stat: {character.GetTotalAttribute(Attribute.Stat):n}");

for (int i = 10; i >= 0; i--)
{
    Task.Delay(1000).Wait();

    Console.WriteLine($"\n{character.Name}'s Stat: {character.GetTotalAttribute(Attribute.Stat):n}");
    character.DisplayBuffs();
    character.TickBuffs();
}

GC.Collect();

Console.ReadLine();