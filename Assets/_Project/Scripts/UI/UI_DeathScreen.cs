using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_DeathScreen : MonoBehaviour {
    private VisualElement _overLay;
    private VisualElement _youDied;

    private void Start() {
        SetElements();
        _overLay.style.opacity = 1;
        _youDied.style.opacity = 0;

        FadeOverlayFromBlack();
    }

    private void SetElements(){
        _overLay = GameManager.Instance.UIManager.Root.Q("Overlay");
        _youDied = GameManager.Instance.UIManager.Root.Q("YouDied");
    }

    private void FadeOverlayFromBlack(){
        StartCoroutine(ElementOpacityRoutine(_overLay, 1f, 0f, 2f));
    }

    private void FadeOverlayToBlack(){
        StartCoroutine(ElementOpacityRoutine(_overLay, 0f, 1f, 1f));
    }

    private void ShowYouDeadSign(){
        StartCoroutine(ElementOpacityRoutine(_youDied, 0f, 1f, 1f));
    }

    public void PlayerDied(){
        _overLay.style.display = DisplayStyle.Flex;
        StartCoroutine(DeathRoutine());
    }

    public IEnumerator DeathRoutine(){
        FadeOverlayToBlack();
        yield return new WaitForSeconds(1f);
        ShowYouDeadSign();
        yield return null;
    }

    private IEnumerator ElementOpacityRoutine(VisualElement element, float start, float end, float duration){
        float time = 0f;
        float currentOpacity;
        do{
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            currentOpacity  = Mathf.Lerp(start, end, t);
            element.style.opacity = currentOpacity;
            yield return null;
        }while(currentOpacity > 0);
        yield return null;
    }

    public void DisableOverlay(){
        _overLay.style.display = DisplayStyle.None;
    }
}