using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    public GameObject loadingScreen;
    public GameObject mainMenuScreen;
    public Slider slider;
    public Text progressText;
    float prog = 0f;

	public void LoadLevel (int sceneIndex)
    {
        Cursor.visible = false;
        StartCoroutine(LoadAsyncFake(sceneIndex));
    }
    //Имитация загрузки уровня, чтобы видеть прогрессбар, т.к. сцена очень легкая
    IEnumerator LoadAsyncFake (int sceneIndex)
    {

        mainMenuScreen.SetActive(false);
        loadingScreen.SetActive(true);

        while (prog < 100f)
        //while (!operation.isDone && prog <= 100f)
        {
            //float progress = Mathf.Clamp01(operation.progress / .9f);
            prog ++;
            slider.value = prog / 100f;
            progressText.text = Mathf.Round (prog) + "%";

            yield return new WaitForSeconds(0.01f);
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        loadingScreen.SetActive(false);
    }


    //Настоящий индикатор загрузки
    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        mainMenuScreen.SetActive(false);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }
}
