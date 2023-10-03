using System.Numerics;
namespace rlsyscli.Disk;

public readonly struct Space<T>
where T : INumber<T>
{
  private T Disk<T>(double space)
  {
    const ushort bytes = 1024;
    var diskSpace = space / Math.Pow(bytes, 3);
    return (T) Convert.ChangeType(double.Round(diskSpace, 2), typeof(T));
  }
  
  public T DiskSpace(double space)
  {
    return Disk<T>(space);
  }
  
  public T DiskPercentage(double fullSpace, double space)
  {
    const float percent = 100.0f;
    var diskPercentage = percent / Disk<double>(fullSpace) * Disk<double>(space);

    if (double.IsNaN(diskPercentage))
    {
      return (T) Convert.ChangeType(percent - 100, typeof(T));
    }
    
    return (T) Convert.ChangeType(double.Round(diskPercentage, 2), typeof(T));
  }
}
