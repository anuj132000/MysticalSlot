using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private string sceneBundleUrl = "../mystical-scene-bundle";
    private string assetsBundleUrl = "../mystical-asset-bundle";
    private const bool isProdBuild = false;

    private static GameManager s_Instance;
    public static GameManager Instance
    {
        get { return s_Instance; }
        private set => s_Instance = value;
    }

    private void Awake() {
        if (s_Instance == this)
            return;

        if (s_Instance == null) {
            s_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }


    void Start()
    {
        loadScene();
    }

    private void loadScene() {
        if (isProdBuild) {
            StartCoroutine(setupDownload());
        }
        else
            SceneManager.LoadSceneAsync("SlotGameScene");
    }

    private IEnumerator setupDownload()
    {
        // Load the scene bundle
        AssetBundleCreateRequest sceneBundleRequest = AssetBundle.LoadFromFileAsync(sceneBundleUrl);
        yield return sceneBundleRequest;

        AssetBundle sceneBundle = sceneBundleRequest.assetBundle;
        if (sceneBundle == null) {
            Debug.LogError("Failed to load Scene AssetBundle!");
            yield break;
        }

        // Load scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("SlotGameScene");
        while (!asyncLoad.isDone) {
            yield return null;
        }

        // Load asset bundle
        AssetBundleCreateRequest assetsBundleRequest = AssetBundle.LoadFromFileAsync(assetsBundleUrl);
        yield return assetsBundleRequest;

        AssetBundle assetsBundle = assetsBundleRequest.assetBundle;
        if (assetsBundle == null) {
            Debug.LogError("Failed to load Assets AssetBundle!");
            yield break;
        }


    }
}
