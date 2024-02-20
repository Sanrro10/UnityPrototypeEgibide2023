using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Enemies.Witch.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> brujas;
    private int _aliveWitches = 3;
    public Canvas fadeOut;
    public Canvas muteall;
    private Image _canvasFade;
    private TextMeshProUGUI _endingText;
    private float _fadeSpeed = 1f;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        LandWitch.AllyDeath += CheckAllWitchesDead;
        _canvasFade = fadeOut.transform.Find("Panel").gameObject.GetComponent<Image>();
        _endingText = fadeOut.transform.Find("Fin").gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void CheckAllWitchesDead()
    {
        _aliveWitches--;
         if (_aliveWitches != 0) return;
        beginTheEnd();
    }
    private void beginTheEnd()
    {
        muteall.gameObject.SetActive(false);
        StartCoroutine(showPanel(_fadeSpeed, _canvasFade));
    }

    private IEnumerator showText(float fade, TextMeshProUGUI text)
    {
        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r,
                text.color.g,
                text.color.b,
                text.color.a + (Time.deltaTime / fade));
            yield return null;
        }
        Invoke(nameof(GoToCredits), 2f);
    }

    private IEnumerator showPanel(float fade, Image panel)
    {
        while (panel.color.a < 1.0f)
        {
            panel.color = new Color(panel.color.r,
                panel.color.g,
                panel.color.b,
                panel.color.a + (Time.deltaTime / fade));
            yield return null;
        }

        StartCoroutine(showText(fade*2, _endingText));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject bruja in brujas)
            {
                bruja.SetActive(true);
            }
            gameObject.GetComponent<CircleCollider2D>().gameObject.SetActive(false);
        }
        
    }

    private void GoToCredits()
    {
        SceneManager.LoadScene("Creditos");
    }

}
