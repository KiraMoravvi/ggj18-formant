public interface IWaveRenderer
{
    bool IsClosest { get; set; }
    float RadiusAt(float position);
}