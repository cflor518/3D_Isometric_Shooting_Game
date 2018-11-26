using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierScript : MonoBehaviour {

	static public SoldierScript SoldierS;
	public float speed = 1;
	

	public float soldierForward = 0;
	public float soldierRightForward = 45;
	public float soldierRight = 90;
	public float soldierRightBack = 135;
	public float soldierBackwards = 180;
	public float soldierLeftBack = 225;
	public float soldierLeft = 270;
	public float soldierLeftForward = 315;

	private void Awake()
	{
		SoldierS = this; //Set the Soldier Singleton
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("Speed is: " + speed);
		/**************************Pulled from SPACESHUMP************/
		//Pull information from the input class
		float xAxis = Input.GetAxis("Horizontal");
		float zAxis = Input.GetAxis("Vertical");
		//Change transform.position based on the axes
		Vector3 pos = transform.position;
		pos.x += xAxis * speed * Time.deltaTime;
		//instead of y I changed to z because soldier can only stay on ground.
		pos.z += zAxis * speed * Time.deltaTime;
		transform.position = pos;
		/**************************END code from SPACESHUMP************/

		/******************************************************************/
		/*******Make the soldier start to run in certain direction*********/

		//Make soldier run left but ignore if right key is already pressed!
		if (Input.GetKeyDown(KeyCode.A) && !Input.GetKey(KeyCode.D))
		{
			//Turn Left
			StartCoroutine(DelaySpeedAtBeginningOfRun());
			transform.rotation = Quaternion.Euler(0, soldierLeft, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//Make soldier run right but ignore if left key is already pressed!
		else if (Input.GetKeyDown(KeyCode.D) && !Input.GetKey(KeyCode.A))
		{
			//Turn right
			StartCoroutine(DelaySpeedAtBeginningOfRun());
			transform.rotation = Quaternion.Euler(0, soldierRight, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//Make soldier run back but ignore if forward key is already pressed!
		else if (Input.GetKeyDown(KeyCode.S) && !Input.GetKey(KeyCode.W))
		{
			//Turn back
			StartCoroutine(DelaySpeedAtBeginningOfRun());
			transform.rotation = Quaternion.Euler(0, soldierBackwards, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//Make soldier run forward but ignore if back key is already pressed!
		else if (Input.GetKeyDown(KeyCode.W) && !Input.GetKey(KeyCode.S))
		{
			//Turn forward
			StartCoroutine(DelaySpeedAtBeginningOfRun());
			transform.rotation = Quaternion.Euler(0, soldierForward, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		/*********************************************************************/
		/*************Press and release going CLOCKWISE MOVEMENT**************/

		//if i have UP already pressed and a press RIGHT, go diagonal up/right
		if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.D))
		{
			transform.rotation = Quaternion.Euler(0, soldierRightForward, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//If I let go of UP while a I have RIGHT pressed, GO right
		if (Input.GetKeyUp(KeyCode.W) && Input.GetKey(KeyCode.D))
		{
			transform.rotation = Quaternion.Euler(0, soldierRight, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//if i have RIGHT already pressed and a press DOWN, go diagonal down/right
		if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.S))
		{
			transform.rotation = Quaternion.Euler(0, soldierRightBack, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//If I let go of RIGHT while a I have DOWN pressed, GO back
		if (Input.GetKeyUp(KeyCode.D) && Input.GetKey(KeyCode.S))
		{
			transform.rotation = Quaternion.Euler(0, soldierBackwards, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//if i have DOWN already pressed and I press LEFT, go diagonal down/LEFT
		if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.A))
		{
			transform.rotation = Quaternion.Euler(0, soldierLeftBack, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//If I let go of DOWN while a I have LEFT pressed, GO LEFT
		if (Input.GetKeyUp(KeyCode.S) && Input.GetKey(KeyCode.A))
		{
			transform.rotation = Quaternion.Euler(0, soldierLeft, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//if i have LEFT already pressed and I press UP, go diagonal UP/LEFT
		if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.W))
		{
			transform.rotation = Quaternion.Euler(0, soldierLeftForward, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//If I let go of LEFT while a I have UP pressed, GO UP
		if (Input.GetKeyUp(KeyCode.A) && Input.GetKey(KeyCode.W))
		{
			transform.rotation = Quaternion.Euler(0, soldierForward, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		/*********************************************************************/
		/*********Press and release going COUNTERCLOCKWISE MOVEMENT***********/

		//if i have UP already pressed and a press LEFT, go diagonal UP/LEFT
		if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.A))
		{
			transform.rotation = Quaternion.Euler(0, soldierLeftForward, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//If I let go of UP while a I have LEFT pressed, GO LEFT
		if (Input.GetKeyUp(KeyCode.W) && Input.GetKey(KeyCode.A))
		{
			transform.rotation = Quaternion.Euler(0, soldierLeft, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}

		//if i have LEFT already pressed and a press DOWN, go diagonal DOWN/LEFT
		if (Input.GetKey(KeyCode.A) && Input.GetKeyDown(KeyCode.S))
		{
			transform.rotation = Quaternion.Euler(0, soldierLeftBack, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//If I let go of LEFT while a I have DOWN pressed, GO DOWN
		if (Input.GetKeyUp(KeyCode.A) && Input.GetKey(KeyCode.S))
		{
			transform.rotation = Quaternion.Euler(0, soldierBackwards, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}

		//if i have DOWN already pressed and a press RIGHT, go diagonal DOWN/RIGHT
		if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.D))
		{
			transform.rotation = Quaternion.Euler(0, soldierRightBack, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//If I let go of DOWN while a I have RIGHT pressed, GO RIGHT
		if (Input.GetKeyUp(KeyCode.S) && Input.GetKey(KeyCode.D))
		{
			transform.rotation = Quaternion.Euler(0, soldierRight, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//if i have RIGHT already pressed and a press UP, go diagonal UP/RIGHT
		if (Input.GetKey(KeyCode.D) && Input.GetKeyDown(KeyCode.W))
		{
			transform.rotation = Quaternion.Euler(0, soldierRightForward, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		//If I let go of RIGHT while a I have UP pressed, GO UP
		if (Input.GetKeyUp(KeyCode.D) && Input.GetKey(KeyCode.W))
		{
			transform.rotation = Quaternion.Euler(0, soldierForward, 0);
			Actions actions = GetComponent<Actions>();
			actions.Run();
		}
		/*********************************************************************/
		/******Check if single button has been let go and change speed********/
		if (Input.GetKeyUp(KeyCode.A))
		{
			//If the other buttons arent currently pressed to run, then stop the running animation
			if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
			{
				//Stop the runnning animation
				Actions idleactions = GetComponent<Actions>();
				idleactions.Stay();
				ChangeToStopSpeed();
			}
		}
		if (Input.GetKeyUp(KeyCode.D))
		{
			//If the other buttons arent currently pressed to run, then stop the running animation
			if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
			{
				//Stop the runnning animation
				Actions idleactions = GetComponent<Actions>();
				idleactions.Stay();
				ChangeToStopSpeed();
			}
		}
		if (Input.GetKeyUp(KeyCode.S))
		{
			//If the other buttons arent currently pressed to run, then stop the running animation
			if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D))
			{
				//Stop the runnning animation
				Actions idleactions = GetComponent<Actions>();
				idleactions.Stay();
				ChangeToStopSpeed();
			}
		}
		if (Input.GetKeyUp(KeyCode.W))
		{
			//If the other buttons arent currently pressed to run, then stop the running animation
			if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
			{
				//Stop the runnning animation
				Actions idleactions = GetComponent<Actions>();
				idleactions.Stay();
				ChangeToStopSpeed();
			}
		}
		/**************************************************************************/
		/****Check if dual button has been let go and change speed for diagonal****/
		//LEFT UP
		if (Input.GetKeyUp(KeyCode.A) && Input.GetKeyUp(KeyCode.W))
		{
			//If the other buttons arent currently pressed to run, then stop the running animation
			if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
			{
				transform.rotation = Quaternion.Euler(0, soldierLeftForward, 0);
				//Stop the runnning animation
				Actions idleactions = GetComponent<Actions>();
				idleactions.Stay();
				ChangeToStopSpeed();
			}
		}
		//LEFT DOWN
		if (Input.GetKeyUp(KeyCode.A) && Input.GetKeyUp(KeyCode.S))
		{
			//If the other buttons arent currently pressed to run, then stop the running animation
			if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D))
			{
				transform.rotation = Quaternion.Euler(0, soldierLeftBack, 0);
				//Stop the runnning animation
				Actions idleactions = GetComponent<Actions>();
				idleactions.Stay();
				ChangeToStopSpeed();
			}
		}
		//RIGHT DOWN
		if (Input.GetKeyUp(KeyCode.D) && Input.GetKeyUp(KeyCode.S))
		{
			//If the other buttons arent currently pressed to run, then stop the running animation
			if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A))
			{
				transform.rotation = Quaternion.Euler(0, soldierRightBack, 0);
				//Stop the runnning animation
				Actions idleactions = GetComponent<Actions>();
				idleactions.Stay();
				ChangeToStopSpeed();
			}
		}
		//RIGHT UP
		if (Input.GetKeyUp(KeyCode.D) && Input.GetKeyUp(KeyCode.W))
		{
			//If the other buttons arent currently pressed to run, then stop the running animation
			if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S))
			{
				transform.rotation = Quaternion.Euler(0, soldierRightForward, 0);
				//Stop the runnning animation
				Actions idleactions = GetComponent<Actions>();
				idleactions.Stay();
				ChangeToStopSpeed();
			}
		}
	}
	IEnumerator DelaySpeedAtBeginningOfRun()
	{ 
		yield return new WaitForSeconds(.7f);
		speed = 3;
	}

	void ChangeToStopSpeed()
	{
		speed = 1;
	}
}