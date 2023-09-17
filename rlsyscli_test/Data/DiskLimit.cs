using System.Numerics;
namespace rlsyscli_test.Data;

public readonly struct DiskLimit<T>
where T : INumber<T>
{
  public string DriveName { get; init; }
  public T TotalStorage { get; init; }
  public T AllocateStorage { get; init; }
  
  //Two consts for setting a default fixed value.
  private const ushort Bytes = 1024;
  private const float Zero = 0.0f;
  
  //Method which contains a list with the volumes to choose from.
  public List<string> CheckVolume()
  {
    var volume = new List<string>
    {
      "/dev", 
      "/run", 
      "/", 
      "/dev/shm",
      "/dev/sda1",
      "/tmp",
      "/run/user/1000"
    };

    return volume;
  }
  
  //Method for calculating the storage.
  public bool CalculateStorage(string driveName, float b)
  {
    if (!CheckVolume().Contains(driveName, StringComparer.OrdinalIgnoreCase))
    {
      return false;
    }
    
    var driveInfo = new DriveInfo(driveName);
    var a = (driveInfo.TotalSize + Zero) / Bytes / Bytes / Bytes;
    return a >= b && b >= Zero;
  }
  
  ///Method for comparing the storage.
  public bool CompareStorage(string driveName, float b)
  {
    if (!CheckVolume().Contains(driveName, StringComparer.OrdinalIgnoreCase))
    {
      return false;
    }
    
    var driveInfo = new DriveInfo(driveName);
    var a = (driveInfo.TotalSize + Zero) / Bytes / Bytes / Bytes;
    return a - b >= Zero && b >= Zero;
  }
  
  ///Method for retrieving the available storage.
  public bool AvailableStorage(string driveName, float b, float c)
  {
    if (!CheckVolume().Contains(driveName, StringComparer.OrdinalIgnoreCase))
    {
      return false;
    }
    
    var driveInfo = new DriveInfo(driveName);
    var a = (driveInfo.TotalSize + Zero) / Bytes / Bytes / Bytes;
    return (a - b - c) >= Zero && b >= Zero && c >= Zero;
  }
}
