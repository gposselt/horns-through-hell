
 
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TypewriterUI : MonoBehaviour
{
    public Canvas introCanvas;
    TMP_Text _tmpProText;
    string writer;

    private string[] introScript = { "Eurydice was slain by a viper's bitter venom.", "Orpheus, her beloved, and  braved the underworld... to save her." };

    [SerializeField] float delayBeforeStart = 1f;
    [SerializeField] float timeBtwChars = 0.1f;
    [SerializeField] float delayAfterSentance = 0.7f;
    [SerializeField] float delayBeforeChange = 3f;
    [SerializeField] string leadingChar = "";

    [SerializeField] AudioClip textSound;

    //[SerializeField] float pause = .4f;
    [SerializeField] bool leadingCharBeforeDelay = true;

    public bool playIntroCutscene = true;

    // Use this for initialization
    void Start()
    {
        _tmpProText = GetComponent<TMP_Text>()!;

        if (!playIntroCutscene)
        {
            introCanvas.enabled = false;
            return;
        }

        if (_tmpProText != null)
        {
            writer = _tmpProText.text;
            _tmpProText.text = "";

            StartCoroutine(TypeWriterTMP());
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
    
}