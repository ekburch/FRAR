using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRAR
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance = null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError($"AudioManager.Awake(): {Instance} already exists, destroying duplicate!");
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
