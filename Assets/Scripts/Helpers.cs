using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers
{
    public static string GetNFTName(bool isFemale, RaceType raceType, ClassType classType, ElementalType elementalType, int hairName, int eyeName)
    {
        return FromDecimalToHex(isFemale ? 1 : 0) + FromDecimalToHex((int)raceType) + FromDecimalToHex((int)classType) + FromDecimalToHex((int)elementalType) + FromDecimalToHex(hairName) + FromDecimalToHex(eyeName);
    }

    public static Color GetElementalColor(ElementalType elementalType)
    {
        switch (elementalType)
        {
            case ElementalType.water:
                return new Color(0, 83, 255, 150);
            case ElementalType.fire:
                return new Color(255, 54, 0, 150);
            case ElementalType.earth:
                return new Color(137, 69, 0, 255);
            case ElementalType.plant:
                return new Color(5, 142, 19, 150);
            case ElementalType.metal:
                return new Color(168, 173, 0, 150);
            case ElementalType.dark:
                return new Color(50, 0, 255, 200);
            case ElementalType.light:
                return new Color(255, 255, 255, 150);
            default:
                return new Color(0, 83, 255, 150);
        }
    }

    public static string FromDecimalToHex(int type)
    {
        return type.ToString("X");
    }
}