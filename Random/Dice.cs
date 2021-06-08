namespace UnityUtilities.Random {
	public class Dice {
		private int _faces;
		public Dice(int faces) {
			_faces = faces;
		}
		public int Roll() {
			var random = new System.Random();
			return random.Next(1, _faces);
		}

		public int Roll(int numDice) {
			var total = 0;
			for (var i = 0; i < numDice; i++) {
				total += Roll();
			}

			return total;
		}
	}
}