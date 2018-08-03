using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	public GameManager manager;

	public float invertForce = 100f;
	public float singleJumpForce = 200f;
	public float doubleJumpForce = 400f;
	public float gravityScaleSingleJump = 3f;
	public float gravityScaleDoubleJump = 1f;
	public float gravityScaleInvert = -10f;
	public float gravityScaleLittleHigh = 5f;
	public float gravityScaleHigh = 10f;

	Rigidbody2D rigidbody;
	private bool IsOnGround;
	private bool TurnOffGround;// Makes the IsOnGround false the next time it is made true
	private bool ResidualClick;
	private Vector3 velocityVector = Vector3.zero;
	private float velocity = 56f/6;
	private bool IsTouchingDeflater;
	private bool IsTouchingDeflaterCover;
	private DeflaterManager currentDeflater;
	private ClickIndicator currentIndicator;
	// Indicates whether rest should be awarded green or not
	private bool WasRestGood;

	enum InstructionState {
		quarter_note, half_note, eight_note_1, eight_note_2, rest, whole, none
	};

	private InstructionState current;

	void Start () {
		rigidbody = GetComponent<Rigidbody2D> ();
		current = InstructionState.none;
		rigidbody.velocity = new Vector2 (0, 0);
		IsTouchingDeflater = false;
		IsTouchingDeflaterCover = false;
		currentDeflater = null;
		currentIndicator = null;
		IsOnGround = false;
		ResidualClick = false;
		WasRestGood = true;
		Time.timeScale = 0;
		TurnOffGround = false;
	}

	void Update () {
		if (manager.GetGameState() == GameManager.GameState.PLAYING) {
			if (rigidbody.velocity.x > velocity || rigidbody.velocity.x < velocity) {
				velocityVector = rigidbody.velocity;
				velocityVector.x = velocity;
				rigidbody.velocity = velocityVector;
			}
				
			switch (current) {
			case InstructionState.quarter_note:
				//Left click down
				if (Input.GetMouseButtonDown (0) && currentIndicator != null) {
					float x = (rigidbody.position.x % 10);
					if (x < 0) {
						x += 10;
					}
					if (x >= 0 && x <= 2 && IsOnGround) {
						rigidbody.gravityScale = gravityScaleSingleJump;
						velocityVector.y = 0;
						rigidbody.velocity = velocityVector;
						rigidbody.AddForce (new Vector2 (0, singleJumpForce));
						currentIndicator.GetComponentInParent<Bumper> ().StartMoving ();
						IsOnGround = false;

						if (x < 0.3f) {
							manager.IndicateClick (currentIndicator, Color.green);
							manager.IncreaseScore (5);
						} else if (x < 0.8f) {
							manager.IndicateClick (currentIndicator, Color.yellow);
							manager.IncreaseScore (3);
						} else {
							manager.IndicateClick (currentIndicator, Color.red);
						}
					} else if (x > 5) {
						ResidualClick = true;
						if (x > 9.7f) {
							manager.IndicateClick (currentIndicator, Color.green);
							manager.IncreaseScore (5);
						} else if (x > 9.2f) {
							manager.IndicateClick (currentIndicator, Color.yellow);
							manager.IncreaseScore (3);
						} else {
							manager.IndicateClick (currentIndicator, Color.red);
						}
					}
				}

				if (ResidualClick && IsOnGround) {
					rigidbody.gravityScale = gravityScaleSingleJump;
					velocityVector.y = 0;
					rigidbody.velocity = velocityVector;
					rigidbody.AddForce (new Vector2 (0, singleJumpForce));
					currentIndicator.GetComponentInParent<Bumper> ().StartMoving ();

					ResidualClick = false;
					IsOnGround = false;
				}

				//Left click up
				if (Input.GetMouseButtonUp (0)) {
					if (IsTouchingDeflater && !IsTouchingDeflaterCover) {
						Vector2 targetPos = new Vector2 ();
						float x = rigidbody.position.x;
						targetPos.x = x - x % 10 + 9.9f;
						targetPos.y = manager.groundY;
						rigidbody.velocity = manager.GetReqVelocity (
							rigidbody.gravityScale * 9.81f,
							rigidbody.position,
							targetPos,
							velocity
						);
						currentDeflater.ChangeColor (Color.green);
						manager.IncreaseScore (5);
					}
					else {
						rigidbody.gravityScale = gravityScaleHigh;
						if (IsTouchingDeflaterCover) {
							currentDeflater.ChangeColor (Color.yellow);
							manager.IncreaseScore (3);
						}
					}
				}
				break;

			case InstructionState.half_note:
				//Left click down

				if (Input.GetMouseButtonDown (0) && currentIndicator != null) {
					float x = rigidbody.position.x % 10;
					if (x < 0) {
						x += 10;
					}
					if (x >= 0 && x <= 2 && IsOnGround) {
						rigidbody.gravityScale = gravityScaleDoubleJump;
						velocityVector.y = 0;
						rigidbody.velocity = velocityVector;
						rigidbody.AddForce (new Vector2 (0, doubleJumpForce));
						currentIndicator.GetComponentInParent<Bumper> ().StartMoving ();
						IsOnGround = false;

						if (x < 0.3f) {
							manager.IndicateClick (currentIndicator, Color.green);
							manager.IncreaseScore (10);
						} else if (x < 0.8f) {
							manager.IndicateClick (currentIndicator, Color.yellow);
							manager.IncreaseScore (6);
						} else {
							manager.IndicateClick (currentIndicator, Color.red);
						}
					} else if (x > 5) {
						ResidualClick = true;
						if (x > 9.7f) {
							manager.IndicateClick (currentIndicator, Color.green);
							manager.IncreaseScore (10);
						} else if (x > 9.2f) {
							manager.IndicateClick (currentIndicator, Color.yellow);
							manager.IncreaseScore (6);
						} else {
							manager.IndicateClick (currentIndicator, Color.red);
						}
						//print ("Residual click");
					}
				}

				if (ResidualClick && IsOnGround) {
					rigidbody.gravityScale = gravityScaleDoubleJump;
					velocityVector.y = 0;
					rigidbody.velocity = velocityVector;
					rigidbody.AddForce (new Vector2 (0, doubleJumpForce));
					currentIndicator.GetComponentInParent<Bumper> ().StartMoving ();

					ResidualClick = false;
					IsOnGround = false;
				}

				//Left click up
				if (Input.GetMouseButtonUp (0)) {
					if (IsTouchingDeflater && !IsTouchingDeflaterCover) {
						Vector2 targetPos = new Vector2 ();
						float x = rigidbody.position.x;
						targetPos.x = Mathf.Round(x / 10) * 10 + 9.7f;
						targetPos.y = manager.groundY;
						rigidbody.velocity = manager.GetReqVelocity (
							rigidbody.gravityScale * 9.81f,
							rigidbody.position,
							targetPos,
							velocity
						);
						currentDeflater.ChangeColor (Color.green);
						manager.IncreaseScore (10);
					}
					else {
						rigidbody.gravityScale = gravityScaleHigh;
						if (IsTouchingDeflater) {
							currentDeflater.ChangeColor (Color.yellow);
							manager.IncreaseScore (6);
						}
					}
				} 
				break;

			case InstructionState.eight_note_1:
				
				//			if (canClick && Input.GetMouseButtonDown (0)) {
				//				rigidbody.gravityScale = gravityScaleInvert;
				//				rigidbody.AddForce (new Vector2 (0, singleJumpForce));
				//				manager.setCommentText ();
				//			}

				//Left click
				if (Input.GetMouseButtonDown (0) && currentIndicator != null) {
					float x = (rigidbody.position.x % 10);
					if (x < 0) {
						x += 10;
					}
					if (x >= 0 && x <= 2 && IsOnGround) {
						rigidbody.gravityScale = gravityScaleSingleJump * 0.65f;

						Vector2 targetPos = new Vector2 ();
						float X = rigidbody.position.x;
						targetPos.x = Mathf.Round(X / 10) * 10 + 9.8f;
						targetPos.y = manager.groundY;
						rigidbody.velocity = manager.GetReqVelocity (
							rigidbody.gravityScale * 9.81f,
							rigidbody.position,
							targetPos,
							velocity
						);

						currentIndicator.GetComponentInParent<Bumper> ().StartMoving ();
						IsOnGround = false;

						if (x < 0.3f) {
							manager.IndicateClick (currentIndicator, Color.green);
							manager.IncreaseScore (5);
						} else if (x < 0.8f) {
							manager.IndicateClick (currentIndicator, Color.yellow);
							manager.IncreaseScore (3);
						} else {
							manager.IndicateClick (currentIndicator, Color.red);
						}
					} else if (x > 5) {
						ResidualClick = true;
						if (x > 9.7f) {
							manager.IndicateClick (currentIndicator, Color.green);
							manager.IncreaseScore (5);
						} else if (x > 9.2f) {
							manager.IndicateClick (currentIndicator, Color.yellow);
							manager.IncreaseScore (3);
						} else {
							manager.IndicateClick (currentIndicator, Color.red);
						}
					}
				}

				if (ResidualClick && IsOnGround) {
					rigidbody.gravityScale = gravityScaleSingleJump * 0.65f;

					Vector2 targetPos = new Vector2 ();
					float X = rigidbody.position.x;
					targetPos.x = Mathf.Round(X / 10) * 10 + 9.8f;
					targetPos.y = manager.groundY;
					rigidbody.velocity = manager.GetReqVelocity (
						rigidbody.gravityScale * 9.81f,
						rigidbody.position,
						targetPos,
						velocity
					);
					currentIndicator.GetComponentInParent<Bumper> ().StartMoving ();

					ResidualClick = false;
					IsOnGround = false;
				}
				break;

			case InstructionState.eight_note_2:
				
				if (Input.GetMouseButtonDown (0) && currentIndicator != null) {
					float x = (rigidbody.position.x % 10);
					if (x < 0) {
						x += 10;
					}
					if (x >= 4.2f && x <= 5.8f) {
						if (x < 5.3f && x > 4.7f) {
							manager.IndicateClick (currentIndicator, Color.green);
							manager.IncreaseScore (5);
						} else if (x < 5.8f && x > 4.2f) {
							manager.IndicateClick (currentIndicator, Color.yellow);
							manager.IncreaseScore (3);
							rigidbody.gravityScale = gravityScaleHigh;
							currentIndicator.transform.parent.parent.GetChild (1).GetChild (0).gameObject.SetActive (true);
						}
					} else if(x < 4.2f) {
						manager.IndicateClick (currentIndicator, Color.red);
						rigidbody.gravityScale = gravityScaleHigh;
						currentIndicator.transform.parent.parent.GetChild (1).GetChild (0).gameObject.SetActive (true);
					}
				}

				if((rigidbody.position.x % 10) > 5.8f && 
					currentIndicator != null && 
					currentIndicator.GetColor() == Color.white
				) {
					rigidbody.gravityScale = gravityScaleHigh;
					manager.IndicateClick(currentIndicator, Color.red);
					currentIndicator.transform.parent.parent.GetChild (1).GetChild (0).gameObject.SetActive (true);
					current = InstructionState.none;
				}

				break;

			case InstructionState.whole:
				//Left click down
				if (Input.GetMouseButtonDown (0) && currentIndicator != null) {
					float x = (rigidbody.position.x % 10);
					if (x < 0) {
						x += 10;
					}
					if (x >= 0 && x <= 2) {
						rigidbody.gravityScale = gravityScaleSingleJump;
						velocityVector.y = 0;
						rigidbody.velocity = velocityVector;
						rigidbody.AddForce (new Vector2 (0, singleJumpForce * 1.24f));
						currentIndicator.GetComponentInParent<Bumper> ().StartMoving ();
						IsOnGround = false;

						if (x < 0.3f) {
							manager.IndicateClick (currentIndicator, Color.green);
							manager.IncreaseScore (10);
						} else if (x < 0.8f) {
							manager.IndicateClick (currentIndicator, Color.yellow);
							manager.IncreaseScore (6);
						} else {
							manager.IndicateClick (currentIndicator, Color.red);
						}
					} else if (x > 5) {
						ResidualClick = true;
						if (x > 9.7f) {
							manager.IndicateClick (currentIndicator, Color.green);
							manager.IncreaseScore (10);
						} else if (x > 9.2f) {
							manager.IndicateClick (currentIndicator, Color.yellow);
							manager.IncreaseScore (6);
						} else {
							manager.IndicateClick (currentIndicator, Color.red);
						}
					}
				}

				if (ResidualClick && IsOnGround) {
					rigidbody.gravityScale = gravityScaleSingleJump;
					velocityVector.y = 0;
					rigidbody.velocity = velocityVector;
					rigidbody.AddForce (new Vector2 (0, singleJumpForce * 1.24f));
					currentIndicator.GetComponentInParent<Bumper> ().StartMoving ();

					ResidualClick = false;
					IsOnGround = false;
				}

				//Left click up
				if (Input.GetMouseButtonUp (0)) {
					if (!IsTouchingDeflaterCover && IsTouchingDeflater) {
						Vector2 targetPos = new Vector2 ();
						float x = rigidbody.position.x;
						targetPos.x = x - x % 10 + 9.7f;
						targetPos.y = manager.groundY;

						rigidbody.velocity = manager.GetReqVelocity (
							rigidbody.gravityScale * 9.81f,
							rigidbody.position,
							targetPos,
							velocity
						);
						currentDeflater.ChangeColor (Color.green);
						manager.IncreaseScore (5);
					} else {
						if (IsTouchingDeflaterCover) {
							currentDeflater.ChangeColor (Color.yellow);
							manager.IncreaseScore (3);
						}
					}
				}
				break;

			case InstructionState.rest:
				if (Input.GetMouseButtonDown (0) && currentIndicator != null) {
					WasRestGood = false;
					currentIndicator.ChangeColor (Color.red);
				}
				break;

			case InstructionState.none:

				break;
			}
		} else if(manager.GetGameState() == GameManager.GameState.FINISHED) {
			velocityVector = rigidbody.velocity;
			velocityVector.x = Mathf.Lerp (0, velocityVector.x, 59 * Time.deltaTime);
			rigidbody.velocity = velocityVector;
		}

	}

	public bool GetResidualClick() {
		return ResidualClick;
	}

	public bool IsInsideRedCircleInQuarterNote() {
		if (current == InstructionState.quarter_note) {
			if (!IsTouchingDeflaterCover && IsTouchingDeflater) {
				return true;
			}
		}
		return false;
	}

	public bool IsOnTriangle() {
		float x = (rigidbody.position.x % 10);
		if (x < 0) {
			x += 10;
		}
		if (x < 0.3f || x > 9.7f) {
			return true;
		}
		return false;
	}

	void OnTriggerEnter2D(Collider2D col) {

		if (col.name.Equals ("click detector")) {
			//Assign the click indicator
			currentIndicator = col.transform.parent.GetChild(0).GetChild(1).GetComponent<ClickIndicator>();
			if (col.tag.Equals ("quarter")) {
				current = InstructionState.quarter_note;
			} else if (col.tag.Equals ("half")) {
				current = InstructionState.half_note;
			} else if (col.tag.Equals ("eight note 1")) {
				current = InstructionState.eight_note_1;
			} else if (col.tag.Equals ("eight note 2")) {
				current = InstructionState.eight_note_2;
			} else if (col.tag.Equals ("rest")) {
				current = InstructionState.rest;
				currentIndicator.ChangeColor (Color.yellow);
			} else if (col.tag.Equals ("whole")) {
				current = InstructionState.whole;
				manager.groundY -= manager.stepHeight;
			}
		}

		if (col.tag.Equals ("flag")) {
			manager.SetGameState(GameManager.GameState.FINISHED);
		}

		if (col.tag.Equals ("soundbar")) {
			manager.playBeat ();
		}  else if (col.tag.Equals ("musicbar")) {
			manager.playMusic ();
		} else if (col.tag.Equals ("deflater")) {
			IsTouchingDeflater = true;
		} else if (col.tag.Equals ("deflater cover")) {
			IsTouchingDeflaterCover = true;
			currentDeflater = col.GetComponentInParent<DeflaterManager> ();
		} else if (col.tag.Equals ("pogo") || col.tag.Equals("last pogo")) {
			if (Input.GetMouseButton (0)) {
				rigidbody.gravityScale = gravityScaleSingleJump;
				manager.OnBallTouchPogo (
					col.transform.parent.GetChild (1),
					velocity,
					rigidbody.gravityScale * 9.81f
				);
			}
			if (col.tag.Equals ("pogo")) {
				if (Input.GetMouseButton (0)) {
					manager.IncreaseScore (10);
				}
			} else {
				if (Input.GetMouseButton (0)) {
					TurnOffGround = true;
					manager.IncreaseScore (5);
				}
			}
 		} 
		if (col.tag.Equals ("pogo")) {
			manager.groundY -= manager.stepHeight;
		}

		// For tutorial
		if (col.tag.Equals ("tut_click")) {
			manager.TutClick ();
		} else if (col.tag.Equals ("tut_unclick")) {
			manager.TutUnclick ();
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.tag.Equals ("deflater")) {
			IsTouchingDeflater = false;
			currentDeflater.StartContracting ();
		} else if (col.tag.Equals ("deflater cover")) {
			IsTouchingDeflaterCover = false;
			//Start expanding the inflater
			if (IsTouchingDeflater) {
				currentDeflater.StartExpanding ();
			} else {
				currentDeflater = null;
			}
		} else if (col.name.Equals ("click detector")) {
			if (col.tag.Equals ("rest") && WasRestGood) {
				currentIndicator.ChangeColor (Color.green);
				manager.IncreaseScore (10);
			}
			WasRestGood = true;
			currentIndicator = null;
		}
	}

	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.tag.Equals ("platform")) {
			IsOnGround = true;
			if (TurnOffGround) {
				IsOnGround = false;
				TurnOffGround = false;
			}
		}	
	}
		
	public void SetVelocity(Vector2 vel) {
		rigidbody.velocity = vel;
	}

	public void SetTargetVel(float vel) {
		velocity = vel;
	}

	public Vector2 GetCurrentPos() {
		return rigidbody.position;
	}
}
