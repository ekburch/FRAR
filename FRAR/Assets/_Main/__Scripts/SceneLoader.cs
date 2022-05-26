using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.SceneSystem;
using Microsoft.MixedReality.Toolkit.Extensions.SceneTransitions;

namespace FRAR
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; set; }

        public event Action OnSceneLoad;
        public event Action OnFadeComplete;

        [SerializeField] private Image m_FadeImage;
        [SerializeField] private Color m_fadeColor = Color.black;
        [SerializeField] private float m_fadeDuration = 1f;
        [SerializeField] private bool m_fadeInOnSceneLoad = false;
        [SerializeField] private bool m_fadeInOnStart = false;
        [SerializeField] private AudioMixerSnapshot m_defaultSnapshot;
        [SerializeField] private AudioMixerSnapshot m_fadedSnapshot;

        private bool m_isFading;
        private float m_fadeStartTime;
        private Color m_fadeOutColor;

        public bool IsFading => m_isFading;

        public bool isTesting = true;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError($"SceneLoader.Awake(): {Instance} already exists, destroying duplicate singleton.");
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                Instance = this;
            }

            m_fadeOutColor = new Color(m_fadeColor.r, m_fadeColor.g, m_fadeColor.b, 0f);
            m_FadeImage.enabled = true;
            //m_FadeImage.CrossFadeColor(m_fadeOutColor, 0f, false, true);
        }

        private void OnEnable()
        {
            // Register callback for everytime a new scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;

            // Initially load the second scene additive
             if (!isTesting) SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }

        private void Start()
        {
            if (m_fadeInOnStart)
            {
                FadeIn(true);
            }
        }

        // Called when a new scene was loaded
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            if (loadMode == LoadSceneMode.Additive)
            {
                // Set the additive loaded scene as active
                // So new instantiated stuff goes here
                // And so we know which scene to unload later
                SceneManager.SetActiveScene(scene);
            }

            if (m_fadeInOnSceneLoad)
            {
                FadeIn(true);
            }
        }

        public void LoadSceneAsync(string _sceneName)
        {
            StartCoroutine(_LoadSceneAsync(_sceneName));
        }

        IEnumerator _LoadSceneAsync(string _sceneName)
        {
            FadeIn(true);
            AsyncOperation asyncOp = SceneManager.LoadSceneAsync(_sceneName);
            while (!asyncOp.isDone)
            {
                yield return null;
            }
            FadeOut(true);
        }

        public static void ReloadScene()
        {
            var currentScene = SceneManager.GetActiveScene();
            var index = currentScene.buildIndex;

            SceneManager.UnloadSceneAsync(currentScene);

            SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        }

        public static void LoadScene(string name)
        {
            var currentScene = SceneManager.GetActiveScene();

            SceneManager.UnloadSceneAsync(currentScene);

            SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        }

        public static void LoadScene(int index)
        {
            var currentScene = SceneManager.GetActiveScene();

            SceneManager.UnloadSceneAsync(currentScene);

            SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);
        }

        #region Screen fading
        public void FadeIn(bool fadeAudio)
        {
            FadeIn(1.0f, m_fadeDuration, fadeAudio);
        }

        public void FadeIn(float value, float duration, bool fadeAudio)
        {
            if (m_isFading) 
                return;
            StartCoroutine(HandleFade(value, duration));

            if (m_defaultSnapshot && fadeAudio)
                m_defaultSnapshot.TransitionTo(duration);
        }

        public void FadeOut(bool fadeAudio)
        {
            FadeOut(0.0f, m_fadeDuration, fadeAudio);
        }

        public void FadeOut(float value, float duration, bool fadeAudio)
        {
            if (m_isFading)
                return;
            StartCoroutine(HandleFade(value, duration));

            // Fade out the audio over the same duration.
            if (m_fadedSnapshot && fadeAudio)
                m_fadedSnapshot.TransitionTo(duration);
        }

        private IEnumerator HandleFade(float value, float duration)
        {
            m_isFading = true;

            var tempColor = m_FadeImage.color;
            float timer = 0f;
            while (timer <= duration)
            {
                timer += Time.deltaTime;
                tempColor.a = value - Mathf.Clamp01(timer / duration);
                yield return null;
            }

            m_FadeImage.color = tempColor;
            m_isFading = false;

            if (OnFadeComplete != null)
            {
                OnFadeComplete();
            }
        }
        #endregion

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void QuitApplication()
        {
            FadeOut(true);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

        #region Tasks called on scene load
        /// <summary>
        /// This is for MVP 2 when we need to transition into 
        /// a new scene that uses several states and time
        /// </summary>
        /// <param name="targetTime"></param>
        /// <returns></returns>
        private async Task SetTimescale(float targetTime)
        {
            Time.timeScale = targetTime;
            await Task.Yield();
        }

        /// <summary>
        /// Audio is a huge component of panel operation
        /// Crossfading audio is important for UX
        /// </summary>
        /// <param name="targetVolume"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        private async Task FadeAudio(float targetVolume, float duration)
        {
            float startTime = Time.realtimeSinceStartup;
            float startVolume = AudioListener.volume;
            while (Time.realtimeSinceStartup < startTime + duration)
            {
                AudioListener.volume = Mathf.Lerp(startVolume, targetVolume, Time.realtimeSinceStartup - startTime / duration);
                await Task.Yield();
            }
            AudioListener.volume = targetVolume;
        }
        #endregion
    }
}