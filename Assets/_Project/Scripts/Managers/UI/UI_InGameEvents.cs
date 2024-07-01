using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_InGameEvents : MonoBehaviour {
    private Label _itemName;
    private Label _itemMessage;

    private void OnEnable() {
        Gun.OnPlayerCloseForPickUp += Gun_OnPlayerCloseForPickUp;
        GameManager.OnGamePaused += GameManager_OnGamePaused;
    }

    private void OnDisable() {
        Gun.OnPlayerCloseForPickUp -= Gun_OnPlayerCloseForPickUp;
        GameManager.OnGamePaused -= GameManager_OnGamePaused;
    }

    private void Start() {
        SetElements();

        _itemName.text = "";
        _itemMessage.text = "";
    }


    private void SetElements(){
        _itemName = GameManager.Instance.UIManager.Root.Q<Label>("ItemName");
        _itemMessage = GameManager.Instance.UIManager.Root.Q<Label>("ItemMessage");
    }

    private void Gun_OnPlayerCloseForPickUp(Gun gun){
        SetElements();
        
        if(_itemName == null || _itemMessage == null){return;}

        _itemName.text = gun.name;
        _itemMessage.text = "Press F to PickUp";
        BlinkText();
    }

    private void GameManager_OnGamePaused(bool paused){
        if(!paused){
            SetElements();
        }
    }
    
    private void BlinkText(){
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine(){
        do{
            float elapsedTime = 0;
            int end = 1;
            int start;
            if (end % 2 == 0){
                end = 1;
                start = 0;
            }else{
                start = 1;
                end = 0;
            }

            elapsedTime += Time.deltaTime;
            float interpolation = Mathf.Clamp01(elapsedTime / 1f);
            _itemMessage.style.opacity = Mathf.Lerp(start, end, interpolation);
            yield return new WaitForSeconds(interpolation);
            yield return null;
        } while (_itemMessage != null);
    }
}