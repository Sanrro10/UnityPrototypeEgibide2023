using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Enemies.Witch.Scripts;
using Entities.Player.Scripts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> brujas;
    [SerializeField] private GameObject _block;
    private int _aliveWitches = 3;
    private GameObject _fadeOut;
    private GameObject _muteall;
    private Image _canvasFade;
    private TextMeshProUGUI _endingText;
    private float _fadeSpeed = 1f;
    private bool _fightStarted = false;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.OnPlayerDeath += DeactivateBlock;
        LandWitch.AllyDeath += CheckAllWitchesDead;
        _muteall = GameObject.Find("Canvas");
        _fadeOut = GameObject.Find("FadeOut");
        _canvasFade = _fadeOut.transform.Find("Panel").gameObject.GetComponent<Image>();
        _endingText = _fadeOut.transform.Find("Fin").gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        Debug.Log("Se ha destruido");
        LandWitch.AllyDeath -= CheckAllWitchesDead;
        PlayerController.OnPlayerDeath -= DeactivateBlock;
    }
    
    private void DeactivateBlock() => _block.SetActive(false);

    private void CheckAllWitchesDead()
    {
        _aliveWitches--;
        Debug.Log("Brujas Vivas" + _aliveWitches);
         if (_aliveWitches > 0) return;
        beginTheEnd();
    }
    private void beginTheEnd()
    {
        StartCoroutine(showPanel(_fadeSpeed, _canvasFade));
        _muteall.gameObject.SetActive(false);
        
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
        if (_fightStarted) return;
        if (other.CompareTag("Player"))
        {
            foreach (GameObject bruja in brujas)
            {
                bruja.SetActive(true);
            }

            _fightStarted = true;
        }
        _block.SetActive(true);
    }

    private void GoToCredits()
    {
        SceneManager.LoadScene("Creditos");
    }

}
