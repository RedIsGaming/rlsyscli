using System.Numerics;
namespace rlsyscli.Disk;

public readonly struct Space<T>
where T : INumber<T>
{
  private T Disk<T>(double space)
  {
    var diskSpace = space / Math.Pow(1024, 3);
    return (T) Convert.ChangeType(double.Round(diskSpace, 2), typeof(T));
  }
  
  public T DiskSpace(double space)
  {
    return Disk<T>(space);
  }
  
  public T DiskPercentage(double fullSpace, double space)
  {
    const byte percent = 100;
    var diskPercentage = percent / Disk<double>(fullSpace) * Disk<double>(space);

    if (double.IsNaN(diskPercentage))
    {
      return (T) Convert.ChangeType(percent - percent, typeof(T));
    }
    
    return (T) Convert.ChangeType(double.Round(diskPercentage, 2), typeof(T));
  }
}
