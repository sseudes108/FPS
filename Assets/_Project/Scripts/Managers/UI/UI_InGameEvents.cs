using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Categorization;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class UI_InGameEvents : MonoBehaviour {
    private Label _itemName;
    private Label _itemMessage;
    private VisualElement _eventMessage;
    private IEnumerator _blinkRoutine;

    private void OnEnable() {
        Gun.OnPlayerCloseForPickUp += Gun_OnPlayerCloseForPickUp;
        Gun.OnPlayerMoveOutRange += Gun_OnPlayerMoveOutRange;
        GameManager.OnGamePaused += GameManager_OnGamePaused;
    }

    private void OnDisable() {
        Gun.OnPlayerCloseForPickUp -= Gun_OnPlayerCloseForPickUp;
        Gun.OnPlayerMoveOutRange -= Gun_OnPlayerMoveOutRange;
        GameManager.OnGamePaused -= GameManager_OnGamePaused;
    }

    private void Start() {
        SetElements();
    }


    private void SetElements(){
        _itemName = GameManager.Instance.UIManager.Root.Q<Label>("ItemName");
        _itemMessage = GameManager.Instance.UIManager.Root.Q<Label>("ItemMessage");
        _eventMessage = GameManager.Instance.UIManager.Root.Q("EventMessage");
    }

    private IEnumerator BlinkText(){
        bool fadeOut = false;
        while(true){
            if(!fadeOut){
                yield return LerpValue(_itemMessage, 1f, 0f, 0.5f); // Fade out
                fadeOut = true;
            }else{
                yield return LerpValue(_itemMessage, 0f, 1f, 0.5f); // Fade in
                fadeOut = false;
            }
            yield return null;
        }
    }
    private IEnumerator LerpValue(VisualElement element, float start, float end, float duration) {
        float elapsedTime = 0f;
        while (elapsedTime < duration){
            float value = Mathf.Lerp(start, end, elapsedTime / duration);
            element.style.opacity = value;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        element.style.opacity = end;
        yield return new WaitForSeconds(0.5f);
    }

    private void Gun_OnPlayerCloseForPickUp(Gun gun){
        SetElements();
        
        if(_itemName == null || _itemMessage == null){return;}

        _itemName.text = gun.name;
        _itemMessage.text = "Press E to PickUp";
        UpdateEventMessageOpacity(true);
        
        if(_blinkRoutine == null){
            _blinkRoutine = BlinkText();
            StartCoroutine(_blinkRoutine);
        }
    }

    private void Gun_OnPlayerMoveOutRange(bool show){
        if(_blinkRoutine != null){
            StopCoroutine(_blinkRoutine);
            _blinkRoutine = null;
        }
        UpdateEventMessageOpacity(show);
    }

    private void UpdateEventMessageOpacity(bool show){
        if(show){
            _eventMessage.style.opacity = 1f;
        }else{
            _eventMessage.style.opacity = 0f;
        }
    }

    private void GameManager_OnGamePaused(bool paused){
        SetElements();
    }
}