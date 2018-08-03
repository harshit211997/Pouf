using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
	
	public AudioClip beat, ouch, music;
	public Ball ball;
	public float stepHeight;
	public GameObject InfoText;
	public Text ScoreText;
	public GameObject GamePlayObjects;
	public GameObject GameOverDialogSuccess, GameOverDialogSuccessLast;
	public GameObject CustomGameOverDialog;
	public GameObject GameOverDialogFail;
	public GameObject TutOverDialog, TutFailDialog;
	public ThemeManager themeManager;
	public SpriteRenderer BackgroundFrame;
	public GameObject measure;
	public GameObject CloudSet;
	public GameObject pauseObjects;
	public enum GameState {
		START, PLAYING, FINISHED
	};
	public bool isTutorial = true;
	public enum TutState {
		BEGIN, NONE, TUT_CLICK, TUT_UNCLICK, END
	};
	public bool tutIsClicked = false;
	public bool tutIsUnclicked = false;
	public float groundY = 1.03168f;

	private TutState curTutState = TutState.BEGIN;
	private AudioSource source;
	private int Score = 0;
	private Vector2 placingPos = Vector2.zero;
	private Vector2 distObstacles = new Vector2 (10f, -5);
	private GameState currentGameState;
	private GameObject obsQuarter, obsHalf, obsTwoE, obsRest, obsWhole, finalPlatform;

	void Start () {
		source = GetComponent<AudioSource> ();
		stepHeight = 1.1f;

		currentGameState = GameState.START;
		if (!isTutorial) {
			ArrangeLevel ();
		}
		GamePlayObjects.SetActive (true);
		if (!isTutorial) {
			GameOverDialogSuccess.SetActive (false);
		}
	}

	void Update() {
		

		if (isTutorial) {
			switch (curTutState) {
			case TutState.NONE:
				if (Input.GetMouseButtonDown (0)) {
					if (ball.IsOnTriangle ()) {
						tutIsClicked = true;
					} else {
						print ("NONE DOWN");
						// Activate gameover dialog with appropriate message
						TutFailDialog.SetActive (true);
						source.volume = 0.3f;
						Time.timeScale = 0f;
						curTutState = TutState.END;
					}
				} else if (Input.GetMouseButtonUp (0)) {
					// Activate gameover dialog with appropriate message
					if (!ball.IsInsideRedCircleInQuarterNote ()) {
						TutFailDialog.transform.GetChild (0).GetComponent<Text> ().text = "Unclick only inside the red circle!";
						TutFailDialog.SetActive (true);
						Time.timeScale = 0f;
						source.volume = 0.3f;
						curTutState = TutState.END;
					} else {
						tutIsUnclicked = true;
					}
				}
				break;
			case TutState.TUT_CLICK:
				if (Input.GetMouseButtonDown (0)) {
					Time.timeScale = 1f;
					InfoText.SetActive (false);
					curTutState = TutState.NONE;
					source.UnPause ();
				}
				break;

			case TutState.TUT_UNCLICK:
				if (Input.GetMouseButtonUp (0)) {
					Time.timeScale = 1f;
					InfoText.SetActive (false);
					curTutState = TutState.NONE;
					source.UnPause ();
				}

				break;

			case TutState.BEGIN:
				if (Input.GetMouseButtonDown (0) && currentGameState == GameState.PLAYING) {
					TutFailDialog.transform.GetChild (0).GetComponent<Text> ().text = "Wait for the instruction to click!";
					TutFailDialog.SetActive (true);
					Time.timeScale = 0;
					curTutState = TutState.END;
					source.volume = 0.3f;
				}
				break;

			}
		}
		if (currentGameState == GameManager.GameState.START) {
			if (Input.GetMouseButtonDown (0)) {
				SetGameState (GameManager.GameState.PLAYING);
				Time.timeScale = 1;
			}
		}
	}

	private void SetThemeContents() {
		// Setting theme contents
		if (StateManager.GetTheme () != (int)Themes.GRASSY) {
			CloudSet.SetActive (false);
		}
		Instantiate(themeManager.platform[StateManager.GetTheme()]);
		obsQuarter = themeManager.quarterObs[StateManager.GetTheme()];
		obsHalf = themeManager.halfObs[StateManager.GetTheme()];
		obsTwoE = themeManager.eightObs[StateManager.GetTheme()];
		obsWhole = themeManager.wholeObs[StateManager.GetTheme()];
		obsRest = themeManager.restObs [StateManager.GetTheme ()];
		finalPlatform = themeManager.finalPlatform[StateManager.GetTheme()];
		BackgroundFrame.sprite = themeManager.background[StateManager.GetTheme()];
		BackgroundFrame.transform.localScale = new Vector3 (
			themeManager.scaleBackground[StateManager.GetTheme()],
			themeManager.scaleBackground[StateManager.GetTheme()],
			themeManager.scaleBackground[StateManager.GetTheme()]
		);
	}

	private void ArrangeLevel() {
		string fileName = "/level";
		if (StateManager.IsCustom ()) {
			fileName = "/custom_level";
		}
		string destination = Application.persistentDataPath + fileName + StateManager.GetLevel() + ".dat";
		FileStream file;

		if (File.Exists (destination)) {
			file = File.OpenRead (destination);
		} else {
			print (destination);
			Debug.LogError("File not found");
			return;
		}

		BinaryFormatter bf = new BinaryFormatter();
		GameData data = (GameData) bf.Deserialize(file);
		StateManager.SetTheme ((Themes)data.theme);
		SetThemeContents ();
		ball.SetTargetVel (data.bpm * distObstacles.x / 60);
		file.Close();

		string[] notes = data.notes.Split (' ');
		for (int i = 0; i < notes.Length; i++) {
			if (notes [i].Equals ("r")) {
				if (obsRest == null)
					print ("rest obs is null");
				GameObject obsClone = GameObject.Instantiate (obsRest);
				obsClone.transform.position = new Vector2 (placingPos.x, obsRest.transform.position.y + placingPos.y);
				placingPos.x += distObstacles.x;
			} else if (notes [i].Equals ("q")) {
				if (obsQuarter == null)
					print ("quarter obs is null");
				GameObject obsClone = GameObject.Instantiate (obsQuarter);
				obsClone.transform.position = new Vector2 (placingPos.x, obsQuarter.transform.position.y + placingPos.y);
				placingPos.x += distObstacles.x;
			} else if (notes [i].Equals ("te")) {
				if (obsTwoE == null)
					print ("two eight obs is null");
				GameObject obsClone = GameObject.Instantiate (obsTwoE);
				obsClone.transform.position = new Vector2 (placingPos.x, obsTwoE.transform.position.y + placingPos.y);
				placingPos.x += distObstacles.x;
			} else if (notes [i].Equals ("h")) {
				if (obsHalf == null)
					print ("half obs is null");
				GameObject obsClone = GameObject.Instantiate (obsHalf);
				obsClone.transform.position = new Vector2 (placingPos.x, obsHalf.transform.position.y + placingPos.y);
				placingPos.x += 2 * distObstacles.x;
			} else if(notes[i].Equals("w")) {
				if (obsWhole == null)
					print ("whole obs is null");
				GameObject obsClone = GameObject.Instantiate (obsWhole);
				obsClone.transform.position = new Vector2 (placingPos.x, obsWhole.transform.position.y + placingPos.y);
				placingPos.x += 4 * distObstacles.x;
				placingPos.y -= 3 * stepHeight;
			} else if(notes[i].Equals("|")) {
				if (measure == null)
					print ("measure is null");
				GameObject mesClone = GameObject.Instantiate (measure);
				mesClone.transform.position = new Vector2 (placingPos.x - 2.5f, measure.transform.position.y + placingPos.y);
			} else {
				print (notes [i]);
				Debug.LogError ("invalid note : " + notes[i]);
			}
		}
		GameObject platformClone = GameObject.Instantiate (finalPlatform);
		platformClone.transform.position = new Vector2 (placingPos.x, finalPlatform.transform.position.y + placingPos.y);

		if (!StateManager.IsCustom ()) {
			//Activate the corresponding family member
			platformClone.transform.GetChild (1).GetChild (StateManager.GetLevel () - 1).gameObject.SetActive (true);
		}
	}

	public void IndicateClick(ClickIndicator indicator, Color color) {
		indicator.ChangeColor (color);
	}

	public void playBeat() {
		source.PlayOneShot (beat);
	}

	private void playOuch() {
		source.PlayOneShot (ouch);
	}

	public void playMusic() {
		source.PlayOneShot (music);
	}

	public void OnBallTouchPogo(Transform target, float ballVel, float g) {
		ball.SetVelocity (
			GetReqVelocity (
				g,
				ball.GetCurrentPos(),
				target.position,
				ballVel
		)
		);
	}

	public Vector2 GetReqVelocity(float g, Vector2 currentPos, Vector2 targetPos, float ballVel) {
		float xi = currentPos.x;
		float xf = targetPos.x;
		float yi = currentPos.y;
		float yf = targetPos.y;

		Vector2 vel = new Vector2 ();
		vel.x = ballVel;
		vel.y = ((yf - yi) / (xf - xi)) * ballVel + 0.5f * g * (xf - xi) / ballVel;
		return vel;
	}

	public float GetPlacingPosY() {
		return placingPos.y;
	}

	public GameState GetGameState() {
		return currentGameState;
	}

	public void SetGameState(GameState state, int Score = -1) {
		currentGameState = state;
		if (state == GameState.PLAYING) {
			InfoText.SetActive (false);
		}
		if (state == GameState.FINISHED) {
			//display the gameover screen with some delay
			Invoke("OnGameOver", 2f);
			source.volume = 0.3f;
		}
	}

	public void OnGameOver() {
		GamePlayObjects.SetActive (false);
		if (!isTutorial) {
			ArrangeGameOverDialog ();

			// unlock next level if possible
			int lockedFrom = PlayerPrefs.GetInt ("locked from");
			if (StateManager.GetLevel () == lockedFrom - 1) {
				// Unlock next level
				PlayerPrefs.SetInt("locked from", StateManager.GetLevel() + 2);
			}
			print(PlayerPrefs.GetInt("locked from"));
		} else {
			TutOverDialog.SetActive (true);
			source.volume = 0.3f;
			curTutState = TutState.END;
		}
	}

	private void ArrangeGameOverDialog() {
		if (StateManager.IsCustom ()) {
			CustomGameOverDialog.SetActive (true);
		} else {
			if (Score >= 260) {
				// Last level
				if (StateManager.GetLevel () == 10) {
					GameOverDialogSuccessLast.transform.GetChild (4).GetComponent<Text> ().text = Score + "";
					Image star1 = GameOverDialogSuccessLast.transform.GetChild (1).GetComponent<Image> ();
					Image star2 = GameOverDialogSuccessLast.transform.GetChild (2).GetComponent<Image> ();
					Image star3 = GameOverDialogSuccessLast.transform.GetChild (3).GetComponent<Image> ();
					if (Score >= 300) {
						star3.gameObject.SetActive (true);
					} else if (Score >= 280) {
						star2.gameObject.SetActive (true);
					} else if (Score >= 260) {
						star1.gameObject.SetActive (true);
					}
					GameOverDialogSuccessLast.SetActive (true);
				} else {
					GameOverDialogSuccess.transform.GetChild (4).GetComponent<Text> ().text = Score + "";
					Image star1 = GameOverDialogSuccess.transform.GetChild (1).GetComponent<Image> ();
					Image star2 = GameOverDialogSuccess.transform.GetChild (2).GetComponent<Image> ();
					Image star3 = GameOverDialogSuccess.transform.GetChild (3).GetComponent<Image> ();
					if (Score >= 300) {
						star3.gameObject.SetActive (true);
					} else if (Score >= 280) {
						star2.gameObject.SetActive (true);
					} else if (Score >= 260) {
						star1.gameObject.SetActive (true);
					}
					GameOverDialogSuccess.SetActive (true);
				}
			} else {
				//Level failed
				GameOverDialogFail.transform.GetChild (1).GetComponent<Text> ().text = Score + "";
				GameOverDialogFail.SetActive (true);
			}
		}
	}

	public void IncreaseScore(int amt) {
		Score += amt;
		ScoreText.text = Score + "";
	}

	public void OnHomeClicked() {
		Time.timeScale = 1;
		StateManager.SetLevel (0);
		SceneManager.LoadScene ("main");
	}

	public void OnReloadClicked() {
		SceneManager.LoadScene ("game");
	}

	public void OnNextClicked() {
		int currentLevel = StateManager.GetLevel ();
		StateManager.SetLevel (currentLevel + 1);
		if (currentLevel == 9 && !StateManager.IsCustom()) {
			SceneManager.LoadScene ("PreBossLevel");
		} else {
			SceneManager.LoadScene ("game");
		}
	}

	public void OnClickPause() {
		Time.timeScale = 0;
		pauseObjects.SetActive (true);
		GamePlayObjects.SetActive (false);
		source.Pause ();
	}

	public void OnClickResume() {
		Time.timeScale = 1f;
		pauseObjects.SetActive (false);
		GamePlayObjects.SetActive (true);
		source.UnPause ();
	}

	public void OnClickLevels() {
		Time.timeScale = 1;
		StateManager.SetLevel (0);
		SceneManager.LoadScene ("levels 1");
	}

	public void OnClickCustomLevels() {
		Time.timeScale = 1;
		StateManager.SetLevel (0);
		SceneManager.LoadScene ("custom levels");
	}

	public void TutClick() {
		if (!tutIsClicked) {
			Time.timeScale = 0;
			InfoText.GetComponent<Text> ().text = "Click and hold on triangle to jump";
			InfoText.SetActive (true);
			curTutState = TutState.TUT_CLICK;
			source.Pause ();
		}
	}

	public void TutUnclick() {
		if (!tutIsUnclicked) {
			Time.timeScale = 0;
			InfoText.GetComponent<Text> ().text = "Unclick inside the red circle to get more points!";
			InfoText.SetActive (true);
			curTutState = TutState.TUT_UNCLICK;
			source.Pause ();
		}
	}

	public void OnClickTutReload() {
		SceneManager.LoadScene ("tutorial");
	}

}
