using System.Numerics;
namespace rlsyscli.Disk;

public readonly struct Space<T>
where T : INumber<T>
{
  //A method to calculate the bytes to a readable format as double.
  private T Disk<T>(double space)
  {
    const ushort bytes = 1024;
    var diskSpace = space / Math.Pow(bytes, 3);
    return (T) Convert.ChangeType(double.Round(diskSpace, 2), typeof(T));
  }
  
  //A method to calculate the space in GB.
  public T DiskSpace(double space)
  {
    return Disk<T>(space);
  }
  
  //A method to calculate the space in %.
  public T DiskPercentage(double fullSpace, double space)
  {
    var diskPercentage = 100.0 / Disk<double>(fullSpace) * Disk<double>(space);

    if (Double.IsNaN(diskPercentage))
    {
      return (T) Convert.ChangeType(0.00, typeof(T));
    }
    
    return (T) Convert.ChangeType(double.Round(diskPercentage, 3), typeof(T));
  }
}
