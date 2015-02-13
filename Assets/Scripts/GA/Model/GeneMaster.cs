
namespace Ai.Ga.Model {

	/**
	 *namespace Ai.Ga	 * */
	public class GeneMaster {

		public string key;
		public uint max;
		public uint offset;

		public GeneMaster (string key, uint max, uint offset=0) {
			this.key = key;
			this.max = max;
			this.offset = offset;
		}

		public override string ToString(){
			return this.key + "<" + this.max;
		}
	}
}
