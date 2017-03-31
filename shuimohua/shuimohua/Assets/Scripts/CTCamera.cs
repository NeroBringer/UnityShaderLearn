using UnityEngine;
using System.Collections;


public class CTCamera : MonoBehaviour {

    public float m_MoveSpeed = 0.05f;
    public float m_RotateSpeed = 0.1f;

    //public bool FlipY
    //{
    //    get
    //    {
    //        return m_bFlipY;
    //    }
    //    set
    //    {
    //        if (m_bFlipY != value)
    //        {
    //            m_RotateSpeed = m_bFlipY ? -m_RotateSpeed : m_RotateSpeed;
    //        }
    //    }
    //}

    //private bool m_bFlipY = true;
    //private bool m_bFlipX = true;
    private Camera m_camera;
    private Transform m_transform;

    //public Camera camera
    //{
    //    get {
    //        if (m_camera == null)
    //        {
    //            m_camera = this.GetComponent<Camera>();
    //        }

    //        return m_camera;
    //    }
    //}
        


	// Use this for initialization
	void Start () {
        m_transform = GetComponent<Transform>();
        //m_RotateSpeed = m_bFlipY ? -m_RotateSpeed : m_RotateSpeed;
    }
	
	// Update is called once per frame
	void Update () {
	    
       // if (Input.GetKeyDown(KeyCode.)
	}

    public void MoveForward()
    {
        m_transform.Translate(Vector3.forward * m_MoveSpeed);  
    }


    public void MoveBack()
    {
        m_transform.Translate(Vector3.back * m_MoveSpeed);
    }

    //鼠标移动的 x ， y偏移量
    public void SetRotate(Vector2 offset)
    {
        Vector3 eulerVec = m_transform.localEulerAngles;

        eulerVec.x = (eulerVec.x + offset.y * -m_RotateSpeed) % 360;
        eulerVec.y = (eulerVec.y + offset.x * m_RotateSpeed) % 360;
        eulerVec.z = 0;

        m_transform.localEulerAngles = eulerVec;
        
    }

    public void MoveLeft()
    {
        m_transform.Translate(Vector3.right * -m_MoveSpeed);
    }

    public void moveRight()
    {
        m_transform.Translate(Vector3.right * m_MoveSpeed);
    }

}

