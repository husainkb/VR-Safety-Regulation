using Oculus.Platform;
using Oculus.Platform.Models;
using UnityEngine;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using UnityEngine.UI;

public class audiorecord : MonoBehaviour
{
    // Start is called before the first frame update
    /* public bool isRecording = false;
     public void ToggleRecording()
     {
         if (isRecording)
         {
             StopRecording();
         }
         else
         {
             StartRecording();
         }
     }
     public void StartRecording()
     {
         // Start audio recording using Unity's built-in Microphone class
         string microphone = Microphone.devices[0]; // Choose the desired microphone device (index 0 here)
         AudioClip recording = Microphone.Start(microphone, false, 10, 44100); // 10 seconds of recording at a sample rate of 44.1 kHz

         // Create a WAV file path to save the recording
         string filePath = Path.Combine(UnityEngine.Application.persistentDataPath, "recording.wav");

         // Save the recording as a WAV file
         SavWav.Save(filePath, recording);

         isRecording = true;
     }

     public void StopRecording()
     {
         // Stop audio recording
         Microphone.End(null);

         isRecording = false;
     }
     public static class SavWav
     {
         public static bool Save(string filePath, AudioClip clip)
         {
             // Convert the AudioClip data to a float array
             float[] data = new float[clip.samples];
             clip.GetData(data, 0);

             // Convert the float array to a short array
             short[] intData = new short[data.Length];
             for (int i = 0; i < data.Length; i++)
             {
                 intData[i] = (short)(data[i] * 32767);
             }

             // Convert the short array to a byte array
             byte[] bytes = new byte[intData.Length * 2];
             Buffer.BlockCopy(intData, 0, bytes, 0, bytes.Length);

             // Create the WAV file
             using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
             {
                 BinaryWriter writer = new BinaryWriter(fileStream);

                 // Write the WAV file header
                 writer.Write(0x46464952); // "RIFF"
                 writer.Write((int)(36 + bytes.Length)); // File size - 8
                 writer.Write(0x45564157); // "WAVE"
                 writer.Write(0x20746D66); // "fmt "
                 writer.Write(16); // Chunk size
                 writer.Write((short)1); // Audio format (1 = PCM)
                 writer.Write((short)1); // Number of channels
                 writer.Write(clip.frequency); // Sample rate
                 writer.Write(clip.frequency * 2); // Byte rate
                 writer.Write((short)2); // Block align
                 writer.Write((short)16); // Bits per sample
                 writer.Write(0x61746164); // "data"
                 writer.Write(bytes.Length); // Subchunk2 size
                 writer.Write(bytes);

                 writer.Close();
             }

             return true;
         }
     }*/


    public Button startButton;
    public Button stopButton;
    public GameObject TopicButton;
    private bool isRecording = false;
    private string audioFileName;
    private AudioSource audioSource;
    private float recordingStartTime;
    private float recordingDuration;
    public Text timerText;

    private void Start()
    {
        startButton.onClick.AddListener(StartRecording);
        stopButton.onClick.AddListener(StopRecording);

        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (isRecording)
        {
            recordingDuration = Time.time - recordingStartTime;
            UpdateTimerText(recordingDuration);
        }
    }
    private void StartRecording()
    {
        isRecording = true;
        recordingStartTime = Time.time;
        audioSource.clip = Microphone.Start(null, false, 1800, 44100);
     
        
        startButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(true);
        TopicButton.SetActive(false);
        oclusActiveCanvas.Instance.menu.SetActive(false);

    }

    private void StopRecording()
    {
        if (!isRecording)
            return;


        isRecording = false;
        Microphone.End(null);
        float recordingDuration = Time.time - recordingStartTime;
        AudioClip clippedClip = AudioClip.Create("clipped_audio", Mathf.CeilToInt(recordingDuration * 44100), 1, 44100, false);
        float[] audioData = new float[Mathf.CeilToInt(recordingDuration * 44100)];
        audioSource.clip.GetData(audioData, 0);
        clippedClip.SetData(audioData, 0);
        SaveRecording(clippedClip);
        StartCoroutine(oclussensordebug.Instance.SendAudio());
        ResetTimerText();

        startButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
        TopicButton.SetActive(true);
    }

    private void SaveRecording(AudioClip clip)
    {
        audioFileName = Path.Combine(UnityEngine.Application.persistentDataPath + "/audio_file.wav");
        SavWav.Save(audioFileName, clip);
        Debug.Log("Recording saved at: " + audioFileName);
    }
    public static class SavWav
    {
        public static bool Save(string filename, AudioClip clip)
        {
            if (!filename.ToLower().EndsWith(".wav"))
            {
                filename += ".wav";
            }

            string filePath = Path.Combine(UnityEngine. Application.persistentDataPath, filename);

            var samples = new float[clip.samples];
            clip.GetData(samples, 0);

            using (var fileStream = CreateEmpty(filePath))
            {
                ConvertAndWrite(fileStream, samples);
                WriteHeader(fileStream, clip);
            }

            return true;
        }

        private static FileStream CreateEmpty(string filePath)
        {
            var fileStream = new FileStream(filePath, FileMode.Create);
            byte emptyByte = new byte();

            for (int i = 0; i < 44; i++) // Write empty header
            {
                fileStream.WriteByte(emptyByte);
            }

            return fileStream;
        }

        private static void ConvertAndWrite(FileStream fileStream, float[] samples)
        {
            var intData = new short[samples.Length];

            for (int i = 0; i < samples.Length; i++)
            {
                intData[i] = (short)(samples[i] * 32767);
            }

            var byteArr = new byte[intData.Length * 2];
            Buffer.BlockCopy(intData, 0, byteArr, 0, byteArr.Length);
            fileStream.Write(byteArr, 0, byteArr.Length);
        }

        private static void WriteHeader(FileStream fileStream, AudioClip clip)
        {
            var hz = clip.frequency;
            var channels = clip.channels;
            var samples = clip.samples;

            fileStream.Seek(0, SeekOrigin.Begin);

            var riff = System.Text.Encoding.UTF8.GetBytes("RIFF");
            fileStream.Write(riff, 0, 4);

            var chunkSize = samples * channels * 2 + 36;
            var chunkSizeArr = BitConverter.GetBytes(chunkSize);
            fileStream.Write(chunkSizeArr, 0, 4);

            var wave = System.Text.Encoding.UTF8.GetBytes("WAVE");
            fileStream.Write(wave, 0, 4);

            var fmt = System.Text.Encoding.UTF8.GetBytes("fmt ");
            fileStream.Write(fmt, 0, 4);

            var subChunk1 = BitConverter.GetBytes(16);
            fileStream.Write(subChunk1, 0, 4);

            ushort audioFormat = 1; // PCM
            fileStream.Write(BitConverter.GetBytes(audioFormat), 0, 2);

            fileStream.Write(BitConverter.GetBytes(channels), 0, 2);
            fileStream.Write(BitConverter.GetBytes(hz), 0, 4);

            var byteRate = hz * channels * 2;
            fileStream.Write(BitConverter.GetBytes(byteRate), 0, 4);

            ushort blockAlign = (ushort)(channels * 2);
            fileStream.Write(BitConverter.GetBytes(blockAlign), 0, 2);

            ushort bps = 16;
            fileStream.Write(BitConverter.GetBytes(bps), 0, 2);

            var dataString = System.Text.Encoding.UTF8.GetBytes("data");
            fileStream.Write(dataString, 0, 4);

            fileStream.Write(BitConverter.GetBytes(samples * channels * 2), 0, 4);

            fileStream.Close();
        }
    }
    private void UpdateTimerText(float duration)
    {
        int minutes = Mathf.FloorToInt(duration / 60f);
        int seconds = Mathf.FloorToInt(duration % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void ResetTimerText()
    {
        timerText.text = "00:00";
    }
}
