using System.Collections.Generic;
using UnityEngine;

public class PlayerTalkToNpc : MonoBehaviour
{
    private List<Npc> _npcs;
    
    private void Start()
    {
        _npcs = new List<Npc>();
    }

    private void Update()
    {
        if (_npcs.Count > 1)
            ManageNpcCanvas();
    }

    public void TryTalkingToNpc()
    {
        var nearestNpc = GetClosestNpc();
        if (nearestNpc != null)
            nearestNpc.OnPlayerInteract();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("NPC") || _npcs.Contains(other.GetComponent<Npc>())) 
            return;
        _npcs.Add(other.GetComponent<Npc>());
        ManageNpcCanvas();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("NPC") || !_npcs.Contains(other.GetComponent<Npc>())) 
            return;
        other.GetComponent<Npc>().DisplayNpcCanvas(false);
        _npcs.Remove(other.GetComponent<Npc>());
    }
    
    private Npc GetClosestNpc()
    {
        Npc closestNpc = null;
        var closestDistance = Mathf.Infinity;
        foreach (var npc in _npcs)
        {
            var distance = Vector3.Distance(transform.position, npc.transform.position);
            if (!(distance < closestDistance)) 
                continue;
            closestDistance = distance;
            closestNpc = npc;
        }
        return closestNpc;
    }

    private void ManageNpcCanvas()
    {
        foreach (var npc in _npcs)
            npc.DisplayNpcCanvas(false);
        var closestNpc = GetClosestNpc();
        if (closestNpc is not null)
            closestNpc.DisplayNpcCanvas(true);
    }
}
