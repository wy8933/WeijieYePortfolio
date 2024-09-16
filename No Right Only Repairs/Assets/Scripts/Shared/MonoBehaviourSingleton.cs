using UnityEngine;

public class MonoBehaviourSingleton<T> : MonoBehaviour
    where T : Component {
    public static T Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this as T;
            //DontDestroyOnLoad(gameObject); // Keep the instance across scenes
        } else {
            Destroy(gameObject);
        }
    }
}