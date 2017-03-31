using UnityEngine;
using System.Collections;

public class GameInput : MonoBehaviour {

    public enum enInputKeys
    {
        w,
        s,
        a,
        d,
        mouseLeft,
        mouseRight,

        KeyCount,
    }

    Vector2 LastMouseDragStartPos = Vector2.zero;
    CTCamera m_ctCamera;

    public static bool[] m_buttonStates = new bool[(int)enInputKeys.KeyCount];

	// Use this for initialization
	void Start () {
        m_ctCamera = GameObject.FindObjectOfType<CTCamera>();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateInput();
        HandleInput();

            
	}

    private void  UpdateInput()
    {
        m_buttonStates[(int)enInputKeys.w] = Input.GetKey(KeyCode.W);
        m_buttonStates[(int)enInputKeys.s] = Input.GetKey(KeyCode.S);
        m_buttonStates[(int)enInputKeys.a] = Input.GetKey(KeyCode.A);
        m_buttonStates[(int)enInputKeys.d] = Input.GetKey(KeyCode.D);

        m_buttonStates[(int)enInputKeys.mouseLeft] = Input.GetMouseButton(0);
        m_buttonStates[(int)enInputKeys.mouseRight] = Input.GetMouseButton(1);
    }


    private void HandleInput()
    {
        if (m_buttonStates[(int)enInputKeys.mouseRight])
        {
            if (LastMouseDragStartPos != Vector2.zero)
            {
                Vector2 mouseDlt = (Vector2)Input.mousePosition - LastMouseDragStartPos;
                m_ctCamera.SetRotate(mouseDlt);
            }

            LastMouseDragStartPos = Input.mousePosition;
        }
        else if (LastMouseDragStartPos != Vector2.zero)
        {
            LastMouseDragStartPos = Vector2.zero;
        }


        if (m_buttonStates[(int)enInputKeys.w])
        {
            m_ctCamera.MoveForward();
        }

        if (m_buttonStates[(int)enInputKeys.s])
        {
            m_ctCamera.MoveBack();
        }

        if (m_buttonStates[(int)enInputKeys.a])
        {
            m_ctCamera.MoveLeft();
        }

        if (m_buttonStates[(int)enInputKeys.d])
        {
            m_ctCamera.moveRight();
        }
    }
}
