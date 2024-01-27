public class DigicodeValidate : InteractableObject
{
    private Digicode _digicode;
    
    private void Awake()
    {
        _digicode = FindObjectOfType<Digicode>();
    }
    
    public override void OnPlayerInteract()
    {
        _digicode.CheckPassword();
        base.OnPlayerInteract();
    }
}
