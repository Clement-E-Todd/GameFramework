using ClementTodd.DataManagement;
using UnityEngine;
using System;

namespace ClementTodd.NodeEvents
{
    public class NodeEventManager : MonoBehaviour
    {
        private static NodeEventManager _Instance;
        public static NodeEventManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<NodeEventManager>();
                }
                return _Instance;
            }
        }

        public NodeEvent CurrentEvent { get; private set; }
        public GameObject[] ObjectReferences { get; private set; }

        public Action OnEventStarted;
        public Action OnEventEnded;

        public Properties globalProperties = new Properties();

        public void StartEvent(NodeEvent nodeEvent, GameObject[] objectReferences)
        {
            if (CurrentEvent != null)
            {
                Debug.LogError("Can't start event; an event graph is already being executed.");
                return;
            }

            if (!nodeEvent.startNode)
            {
                Debug.LogError("Can't start event: no start node was set.");
                return;
            }

            if (nodeEvent.startNode.graph != nodeEvent)
            {
                Debug.LogError("Can't start event: start node does not belong to this event graph.");
                return;
            }

            CurrentEvent = nodeEvent;
            ObjectReferences = objectReferences;

            OnEventStarted?.Invoke();

            CurrentEvent.ExecuteStartNode();
        }

        public void EndEvent()
        {
            if (CurrentEvent != null)
            {
                OnEventEnded?.Invoke();

                CurrentEvent = null;
            }
        }

        
    }
}