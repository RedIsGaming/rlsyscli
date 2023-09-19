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
    var diskPercentage = 100.0 / Disk<double>(fullSpace) * Disk<double>(space);
    return (T) Convert.ChangeType(double.Round(diskPercentage, 2), typeof(T));
  }
}
