using System.Collections.Generic;
using UnityEngine;

public class TalkToNpc : MonoBehaviour
{
    private List<Npc> _npcs;
    
    private void Start()
    {
        _npcs = new List<Npc>();
        InvokeRepeating(nameof(ManageNpcCanvas), 0f, 0.2f);
    }
    
    public void TryTalkingToNpc()
    {
        Npc nearestNpc = GetClosestNpc();
        if (nearestNpc != null)
            nearestNpc.OnPlayerInteract();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("NPC") || _npcs.Contains(other.GetComponent<Npc>())) 
            return;
        _npcs.Add(other.GetComponent<Npc>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("NPC") || !_npcs.Contains(other.GetComponent<Npc>())) 
            return;
        _npcs.Remove(other.GetComponent<Npc>());
    }
    
    private Npc GetClosestNpc()
    {
        Npc closestNpc = null;
        float closestDistance = Mathf.Infinity;
        foreach (var npc in _npcs)
        {
            float distance = Vector3.Distance(transform.position, npc.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNpc = npc;
            }
        }
        return closestNpc;
    }

    private void ManageNpcCanvas()
    {
        if (_npcs.Count == 0)
            return;
        foreach (var npc in _npcs)
            npc.DisplayNpcCanvas(false);
        var closestNpc = GetClosestNpc();
        if (closestNpc is not null)
            closestNpc.DisplayNpcCanvas(true);
    }
}
