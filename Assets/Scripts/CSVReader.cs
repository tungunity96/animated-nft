using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    [SerializeField] private TextAsset textAssetData;
    [SerializeField] private int colNum;
    public CharacterList ReadCSV()
    {
        CharacterList characterList = new CharacterList();
        string[] data = textAssetData.text.Split(new string[] { ",", "\n" }, System.StringSplitOptions.None);
        Debug.Log($"data length {data.Length}");
        int tableSize = data.Length / colNum;
        Debug.Log($"tableSize {tableSize}");

        characterList.characters = new Character[tableSize];
        for (int i = 1; i < tableSize; i++)
        {
            characterList.characters[i] = new Character();
            characterList.characters[i].gender = int.Parse(data[colNum * i]);
            characterList.characters[i].race = int.Parse(data[colNum * i + 1]);
            characterList.characters[i].classType = int.Parse(data[colNum * i + 2]);
            characterList.characters[i].sin = int.Parse(data[colNum * i + 3]);
            characterList.characters[i].elemental = int.Parse(data[colNum * i + 4]);
            characterList.characters[i].hair = int.Parse(data[colNum * i + 5]);
            characterList.characters[i].eyes = int.Parse(data[colNum * i + 6]);
            characterList.characters[i].mouth = int.Parse(data[colNum * i + 7]);
            characterList.characters[i].clothes = int.Parse(data[colNum * i + 8]);
        }
        return characterList;
    }
}
