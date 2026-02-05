
 
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class typewriterUI : MonoBehaviour
{
    public Canvas introCanvas;
    Text _text;
    TMP_Text _tmpProText;
    string writer;
    

    private string[] introScript = { "Eurydice was slain by a viper's bitter venom.", "Orpheus, her beloved, and  braved the underworld... to save her." };

    [SerializeField] float delayBeforeStart = 1f;
    [SerializeField] float timeBtwChars = 0.1f;
    [SerializeField] float delayAfterSentance = 0.7f;
    [SerializeField] float delayBeforeChange = 3f;
    [SerializeField] string leadingChar = "";

    [SerializeField] AudioClip textSound;

    private AudioSource textAudio;
    //[SerializeField] float pause = .4f;
    [SerializeField] bool leadingCharBeforeDelay = true;

    private bool playBloop = false;
    //private bool allDone = false;

    // Use this for initialization
    void Start()
    {
        
        textAudio = GetComponent<AudioSource>();
        _text = GetComponent<Text>()!;
        _tmpProText = GetComponent<TMP_Text>()!;

        if (_text != null)
        {
            writer = _text.text;
            _text.text = "";

            StartCoroutine(TypeWriterText());
        }

        if (_tmpProText != null)
        {
            writer = _tmpProText.text;
            _tmpProText.text = "";

            StartCoroutine(TypeWriterText());
        }

      //  SoundFXManager.Instance.PlaySoundFXClip(textSound, transform, 1.0f);

    }



    IEnumerator TypeWriterText()
    {
        _text.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

        for (int i = 0; i < writer.Length; i++)
        {
            char c = writer[i];

            if (_text.text.Length > 0)
            {
                _text.text = _text.text.Substring(0, _text.text.Length - leadingChar.Length);
               
            }

            

            _text.text += c;
            _text.text += leadingChar;
            //textAudio.Play();
            SoundFXManager.Instance.PlaySoundFXClip(textSound, transform, 1.0f);
            playBloop = true;

            yield return new WaitForSeconds(timeBtwChars);

        }

        if (leadingChar != "")
        {
            _text.text = _text.text.Substring(0, _text.text.Length - leadingChar.Length);
        }
    }

    IEnumerator TypeWriterTMP()
    {
        _tmpProText.text = leadingCharBeforeDelay ? leadingChar : "";

        yield return new WaitForSeconds(delayBeforeStart);

        for (int i = 0; i < writer.Length; i++)
        {
            char c = writer[i];
            if (_tmpProText.text.Length > 0)
            {
                _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
            }
            _tmpProText.text += c;
            _tmpProText.text += leadingChar;

            SoundFXManager.Instance.PlaySoundFXClip(textSound, transform, 1.0f);

            if (writer[i] == '.')
            {
                yield return new WaitForSeconds(delayAfterSentance);
            }
            yield return new WaitForSeconds(timeBtwChars);
        }

        if (leadingChar != "")
        {
            _tmpProText.text = _tmpProText.text.Substring(0, _tmpProText.text.Length - leadingChar.Length);
        }
        yield return new WaitForSeconds(delayBeforeChange);
        introCanvas.enabled = false;
    }

    private void Update()
    {
        if (playBloop) {
            SoundFXManager.Instance.PlaySoundFXClip(textSound, transform, 1.0f);
            playBloop = false;
        }

    }



}