using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Character
{
    public int gender;
    public int race;
    public int classType;
    public int sin;
    public int elemental;
    public int hair;
    public int eyes;
    public int mouth;
    public int clothes;
}

[Serializable]
public class CharacterList
{
    public Character[] characters;
}