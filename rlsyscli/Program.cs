using ConsoleTables;
using rlsyscli.Disk;
namespace rlsyscli;

public class Program
{
  private static readonly Table<ConsoleTable> Table = new()
  {
    CreateTable = new ConsoleTable(
      "Volume path", "Total space", "Used space", "Free space", "Percent used space", "Percent free space"
    )
  };
  
  private static void FillRow(DriveInfo path, IReadOnlyList<double> diskSpace, IReadOnlyList<double> diskPercentage)
  {
    Table.CreateTable.AddRow(path.Name, string.Format("{0:F2}GB", diskSpace[0]), 
      string.Format("{0:F2}GB", diskSpace[1]), string.Format("{0:F2}GB", diskSpace[2]), 
      string.Format("{0:F2}%", diskPercentage[0]), string.Format("{0:F2}%", diskPercentage[1])
    );
  }
  
  private static List<double> GetDiskSpace(Space<double> space, Volume<double> volume)
  {
    return new List<double>
    {
      space.DiskSpace(volume.TotalSize), space.DiskSpace(volume.UsedSpace), space.DiskSpace(volume.FreeSpace)
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
        .Where(d =>
        {
          try
          {
            var _ = d.TotalSize;
            return true;
          }
          catch (UnauthorizedAccessException)
          {
            return false;
          }
        })
        .OrderByDescending(d => d.TotalSize)
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
    Table.CreateTable.Write(Format.Alternative);
  }
}
