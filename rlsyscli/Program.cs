using System.Data;
using rlsyscli.Disk;
using rlsyscli.Table;
namespace rlsyscli;

public class Program
{
  //An object for using the table.
  private static TableView<string> tableView = new()
  {
    DataTables = new DataTable(),
    Lines = "+----------------+----------+----------+----------+--------+--------+"
  };
  
  //A method to print the disks.
  private static void PrintDrive(DriveInfo path, List<double> diskSpace, List<double> diskPercentage)
  {
    var row = String.Format("| {0,14} | {1,6}GB | {2,6}GB | {3,6}GB | {4,5}% | {5,5}% |",
      path.Name,
      diskSpace[0], 
      diskSpace[1], 
      diskSpace[2], 
      diskPercentage[0], 
      diskPercentage[1]
    );

    Console.WriteLine($"{tableView.Lines}\n{row}");
  }
  
  //A method to get the disk storage calculated in GB.
  private static List<double> GetDiskSpace(Space<double> space, Volume<double> volume)
  {
    return new List<double>
    {
      space.DiskSpace(volume.TotalSize),
      space.DiskSpace(volume.UsedSpace),
      space.DiskSpace(volume.FreeSpace)
    };
  }
  
  //A method to get the disk storage calculated in %.
  private static List<double> GetDiskPercentage(Space<double> space, Volume<double> volume)
  {
    return new List<double>
    {
      space.DiskPercentage(volume.TotalSize, volume.UsedSpace),
      space.DiskPercentage(volume.TotalSize, volume.FreeSpace)
    };
  }
  
  //A method to get the disk spaces in GB and %.
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

          PrintDrive(args, GetDiskSpace(space, volume), GetDiskPercentage(space, volume));
        }
      }

      catch (UnauthorizedAccessException) {}
    }
  }
  
  //A method to get all the current drives that are in use and those that can be shown.
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
  
  //The main application
  public static void Main()
  {
    tableView.WriteColumn();
    RetrieveDisk();
    Console.WriteLine(tableView.Lines);
  }
}
