public interface IBurnAble
{
    public bool IsBurning { get; set; }
    public void StartBurning(int DamagePerSecond);
    public void StopBurning();  
}
