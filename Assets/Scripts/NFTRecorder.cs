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
    [SerializeField] private int startIndex = 0;
    private CharacterList characterList;
    private bool isFinishSingle = false;

    private void Awake()
    {
        csvReader = GetComponent<CSVReader>();
    }
    private void Start()
    {
        recordController = new RecordController();
        characterList = csvReader.ReadCSV();
        StartCoroutine(RecordNFTs());
    }

    private void SetupCharacter(int gender, RaceType raceType, ClassType classType, ElementalType elementalType, int hair, int eye, int mouth, int clothes)
    {
        bool isFemale = gender == 1;
        spineController = isFemale ? femaleCharacterController : maleCharacterController;
        femaleCharacterController.gameObject.SetActive(isFemale);
        maleCharacterController.gameObject.SetActive(!isFemale);
        spineController.ChangeSkin(raceType, classType, clothes.ToString(), eye.ToString(), hair.ToString(), mouth.ToString());
        // SetupMagicCircle(elementalType);
        SetupBackgrounds(raceType);
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
    //         int hairNum = isFemale ? femaleHairNum : maleHairNum;
    //         int eyeNum = isFemale ? femaleEyeNum : maleEyeNum;
    //         int total = Enum.GetNames(typeof(RaceType)).Length * Enum.GetNames(typeof(ClassType)).Length * Enum.GetNames(typeof(ElementalType)).Length * hairNum * eyeNum;
    //         // GetPlayerPrefs();

    //         for (int raceType = startRace; raceType < Enum.GetNames(typeof(RaceType)).Length; raceType++)
    //         {
    //             SetupBackgrounds((RaceType)raceType);
    //             for (int classType = startClass; classType < Enum.GetNames(typeof(ClassType)).Length; classType++)
    //             {
    //                 for (int elementalType = startElemental; elementalType < Enum.GetNames(typeof(ElementalType)).Length; elementalType++)
    //                 {
    //                     for (int hair = 0; hair < hairNum; hair++)
    //                         for (int eye = 0; eye < eyeNum; eye++)
    //                         {
    //                             isFinishSingle = false;
    //                             count++;
    //                             //TODO: Change setup character
    //                             SetupCharacter((RaceType)raceType, (ClassType)classType, hair + 1, eye + 1, 1);
    //                             Debug.Log("**** Start record " + Helpers.GetNFTName(isFemale, (RaceType)raceType, (ClassType)classType, (ElementalType)elementalType, hair, eye));
    //                             Debug.Log("**** " + count + "/" + total + " ****");
    //                             if (recordVideo && captureScreen)
    //                             {
    //                                 SetupParticles((ElementalType)elementalType);
    //                                 StartCoroutine(CaptureCharacterImage((RaceType)raceType, (ClassType)classType, (ElementalType)elementalType, hair, eye, 1.7f));
    //                                 StartCoroutine(StartRecordNFT((RaceType)raceType, (ClassType)classType, (ElementalType)elementalType, hair, eye));
    //                             }
    //                             else if (!recordVideo && captureScreen)
    //                             {
    //                                 SetupScreenResolution(480, 480);
    //                                 SetupMagicCircle((ElementalType)elementalType);
    //                                 StartCoroutine(CaptureCharacterImage((RaceType)raceType, (ClassType)classType, (ElementalType)elementalType, hair, eye, 0.2f, true));
    //                             }
    //                             else
    //                             {
    //                                 SetupParticles((ElementalType)elementalType);
    //                                 StartCoroutine(StartRecordNFT((RaceType)raceType, (ClassType)classType, (ElementalType)elementalType, hair, eye));
    //                             }
    //                             yield return new WaitUntil(() => isFinishSingle);
    //                             if (recordVideo) yield return new WaitForSeconds(0.25f);
    // #if UNITY_EDITOR
    //                             if (count >= total)
    //                             {
    //                                 Debug.Log("**** Stop Playing Unity ****");
    //                                 UnityEditor.EditorApplication.isPlaying = false;
    //                             }
    // #endif
    //                         }
    //                     // SavePlayerPrefs(raceType, classType, elementalType);
    //                 }
    //             }
    //         }
    //     }

    private IEnumerator RecordNFTs()
    {
        GetPlayerPrefs();
        var characters = characterList.characters;
        if (!saveCount)
            count = 0;
        else
            startIndex = count;
        for (int i = startIndex; i < characters.Length; i++)
        {
            isFinishSingle = false;
            count++;
            var character = characters[i];
            // if (character == null) continue;
            var isFemale = character.gender == 1;
            SetupCharacter(character.gender, (RaceType)character.race, (ClassType)character.classType, (ElementalType)character.elemental, character.hair + 1, character.eyes + 1, character.mouth + 1, character.clothes + 1);
            Debug.Log("**** Start record character index " + count);
            Debug.Log("---" + count + "/" + characters.Length + "---");
            if (recordVideo && captureScreen)
            {
                SetupParticles((ElementalType)character.elemental);
                StartCoroutine(CaptureCharacterImage(isFemale, (RaceType)character.race, (ClassType)character.classType, (ElementalType)character.elemental, character.hair, character.eyes, character.mouth, character.clothes, 1.7f));
                StartCoroutine(StartRecordNFT(isFemale, (RaceType)character.race, (ClassType)character.classType, (ElementalType)character.elemental, character.hair, character.eyes, character.mouth, character.clothes));
            }
            else if (!recordVideo && captureScreen)
            {
                SetupScreenResolution(480, 480);
                SetupMagicCircle((ElementalType)character.elemental);
                StartCoroutine(CaptureCharacterImage(isFemale, (RaceType)character.race, (ClassType)character.classType, (ElementalType)character.elemental, character.hair, character.eyes, character.mouth, character.clothes, 0.2f, true));
            }
            else
            {
                SetupParticles((ElementalType)character.elemental);
                StartCoroutine(StartRecordNFT(isFemale, (RaceType)character.race, (ClassType)character.classType, (ElementalType)character.elemental, character.hair, character.eyes, character.mouth, character.clothes));
            }
            yield return new WaitUntil(() => isFinishSingle);
            if (saveCount) SavePlayerPrefs();
            if (recordVideo) yield return new WaitForSeconds(0.25f);
#if UNITY_EDITOR
            if (count == characters.Length - 1)
            {
                Debug.Log("**** Stop Playing Unity ****");
                UnityEditor.EditorApplication.isPlaying = false;
            }
#endif
        }
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
        string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory) + "/NFTImages/";

        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);

        string fileName = Helpers.GetNFTName(isFemale, raceType, classType, elementalType, hair, eyes, mouth, clothes) + ".png";
        yield return new WaitForSeconds(delay);
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, fileName), 1);
        Debug.Log("- Captured " + " - " + folderPath + fileName);
        if (captureScreenOnly) isFinishSingle = true;
    }
}