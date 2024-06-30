using JetBrains.Annotations;
using UnityEngine;

public class Testing : MonoBehaviour{
    public void EventChecker(MonoBehaviour caller, string method){
        Debug.Log($"{caller.name} - {method}");
    }
}