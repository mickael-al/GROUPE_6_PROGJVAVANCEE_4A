using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WJ
{

    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip mapSound;
        private AudioSource _audioSourceOst;
        private AudioSource _audioSourceFx;
        private float VolumeGlobal;
        private float VolumeMusique;
        private float volumeModifierGontrant = 1.0f;
        public float VolumeModifierGontrant
        {
            get { return volumeModifierGontrant; }
            set
            {
                volumeModifierGontrant = value;
                _audioSourceOst.volume = VolumeGlobal * VolumeMusique * volumeModifierGontrant;
            }
        }

        public static AudioManager Instance { get; private set; }

        public AudioClip MapSound
        {
            get { return mapSound; }
            set { mapSound = value; }
        }

        public float Volume
        {
            get { return VolumeGlobal; }
            set {  
                VolumeGlobal = value; 
                _audioSourceOst.volume = VolumeGlobal * VolumeMusique;
                _audioSourceFx.volume = VolumeGlobal;
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Instance.MapSound = mapSound;
                Destroy(gameObject);
            }
        }

        public float PithchModifier
        {
            get { return _audioSourceOst.pitch; }
            set { _audioSourceOst.pitch = value; }
        }

        private void Start()
        {
            VolumeGlobal = PlayerPrefs.GetFloat("VolumeGlobal", 0.5f);
            VolumeMusique = PlayerPrefs.GetFloat("VolumeMusique", 0.8f);
            _audioSourceOst = CreateAudioSource(mapSound, true);
            _audioSourceFx = CreateAudioSource(null, false);
            StartCoroutine(StartOstSound());
        }

        private AudioSource CreateAudioSource(AudioClip clip, bool loop)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.playOnAwake = false;
            audioSource.loop = loop;
            audioSource.volume = VolumeGlobal;
            return audioSource;
        }

        public static void PlayFx3DSound(GameObject obj, AudioClip clip)
        {
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = obj.AddComponent<AudioSource>();
                audioSource.spatialBlend = 1.0f;
                audioSource.maxDistance = 80.0f;
                audioSource.dopplerLevel = 0.0f;
                audioSource.playOnAwake = false;
                audioSource.loop = false;
                audioSource.volume = Instance.VolumeGlobal;
            }
            audioSource.PlayOneShot(clip);
        }

        public IEnumerator CheckAudioExternVolume()
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("AudioExtern");
            List<AudioSource> source = new List<AudioSource>();
            foreach (var g in gos)
            {
                source.Add(g.GetComponent<AudioSource>());
            }
            float volume = 0.0f;
            while (volume <= Instance.VolumeGlobal)
            {
                volume += Time.deltaTime;
                foreach (var s in source)
                {
                    s.volume = volume;
                }
                yield return null;
            }
        }

        public static void PlayFxSound(AudioClip clip)
        {
            Instance._audioSourceFx.PlayOneShot(clip);
        }

        public IEnumerator StartOstSound()
        {
            if (_audioSourceOst != null)
            {
                while (_audioSourceOst.volume > 0)
                {
                    _audioSourceOst.volume -= Time.deltaTime;
                    yield return null;
                }
            }
            _audioSourceOst.Stop();
            _audioSourceOst.pitch = 1.0f;
            volumeModifierGontrant = 1.0f;
            _audioSourceOst.clip = mapSound;
            _audioSourceOst.Play();
            while (_audioSourceOst.volume < VolumeGlobal * VolumeMusique)
            {
                _audioSourceOst.volume += Time.deltaTime;
                yield return null;
            }
            yield break;
        }

        public IEnumerator StopOstSound()
        {
            if (_audioSourceOst != null)
            {
                while (_audioSourceOst.volume > 0)
                {
                    _audioSourceOst.volume -= Time.deltaTime;
                    yield return null;
                }
            }
            _audioSourceOst.Stop();
            _audioSourceOst.pitch = 1.0f;
            volumeModifierGontrant = 1.0f;
            yield break;
        }
    }
}
