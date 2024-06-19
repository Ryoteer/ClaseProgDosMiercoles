using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveLoading : MonoBehaviour
{
    [SerializeField] string _sceneToLoad;
    [SerializeField] Animation animationClip;  

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out Player player))
        {
            StartCoroutine(LoadSceneAdditive(_sceneToLoad));
        }  
    }

    public IEnumerator LoadSceneAdditive(string sceneToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        
        while(!operation.isDone) 
        {
            yield return null;        
        }
        operation.completed += OpenDoor;
    }

    public void OpenDoor(AsyncOperation op)
    {           
        animationClip.Play();       
    }

    





}
