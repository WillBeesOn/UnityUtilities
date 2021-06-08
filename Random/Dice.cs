namespace UnityUtilities.Random {
	public static class Dice {
		public static Roll(int faces, int numDice = 1) {
			var random = new System.Random();

			var total = 0;
			for (var i = 0; i < numDice; i++) {
				total += random.Next(1, faces);
			}

			return total;
		}
	}
}