using UnityEngine;

public class LobbyControllerAccessor : MonoBehaviour
{
    public void StartGame()
    {
        LobbyController.Instance.StartGame();
    }
}