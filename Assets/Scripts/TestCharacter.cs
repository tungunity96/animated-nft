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
    public string mouthName;
    public AnimationName animationName;
    public bool isTestAll;
    private void Start()
    {
        spineController = GetComponent<SpineController>();
        if (!IsValidTest()) return;
        else if (isTestAll) StartCoroutine(TestAll());
        else TestSingle();
    }

    private bool IsValidTest()
    {
        return !String.IsNullOrEmpty(eyeName) && !String.IsNullOrEmpty(hairName) && !String.IsNullOrEmpty(mouthName);
    }

    private IEnumerator TestAll()
    {
        for (int classType = 0; classType < Enum.GetNames(typeof(ClassType)).Length; classType++)
        {
            spineController.ChangeSkin(raceType, (ClassType)classType, eyeName, hairName, mouthName);
            yield return spineController.PlayAnimation(Helpers.GetAnimationName(animationName));
            yield return spineController.PlayAnimation(Helpers.GetAnimationName(animationName));
            yield return spineController.PlayAnimation(Helpers.GetAnimationName(animationName));
        }
    }

    private void TestSingle()
    {
        spineController.ChangeSkin(raceType, classType, eyeName, hairName, mouthName);
        StartCoroutine(spineController.PlayAnimation(Helpers.GetAnimationName(animationName), true));
    }
}