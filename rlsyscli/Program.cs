using rlsyscli.DiskVolume;
namespace rlsyscli;

public class Program
{
  private static Volume<string> VolumePath()
  {
    Volume<string> volume = new();
    
    try
    {
      volume = new()
      {
        Path = new List<string>
        {
          "/dev",
          "/run",
          "/",
          "/dev/shm",
          "/boot/efi",
          "/tmp",
          "/run/user/1000"
        }
      };

      return volume;
    }

    catch (DriveNotFoundException e)
    {
      Console.WriteLine("Drive not found \n" + e.Message);
    }

    return volume;
  }

  private static List<string> FilterPath()
  {
    var volumePath = VolumePath()
      .Path
      .Where(v => v.Length <= 7)
      .Order()
      .ToList();

    return volumePath;
  }
  
  public static void Main()
  {
    const ushort bytes = 1024;
    
    foreach (var path in FilterPath())
    {
      var driveInfo = new DriveInfo(path);
      var drive = (driveInfo.TotalSize + 0.0f) / bytes / bytes / bytes;
      var filter = float.Round(drive, 2, MidpointRounding.AwayFromZero);
      
      Console.WriteLine("Volume | Size");
      Console.WriteLine(path + " " + filter + " GB");
    }
  }
}
