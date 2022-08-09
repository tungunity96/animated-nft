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
        spineController.ChangeSkin(raceType, classType, eyeName, hairName);
    }
}