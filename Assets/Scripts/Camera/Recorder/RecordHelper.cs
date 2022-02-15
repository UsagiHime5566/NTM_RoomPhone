using System.Collections;
using UnityEngine;
using NatSuite.Recorders;
using NatSuite.Recorders.Clocks;
using NatSuite.Recorders.Inputs;

public class RecordHelper : MonoBehaviour
{
    [Header(@"Recording")]
    public int videoWidth = 720;
    public int videoHeight = 1280;
    public int videoBitRateKbps = 5000;
    public bool recordMicrophone;
    public Camera recordCam;

    [Header("Use Debug File")]
    public bool useDebug;

    private MP4Recorder recorder;
    private CameraInput cameraInput;
    private AudioInput audioInput;
    private AudioSource microphoneSource;

    private IEnumerator Start()
    {
        // Start microphone
        microphoneSource = gameObject.AddComponent<AudioSource>();
        microphoneSource.mute =
        microphoneSource.loop = true;
        microphoneSource.bypassEffects =
        microphoneSource.bypassListenerEffects = false;
        microphoneSource.clip = Microphone.Start(null, true, 1, AudioSettings.outputSampleRate);
        yield return new WaitUntil(() => Microphone.GetPosition(null) > 0);
        microphoneSource.Play();
    }

    private void OnDestroy()
    {
        // Stop microphone
        microphoneSource.Stop();
        Microphone.End(null);
    }

    public void StartRecording()
    {
        if(useDebug) return;

        // Start recording
        var frameRate = 30;
        var sampleRate = recordMicrophone ? AudioSettings.outputSampleRate : 0;
        var channelCount = recordMicrophone ? (int)AudioSettings.speakerMode : 0;
        var clock = new RealtimeClock();
        recorder = new MP4Recorder(videoWidth, videoHeight, frameRate, sampleRate, channelCount, videoBitRate: videoBitRateKbps * 1000, audioBitRate: 96_000);
        // Create recording inputs
        cameraInput = new CameraInput(recorder, clock, recordCam);
        audioInput = recordMicrophone ? new AudioInput(recorder, clock, microphoneSource, true) : null;
        // Unmute microphone
        microphoneSource.mute = audioInput == null;
    }

    public async void StopRecording(bool shouldUpload)
    {
        if(useDebug) return;
        
        // Mute microphone
        microphoneSource.mute = true;
        // Stop recording
        audioInput?.Dispose();
        cameraInput.Dispose();
        var path = await recorder.FinishWriting();

        // Playback recording
        Debug.Log($"Saved recording to: {path}");

        if(shouldUpload){
            RecordManager.instance.FilePath = path;
        }
        
        //Handheld.PlayFullScreenMovie($"file://{path}");
    }
}