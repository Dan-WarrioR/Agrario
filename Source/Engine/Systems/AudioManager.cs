﻿using SFML.Audio;
using Source.Engine.Tools;
using Source.Engine.Tools.ProjectUtilities;

namespace Source.Engine.Systems
{
    public class AudioManager
    {
        private Dictionary<string, Sound> _sounds = new();

        public AudioManager()
        {
            Dependency.Register(this);
        }

        public void LoadSounds()
        {
            string folderPath = Path.Combine(PathHelper.ResourcesPath, "Sounds");

            if (!Directory.Exists(folderPath))
            {
                return;
            }

            var files = Directory.GetFiles(folderPath, "*.wav");

            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                Sound sound = new(new SoundBuffer(file));

                _sounds.Add(fileName, sound);
            }
        }

        public void PlayLooped(string soundName)
        {
            if (!_sounds.TryGetValue(soundName, out var sound))
            {
                Debug.LogWarning("No sound in collection!");

                return;
            }

            sound.Loop = true;

            sound.Play();
        }

        public void PlayOnced(string soundName)
        {
            if (!_sounds.TryGetValue(soundName, out var sound))
            {
                Debug.LogWarning("No sound in collection!");

                return;
            }

            if (sound.Status == SoundStatus.Playing)
            {
                return;
            }

            sound.Loop = false;

            sound.Play();

        }

        public void StopSound(string soundName)
        {
            if (!_sounds.TryGetValue(soundName, out var sound))
            {
                Debug.LogWarning("No sound in collection!");

                return;
            }

            sound.Stop();
        }

        public void StopAllSounds()
        {
            foreach (var sound in _sounds.Values)
            {
                sound.Stop();
            }
        }

        /// <summary>
        /// The volume is a value between 0 (mute) and 100 (full volume)
        /// </summary>
        public void SetVolume(string soundName, float volume)
        {
            if (!_sounds.TryGetValue(soundName, out var sound))
            {
                Debug.LogWarning("No sound in collection!");

                return;
            }

            sound.Volume = volume;
        }
    }
}