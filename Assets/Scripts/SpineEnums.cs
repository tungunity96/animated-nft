using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AtlasType
{
    Root,
    Weapon,
    Race,
    Class,
    ClassForeHand,
    Eyes,
    Hair,
    Mouth,
}

public enum WeaponType
{
    bow = 0,
    greatsword = 1,
    wand = 2,
    spear = 3,
    sword = 4,
    dagger = 5,
    gun = 6,
    magicstaff = 7,
    crossbow = 8,
    scythe = 9
}

public enum RaceType
{
    Human = 0,
    Elf = 1,
    Devil = 2,
    Angel = 3,
    Orc = 4,
    Undead = 5,
    Steamborg = 6
}

public enum ClassType
{
    hunter = 0,
    knight = 1,
    mage = 2,
    monk = 3,
    priest = 4,
    assassin = 5,
    pirate = 6,
    druid = 7,
    engineer = 8,
    shaman = 9
}

public enum ElementalType
{
    water = 0,
    fire = 1,
    earth = 2,
    plant = 3,
    metal = 4,
    dark = 5,
    light = 6
}

public enum AnimationName
{
    Die,
    Idle1,
    Idle2,
    GetHit,
    Walk,
    HeavyMeleeAttack,
    BowAttack,
    CrossbowAttack,
    GunAttack,
    LightMeleeAttack,
    WandAttack,
    CastSkill1,
    CastSkill2
}