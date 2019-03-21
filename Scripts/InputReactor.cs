using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InputReactor : MonoBehaviour
{
    [System.Serializable]
    public class InputEventControl
    {
        [SerializeField] protected string button = string.Empty;
        [SerializeField] protected KeyCode keyCode = KeyCode.None;
        [SerializeField] protected UnityEvent reactions;

        public string Button { get { return button; } set { button = value; } }
        public KeyCode KeyCode { get { return keyCode; } set { keyCode = value; } }
        public UnityEvent Reactions { get { return reactions; } set { reactions = value; } }
    }

    [SerializeField] protected InputEventControl[] inputEvents;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var control in inputEvents)
        {
            if (control.Button != string.Empty && Input.GetButtonDown(control.Button))
            {
                control.Reactions.Invoke();
            }
            else if (control.KeyCode != KeyCode.None && Input.GetKeyDown(control.KeyCode))
            {
                control.Reactions.Invoke();
            }
        }
    }
}