using NAudio.Wave;

namespace Sound
{
    public class Player
    {
        public string Name;
        public readonly string filePath;
        private WaveOutEvent waveOut;
        private AudioFileReader audioFileReader;

        private static WaveOutEvent staticwaveout = new WaveOutEvent();
        private static float? _volume = null;
        public static float Volume
        {
            get => _volume ?? 0.5f;
            set
            {
                _volume = value;
                staticwaveout.Volume = value;
            }
        }

        public Player(string name = "", string filepath = "")
        {
            this.Name = name;
            this.filePath = filepath;
            if(filePath != "")
            {
                waveOut = new WaveOutEvent();
                audioFileReader = new AudioFileReader(filePath);
                waveOut.Init(audioFileReader);
            }
        }

        public void Play()
        {
            if (waveOut != null)
            {
                waveOut.Play();
            }
        }

        public void PlayLooping()
        {
            if (waveOut != null)
            {
                audioFileReader.Position = 0;
                waveOut.Play();
                waveOut.PlaybackStopped += LoopPlaybackStopped;
            }
        }

        public void Stop()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                audioFileReader.Position = 0; 
                waveOut.PlaybackStopped -= LoopPlaybackStopped;
            }
        }

        private void LoopPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (waveOut != null)
            {
                audioFileReader.Position = 0;
                waveOut.Play();
            }
        }
    }
}
