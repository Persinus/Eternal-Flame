using UnityEngine;


namespace EternalFlame 
{
    public class ServerGame : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private ServerGameManager networkManager;
    [System.Obsolete("This method is obsolete. Use StartGame() in NetworkManager instead.")]
    void Start()
    {
       
        if (networkManager != null)
        {
            networkManager.StartGame();
            Debug.Log("Game started with NetworkManager.");
        }
        else
        {
            Debug.LogError("NetworkManager not found in the scene.");
        }
    }
}
    
}
