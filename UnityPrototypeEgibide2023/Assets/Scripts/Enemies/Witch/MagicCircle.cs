using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircle : MonoBehaviour
{

    private int _damage;

    private GameObject _playerRef;
    private LandWitchData _witchData;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerRef = GameController.Instance.GetPlayerGameObject();
        _witchData = GameObject.Find("SorginaLand").GetComponent<LandWitch>().landWitchData;


        InvokeRepeating(nameof(MovementLogic),0,0.02f);
        Invoke(nameof(ActivationLogic), _witchData.magicCircleChargeDuration);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one * (1 + 0.2f * Mathf.Sin(Time.time));
    }

    private void MovementLogic()
    {
        //Might need changes
        gameObject.transform.position = _playerRef.transform.position;
    }

    private void ActivationLogic()
    {
        ChangeColor();
        CancelInvoke(nameof(MovementLogic));
        Invoke(nameof(Activate),_witchData.magicCircleActivationDelay);
    }

    private void Activate()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        Invoke(nameof(EndOfLife), _witchData.magicCircleEffectDuration);
    }
    private void EndOfLife()
    {
        Destroy(gameObject);
    }

    private void ChangeColor()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerRef.gameObject.GetComponent<HealthComponent>().SendMessage("RemoveHealth", _witchData.magicCircleDamage, SendMessageOptions.RequireReceiver);
        }
    }
}
