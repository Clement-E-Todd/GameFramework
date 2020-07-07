using System.Collections.Generic;
using UnityEngine;

namespace ClementTodd_v0_0_1
{
    public class InteractAbility : Ability
    {
        private List<InteractionZone> interactablesInRange = new List<InteractionZone>();

        private bool interactHeld = false;

        private InteractionZone targetInteraction = null;

        private void FixedUpdate()
        {
            // Scan any interation zones in range to see which one can be used, if any.
            targetInteraction = null;

            if (interactablesInRange.Count == 1)
            {
                if (interactablesInRange[0].CanInteractionBeCalled(transform))
                {
                    targetInteraction = interactablesInRange[0];
                }
            }
            else if (interactablesInRange.Count > 1)
            {
                // Score each interactable in range based on how directly the player is facing it.
                // Note that lowest score wins (because score is based primarily on distance)
                float bestScore = 0f;

                for (int i = 0; i < interactablesInRange.Count; i++)
                {
                    InteractionZone interactable = interactablesInRange[i];
                    if (!interactable.CanInteractionBeCalled(transform))
                    {
                        continue;
                    }

                    float score = Vector3.Distance(transform.position, interactablesInRange[i].sourceTransform.position);

                    if (interactable.sourceTransform)
                    {
                        Vector3 directionToSource = (interactable.sourceTransform.position - transform.position).normalized;
                        float dot = Vector3.Dot(transform.forward, directionToSource);
                        score *= 1f - dot;
                    }

                    if (score < bestScore || targetInteraction == null)
                    {
                        bestScore = score;
                        targetInteraction = interactable;
                    }
                }
            }

            // When the character interacts, trigger the target interaction
            if (targetInteraction && behaviourData.interact && !interactHeld)
            {
                targetInteraction.DoInteraction(this);
            }
            interactHeld = behaviourData.interact;
        }

        private void OnTriggerEnter(Collider other)
        {
            InteractionZone interactable = other.GetComponent<InteractionZone>();
            if (interactable && !interactablesInRange.Contains(interactable))
            {
                interactablesInRange.Add(interactable);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            InteractionZone interactable = other.GetComponent<InteractionZone>();
            if (interactable)
            {
                interactablesInRange.Remove(interactable);
            }
        }
    }
}