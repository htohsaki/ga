using System;
using UnityEngine;
using Ai.Ga.Model;

namespace Ai.Ga.Model {

	public class GeneCode {

		public GeneMaster master;

		private uint _value;

		private static System.Random randomSeed = new System.Random ();

		public GeneCode (GeneMaster master) {
			this.master = master;
		}

		/**
		 * 範囲内でランダムの値を設定する
		 * */
		public GeneCode SetRandom(){

			int v = randomSeed.Next((int)this.master.max);
			this._value = Convert.ToUInt32(v);
			return this;
		}


		/**
		 * 値を設定する
		 * */
		public uint Value{
			get{
				return this._value;
			}
			set{
				if (value > this.master.max) {
					this._value = this.master.max - 1;
				} else {
					this._value = value;
				}
			}
		}

		public GeneCode Clone(){
			GeneCode gc = new GeneCode (this.master);
			gc.Value = this.Value;
			return gc;
		}

		public override string ToString(){
			string s = "(" + this.master.key + "," + (this.Value + this.master.offset) + ")";
			return s;
		}

	}
}

