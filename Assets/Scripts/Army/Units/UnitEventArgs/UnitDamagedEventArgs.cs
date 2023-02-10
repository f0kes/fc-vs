namespace Army.Units.UnitEventArgs
{
	public class UnitDamagedEventArgs : System.EventArgs
	{
		public Unit Target { get; set; }
		public float Damage { get; set; }
	}
	public class UnitKilledEventArgs : System.EventArgs
	{
		public Unit Target { get; set; }
	}
	public class UnitAttackedEventArgs : System.EventArgs
	{
		public Unit Target { get; set; }
		public Unit Attacker { get; set; }
	}
}