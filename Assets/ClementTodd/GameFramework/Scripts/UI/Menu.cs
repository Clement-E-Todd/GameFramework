using UnityEngine;

namespace ClementTodd.GUI
{
    public class Menu : MonoBehaviour
    {
        public MenuItem defaultItem;
        public MenuItem cancelItem;
        private MenuItem selectedItem;

        [System.Serializable]
        public class SelectionHighlight
        {
            public RectTransform highlight;
            public Animator animator;
            public float moveTime;
            public bool matchXPosition;
            public bool matchYPosition;
        }
        public SelectionHighlight selectionUI;
        private Vector2 selectionMoveVelocity;

        private bool _Confirmed = false;
        private bool Confirmed
        {
            get
            {
                return _Confirmed;
            }

            set
            {
                _Confirmed = value;
                selectionUI.animator.SetBool("Confirmed", value);
            }
        }

        private void Awake()
        {
            SetSelectionToDefault();
        }

        private void Update()
        {
            // Dynamically animate the option highlight towards the selected option
            Vector2 destination = GetSelectionDestination();
            selectionUI.highlight.anchoredPosition = Vector2.SmoothDamp(
                selectionUI.highlight.anchoredPosition,
                destination,
                ref selectionMoveVelocity,
                selectionUI.moveTime);
        }

        public void EnableSelection()
        {
            Confirmed = false;
        }

        public void OnConfirm()
        {
            if (!Confirmed)
            {
                Confirmed = true;
                selectedItem.onSelected?.Invoke();
            }
        }

        public void OnNavigate(Vector2 navigation)
        {
            if (!Confirmed)
            {
                if (Mathf.Abs(navigation.x) < Mathf.Abs(navigation.y))
                {
                    if (navigation.y > 0f && selectedItem.neighbours.up)
                    {
                        selectedItem = selectedItem.neighbours.up;
                    }
                    else if (navigation.y < 0f && selectedItem.neighbours.down)
                    {
                        selectedItem = selectedItem.neighbours.down;
                    }
                }
                else
                {
                    if (navigation.x > 0f && selectedItem.neighbours.right)
                    {
                        selectedItem = selectedItem.neighbours.right;
                    }
                    else if (navigation.x < 0f && selectedItem.neighbours.left)
                    {
                        selectedItem = selectedItem.neighbours.left;
                    }
                }
            }
        }

        public void SetSelectionToDefault(bool instant = true)
        {
            selectedItem = defaultItem;

            if (instant)
            {
                selectionUI.highlight.anchoredPosition = GetSelectionDestination();
            }
        }

        public Vector2 GetSelectionDestination()
        {
            return new Vector2(
                selectionUI.matchXPosition ? selectedItem.RectTransform.anchoredPosition.x : selectionUI.highlight.anchoredPosition.x,
                selectionUI.matchYPosition ? selectedItem.RectTransform.anchoredPosition.y : selectionUI.highlight.anchoredPosition.y);
        }
    }
}