using UnityEngine;
using System;
using System.Collections.Generic;
using Ai.Ga.Model;

namespace Ai.Ga.Model {
	/**
	 * 遺伝子コードセットのひな形
	 * */
	public class GeneMasterSet{
		Dictionary<String, GeneMaster> codeSet;

		public GeneMasterSet(){
			this.codeSet = new Dictionary<String, GeneMaster> ();

		}

		/**
		 * GeneMasterを追加する
		 * */
		public GeneMasterSet Add(GeneMaster geneMaster){
			this.codeSet.Add (geneMaster.key, geneMaster);
			return this;
		}

		public int Count{
			get{ 
				return this.codeSet.Count;
			}
		}

		/**
		 * この遺伝子コードセットをひな形にしたランダムなGeneCodeSetを作る
		 * */
		public GeneCodeSet MakeRandomGeneCodeSet(){
			GeneCodeSet gcs = new GeneCodeSet (this);

			foreach (string key in this.Keys) {
				GeneCode gc = new GeneCode (this.codeSet [key]);
				gc.SetRandom ();
				gcs.Add (gc);
			}

			return gcs;
		}

		public GeneMaster this[String key]{
			get { 
				return this.codeSet [key];
			}
			set {
				this.codeSet [key] = value;
			}
		}

		public Dictionary<String, GeneMaster>.KeyCollection Keys{
			get{
				return this.codeSet.Keys;
			}
		}

		public override string ToString(){
			string s = "";
			foreach (string key in this.Keys) {
				s += key + "=" + this [key].ToString () + ",";
			}
			return s;
		}

		public string ToCSV(){
			string s = "";
			foreach (string key in this.Keys) {
				s += key + "(" + this[key].max + "),";
			}
			s += "score";
			return s;
		}


	}
}

