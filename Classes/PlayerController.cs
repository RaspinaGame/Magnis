using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public  float MovmentSpeed = 0.2f;
	public 	float Xmax= 7, Xmin = -7;
	public 	float Ymax = 1000, Ymin = -1000;
    public LineRenderer anchorLine;
    public GameObject fingerHead;
    public GameObject anchorCenter;
    public GameObject anchorHead;
    public Animator anim;
    public float scaleRate = 0.5f;

	private Vector3 DesiredPosition;
    private int scaleState;
    private Quaternion starttingRotation;
    private bool bIsInContact;
    private float lastScaleTime;

    /////////////////////////////// debug
    float maxSpeed;
    //float lastVelocityAdjustmentTime;
    //private string DTouchWorldPosition;
    //private string DFinalPosition;
    //Vector2 collisionSurfaceNormal;
    //Vector2 collisionpoint;
    //MoverComponent colliderMoverComponent;

	// Use this for initialization
	void Start () 
	{
        anchorCenter.transform.position = GetBasePosition();
        starttingRotation = transform.rotation;
	}

	Vector3 GetBasePosition()
	{
		//Camera MyCamera = FindObjectOfType<Camera> ();
		//return MyCamera.transform.position;
		//return transform.parent.position;
		return new Vector3(0,0,-2);
	}

	Vector3 GetInput()
	{
		if (Application.isEditor)
		{
			return Input.mousePosition ;
		}
		else if ( Input.touchCount > 0)
		{
			return Input.GetTouch(0).position ;
		}
		return new Vector3 (-1000, -1000, -1000);
	}

	void ProcessTouch(Vector3 InputPosition)
	{
		Vector3 FinalPosition, TouchWorldPosition, BasePosition;
		if ( InputPosition != new Vector3 (-1000, -1000, -1000) )
		{
            
			TouchWorldPosition = Camera.main.ScreenToWorldPoint(InputPosition);
            TouchWorldPosition.x = Mathf.Clamp(TouchWorldPosition.x, Xmin, Xmax);
            TouchWorldPosition.y = Mathf.Clamp(TouchWorldPosition.y, Ymin, Ymax);
            anchorLine.SetPosition(1, new Vector3(TouchWorldPosition.x, TouchWorldPosition.y, -1));
            fingerHead.transform.position = new Vector3(TouchWorldPosition.x, TouchWorldPosition.y, -1);
			BasePosition = GetBasePosition();
			TouchWorldPosition.z = -2;
			FinalPosition = BasePosition + BasePosition - TouchWorldPosition;
			DesiredPosition = FinalPosition;

			/// debug
			//		print (transform.parent.name + TouchWorldPosition  + BasePosition.ToString() + FinalPosition.ToString());
			//DTouchWorldPosition = TouchWorldPosition.ToString();
			//DFinalPosition = FinalPosition.ToString();
		}
	}

    // Update is called once per frame
	void Update ()
	{
		if ( !GameInfo.IsGamePaused() )
		{
			
			//print ( transform.position + " " + DesiredPosition + " " + MovmentSpeed );
            //rigidbody2D.position = Vector3.Lerp(rigidbody2D.position, DesiredPosition, 0.1f);
            ProcessTouch ( GetInput() );
            anchorLine.SetPosition(0, new Vector3(DesiredPosition.x, DesiredPosition.y, -1));
            anchorHead.transform.position = new Vector3(DesiredPosition.x, DesiredPosition.y, -1);
		}

		if (Input.GetKey (KeyCode.Escape)) 
			Application.LoadLevel (0);
	}

    void FixedUpdate()
    {
        if (!GameInfo.IsGamePaused())
        {
            MoveToDesiredPosition(DesiredPosition);
        }
 
    }

    void LateUpdate()
    {
        //if (!GameInfo.IsGamePaused())
        //{
            
        //    // if (transform.localScale == desiredScale)
        //   // transform.localScale = Vector3.Lerp(transform.localScale, desiredScale, 0.2f);
        //}
    }


    Vector3 velocity;
    Vector3 lastVelocity;
    void MoveToDesiredPosition(Vector2 inDesiredPosition ) 
    {
        RaycastHit2D traceinfo;

        velocity = rigidbody2D.velocity;
        Vector3.SmoothDamp(rigidbody2D.position, inDesiredPosition, ref velocity, 0.015f, 3000f, Time.fixedDeltaTime);
        
        if (bIsInContact)
        {

            traceinfo = Physics2D.Raycast(rigidbody2D.position, velocity, scaleState + 1f, LayerMask.GetMask("obstacle"));
            Debug.DrawRay(rigidbody2D.position, traceinfo.normal, Color.red);
            print(traceinfo.collider);
            if (traceinfo.collider != null)
            {
                Vector3 newVelocity;

             //   Debug.DrawRay(rigidbody2D.position, lastVelocity, Color.yellow);
                newVelocity = (velocity + lastVelocity) / 2f;
                lastVelocity = velocity;
                velocity = newVelocity;
                newVelocity = Vector3.Project(velocity, Vector2.right);
                
                if (newVelocity.magnitude < 600f)
                {
                   velocity.x = Mathf.Lerp(0f, velocity.x, Mathf.Pow(newVelocity.magnitude/600f, 2f));
                   velocity.y = Mathf.Lerp(0f, velocity.y, Mathf.Pow(newVelocity.magnitude/600f, 2f));
                }
            }
            //Debug.DrawRay(rigidbody2D.position, Vector2.right * 10f, Color.green);
         }
         
         rigidbody2D.velocity = velocity;
         Debug.DrawRay(rigidbody2D.position, velocity);
    }

	void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.name == "LevelSplitter") 
		{
			GameInfo.LevelFinished();
		}
        else
        {
            bIsInContact = true;
        //    CancelInvoke("Scale");
	    //	InvokeRepeating("Scale",Time.deltaTime,0.6f);
        }

		//foreach (ContactPoint2D contact in collision.contacts) 
		//{
		//	Debug.DrawRay(contact.point, contact.normal, Color.white);
		//}

	}

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.gameObject.name == "LevelSplitter")
        {
            //GameInfo.LevelFinished();
        }
        else
        {
            bIsInContact = true;
            if (Time.time - lastScaleTime >= scaleRate)
            {
                Scale();
                lastScaleTime = Time.time;
            }
        }

    }

    void Scale()
    {
        if (scaleState < 7)
            scaleState++;
        anim.SetInteger("scaleState", scaleState);
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        bIsInContact = false;
        //CancelInvoke("Scale");
        //MovmentSpeed = starttingMovmentSpeed;
        //transform.parent = null;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "BoundaryTirigger")
        {
            GameInfo.PauseGame();
            Invoke("GameOver", 1.5f);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "LevelSplitter")
        {
            GameInfo.LevelFinished();
        }
        else if (other.gameObject.name == "Obstacle")
        {
            GameInfo.PauseGame();
            Invoke("GameOver", 1.5f);
        }
        //else if (other.gameObject.name == "SceneSplitter")
        //{
        //    GameInfo.LevelFinished();
        //}
        

    }



	void GameOver()
	{
		GameInfo.GameOver();
        transform.rotation = starttingRotation;
       // anchorLine.SetPosition(0, GetBasePosition());
       // anchorLine.SetPosition(1, GetBasePosition());
       // anchorHead.transform.position = GetBasePosition();
        transform.position = DesiredPosition;
        rigidbody2D.velocity = Vector2.zero;
        scaleState = 0;
        anim.SetInteger("scaleState", scaleState);
	}

	// Debug Part

    void OnGUI()
    {
        GUI.color = Color.white;

        GUIStyle _style = GUI.skin.GetStyle("Label");
        _style.alignment = TextAnchor.UpperLeft;
        _style.fontSize = 20;

        if (rigidbody2D.velocity.magnitude > maxSpeed)
            maxSpeed = rigidbody2D.velocity.magnitude;
        GUI.Label(new Rect(20, 20, 200, 200), "maxSpeed : " + maxSpeed);
        //print ("DTouchWorldPosition" + DTouchWorldPosition);
        //_style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(20, 40, 200, 200), "Contact : " + bIsInContact);
        GUI.Label(new Rect(20, 60, 200, 200), "scaleState : " + scaleState);
        //print ("DFinalPosition" + DFinalPosition);
        //_style.alignment = TextAnchor.MiddleCenter;
        //GUI.Label (new Rect (Screen.width - 320, 20, 200, 200), "LB(1):" + Input.touches.GetLowerBound (1) + " UB(0):" + Input.touches.GetUpperBound (1), _style);
    }
}
