using System.Collections.Generic;
using UnityEngine;

namespace ClementTodd_v0_0_1
{
    public class InteractAbility : Ability
    {
        // Interactables
        private List<InteractionZone> interactablesInRange = new List<InteractionZone>();
        private InteractionZone targetInteraction = null;

        // Input
        private bool interactHeld = false;


        public GameObject arrowUIPrefab;
        private GameObject arrowUI;

        private void Awake()
        {
            if (arrowUIPrefab)
            {
                arrowUI = Instantiate(arrowUIPrefab);
                arrowUI.SetActive(false);
            }
        }

        private void FixedUpdate()
        {
            // Scan any interation zones in range to see which one can be used, if any.
            InteractionZone previousTarget = targetInteraction;
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

            // Notify the target if it has just started being targeted
            if (targetInteraction != previousTarget && tag == "Player")
            {
                if (targetInteraction)
                {
                    targetInteraction.OnTargetedByPlayer(this);
                }
                else if (HUD.instance)
                {
                    HUD.instance.HideInteractionPrompt();
                }
            }

            // If this charater has an interaction UI arrow, update it to point out the target
            if (!arrowUI && arrowUIPrefab)
            {
                arrowUI = Instantiate(arrowUIPrefab);
            }

            if (arrowUI)
            {
                if (targetInteraction && targetInteraction.uiTargetTransform)
                {
                    arrowUI.SetActive(true);
                    arrowUI.transform.SetParent(targetInteraction.uiTargetTransform);
                    arrowUI.transform.localPosition = Vector3.zero;
                    arrowUI.transform.localScale = Vector3.one;
                }
                else
                {
                    arrowUI.SetActive(false);
                    arrowUI.transform.SetParent(null);
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