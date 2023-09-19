namespace rlsyscli.Disk;

public readonly struct Path
{
  private Volume<double> VolumePath()
  {
    Volume<double> volume = new();
    
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
      Console.WriteLine("Drive not found" + e.Message + "\n");
    }

    return volume;
  }

  public List<string> FilterPath()
  {
    var volumePath = VolumePath()
      .Path
      .Where(v => v.Length <= 7)
      .Order()
      .ToList();

    return volumePath;
  }
}
