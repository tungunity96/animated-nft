using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using System;

public class NFTRecorder : MonoBehaviour
{
    RecordController recordController;

    [Header("Config")]
    private SpineController spineController;
    public SpriteRenderer magicCircleSprite;
    public Transform character;
    public SpineController maleCharacterController;
    public SpineController femaleCharacterController;
    public ParticleSystem psAuraGround;
    public ParticleSystem psMagicCircle;
    public RaceBackground[] raceBackgrounds;
    public int femaleHairNum = 12;
    public int femaleEyeNum = 8;
    public int maleHairNum = 6;
    public int maleEyeNum = 6;
    public bool recordVideo = false;
    public bool captureScreen = false;

    [Header("Change character trails")]
    public RaceType raceType;
    public bool isFemale = false;
    private bool isFinishSingle = false;

    private void Start()
    {
        recordController = new RecordController();
        StartCoroutine(RecordNFTs());
        // #if UNITY_EDITOR
        //         Debug.Log("**** Stop Playing Unity ****");
        //         UnityEditor.EditorApplication.isPlaying = false;
        // #endif
    }

    private void SetupCharacter(ClassType classType, int hair, int eye)
    {
        spineController = isFemale ? femaleCharacterController : maleCharacterController;
        femaleCharacterController.gameObject.SetActive(isFemale);
        maleCharacterController.gameObject.SetActive(!isFemale);
        spineController.ChangeSkin(raceType, classType, eye.ToString(), hair.ToString());
    }

    private void SetupParticles(ElementalType elementalType)
    {
        psAuraGround.gameObject.SetActive(true);
        psMagicCircle.gameObject.SetActive(true);
        psAuraGround.startColor = Helpers.GetElementalColor(elementalType);
        psMagicCircle.startColor = Helpers.GetElementalColor(elementalType);
    }

    private void SetupMagicCircle(ElementalType elementalType)
    {
        magicCircleSprite.gameObject.SetActive(true);
        magicCircleSprite.color = Helpers.GetElementalColor(elementalType);
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

    private IEnumerator RecordNFTs()
    {
        int hairNum = isFemale ? femaleHairNum : maleHairNum;
        int eyeNum = isFemale ? femaleEyeNum : maleEyeNum;
        SetupBackgrounds(raceType);
        for (int classType = 0; classType < Enum.GetNames(typeof(ClassType)).Length; classType++)
            for (int elemental = 0; elemental < Enum.GetNames(typeof(ElementalType)).Length; elemental++)
                for (int hair = 0; hair < hairNum; hair++)
                    for (int eye = 0; eye < eyeNum; eye++)
                    {
                        isFinishSingle = false;
                        SetupCharacter((ClassType)classType, hair + 1, eye + 1);
                        Debug.Log("**** Start record " + Helpers.GetNFTName(isFemale, raceType, (ClassType)classType, (ElementalType)elemental, hair, eye));
                        if (recordVideo && captureScreen)
                        {
                            SetupParticles((ElementalType)elemental);
                            StartCoroutine(CaptureCharacterImage((ClassType)classType, (ElementalType)elemental, hair, eye, 1.7f));
                            StartCoroutine(StartRecordNFT((ClassType)classType, (ElementalType)elemental, hair, eye));
                        }
                        else if (!recordVideo && captureScreen)
                        {
                            SetupScreenResolution(480, 480);
                            SetupMagicCircle((ElementalType)elemental);
                            StartCoroutine(CaptureCharacterImage((ClassType)classType, (ElementalType)elemental, hair, eye, 0.2f, true));
                        }
                        else
                        {
                            SetupParticles((ElementalType)elemental);
                            StartCoroutine(StartRecordNFT((ClassType)classType, (ElementalType)elemental, hair, eye));
                        }
                        yield return new WaitUntil(() => isFinishSingle);
                        yield return new WaitForSeconds(0.5f);
                    }
    }

    private IEnumerator StartRecordNFT(ClassType classType, ElementalType elementalType, int hairName, int eyeName)
    {
        recordController.StartRecording(isFemale, raceType, classType, elementalType, hairName, eyeName);
        Debug.Log("- Start Recording NFT");
        yield return spineController.PlayAnimation("Idle");
        yield return spineController.PlayAnimation("Idle");
        yield return spineController.PlayAnimation("Idle");
        recordController.StopRecording();
        isFinishSingle = true;
        Debug.Log("- Stop Recording NFT");
    }

    private IEnumerator CaptureCharacterImage(ClassType classType, ElementalType elementalType, int hairName, int eyeName, float delay = 0.2f, bool captureScreenOnly = false)
    {
        // string folderPath = "Assets/NFTImages/";
        string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Assets/NFTImages/";

        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);

        string fileName = Helpers.GetNFTName(isFemale, raceType, classType, elementalType, hairName, eyeName) + ".png";
        yield return new WaitForSeconds(delay);
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, fileName), 1);
        Debug.Log("- Captured " + raceType.ToString() + " - " + classType.ToString() + folderPath + fileName);
        if (captureScreenOnly) isFinishSingle = true;
    }
}