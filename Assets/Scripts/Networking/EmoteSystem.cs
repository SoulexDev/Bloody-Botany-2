using FishNet.Object;
using System.Collections.Generic;
using UnityEngine;

public class EmoteSystem : NetworkBehaviour
{
    [SerializeField] private Rigidbody m_Rigidbody;
    [SerializeField] private List<Emote> m_Emotes;

    private void Update()
    {
        if (!IsOwner)
            return;

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    PlayEmoteServer(transform.position + m_Rigidbody.linearVelocity * Time.deltaTime, 0);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    PlayEmoteServer(transform.position + m_Rigidbody.linearVelocity * Time.deltaTime, 1);
        //}
    }
    [ServerRpc]
    private void PlayEmoteServer(Vector3 position, int emoteID)
    {
        PlayEmote(position, emoteID);
    }
    [ObserversRpc]
    private void PlayEmote(Vector3 position, int emoteID)
    {
        Emote e = m_Emotes[emoteID];
        Instantiate(e.emoteObject, position, Quaternion.identity);
    }
}