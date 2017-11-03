public class Controller : Redirector
{
    private float _currentGain;

    public void SetGain(float gain)
    {
        _currentGain = gain;
    }

    public void GetGain(out float gain)
    {
        gain = _currentGain;
    }

    public override void ApplyRedirection()
    {

        InjectRotation(_currentGain * redirectionManager.deltaDir);
    }
}