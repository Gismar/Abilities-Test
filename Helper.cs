using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
