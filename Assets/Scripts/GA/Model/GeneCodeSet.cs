using UnityEngine;
using System;
using System.Collections.Generic;
using Ai.Ga.Model;

namespace Ai.Ga.Model {
	/**
	 * 遺伝子コードのセット
	 * */
	public class GeneCodeSet{
		public GeneMasterSet geneMasterSet;

		public Dictionary<String, GeneCode> codeSet;

		public float score = 0.0f;

		public GeneCodeSet(GeneMasterSet geneMasterSet){

			this.geneMasterSet = geneMasterSet;

			this.codeSet = new Dictionary<String, GeneCode> ();

		}


		public void Add(GeneCode geneCode){
			this.codeSet.Add (geneCode.master.key, geneCode);
		}

		public GeneCode this[String key]{
			get { 
				return this.codeSet [key];
			}
			set {
				this.codeSet [key] = value;
			}
		}

		public Dictionary<String, GeneCode>.KeyCollection Keys{
			get{
				return this.codeSet.Keys;
			}
		}

		public override string ToString(){
			string s = "";
			foreach (string key in this.Keys) {
				s += key + "=" + this [key].ToString () + ",";
			}
			s += "score=" + this.score;
			return s;
		}

		public GeneCodeSet Clone(){
			GeneCodeSet gcs = new GeneCodeSet (this.geneMasterSet);

			foreach (String key in this.Keys) {
				gcs.Add ((this [key]).Clone ());
			}

			return gcs;
		}


		/**
		 * この遺伝子コードセットを元に一つの遺伝子を変更した突然変異を作る
		 * */
		public List<GeneCodeSet> MakeMutations(){
			List<GeneCodeSet> mutations = new List<GeneCodeSet> ();

			foreach (String key in this.Keys) {
				GeneCodeSet gcs = this.Clone ();
				gcs [key].SetRandom ();
				mutations.Add (gcs);
			}

			return mutations;
		}

		public string ToCSV(){
			string s = "";
			foreach (string key in this.Keys) {
				s += (this.codeSet[key].Value+this.codeSet[key].master.offset) + ",";
			}
			s += this.score;
			return s;
		}


		/**
		 * GeneCodeSet aとbの遺伝子の交叉を行う
		 * その際、mutationProbabilityの確率で突然変異をおこす 0.0fで突然変異なし、1.0fで全て突然変異
		 * 
		 * **/
		public static GeneCodeSet Mix(GeneCodeSet a, GeneCodeSet b, float mutationProbability = 0.0f){

			GeneCodeSet codeSet = new GeneCodeSet (a.geneMasterSet);

			foreach (String key in a.Keys) {
				if (mutationProbability > UnityEngine.Random.value) {

					GeneCode code = a [key] as GeneCode;

					GeneCode newCode = new GeneCode (code.master);
					newCode.SetRandom ();
					codeSet.Add (newCode);

				} else {

					if (UnityEngine.Random.value < 0.5f) {
						codeSet.Add (a [key]);
					} else {
						codeSet.Add (b [key]);
					}

				}
			}

			return codeSet;
		}



	}
}

