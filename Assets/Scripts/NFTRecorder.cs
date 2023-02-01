using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using System;
using System.Linq;

[System.Serializable]
public struct RaceBackground
{
    public RaceType raceType;
    public GameObject background;
    public Vector3 characterPos;
}

[System.Serializable]
public struct ElementalSprite
{
    public ElementalType elementalType;
    public Sprite elementalCircleSprite;
}

public class NFTRecorder : MonoBehaviour
{
    RecordController recordController;
    CSVReader csvReader;
    [Header("Record Config")]
    public RaceType recordRace;
    public ClassType startClass;
    public bool isFemale = false;

    [Header("Config")]
    private SpineController spineController;
    public SpriteRenderer magicCircleSprite;
    public Transform character;
    public SpineController maleCharacterController;
    public SpineController femaleCharacterController;
    public ParticleSystem psAuraGround;
    public ParticleSystem psMagicCircle;
    public RaceBackground[] raceBackgrounds;
    public ElementalSprite[] elementalSprites;
    public bool recordVideo = false;
    public bool captureScreen = false;
    public bool saveCount = true;
    private int count = 0;
    private CharacterList characterList;
    private bool isFinishSingle = false;

    private void Awake()
    {
        csvReader = GetComponent<CSVReader>();
    }
    private void Start()
    {
        recordController = new RecordController();
        // characterList = csvReader.ReadCSV();
        StartCoroutine(RecordImagesByGenderAndRace(isFemale, recordRace));
    }

    private void SetupCharacter(int gender, RaceType raceType, ClassType classType, ElementalType elementalType, int hair, int eye, int mouth, int clothes)
    {
        bool isFemale = gender == 1;
        spineController = isFemale ? femaleCharacterController : maleCharacterController;
        femaleCharacterController.gameObject.SetActive(isFemale);
        maleCharacterController.gameObject.SetActive(!isFemale);
        spineController.ChangeSkin(raceType, classType, hair, eye, mouth, clothes);
        // SetupMagicCircle(elementalType);
        SetupBackgrounds(raceType);
        if (classType == ClassType.knight) character.transform.position = new Vector3(0.40f, character.transform.position.y);
        else character.transform.position = new Vector3(0f, character.transform.position.y);
    }

    private void SetupParticles(ElementalType elementalType)
    {
        if (!recordVideo)
        {
            psMagicCircle.gameObject.SetActive(false);
            return;
        }
        var sprite = elementalSprites.FirstOrDefault((elementalSprite) => elementalType == elementalSprite.elementalType);
        if (sprite.elementalCircleSprite == null)
        {
            Debug.LogError($"elementalType {elementalType.ToString()} not valid!");
            return;
        }
        psAuraGround.gameObject.SetActive(true);
        psMagicCircle.gameObject.SetActive(true);
        var psRenderer = psMagicCircle.gameObject.GetComponent<ParticleSystemRenderer>();
        psRenderer.material.mainTexture = sprite.elementalCircleSprite.texture;
        // var mainPs = psMagicCircle.main;
        // mainPs.startColor = Helpers.GetElementalColor(elementalType);
        // mainPs = psAuraGround.main;
        // mainPs.startColor = Helpers.GetElementalColor(elementalType);
    }

    private void SetupMagicCircle(ElementalType elementalType)
    {
        if (recordVideo)
        {
            magicCircleSprite.gameObject.SetActive(false);
            return;
        }
        var sprite = elementalSprites.FirstOrDefault((elementalSprite) => elementalType == elementalSprite.elementalType);
        if (sprite.elementalCircleSprite == null)
        {
            Debug.LogError($"elementalType {elementalType.ToString()} not valid!");
            return;
        }
        magicCircleSprite.gameObject.SetActive(true);
        // magicCircleSprite.color = Helpers.GetElementalColor(elementalType);
        magicCircleSprite.sprite = sprite.elementalCircleSprite;
    }

    private void SetupBackgrounds(RaceType raceType)
    {
        foreach (var bg in raceBackgrounds)
        {
            if (raceType == bg.raceType)
            {
                bg.background.SetActive(true);
                character.transform.position = bg.characterPos;
                bg.background.GetComponent<Animator>().enabled = recordVideo;
            }
            else
            {
                bg.background.GetComponent<Animator>().enabled = false;
                bg.background.SetActive(false);
            }
        }
    }

    private void SetupScreenResolution(int width, int height)
    {
        Screen.SetResolution(width, height, true, 60);
    }

    private void GetPlayerPrefs()
    {
        count = PlayerPrefs.GetInt("count", 0);
    }

    private void SavePlayerPrefs()
    {
        PlayerPrefs.SetInt("count", count);
    }

    //     private IEnumerator RecordNFTs()
    //     {
    //         GetPlayerPrefs();
    //         var characters = characterList.characters;
    //         if (!saveCount)
    //             count = 0;
    //         else
    //             startIndex = count;
    //         for (int i = startIndex; i < characters.Length; i++)
    //         // for (int i = startIndex; i < startIndex + 5000; i++)
    //         {
    //             isFinishSingle = false;
    //             count++;
    //             var character = characters[i];
    //             if (character == null) continue;
    //             if (character.gender != 1 || (ClassType)character.classType != ClassType.knight) continue;
    //             var isFemale = character.gender == 1;
    //             SetupCharacter(character.gender, (RaceType)character.race, (ClassType)character.classType, (ElementalType)character.elemental, character.hair + 1, character.eyes + 1, character.mouth + 1, character.clothes + 1);
    //             Debug.Log("**** Start record character index " + count);
    //             Debug.Log("---" + count + "/" + characters.Length + "---");
    //             if (recordVideo && captureScreen)
    //             {
    //                 SetupParticles((ElementalType)character.elemental);
    //                 StartCoroutine(CaptureCharacterImage(isFemale, (RaceType)character.race, (ClassType)character.classType, (ElementalType)character.elemental, character.hair, character.eyes, character.mouth, character.clothes, 1.7f));
    //                 StartCoroutine(StartRecordNFT(isFemale, (RaceType)character.race, (ClassType)character.classType, (ElementalType)character.elemental, character.hair, character.eyes, character.mouth, character.clothes));
    //             }
    //             else if (!recordVideo && captureScreen)
    //             {
    //                 SetupScreenResolution(480, 480);
    //                 SetupMagicCircle((ElementalType)character.elemental);
    //                 StartCoroutine(CaptureCharacterImage(isFemale, (RaceType)character.race, (ClassType)character.classType, (ElementalType)character.elemental, character.hair, character.eyes, character.mouth, character.clothes, 0.2f, true));
    //             }
    //             else
    //             {
    //                 SetupParticles((ElementalType)character.elemental);
    //                 StartCoroutine(StartRecordNFT(isFemale, (RaceType)character.race, (ClassType)character.classType, (ElementalType)character.elemental, character.hair, character.eyes, character.mouth, character.clothes));
    //             }
    //             yield return new WaitUntil(() => isFinishSingle);
    //             if (saveCount) SavePlayerPrefs();
    //             if (recordVideo) yield return new WaitForSeconds(0.25f);
    // #if UNITY_EDITOR
    //             if (count == characters.Length - 1)
    //             {
    //                 Debug.Log("**** Stop Playing Unity ****");
    //                 UnityEditor.EditorApplication.isPlaying = false;
    //             }
    // #endif
    //         }
    //     }

    private IEnumerator RecordImagesByGenderAndRace(bool isFemale, RaceType raceType)
    {
        var hairLength = isFemale ? 12 : 7;
        var eyesLength = isFemale ? 13 : 11;
        var mouthLength = 8; //male = female = 8
        var clothesLength = 6; //male = female = 6

        for (int classIndex = (int)startClass; classIndex < 10; classIndex++)
        {
            var classType = (ClassType)classIndex;
            for (int elementalIndex = 0; elementalIndex < 7; elementalIndex++)
            {
                var elementalType = (ElementalType)elementalIndex;
                for (int hairIndex = 0; hairIndex < hairLength; hairIndex++)
                {
                    for (int eyesIndex = 0; eyesIndex < eyesLength; eyesIndex++)
                    {
                        if (isFemale && eyesIndex == 12 && raceType != RaceType.Human && raceType != RaceType.Steamborg) continue;
                        else if (!isFemale && eyesIndex == 10 && raceType != RaceType.Human && raceType != RaceType.Steamborg) continue;
                        for (int mouthIndex = 0; mouthIndex < mouthLength; mouthIndex++)
                        {
                            if (mouthIndex == 2 && raceType != RaceType.Orc) continue;
                            for (int clothesIndex = 0; clothesIndex < clothesLength; clothesIndex++)
                            {
                                isFinishSingle = false;
                                count++;
                                Debug.Log("**** Start record character index " + count);
                                SetupCharacter(isFemale ? 1 : 0, raceType, classType, elementalType, hairIndex + 1, eyesIndex + 1, mouthIndex + 1, clothesIndex + 1);
                                SetupScreenResolution(480, 480);
                                SetupMagicCircle(elementalType);
                                StartCoroutine(CaptureCharacterImage(isFemale, raceType, classType, elementalType, hairIndex + 1, eyesIndex + 1, mouthIndex + 1, clothesIndex + 1, 0.1f, true));
                                yield return new WaitUntil(() => isFinishSingle);
                            }
                        }
                    }
                }
            }
        }
#if UNITY_EDITOR
        Debug.Log("**** Stop Playing Unity ****");
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private IEnumerator StartRecordNFT(bool isFemale, RaceType raceType, ClassType classType, ElementalType elementalType, int hairName, int eyeName, int mouth, int clothes)
    {
        recordController.StartRecording(isFemale, raceType, classType, elementalType, hairName, eyeName, mouth, clothes);
        Debug.Log("- Start Recording NFT");
        yield return spineController.PlayAnimation(Helpers.GetAnimationName(AnimationName.Idle1));
        yield return spineController.PlayAnimation(Helpers.GetAnimationName(AnimationName.Idle1));
        yield return spineController.PlayAnimation(Helpers.GetAnimationName(AnimationName.Idle1));
        recordController.StopRecording();
        isFinishSingle = true;
        Debug.Log("- Stop Recording NFT");
    }

    private IEnumerator CaptureCharacterImage(bool isFemale, RaceType raceType, ClassType classType, ElementalType elementalType, int hair, int eyes, int mouth, int clothes, float delay = 0.2f, bool captureScreenOnly = false)
    {
        // string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + "/GeneratedNFTImages" + $"/{raceType.ToString()}" + $"/{classType.ToString()}" + "/";
        string folderPath = "E://" + "/GeneratedNFTImages" + $"/{raceType.ToString()}" + $"/{classType.ToString()}" + "/";

        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);

        string fileName = Helpers.GetNFTName(isFemale, raceType, classType, elementalType, hair, eyes, mouth, clothes) + ".png";
        yield return new WaitForSeconds(delay);
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, fileName), 1);
        Debug.Log("- Captured " + " - " + folderPath + fileName);
        if (captureScreenOnly) isFinishSingle = true;
    }
}