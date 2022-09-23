using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FRAR
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance = null;

        [Header("Set in inspector")]
        [SerializeField] AudioSource m_audioSource0 = default;
        [SerializeField] AudioSource m_audioSource1 = default;
        [SerializeField] AudioSource m_sfxSource = default;

        public AudioClip m_mainMenuMusic = default;
        public AudioClip m_quizMenuMusic = default;
        public AudioClip m_correctSFX = default;
        public AudioClip m_incorrectSFX = default;
        public AudioClip[] m_inGameMusic = default;

        [SerializeField] bool m_isLooping;
        public bool IsLooping { get => m_isLooping; private set => m_isLooping = value; }

        bool isCurrentSource = true;

        Coroutine _currSourceFadeRoutine = null;
        Coroutine _newSourceFadeRoutine = null;

        public int m_currentTrackIndex = 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError($"SoundManager.Awake(): {Instance} already exists, destroying duplicate.");
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            m_currentTrackIndex = Random.Range(0, m_inGameMusic.Length);
			ChangeMusic(1);
        }

        private void Update()
        {
            if (m_audioSource1 == null || m_audioSource0 == null)
                InitAudioSources();
            if (!IsPlaying)
            {
                if (!IsLooping)
                {
                    m_currentTrackIndex++;
                    if (m_currentTrackIndex >= m_inGameMusic.Length)
                        m_currentTrackIndex = 0;
                    AudioClip newClip = m_inGameMusic[m_currentTrackIndex];
                    CrossFadeAudio(newClip, 1f, .25f, 0f);
                }
			}
		}

        public void PlayLooping(AudioClip clip, bool isLooping)
        {
            m_audioSource1.clip = clip;
            if (isLooping)
                m_audioSource1.loop = true;
            m_audioSource1.Play();
        }

        public void PlayOneShot(AudioClip clip)
        {
            m_sfxSource.clip = clip;
            m_sfxSource.PlayOneShot(clip);
        }

        public void PlayClip(AudioClip clip)
        {
            m_sfxSource.clip = clip;
            m_sfxSource.volume = 1;
            m_sfxSource.Play();
        }

        public void ChangeMusic(int track)
        {
            AudioClip clip = null;
            if (IsPlaying)
            {
                switch(track)
                {
                    case 1:
                        IsLooping = true;
                        clip = m_mainMenuMusic;
                        break;
                    case 2:
                        IsLooping = true;
                        clip = m_quizMenuMusic;
                        break;
                    case 3:
                        IsLooping = false;
                        clip = m_inGameMusic[m_currentTrackIndex];
                        break;
                    default:
                        break;
                }
                CrossFadeAudio(clip, 1f, 0.25f, 0f);
            }
            else
            {
                return;
            }
        }

        //To Do: Replace with an actual generic function
        public void StopAudioSource()
        {
            m_sfxSource.volume = 0;
            m_sfxSource.Stop();
        }

        public void StopAllAudioSources()
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            foreach (var source in audioSources)
            {
                source.Stop();
            }
        }

        public void CrossFadeAudio(AudioClip clip, float maxVolume, float fadingTime, float delay = 0)
        {
            StartCoroutine(FadeAudio(clip, maxVolume, fadingTime, delay));
        }

        IEnumerator FadeAudio(AudioClip clip, float maxVolume, float fadingTime, float delay = 0)
        {
            if (delay > 0)
                yield return new WaitForSeconds(delay);

            AudioSource currentSource, newSource;
            if (isCurrentSource)
            {
                currentSource = m_audioSource0;
                newSource = m_audioSource1;
            }
            else
            {
                currentSource = m_audioSource1;
                newSource = m_audioSource0;
            }

            newSource.clip = clip;
            newSource.Play();
            newSource.volume = 0;
			newSource.loop = IsLooping;

            if (_currSourceFadeRoutine != null)
            {
                StopCoroutine(_currSourceFadeRoutine);
            }

            if (_newSourceFadeRoutine != null)
            {
                StopCoroutine(_newSourceFadeRoutine);
            }

            _currSourceFadeRoutine = StartCoroutine(FadeSource(currentSource, currentSource.volume, 0, fadingTime));
            _newSourceFadeRoutine = StartCoroutine(FadeSource(newSource, newSource.volume, maxVolume, fadingTime));

            isCurrentSource = !isCurrentSource;

            yield break;
        }

        IEnumerator FadeSource(AudioSource source, float startVolume, float endVolume, float duration)
        {
            float startTime = Time.time;

            while (true)
            {
                float elapsed = Time.time - startTime;
                source.volume = Mathf.Clamp01(Mathf.Lerp(startVolume, endVolume, elapsed / duration));

                if (source.volume == endVolume)
                {
                    break;
                }
                yield return null;
            }
        }

        void InitAudioSources()
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();

            if (ReferenceEquals(audioSources, null) || audioSources.Length == 0)
            {
                m_audioSource1 = gameObject.AddComponent<AudioSource>();
                m_audioSource0 = gameObject.AddComponent<AudioSource>();
                return;
            }

            switch (audioSources.Length)
            {
                case 1:
                    {
                        m_audioSource0 = audioSources[0];
                        m_audioSource1 = gameObject.AddComponent<AudioSource>();
                    }
                    break;
                default:
                    {
                        m_audioSource0 = audioSources[0];
                        m_audioSource1 = audioSources[1];
                    }
                    break;
            }
        }

        public bool IsPlaying
        {
            get
            {
                if (m_audioSource1 == null || m_audioSource0 == null)
                {
                    return false;
                }

                return m_audioSource0.isPlaying || m_audioSource1.isPlaying;
            }
        }
    }
}
