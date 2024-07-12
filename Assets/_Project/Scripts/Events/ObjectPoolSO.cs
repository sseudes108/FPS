using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "ObjectPoolSO", menuName = "FPS/ObjectPoolSO", order = 0)]
public class ObjectPoolSO : ScriptableObject {
    public Vector3 InstancePosition {get; private set;}

    public void SetPosition(Vector3 position){
        InstancePosition = position;
    }

    public ObjectPool<T> CreatePool<T>(T prefab, Vector3 position) where T : MonoBehaviour{
        SetPosition(position);

        var objectPool = new ObjectPool<T>(()=>{
            return Instantiate(prefab, InstancePosition, Quaternion.identity);
        }, newObject =>{
            if(newObject != null){
                newObject.transform.position = InstancePosition;
                newObject.gameObject.SetActive(true);
            }
        }, newObject =>{
            newObject.gameObject.SetActive(false);
        }, newObject =>{
            Destroy(newObject);
        }, false, 50, 70);

        return objectPool;
    }

    public ObjectPool<AudioSource> CreateAudioPool(AudioSource prefab){
        InstancePosition = Vector3.zero;

        var audioPool = new ObjectPool<AudioSource>(()=>{
            return Instantiate(prefab, InstancePosition, Quaternion.identity);
        }, newAudioObject =>{
            if(newAudioObject != null){
                newAudioObject.transform.position = InstancePosition;
                newAudioObject.gameObject.SetActive(true);
            }
        }, newAudioObject =>{
            newAudioObject.gameObject.SetActive(false);
        }, newAudioObject =>{
            Destroy(newAudioObject);
        }, false, 50, 70);

        return audioPool;
    }
}