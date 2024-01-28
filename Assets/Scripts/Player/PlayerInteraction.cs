using System.Collections;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Transform holdPosition;
    private Transform _cameraTransform;
    private GameObject _heldObject;
    private Rigidbody _heldObjectRb;
    private HandleCursor _handleCursor;

    private void Awake()
    {
        if (Camera.main != null) _cameraTransform = Camera.main.transform;
        _handleCursor = FindObjectOfType<HandleCursor>();
    }

    private void Update()
    {
        CheckIfObjectIsGrabbable();
    }

    public void Interact(float force)
    {
        if (_heldObject != null)
        {
            _heldObjectRb.useGravity = true;
            _heldObjectRb.AddForce(_cameraTransform.forward * force, ForceMode.VelocityChange);
            _heldObject.GetComponent<GrabbableObject>().ObjectIsGrabbed(false);
            StopCoroutine(UpdateHoldPositionRoutine());
            _heldObject = null;
            _heldObjectRb = null;
        }
        else
        {
            if (!Physics.Raycast(_cameraTransform.position, _cameraTransform.forward, out RaycastHit hit, 2f)) 
                return;
            if (IsObjectGrabbable(hit))
            {
                _heldObject = hit.collider.gameObject;
                _heldObjectRb = _heldObject.GetComponent<Rigidbody>();
                _heldObjectRb.useGravity = false;
                _heldObject.GetComponent<GrabbableObject>().ObjectIsGrabbed(true);
                StartCoroutine(UpdateHoldPositionRoutine());
            }
            else if (IsObjectInteractable(hit))
                hit.collider.GetComponent<Interactable>().OnPlayerInteract();
        }
    }

    private void CheckIfObjectIsGrabbable()
    {
        if (_heldObject is not null)
        {
            _handleCursor.DisplayGrabCursor();
            _handleCursor.SetGrabCursorColor(new Color(1, 1, 1, 0.5f));
            return;
        }
        var ray = new Ray(_cameraTransform.position, _cameraTransform.forward);
        if (Physics.Raycast(ray, out var hit, 2f) && IsObjectGrabbable(hit))
        {
            _handleCursor.DisplayGrabCursor();
            _handleCursor.SetGrabCursorColor(new Color(1, 1, 1, 1f));
        }
        else if (Physics.Raycast(ray, out hit, 2f) && IsObjectInteractable(hit))
            _handleCursor.DisplayUseCursor();
        else
            _handleCursor.DisplayCrosshair();
    }
    
    private bool IsObjectGrabbable(RaycastHit hit)
    {
        var grabbableObject = hit.collider.GetComponent<GrabbableObject>();
        return hit.collider.CompareTag("Grabbable") && grabbableObject is not null && grabbableObject.isGrabbable;
    }
    
    private bool IsObjectInteractable(RaycastHit hit)
    {
        var interactableObject = hit.collider.GetComponent<Interactable>();
        if (interactableObject is null && hit.collider.CompareTag("Interactable"))
            Debug.Log("No interactable component found");
        return hit.collider.CompareTag("Interactable");
    }

    private IEnumerator UpdateHoldPositionRoutine()
    {
        while (_heldObject is not null && _heldObjectRb is not null)
        {
            Vector3 desiredVelocity = (holdPosition.position - _heldObject.transform.position) * 8f;
            _heldObjectRb.velocity = Vector3.Lerp(_heldObjectRb.velocity, desiredVelocity, 0.1f);
            yield return new WaitForFixedUpdate();
        }
    }
}
