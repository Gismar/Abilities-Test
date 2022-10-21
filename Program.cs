using Abilities_Test;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Attribute = Abilities_Test.Attribute;
using Newtonsoft.Json.Linq;

string fileName = $"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\Player.json";
Character player = CreateCharacter(fileName, out var abilityFiles);
foreach (KeyValuePair<string, int> ability in abilityFiles)
    CreateAbility(player, ability.Key, ability.Value);

fileName = $"{AppDomain.CurrentDomain.BaseDirectory}\\Data\\Slime.json";
Character enemy = CreateCharacter(fileName, out abilityFiles);
foreach (KeyValuePair<string, int> ability in abilityFiles)
    CreateAbility(enemy, ability.Key, ability.Value);


static Character CreateCharacter(string fileName, out Dictionary<string, int> abilities)
{
    Character player;
    using StreamReader stream = new(fileName);
    using JsonTextReader reader = new(stream);
    JObject file = (JObject)JToken.ReadFrom(reader);

    //Set Attributes
    Dictionary<Attribute, int> attributes = file["Attributes"]
        .Children<JObject>()
        .ToDictionary(
            key => Enum.Parse<Attribute>(key.Properties().First().Name),
            value => (int)value.Properties().First().Value
        );

    //Get abilities
    abilities = new();
    foreach (var array in file["Abilities"])
        abilities.Add((string)array["FileUrl"], (int)array["Level"]);

    player = new Character((string)file["Name"], attributes);

    return player;
}

static void CreateAbility(Character character, string fileName, int level)
{
    using StreamReader stream = new($"{AppDomain.CurrentDomain.BaseDirectory}Data\\{fileName}");
    using JsonTextReader reader = new(stream);
    JObject file = (JObject)JToken.ReadFrom(reader);

    string name = (string)file["Name"];
    JToken data = file["Data"].First(d => (int)d["Level"] == level);

    IAbility? ability;
    switch ((string)file["Type"])
    {
        case "AttackAbility":
            ability = Helper.CreateAttackAbility(level, name, data);
            if (ability != null)
                character.AddAbility(ability);
            break;

        case "Buff":
            ability = Helper.CreateBuffAbility(level, name, data);
            if (ability != null)
                character.AddAbility(ability);
            break;

        case "NonStackingBuff":
            ability = Helper.CreateNonStackingBuffAbility(level, name, data);
            if (ability != null)
                character.AddAbility(ability);
            break;
    }
}

Random ran = new();
while (player.CurrentHealth > 0 && enemy.CurrentHealth > 0)
{
    switch (ran.Next(0, 4))
    {
        case 0:
            BBuffAAtt(player, enemy);
            break;
        case 1:
            BAttAAtt(player, enemy);
            break;
        case 2:
            BAttABuff(player, enemy);
            break;
        case 3:
            BBuffABuff(player, enemy);
            break;
    }

    Console.ReadLine();
}

GC.Collect();

static void BBuffAAtt(Character a, Character b)
{
    b.Abilities[1].Cast(b);
    a.Abilities[0].Cast(b);
    Console.WriteLine($"{b.Name} HP: {b.CurrentHealth}\n" +
        $"{a.Name} HP: {a.CurrentHealth}\n");
    b.TickBuffs();
    a.TickBuffs();
}

static void BAttAAtt(Character a, Character b)
{
    b.Abilities[0].Cast(a);
    a.Abilities[0].Cast(b);
    Console.WriteLine($"{b.Name} HP: {b.CurrentHealth}\n" +
        $"{a.Name} HP: {a.CurrentHealth}\n");
    b.TickBuffs();
    a.TickBuffs();
}

static void BAttABuff(Character a, Character b)
{
    b.Abilities[0].Cast(a);
    a.Abilities[1].Cast(a);
    Console.WriteLine($"{b.Name} HP: {b.CurrentHealth}\n" +
        $"{a.Name} HP: {a.CurrentHealth}\n");
    b.TickBuffs();
    a.TickBuffs();
}

static void BBuffABuff(Character a, Character b)
{
    b.Abilities[1].Cast(b);
    a.Abilities[1].Cast(a);
    Console.WriteLine($"{b.Name} HP: {b.CurrentHealth}\n" +
        $"{a.Name} HP: {a.CurrentHealth}\n");
    b.TickBuffs();
    a.TickBuffs();
}