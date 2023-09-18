using System.Data;
using rlsyscli.DiskVolume;
namespace rlsyscli;

public class Program
{
  private const ushort Bytes = 1024;
  private static Volume<string, float> VolumePath()
  {
    Volume<string, float> volume = new();
    
    try
    {
      volume = new Volume<string, float>
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

  private static List<string> FilterPath()
  {
    var volumePath = VolumePath()
      .Path
      .Where(v => v.Length <= 7)
      .Order()
      .ToList();

    return volumePath;
  }

  private static float DiskSpace(float space)
  {
    var diskSpace= (space + 0.0f) / Bytes / Bytes / Bytes;
    var preciseSpace= float.Round(diskSpace, 2);
    
    return preciseSpace;
  }

  private static float DiskSpacePercentage(float totalSpace, float space)
  {
    var diskSpace= (space + 0.0f) / Bytes / Bytes / Bytes;
    var preciseSpace= float.Round(diskSpace, 2);
    
    return float.Round(100 / DiskSpace(totalSpace) * preciseSpace, 2);
  }
  
  public static void Main()
  {
    const string lines = "+--------+------------+------------+--------------------+------------+--------------------+";
    var dataTable = new DataTable();
    
    var columns = new List<string> {
      "Volume",
      "Total size",
      "Used space",
      "Percent used space",
      "Free space",
      "Percent free space"
    };
    
    foreach (var args in columns)
    {
      dataTable.Columns.Add(args);
    }
    
    Console.WriteLine(lines);
    
    foreach (var args in dataTable.Columns)
    {
      var column = String.Format("| {0,6} ", args);
      Console.Write(column);
      
      if (args == dataTable.Columns[^1])
      {
        Console.WriteLine("|");
      }
    }
    
    foreach (var path in FilterPath())
    {
      var driveInfo = new DriveInfo(path);
      var volume = new Volume<string, float>
      {
        TotalSize = driveInfo.TotalSize,
        UsedSpace = driveInfo.AvailableFreeSpace,
        FreeSpace = driveInfo.TotalSize - driveInfo.AvailableFreeSpace
      };

      var diskSpace = new List<float>
      {
        DiskSpace(volume.TotalSize),
        DiskSpace(volume.UsedSpace),
        DiskSpace(volume.FreeSpace)
      };

      var diskSpacePercentage = new List<float>
      {
        DiskSpacePercentage(driveInfo.TotalSize, volume.UsedSpace),
        DiskSpacePercentage(driveInfo.TotalSize, volume.FreeSpace)
      };
      
      var row = String.Format("" + 
        "| {0,6} | {1,8}GB | {2,8}GB | {3,17}% | {4,8}GB | {5,17}% |", 
        driveInfo.VolumeLabel, diskSpace[0], diskSpace[1],
        diskSpacePercentage[0], diskSpace[2], diskSpacePercentage[1]
      );
      
      Console.WriteLine(lines);
      Console.WriteLine(row);
    }
    
    Console.WriteLine(lines);
  }
}
