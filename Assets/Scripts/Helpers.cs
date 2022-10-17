using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers
{
    public static string GetNFTName(bool isFemale, RaceType raceType, ClassType classType, ElementalType elementalType, int hairName, int eyeName, int mouth, int clothes)
    {
        return FromDecimalToHex(isFemale ? 1 : 0) + FromDecimalToHex((int)raceType) + FromDecimalToHex((int)classType) + FromDecimalToHex((int)elementalType) + FromDecimalToHex(hairName) + FromDecimalToHex(eyeName) + FromDecimalToHex(mouth) + FromDecimalToHex(clothes);
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

    public static string GetAnimationName(AnimationName animName)
    {
        switch (animName)
        {
            case AnimationName.GetHit:
                return "get_hit";
            case AnimationName.Idle1:
                return "idle1";
            case AnimationName.Idle2:
                return "idle2";
            case AnimationName.Walk:
                return "walk";
            case AnimationName.Die:
                return "die";
            case AnimationName.CastSkill1:
                return "cast_skill_1";
            case AnimationName.CastSkill2:
                return "cast_skill_2";
            case AnimationName.BowAttack:
                return "attack_bow";
            case AnimationName.CrossbowAttack:
                return "attack_crossbow";
            case AnimationName.WandAttack:
                return "attack_wand";
            case AnimationName.GunAttack:
                return "attack_gun";
            case AnimationName.LightMeleeAttack:
                return "attack_light_melee";
            case AnimationName.HeavyMeleeAttack:
                return "attack_heavy_melee";
            default:
                return "idle1";
        }
    }

    public static string FromDecimalToHex(int type)
    {
        return type.ToString("X");
    }
}