using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class charColors
{
    public static Color GetColor(CharacterColors color)
    {
        return color switch
        {
            CharacterColors.Orgulho_Lilac => HexToColor("#D6BDFF"),
            CharacterColors.Gula_Peach => HexToColor("#FFE8D8"),
            CharacterColors.Avareza_Cream => HexToColor("#FFF8D7"),
            CharacterColors.Inveja_Mint => HexToColor("#DAFFD7"),
            CharacterColors.Preguica_SkyBlue => HexToColor("#D7EFFF"),
            CharacterColors.Luxuria_Pink => HexToColor("#FED7FF"),
            CharacterColors.Ira_Rose => HexToColor("#FFD7DB"),
            _ => Color.white,
        };
    }

    private static Color HexToColor(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }
}
