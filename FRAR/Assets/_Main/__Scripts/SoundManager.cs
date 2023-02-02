using System.Collections;
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
        [SerializeField] AudioSource m_sfxLoopSource = default;
        public AudioSource[] m_sfxSources = default;
        public AudioSource[] m_musicSources = default;

        public AudioClip m_mainMenuMusic = default;
        public AudioClip m_quizMenuMusic = default;
        public AudioClip m_correctSFX = default;
        public AudioClip m_incorrectSFX = default;
        public AudioClip[] m_inGameMusic = default;

        public AudioClip[] m_engineSFX = default;
        public AudioClip m_engineStartSFX = default;
        public AudioClip m_engineLoopSFX = default;
        public AudioClip m_engineStopSFX = default;

        [SerializeField] bool m_isLooping;
        public bool IsLooping { get => m_isLooping; private set => m_isLooping = value; }

        [SerializeField] bool m_shouldBePlaying;
        public bool ShouldBePlaying { get => m_shouldBePlaying; private set => m_shouldBePlaying = value; }

        bool isCurrentSource = true;

        private double nextClipTime;
        private int nextClipIndex = 0;
        // For swapping between AudioSources
        private int toggle = 0; 
        [SerializeField] bool m_isPlayingScheduled;
        public bool IsPlayingScheduled { get => m_isPlayingScheduled; set => m_isPlayingScheduled = value; }

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
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                PlayLooping(m_mainMenuMusic, true);
            }
            nextClipTime = AudioSettings.dspTime + 1f;
			//ChangeMusic(1);
		}

        private void Update()
        {
            if (m_audioSource1 == null || m_audioSource0 == null)
                InitAudioSources();
            if (!IsPlaying)
            {
                if (!IsLooping && ShouldBePlaying)
                {
                    m_currentTrackIndex = m_currentTrackIndex < m_inGameMusic.Length - 1 ? m_currentTrackIndex + 1 : 0;
                    AudioClip newClip = m_inGameMusic[m_currentTrackIndex];
                    CrossFadeAudio(newClip, 1f, .25f, 0f);
                }
			}
			
            AudioSource[] sources = null;
			AudioClip[] clips = null;
            
            if (m_isPlayingScheduled)
            {
                switch (SceneManager.GetActiveScene().name)
                {
                    case "ExploreMode":
                    case "SingleLine":
                        sources = m_sfxSources;
                        clips = m_engineSFX;
                        break;
                    case "ChallengeMode":
                        sources = m_musicSources;
                        clips = m_inGameMusic;
                        break;
                }
                PlayScheduled(sources, clips);
            }
            else
            {
                if (sources != null && sources == m_sfxSources)
                {
                    for (int i = 0; i < sources.Length; i++)
                    {
                        sources[i].Stop();
                    }
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

        public void PlayScheduled(AudioSource[] audioSources, AudioClip[] audioClips)
        {
            double time = AudioSettings.dspTime;

            if (time > nextClipTime - 1)
            {
                AudioClip clip = audioClips[nextClipIndex];
                audioSources[toggle].clip = clip;
                audioSources[toggle].loop = audioSources[toggle].clip.name == "engine_idling";
                audioSources[toggle].PlayScheduled(nextClipTime);

                //nextClipTime += audioClips[toggle].length;
                double duration = (double)clip.samples / clip.frequency;
                nextClipTime = nextClipTime + duration;
                toggle = 1 - toggle;
                nextClipIndex = nextClipIndex < audioClips.Length - 1 ? nextClipIndex + 1 : 0;
            }
        }

		#region Engine SFX
        /// <summary>
        /// Including this to temporarily test out engine sounds
        /// Down the line will swap out
        /// </summary>
        /// <param name="track"></param>
        public IEnumerator PlayEngineSounds()
        {
            //double startTime = AudioSettings.dspTime + 0.2f;
            m_sfxLoopSource.PlayOneShot(m_engineStartSFX);
            yield return new WaitForSeconds(m_engineStartSFX.samples / m_engineStartSFX.frequency);
			m_sfxLoopSource.clip = m_engineLoopSFX;
			m_sfxLoopSource.loop = true;
			m_sfxLoopSource.Play();
            //m_sfxSource.PlayScheduled(AudioSettings.dspTime + m_engineStartSFX.length);
        }

        public void StopEngineSounds()
        {
			m_sfxLoopSource.loop = false;
			m_sfxLoopSource.clip = m_engineStopSFX;
            m_sfxLoopSource.PlayOneShot(m_engineStopSFX);
            //m_sfxSource.Stop();
        }

		#endregion

		public void ChangeMusic(int track)
        {
            AudioClip clip = null;
			switch (track)
			{
                case 0:
                    IsLooping = false;
                    ShouldBePlaying = false;
                    break;
			    case 1:
					IsLooping = true;
                    ShouldBePlaying = true;
                    clip = m_mainMenuMusic;
					break;
				case 2:
					IsLooping = true;
                    ShouldBePlaying = true;
                    clip = m_quizMenuMusic;
					break;
				case 3:
					IsLooping = false;
                    ShouldBePlaying = true;
                    clip = m_inGameMusic[m_currentTrackIndex];
					break;
				}
			CrossFadeAudio(clip, 1f, 0.25f, 0f);
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
