using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
using UnityEditor.Recorder;
using UnityEditor.Recorder.Input;
using System.IO;

public class RecordController
{
    private UnityEditor.Recorder.RecorderControllerSettings controllerSettings;
    private UnityEditor.Recorder.RecorderController recorderController;
    private UnityEditor.Recorder.MovieRecorderSettings videoRecorder;

    private float timeInterval = 1.6f;

    public bool isRecording = false;

    public RecordController()
    {
        controllerSettings = ScriptableObject.CreateInstance<RecorderControllerSettings>();
        recorderController = new RecorderController(controllerSettings);
        videoRecorder = ScriptableObject.CreateInstance<MovieRecorderSettings>();
        // videoRecorder.name = "My Recorder";
        videoRecorder.Enabled = true;
        // videoRecorder.OutputFormat = MovieRecorderSettings.VideoRecorderOutputFormat.WebM;
        videoRecorder.VideoBitRateMode = UnityEditor.VideoBitrateMode.High;
        videoRecorder.AudioInputSettings.PreserveAudio = true;
        videoRecorder.ImageInputSettings = new GameViewInputSettings()
        {
            OutputWidth = 480,
            OutputHeight = 480
        };

        controllerSettings.AddRecorderSettings(videoRecorder);
        controllerSettings.SetRecordModeToManual();
        // controllerSettings.SetRecordModeToTimeInterval(0, timeInterval);
        controllerSettings.FrameRatePlayback = FrameRatePlayback.Constant;
        controllerSettings.FrameRate = 60;

        RecorderOptions.VerboseMode = false;
    }

    public void StartRecording(bool isFemale, RaceType raceType, ClassType classType, ElementalType elementalType, int hairName, int eyeName)
    {
        // string mediaOutputFolder = Path.Combine(Application.dataPath, "..", "Recordings");
        // string mediaOutputFolder = "Assets/NFTVideos/";
        string mediaOutputFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Assets/NFTVideos/";
        if (!System.IO.Directory.Exists(mediaOutputFolder))
            System.IO.Directory.CreateDirectory(mediaOutputFolder);

        videoRecorder.OutputFile = Path.Combine(mediaOutputFolder, Helpers.GetNFTName(isFemale, raceType, classType, elementalType, hairName, eyeName));

        recorderController.PrepareRecording();
        recorderController.StartRecording();
        isRecording = true;
        Debug.Log("*** Start Recording");
    }

    public void StopRecording()
    {
        recorderController.StopRecording();
        isRecording = false;
        Debug.Log("*** Stop Recording");
    }
}