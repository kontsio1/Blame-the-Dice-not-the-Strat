public class LandUnit : Unit { }

public class AirUnit : Unit
{
    public virtual bool CanLandSafely { get; } = true;
}

public class NavalUnit : Unit
{
    public virtual bool CanBombardLandUnits { get; } = false;
    public virtual bool CanSurpriseAttack { get; } = false;
    public virtual int FighterCapacity { get; } = 0;
}
