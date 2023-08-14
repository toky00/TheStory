using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField]
    Camera camera;
    
    [SerializeField]
    Image reticle;

    [SerializeField]
    Text interactText;

    [SerializeField]
    float maxDistance;

    [SerializeField]
    Color defaultColor;

    [SerializeField]
    Color interactColor;

    // Update is called once per frame
    void Update()
    {
        reticle.color = defaultColor;
        interactText.enabled = false;
        
        // Ensure a camera has been applied to raycast from.
        if(camera is not null)
        {
            // Raycast from the center of the camera, outwards to a maximum interaction distance.
            Ray ray = camera.ViewportPointToRay(Vector3.one * .5f);
            
            // DEBUG: Draw a ray in the scene to view interaction ray.
            Debug.DrawRay(ray.origin, ray.direction * maxDistance, Color.red);
            
            // Raycast, only checking for objects on the interact layer.
            if (Physics.Raycast(ray, out RaycastHit raycast, maxDistance, LayerMask.GetMask("Interact")))
            {
                // Get the interact component to learn more about what was found.
                Interact interactable = raycast.transform.gameObject.GetComponent<Interact>();
                if (interactable is not null)
                {
                    // Update the ui.
                    reticle.color = interactColor;
                    interactText.color = interactColor;
                    interactText.enabled = true;
                    interactText.text = interactable.Properties.DisplayText;
                    
                    // Check for activation.
                    if(Input.GetMouseButtonDown(0))
                        interactable.Activate();
                }
            }
        }
    }
}
