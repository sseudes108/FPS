using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_InGameEvents : MonoBehaviour {
    [field:SerializeField] public GunManagerSO GunManager { get; private set;}
    [field:SerializeField] public GameManagerSO GameManager { get; private set;}

    private Label _itemName;
    private Label _itemMessage;
    private VisualElement _eventMessage;
    private IEnumerator _blinkRoutine;

    private void OnEnable() {
        GunManager.OnPlayerClosePickUp.AddListener(Gun_OnPlayerCloseForPickUp);
        GunManager.OnPlayerMoveOutRange.AddListener(Gun_OnPlayerMoveOutRange);
        GameManager.OnGamePaused.AddListener(GameManager_OnGamePaused);
    }

    private void OnDisable() {
        GunManager.OnPlayerClosePickUp.RemoveListener(Gun_OnPlayerCloseForPickUp);
        GunManager.OnPlayerMoveOutRange.RemoveListener(Gun_OnPlayerMoveOutRange);
        GameManager.OnGamePaused.RemoveListener(GameManager_OnGamePaused);
    }

    private void Start() {
        SetElements();
    }

    private void SetElements(){
        _itemName = GameController.Instance.UIManager.Root.Q<Label>("ItemName");
        _itemMessage = GameController.Instance.UIManager.Root.Q<Label>("ItemMessage");
        _eventMessage = GameController.Instance.UIManager.Root.Q("EventMessage");
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
            if(_eventMessage != null){
                _eventMessage.style.opacity = 1f;
            }
        }else{
            if(_eventMessage != null){
                _eventMessage.style.opacity = 0f;
            }
        }
    }

    private void GameManager_OnGamePaused(bool paused){
        SetElements();
    }
}