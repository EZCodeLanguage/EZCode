using NAudio.Wave;

namespace Sound
{
    public class Player
    {
        public string Name;
        public readonly string filePath;
        private WaveOutEvent waveOut;
        private AudioFileReader audioFileReader;

        public float Volume
        {
            get => waveOut?.Volume ?? 0.5f;
            set
            {
                if (waveOut != null)
                {
                    waveOut.Volume = value;
                }
            }
        }

        public Player(string name = "", string filepath = "", float volume = 0.5f)
        {
            this.Name = name;
            this.Volume = volume;
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
                audioFileReader.Position = 0; // Reset position to the beginning
                waveOut.Play();
                waveOut.PlaybackStopped += LoopPlaybackStopped;
            }
        }

        public void Stop()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                audioFileReader.Position = 0; // Reset position to the beginning
                waveOut.PlaybackStopped -= LoopPlaybackStopped;
            }
        }

        private void LoopPlaybackStopped(object sender, StoppedEventArgs e)
        {
            // When playback stops, reset the position and play again (looping)
            if (waveOut != null)
            {
                audioFileReader.Position = 0;
                waveOut.Play();
            }
        }
    }
}
