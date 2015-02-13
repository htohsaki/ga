using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Ai.Ga;
using Ai.Ga.Model;

public class test : MonoBehaviour {

	private GaMain gaMain;

	// Use this for initialization
	void Start () {
		GeneMaster gm1 = new GeneMaster ("height", 100, 110);
		GeneMaster gm2 = new GeneMaster ("weight", 100, 40);
		GeneMaster gm3 = new GeneMaster ("face", 10,0);

		GeneMasterSet gms = new GeneMasterSet ();
		gms.Add (gm1).Add (gm2).Add (gm3);

		this.gaMain = new GaMain(gms, 1000, 30);
		this.gaMain.onComplete += OnCompleteGA;
		this.gaMain.onProgress += OnProgressGA;
		this.gaMain.TurnOnLog ("gaLog");
		this.gaMain.FirstGeneration ();
	}
	
	// Update is called once per frame
	void Update () {
		GeneCodeSet gcs = this.gaMain.GetNextCodeSet ();
		while (gcs != null) {
			float scoreSub = 0.0f;

			scoreSub += this.subDevAbs(183.1f,gcs ["height"].Value+gcs["height"].master.offset,0.1f);
			//Debug.Log (this.subDevAbs (183.1f, gcs ["height"].Value + gcs ["height"].master.offset, 0.1f));
			float bmi = (gcs ["weight"].Value+gcs["weight"].master.offset) / Mathf.Pow ((gcs ["height"].Value+gcs["height"].master.offset) / 100.0f, 2);
			scoreSub += this.subDevAbs(18.5f,bmi,0.4f);
			//Debug.Log (this.subDevAbs(18.5f,bmi,0.4f));
			scoreSub += this.subDevAbs(7.0f,gcs["face"].Value,2.0f);
			//Debug.Log (this.subDevAbs(7.0f,gcs["face"].Value,20.0f));

			this.gaMain.SetScore (scoreSub);
			//Debug.Log (gcs.ToString() + "..." + bmi);

			gcs = this.gaMain.GetNextCodeSet ();
		}

		this.gaMain.Evolution (10);
	}

	private float subDevAbs(float a, float b, float dev){
		if (a == b) {
			return 0.0f;
		} else {
			return Mathf.Abs((a - b)/dev);
		}
	}

	private void OnCompleteGA(List<GeneCodeSet> selectedGeneCodeSet){
		Debug.Log ("End");
		Debug.Log (selectedGeneCodeSet [0].ToString ());
	}

	private void OnProgressGA(List<GeneCodeSet> currentGeneCodeSet, int generationCnt){
		Debug.Log ("evolution " + generationCnt);
	}



}
