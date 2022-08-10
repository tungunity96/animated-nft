using System;
using System.Collections;
using UnityEngine;

public class TestCharacter : MonoBehaviour
{
    private SpineController spineController;
    public RaceType raceType;
    public ClassType classType;
    public ElementalType elementalType;
    public string hairName;
    public string eyeName;
    private void Start()
    {
        spineController = GetComponent<SpineController>();
        // spineController.ChangeSkin(raceType, classType, eyeName, hairName);
        // StartCoroutine(spineController.PlayAnimation("Idle", true));
        StartCoroutine(test());
    }

    private IEnumerator test()
    {
        for (int classType = 0; classType < Enum.GetNames(typeof(ClassType)).Length; classType++)
        {
            spineController.ChangeSkin(raceType, (ClassType)classType, eyeName, "6");
            yield return spineController.PlayAnimation("Idle");
            yield return spineController.PlayAnimation("Idle");
            yield return spineController.PlayAnimation("Idle");
        }
    }
}