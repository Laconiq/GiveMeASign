using UnityEngine;

public class Ladder : Interactable
{
    private PlayerController _playerController;
    
    private void Start()
    {
        _playerController = GameManager.Instance.playerController;
    }
    
    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        _playerController.SetIsOnLadder(!_playerController.GetIsOnLadder());
    }
    
    private void OnTriggerExit(Collider other)
    {
        _playerController.SetIsOnLadder(false);
    }
}
