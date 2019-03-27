using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour {

    public AudioMixer audioMixer;
    public GameObject settingsScreen;
    public GameObject mainMenuScreen;
    public GameObject pauseMenuScreen;
    public GameObject LevelCompleteMenuScreen;
    public Slider loadingSlider;
    public Slider soundSlider;
    public Slider musicSlider;
    public GameObject desktopOptions;
    public Dropdown resolutionDropdown;
    public static bool muteIsPressed = false;
    public Sprite muteBtnPressed;
    public Sprite muteBtnNormal;
    public Button muteMusicBtn;
    GameObject TouchUI;
    public GameObject MenuBtn;
    [HideInInspector] public float musicVolumeSldr = 0;
    float musicMixerVal;
    [HideInInspector]public bool musicOff = false;
    [HideInInspector]public static bool _paused = false;
    Component[] _comp;
    Resolution[] resolutions;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //musicSlider.value = Mixer_Get_Float();
        GetMainCamera();

#if UNITY_ANDROID
        desktopOptions.SetActive(false);
#endif
    }

    private void Start()
    {
        //Получаем текущие разрешения и заполняем список в настройках доступными разрешениямм
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && TouchUI == null)
        {
#if UNITY_STANDALONE
        MenuBtn.SetActive(false);
#endif
#if UNITY_ANDROID
        MenuBtn.SetActive(true);
            TouchUI = GameObject.Find("TouchUI");
#endif
        }

#if UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!_paused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
#endif
        if (FinishPlatform.LevelComplete)
        {
            LevelCompleteMenuScreen.SetActive(true);
#if UNITY_ANDROID
            TouchUI.SetActive(false);
            MenuBtn.SetActive(false);
#endif
        }
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void GetMainCamera()
    {
        _comp = GetComponentsInChildren(typeof (Canvas), true);
        foreach (Canvas cv in _comp)
        {
            if (cv.worldCamera == null)
            cv.worldCamera = FindObjectOfType<Camera>();
        }
    }

    //Получаем значение громкости музыки из миксера
    public float Mixer_Get_Float()
    {
        float temp;
        audioMixer.GetFloat("musicVol", out temp);
        temp = Mathf.InverseLerp(-80, 20, temp);
        return temp;
    }

    public void OnExitPressed()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnSettingsPressed()
    {
        settingsScreen.SetActive(true);
        mainMenuScreen.SetActive(false);
    }

    public void OnSettingsPausedPressed()
    {
        settingsScreen.SetActive(true);
        pauseMenuScreen.SetActive(false);
    }
    
    public void OnMusicMutePressed()
    {
        if (!musicOff)
        {
            musicOff = true;
            musicVolumeSldr = musicSlider.value;
            audioMixer.GetFloat("musicVol", out musicMixerVal);           
            audioMixer.SetFloat("musicVol", -80f);
            musicSlider.value = 0.001f;
            muteIsPressed = true;
            muteMusicBtn.image.sprite = muteBtnPressed;
        }
        else
        {
            musicOff = false;
            audioMixer.SetFloat("musicVol", musicMixerVal);
            musicSlider.value = musicVolumeSldr;
            muteIsPressed = false;
            muteMusicBtn.image.sprite = muteBtnNormal;
        }
    }

    public void OnBackPressed()
    {
        if (_paused)
        {
            settingsScreen.SetActive(false);
            pauseMenuScreen.SetActive(true);
        }
        else
        {
            settingsScreen.SetActive(false);
            mainMenuScreen.SetActive(true);
        }
    }

    public void OnBackPausePressed()
    {
        Resume();
    }

    public void OnRestartPressed()
    {
        if (FinishPlatform.LevelComplete)
        {
            LevelCompleteMenuScreen.SetActive(false);
            FinishPlatform.LevelComplete = false;

        }
        else
        {
            pauseMenuScreen.SetActive(false);
            Time.timeScale = 1;
        }
#if UNITY_STANDALONE
        Cursor.visible = false;
#endif
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void FixedUpdate()
    {
        GetMainCamera();

        if (Controller.dead)
        {
            Pause();
        }
    }

    public void Resume()
    {
#if UNITY_STANDALONE
        Cursor.visible = false;
#endif
        pauseMenuScreen.SetActive(false);
        if (_paused)
        {
            Time.timeScale = 1;
            _paused = false;
        }
#if UNITY_ANDROID
        TouchUI.SetActive(true);
        MenuBtn.SetActive(true);
#endif

    }

    public void Pause()
    {
        if (!mainMenuScreen.activeInHierarchy)
        {
            _paused = true;
#if UNITY_STANDALONE
            Cursor.visible = true;
#endif
#if UNITY_ANDROID
            TouchUI.SetActive(false);
            MenuBtn.SetActive(false);
#endif
            pauseMenuScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
