using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//todo: replace HTH logo rawImage with a UI Image

public class MenuManager : MonoBehaviour
{
    public Canvas introCanvas;
    public Canvas mainMenu;
    public Button playButton;
    public Button quitButton;
    public Button settingsButton;
    public Slider volumeSlider;
    public Slider sfxSlider;
    public Slider masterSlider;
    public AudioMixer audioMixer;

    public GameObject settingsScreen;
    

    [SerializeField] public float rotationSpeed = 2.0f;
    [SerializeField] public float rotationAmount = 0.5f;

    public GameObject boat;
    float timer = 0;
    public float bobScale = 1.0f;
    private float boatBaseY;


    [SerializeField] AudioClip mainTheme;

    public IEnumerator PlayMainTheme()
    {

        yield return new WaitForSeconds(10.0f);
        
        SoundFXManager.Instance.PlaySoundFXClip(mainTheme, transform, 1.0f);
        
        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingsScreen.SetActive(false);
        // SoundFXManager.Instance.PlaySoundFXClip(mainTheme, transform, 0.7f);
        StartCoroutine(PlayMainTheme());
        introCanvas.enabled = true;
        mainMenu.enabled = false;
        boatBaseY = boat.transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {
        if (!introCanvas.enabled)
        {
            timer += Time.deltaTime/1.8f;
            mainMenu.enabled = true;

            float sineTime = Mathf.Sin(timer);

            //rock the boat
            boat.transform.Rotate(0, 0, sineTime/(30*Mathf.PI));
            
            //move up and down a little bit each second
            Vector3 pos = boat.transform.position;
            pos.y += (sineTime / (30 * Mathf.PI)) * bobScale * Time.deltaTime;
            boat.transform.position = pos;
        }
       

    }

    public void Play()
    {
        SceneManager.LoadScene("Scenes/TilemapLevelTestThing 1");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Settings()
    {
        settingsScreen.SetActive(true);
    }

    public void SliderChange()
    {
        audioMixer.SetFloat("MusicVol", LinearToDB(volumeSlider.value));
        audioMixer.SetFloat("MasterVol", LinearToDB(masterSlider.value));
        audioMixer.SetFloat("SFXVol", LinearToDB(sfxSlider.value));
    }

    float LinearToDB(float value)
    {
        if (value <= 0.0001f)
            return -80f; // silence

        return Mathf.Log10(value) * 20f;
    }

    public void CloseSettings()
    {
        settingsScreen.SetActive(false);
    }
}
