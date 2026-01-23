namespace axis_console_project.UnitTypes;

public abstract class Unit
{
    protected string Name { get; set; } = "";
    public int Cost { get; set; }
    protected int Health { get; set; } = 1;
    protected virtual int Attack { get; set; }
    protected int Defence { get; set; }
    public bool ParticipatesInBattle { get; set; } = true;
    public Boolean isAttacking { get; set; }
    public Boolean isAlive
    {
        get { return Health > 0; }
    }
    private readonly Random _dice = new Random();

    public virtual bool Fire()
    {
        if (isAlive)
        {
            var roll = _dice.Next(1, 7);
            if (isAttacking)
            {
                // Console.WriteLine($"{Name} is attacking");
                // Console.WriteLine($"{Name} rolled a {roll}");
                if (roll <= Attack)
                {
                    // Console.WriteLine("It's a hit!");
                    return true;
                }
                // else
                // Console.WriteLine("It's a miss");
            }
            else
            {
                // Console.WriteLine($"{Name} is defending");
                // Console.WriteLine($"{Name} rolled a {roll}");
                if (roll <= Defence)
                {
                    // Console.WriteLine("It's a hit!");
                    return true;
                }
                // else
                // Console.WriteLine("It's a miss");
            }
        }
        return false;
    }

    public void TakeHit()
    {
        Health -= 1;
    }
}
