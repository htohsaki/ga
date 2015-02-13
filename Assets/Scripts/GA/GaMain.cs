using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Ai.Ga.Model;

namespace Ai.Ga {

	/**
	 * GAアルゴリズムの処理クラス
	 * **/
	public class GaMain {

		public delegate void DComplete (List<GeneCodeSet> currentGeneCodeSet);
		public delegate void DProgress (List<GeneCodeSet> currentGeneCodeSet, int generationCnt);

		public event DComplete onComplete  = delegate(List<GeneCodeSet> currentGeneCodeSet) {};
		public event DProgress onProgress  = delegate(List<GeneCodeSet> currentGeneCodeSet, int generationCnt) {};

		public int currentGeneration = 0;

		private GeneMasterSet geneMasterSet;
		private int firstGenerationCnt;
		private int tryGeneration;
		private List<GeneCodeSet> currentGeneCodeSet;
		private List<GeneCodeSet> selectedGeneCodeSet;
		private int execGeneSetIndex = -1;

		private string logKeyWord = "";
		private bool logOn = false;

		public GaMain (GeneMasterSet geneMasterSet, int firstGenerationCnt, int tryGeneration) {

			this.geneMasterSet = geneMasterSet;

			this.firstGenerationCnt = firstGenerationCnt;

			this.tryGeneration = tryGeneration;

			this.selectedGeneCodeSet = new List<GeneCodeSet> ();
		}

		public void TurnOnLog(string logKeyWord){
			this.logKeyWord = logKeyWord;
			this.logOn = true;
		}

		public void TurnOffLog(){
			this.logOn = false;
		}

		/**
		 * 第一世代の作成
		 * */
		public void FirstGeneration () {
			this.currentGeneCodeSet = new List<GeneCodeSet> ();
			for (int i = 0; i < this.firstGenerationCnt; i++) {
				this.currentGeneCodeSet.Add (this.geneMasterSet.MakeRandomGeneCodeSet ());
			}
		}

		/**
		 * 次の遺伝子コードセットを取得する
		 * 次がなければnullを返す
		 * */
		public GeneCodeSet GetNextCodeSet(){
			this.execGeneSetIndex++;
			if (this.execGeneSetIndex >= this.currentGeneCodeSet.Count) {
				return null;
			}
			return this.currentGeneCodeSet [this.execGeneSetIndex];
		}

		/**
		 * 現在の遺伝子コードセットの適正評価のスコアを設定する
		 * score は 大きいほど適正が低い
		 * */
		public GeneCodeSet SetScore(float score){
			this.currentGeneCodeSet [this.execGeneSetIndex].score = score;
			return this.currentGeneCodeSet [this.execGeneSetIndex];
		}
			
		/**
		 * 上位percent%を選択
		 * 上位percent%の数が2に満たないときはFalseを返す
		 * **/
		private bool Choose (int percent) {
			//scoreでsortする
			this.currentGeneCodeSet.Sort (CompareScore);
			float n = this.currentGeneCodeSet.Count * percent / 100.0f;
			if (n < 2) {
				return false;
			}
			this.selectedGeneCodeSet = new List<GeneCodeSet> ();
			for (int i = 0; i < n; i++) {
				this.selectedGeneCodeSet.Add (this.currentGeneCodeSet [i]);
			}
			return true;
		}

		/**
		 * 次の世代へ進化させる
		 * **/
		public void Evolution (int percent) {
			List<GeneCodeSet> nextGeneCodeSet = new List<GeneCodeSet> ();
			this.execGeneSetIndex = -1;

			this.WriteLog ();

			if (this.Choose (percent)) {
				//トップpercentのグループ内で交配を行う
				//this.selectedGeneCodeSet 
				if (this.currentGeneration > this.tryGeneration) {
					this.onComplete (this.selectedGeneCodeSet);
					return;
				}

				for (int i = 1; i < this.selectedGeneCodeSet.Count; i++) {
					GeneCodeSet gcs = GeneCodeSet.Mix (this.selectedGeneCodeSet [0], 
						                  this.selectedGeneCodeSet [i]);
					nextGeneCodeSet.Add (gcs);

					gcs = GeneCodeSet.Mix (this.selectedGeneCodeSet [0], 
						this.selectedGeneCodeSet [i], 0.3f);
					nextGeneCodeSet.Add (gcs);
				}
				if (this.selectedGeneCodeSet.Count > 2) {
					for (int i = 2; i < this.selectedGeneCodeSet.Count; i++) {
						GeneCodeSet gcs = GeneCodeSet.Mix (this.selectedGeneCodeSet [1], 
							this.selectedGeneCodeSet [i]);
						nextGeneCodeSet.Add (gcs);
					}
				}

				//トップから突然変異したものと、交配したものを加える
				this.currentGeneCodeSet = this.selectedGeneCodeSet [0].MakeMutations ();
				this.currentGeneCodeSet.AddRange (this.selectedGeneCodeSet [0].MakeMutations ());
				this.currentGeneCodeSet.AddRange (this.selectedGeneCodeSet [0].MakeMutations ());
				this.currentGeneCodeSet.AddRange (nextGeneCodeSet);

				this.onProgress (this.currentGeneCodeSet, this.currentGeneration);

				this.currentGeneration++;

			} else {
				this.onComplete (this.selectedGeneCodeSet);
			}

		}


		private int CompareScore(GeneCodeSet x, GeneCodeSet y){
			if(x.score < y.score)
			{
				return -1;
			}
			else if(x.score > y.score)
			{
				return 1;
			}
			return 0;
		}

		private void WriteLog(){
			if (this.logOn) {

				string s = this.currentGeneration + "\n";
				this.currentGeneCodeSet.Sort (CompareScore);
				s += this.currentGeneCodeSet [0].geneMasterSet.ToCSV () + "\n";

				for (int i = 0; i < this.currentGeneCodeSet.Count; i++) {
					s += this.currentGeneCodeSet [i].ToCSV () + "\n";
				}

				string fileName = Application.persistentDataPath + @"/" + 
					this.logKeyWord + "_" + this.currentGeneration + ".csv";
				StreamWriter saveWriter = new StreamWriter (fileName, 
					false, System.Text.Encoding.GetEncoding ("utf-8"));
				saveWriter.NewLine = "\n";
				saveWriter.Write (s);
				saveWriter.Close ();
	

				s = this.currentGeneration + "\n";
				this.selectedGeneCodeSet.Sort (CompareScore);
				for (int i = 0; i < this.selectedGeneCodeSet.Count; i++) {
					s += this.selectedGeneCodeSet [i].ToCSV () + "\n";
				}

				fileName = Application.persistentDataPath + @"/" + 
					this.logKeyWord + "_" + this.currentGeneration + "_sel" + ".csv";
				saveWriter = new StreamWriter (fileName, 
					false, System.Text.Encoding.GetEncoding ("utf-8"));
				saveWriter.NewLine = "\n";
				saveWriter.Write (s);
				saveWriter.Close ();
			}

		}

	}


}
