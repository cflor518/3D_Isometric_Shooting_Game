﻿using System.Collections;
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
	public bool ignoreMovement;
	public bool commenceProjectile;
	public float speedDelayToRunAfterShoot = .9f;
	public float speedDelayToRun = .7f;
	public float timeToSwitchFromShootingToIdle = 1f;
	public float timeToSwitchFromRunToShooting = .8f;
	public float timeToSwitchFromIdleToShooting = .2f;
	public float timeUntilShootCommencement;
	public GameObject Bullet;
	private GameObject BulletSpawnObject;
	private Transform BulletSpawn;
	public float bullet_power = 50;
	public float lastShot;
	public float delayBetweenShots = .4f;

	
	GameObject otherObject;
	Animator animator;

	/*
	 * Code to make soldier have a rifle
			GameObject soldier = GameObject.FindWithTag("Soldier");
			PlayerController playerScript = soldier.GetComponent<PlayerController>();
			playerScript.SetArsenal("AK-74M");*/

	private void Awake()
	{
		SoldierS = this; //Set the Soldier Singleton
		ignoreMovement = false;
		commenceProjectile = false;
		animator = this.GetComponent<Animator>();
		timeUntilShootCommencement = timeToSwitchFromRunToShooting;

}


	void Update () {
		//Debug.Log("timeToSwitch is : " + timeUntilShootCommencement);
		//if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Aiming"))
	//	{
	//		Debug.Log("An animation is playing");
	//	}




		/******************************************************************/
		/***********************Shooting mechanisms************************/

		//If you pressed the button to shoot
		if (Input.GetKey(KeyCode.Mouse0))
		{
			//Change the speed to walking speed.
			ChangeToStopSpeedToWalk();
			//ignore all movement
			ignoreMovement = true;
			//Start the shooting animation
			Actions actions = GetComponent<Actions>();
			actions.Attack();
			//Read coroutine method
			StartCoroutine(WaitForShootingAnimationToStartBeforeShootingBullets());

			//If the animation playing is at idle, its takes less time for the
			//character to start shooting, so set the time it takes bullets
			//to start shooting appropriately
			if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				timeUntilShootCommencement = timeToSwitchFromIdleToShooting;

			}else{ //Then he probably was running, so change the shooting commencment to wait longer
				timeUntilShootCommencement = timeToSwitchFromRunToShooting;
			}

			//TODO: clean the logic of these if statements.
			//If its too soon to shoot
			if (Time.time - lastShot < delayBetweenShots)
			{
				//then dont shoot and return
				return;
			}
			else
			{//if its time to shoot the
				//first check if im not moving and check if soldier has achieved a position to shoot.
				if (ignoreMovement == true && commenceProjectile == true)
				{
					//Find where the outlet of the gun is
					BulletSpawnObject = GameObject.FindGameObjectWithTag("BulletSpawner");
					//Give me the transform of that position.
					BulletSpawn = BulletSpawnObject.transform;
					//Instantiate a buller clone
					var aBullet = Instantiate(Bullet, BulletSpawn.transform.position, Quaternion.identity);
					//get the rigibody of that bullet
					Rigidbody BulletRigidbody = aBullet.GetComponent<Rigidbody>();
					//shoot the bullet in the direction that the soldier is facing.
					BulletRigidbody.velocity = this.transform.rotation * Vector3.forward * bullet_power;
					//after a second, destroy that buller
					Destroy(aBullet.gameObject, 1f);
					//keep track of when a bullet was shot, to make sure a bullet doesnt shoot again too early.
					lastShot = Time.time;
				}
			}
			




		}
		//If player has let go of the shoot button.
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			//Make sure the shooting animation has stopped to start running
			StartCoroutine(ContinueRunningAfterShooting());
			//Let no more bullets fly.
			commenceProjectile = false;

			//TODO: Need to figure out how to give off last shot
			//after shoot button released because animation shoots
			//one more time after releasing the shoot button.

		}


		//If the mouse key is down, make sure there is no movement, only rotation.
		if (!Input.GetKey(KeyCode.Mouse0) && ignoreMovement == false)
		{

			/**************************Actual movement of Soldier in World************/
			//Pull information from the input class
			float xAxis = Input.GetAxis("Horizontal");
			float zAxis = Input.GetAxis("Vertical");
			Vector3 pos = transform.position;

			pos.x += xAxis * speed * Time.deltaTime;
			pos.z += zAxis * speed * Time.deltaTime;
			transform.position = pos;
		}


			/******************************************************************/
			/*******Make the soldier start to turn in certain direction*********/

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
			/* BUTTONS LET GO*/
			/******Check if single button has been let go and change speed********/
			if (Input.GetKeyUp(KeyCode.A))
			{
				//If the other buttons arent currently pressed to run, then stop the running animation
				if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
				{
					//Stop the runnning animation
					Actions idleactions = GetComponent<Actions>();
					idleactions.Stay();
					ChangeToStopSpeedToWalk();
				}
			}
			else if (Input.GetKeyUp(KeyCode.D))
			{
				//If the other buttons arent currently pressed to run, then stop the running animation
				if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
				{
					//Stop the runnning animation
					Actions idleactions = GetComponent<Actions>();
					idleactions.Stay();
					ChangeToStopSpeedToWalk();
				}
			}
			else if (Input.GetKeyUp(KeyCode.S))
			{
				//If the other buttons arent currently pressed to run, then stop the running animation
				if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D))
				{
					//Stop the runnning animation
					Actions idleactions = GetComponent<Actions>();
					idleactions.Stay();
					ChangeToStopSpeedToWalk();
				}
			}
			else if (Input.GetKeyUp(KeyCode.W))
			{
				//If the other buttons arent currently pressed to run, then stop the running animation
				if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
				{
					//Stop the runnning animation
					Actions idleactions = GetComponent<Actions>();
					idleactions.Stay();
					ChangeToStopSpeedToWalk();
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
					ChangeToStopSpeedToWalk();
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
					ChangeToStopSpeedToWalk();
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
					ChangeToStopSpeedToWalk();
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
					ChangeToStopSpeedToWalk();
				}
			}
			/**************************************************************************/
		
	}//Update



	//If i just let go of the shoot button, we want to wait 
	IEnumerator ContinueRunningAfterShooting()
	{
			//Change to the stay animation
			Actions idleactions = GetComponent<Actions>();
			idleactions.Stay();
			//Wait for the transition from fire to idle
			yield return new WaitForSeconds(timeToSwitchFromShootingToIdle);
			//enable being able to run again.
			ignoreMovement = false;
			/**************************************************************************/
			/*Keys can still be pressed after shooting have to reinitalize the running
			in whatever direction the keys are being pressed
			The difference between these the ones above is that we arent listening 
			for key up or down. we are just listening if keys are already pressed
			changed getkeydown to getkey*/
			//UP/RIGHT
			if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
			{
				StartCoroutine(DelaySpeedAtBeginningOfRunAfterShoot());
				transform.rotation = Quaternion.Euler(0, soldierRightForward, 0);
				Actions actions = GetComponent<Actions>();
				actions.Run();
			}
			//UP/LEFT
			else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
			{
				StartCoroutine(DelaySpeedAtBeginningOfRunAfterShoot());
				transform.rotation = Quaternion.Euler(0, soldierLeftForward, 0);
				Actions actions = GetComponent<Actions>();
				actions.Run();

			}
			//DOWN/RIGHT
			else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
			{
				StartCoroutine(DelaySpeedAtBeginningOfRunAfterShoot());
				transform.rotation = Quaternion.Euler(0, soldierRightBack, 0);
				Actions actions = GetComponent<Actions>();
				actions.Run();
			}
			//DOWN/LEFT
			else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
			{
				StartCoroutine(DelaySpeedAtBeginningOfRunAfterShoot());
				transform.rotation = Quaternion.Euler(0, soldierLeftBack, 0);
				Actions actions = GetComponent<Actions>();
				actions.Run();

			}
			//LEFT
			else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
			{
				StartCoroutine(DelaySpeedAtBeginningOfRunAfterShoot());
				transform.rotation = Quaternion.Euler(0, soldierLeft, 0);
				Actions actions = GetComponent<Actions>();
				actions.Run();
			}
			//RIGHT
			else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
			{
				StartCoroutine(DelaySpeedAtBeginningOfRunAfterShoot());
				transform.rotation = Quaternion.Euler(0, soldierRight, 0);
				Actions actions = GetComponent<Actions>();
				actions.Run();
			}
			//DOWN
			else if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
			{
				StartCoroutine(DelaySpeedAtBeginningOfRunAfterShoot());
				transform.rotation = Quaternion.Euler(0, soldierBackwards, 0);
				Actions actions = GetComponent<Actions>();
				actions.Run();
			}
			//UP
			else if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
			{
				StartCoroutine(DelaySpeedAtBeginningOfRunAfterShoot());
				transform.rotation = Quaternion.Euler(0, soldierForward, 0);
				Actions actions = GetComponent<Actions>();
				actions.Run();
			}
			/**************************************************************************/

		
	}//End ContinueAfterShooting
	IEnumerator DelaySpeedAtBeginningOfRun()
	{
		yield return new WaitForSeconds(speedDelayToRun);
		speed = 3;
	}
	//We need to wait a little longer before we upgrade the speed
	//because the shoot animation takes a little longer to finish
	IEnumerator WaitForShootingAnimationToStartBeforeShootingBullets()
	{

		yield return new WaitForSeconds(timeUntilShootCommencement);
		if (Input.GetKey(KeyCode.Mouse0))
		{
			commenceProjectile = true;
		}
	}
	//We need to wait a little longer before we upgrade the speed
	//because the shoot animation takes a little longer to finish
	IEnumerator DelaySpeedAtBeginningOfRunAfterShoot()
	{
		yield return new WaitForSeconds(speedDelayToRunAfterShoot);
		speed = 3;
	}

	void ChangeToStopSpeedToWalk()
	{
		speed = 1;
	}

}