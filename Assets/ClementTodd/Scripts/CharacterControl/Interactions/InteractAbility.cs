using System.Collections.Generic;
using UnityEngine;

namespace ClementTodd.CharacterControl
{
    public class InteractAbility : Ability
    {
        // Interactables
        private List<InteractionZone> interactablesInRange = new List<InteractionZone>();
        private InteractionZone targetInteraction = null;

        public GameObject arrowUIPrefab;
        private GameObject arrowUI;

        private bool enableInteractionUI = false;

        public void Interact()
        {
            if (targetInteraction)
            {
                if (InteractionGUI.instance && enableInteractionUI)
                {
					InteractionGUI.instance.OnPlayerInteractionTriggered();
                }

                targetInteraction.DoInteraction(this);
            }
        }

        private void Awake()
        {
            if (arrowUIPrefab)
            {
                arrowUI = Instantiate(arrowUIPrefab);
                arrowUI.SetActive(false);
            }
        }

        private void OnEnable()
        {
            character.OnControlStateEnter += OnStateEnter;
        }

        private void OnDisable()
        {
            character.OnControlStateEnter -= OnStateEnter;
        }

        private void FixedUpdate()
        {
            ScanForTargets();
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

        private void OnStateEnter(CharacterControlState state)
        {
            // Clear our target when transitioning between states
            targetInteraction = null;

            // Only enable HUD animations if the character is human-controlled
            enableInteractionUI = state is PlayerCharacterControlState;
        }

        private void ScanForTargets()
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

            // Show or hide the interaction prompt UI in the HUD when the player's target changes
            if (InteractionGUI.instance && enableInteractionUI && targetInteraction != previousTarget)
            {
                if (targetInteraction)
                {
                    string targetName = targetInteraction.GetTargetName();
                    string actionName = targetInteraction.GetActionName();
                    string preposition = targetInteraction.GetPreposition();

					InteractionGUI.instance.ShowInteractionPrompt(actionName, preposition + " " + targetName);
                }
                else if (InteractionGUI.instance)
                {
					InteractionGUI.instance.HideInteractionPrompt();
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
        }
    }
}