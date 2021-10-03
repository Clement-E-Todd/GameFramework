using UnityEngine;
using System;

namespace ClementTodd.EventGraphs
{
    public class EventGraphManager : MonoBehaviour
    {
        private static EventGraphManager _Instance;
        public static EventGraphManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = FindObjectOfType<EventGraphManager>();
                }
                return _Instance;
            }
        }

        public EventGraph CurrentEvent { get; private set; }
        public GameObject[] ObjectReferences { get; private set; }

        public Action OnEventStarted;
        public Action OnEventEnded;

        public Properties globalProperties = new Properties();

        public void StartEvent(EventGraph eventGraph, GameObject[] objectReferences)
        {
            if (CurrentEvent != null)
            {
                Debug.LogError("Can't start event; an event graph is already being executed.");
                return;
            }

            if (!eventGraph.startNode)
            {
                Debug.LogError("Can't start event: no start node was set.");
                return;
            }

            if (eventGraph.startNode.graph != eventGraph)
            {
                Debug.LogError("Can't start event: start node does not belong to this event graph.");
                return;
            }

            CurrentEvent = eventGraph;
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