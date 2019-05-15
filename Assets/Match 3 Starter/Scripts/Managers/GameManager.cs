using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public GameObject faderObj;
	public Image faderImg;
	public bool gameOver = false;

	public float fadeSpeed = .02f;

	private Color fadeTransparency = new Color(0, 0, 0, .04f);
	private string currentScene;
	private AsyncOperation async;

	void Awake() {
		if (instance == null) {
			DontDestroyOnLoad(gameObject);
			instance = GetComponent<GameManager>();
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
		} else {
			Destroy(gameObject);
		}
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			ReturnToMenu();
		}
	}

	public void LoadScene(string sceneName) {
		instance.StartCoroutine(Load(sceneName));
		instance.StartCoroutine(FadeOut(instance.faderObj, instance.faderImg));
	}

	public void ReloadScene() {
		LoadScene(SceneManager.GetActiveScene().name);
	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
		currentScene = scene.name;
		instance.StartCoroutine(FadeIn(instance.faderObj, instance.faderImg));
	}

	IEnumerator FadeOut(GameObject faderObject, Image fader) {
		faderObject.SetActive(true);
		while (fader.color.a < 1) {
			fader.color += fadeTransparency;
			yield return new WaitForSeconds(fadeSpeed);
		}
		ActivateScene();
	}

	IEnumerator FadeIn(GameObject faderObject, Image fader) {
		while (fader.color.a > 0) {
			fader.color -= fadeTransparency;
			yield return new WaitForSeconds(fadeSpeed);
		}
		faderObject.SetActive(false);
	}

	IEnumerator Load(string sceneName) {
		async = SceneManager.LoadSceneAsync(sceneName);
		async.allowSceneActivation = false;
		yield return async;
		isReturning = false;
    }

	public void ActivateScene() {
		async.allowSceneActivation = true;
	}

	public string CurrentSceneName {
		get{
			return currentScene;
		}
	}

	public void ExitGame() {
		#if UNITY_STANDALONE
			Application.Quit();
		#endif

		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}

	private bool isReturning = false;
	public void ReturnToMenu() {
		if (isReturning) {
			return;
		}

        if (CurrentSceneName != "Menu") {
			StopAllCoroutines();
			LoadScene("Menu");
			isReturning = true;
        }
	}

}
