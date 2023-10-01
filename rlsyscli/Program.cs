using ConsoleTables;
using rlsyscli.Disk;
namespace rlsyscli;

public class Program
{
  private static Table<ConsoleTable> table = new()
  {
    CreateTable = new ConsoleTable(
      "Volume path", "Total space", "Used space",
      "Free space", "Percent used space", "Percent used free"
    )
  };
  
  private static void FillRow(DriveInfo path, List<double> diskSpace, List<double> diskPercentage)
  {
    table.CreateTable.AddRow(
      path.Name, diskSpace[0] + String.Join("", "GB"), 
      diskSpace[1] + String.Join("", "GB"), diskSpace[2] + String.Join("", "GB"), 
      diskPercentage[0] + String.Join("", "%"), diskPercentage[1] + String.Join("", "%")
    );
  }

  private static void WriteTable()
  {
    table.CreateTable.Write(Format.Alternative);
  }
  
  private static List<double> GetDiskSpace(Space<double> space, Volume<double> volume)
  {
    return new List<double>
    {
      space.DiskSpace(volume.TotalSize), space.DiskSpace(volume.UsedSpace),
      space.DiskSpace(volume.FreeSpace)
    };
  }
  
  private static List<double> GetDiskPercentage(Space<double> space, Volume<double> volume)
  {
    return new List<double>
    {
      space.DiskPercentage(volume.TotalSize, volume.UsedSpace),
      space.DiskPercentage(volume.TotalSize, volume.FreeSpace)
    };
  }
  
  private static void FilterDisk(List<DriveInfo> driveInfo, Space<double> space)
  {
    foreach (var args in driveInfo)
    {
      try
      {
        if (args.AvailableFreeSpace > 0.00)
        {
          var volume = new Volume<double>
          {
            TotalSize = args.TotalSize,
            UsedSpace = args.AvailableFreeSpace,
            FreeSpace = args.TotalSize - args.AvailableFreeSpace
          };
          
          FillRow(args, GetDiskSpace(space, volume), GetDiskPercentage(space, volume));
        }
      }

      catch (UnauthorizedAccessException) {}
    }
  }
  
  private static void RetrieveDisk()
  {
    try
    {
      var driveInfo = DriveInfo.GetDrives()
        .Where(d => d.IsReady)
        .OrderBy(d => d.Name)
        .Distinct()
        .ToList();
      
      var space = new Space<double>();
      FilterDisk(driveInfo, space);
    }

    catch (UnauthorizedAccessException err)
    {
      Console.WriteLine(err.Message);
    }
  }
  
  public static void Main()
  {
    RetrieveDisk();
    WriteTable();
  }
}
