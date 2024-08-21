using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;


public enum Sounds
{
    BtnClick,
    BtnDrag,
    BtnClose,
}

public enum Music
{
    MainMenu,
    Race,
}

namespace Core
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioData audioData;
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _soundSource;

        [Inject] private PlayerData _playerData;

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
            _playerData.GameSettings.OnMusicVolumeChanged += UpdateMusicVolume;
        }

        private void UpdateMusicVolume()
        {
            _musicSource.volume = _playerData.GameSettings.MusicVolume;
        }

        private void OnSceneLoad(Scene scene, LoadSceneMode arg1)
        {
            if (scene.buildIndex == 0) //MainMenu
            {
                Play(Music.MainMenu);
            }
            else
            {
                Play(Music.Race);
            }
        }

        public void Play(Music music)
        {
            var data = audioData.MusicData.FirstOrDefault(x => x.Music == music);
            if (data == null)
                return;

            _musicSource.volume = _playerData.GameSettings.MusicVolume;
            _musicSource.clip = data.Clip;
            _musicSource.Play();
        }

        public void Play(Sounds sound)
        {
            var data = audioData.SoundData.FirstOrDefault(x => x.Sounds == sound);
            if (data == null)
                return;

            _soundSource.clip = data.Clip;
            _soundSource.PlayOneShot(data.Clip, _playerData.GameSettings.SoundVolume);


        }
    }
}
