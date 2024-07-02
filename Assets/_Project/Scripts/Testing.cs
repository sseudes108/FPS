using UnityEngine;

public class Testing : MonoBehaviour{
    public void EventChecker(MonoBehaviour caller, string method){
        Debug.Log($"{caller.name} - {method}");
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.G)) {
            var player = FindAnyObjectByType<Player>();
            player.GetComponent<Health>().TakeDamage(1);
        }
    }
}