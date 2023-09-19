using System.Data;
using rlsyscli.Disk;
using rlsyscli.Table;
using Path = rlsyscli.Disk.Path;
namespace rlsyscli;

public class Program
{
  private static TableView<string> tableView = new()
  {
    DataTables = new DataTable(),
    Lines = "+--------+------------+------------+--------------------+------------+--------------------+"
  };
  
  private static void PrintDrive(string path, List<double> diskSpace, List<double> diskPercentage)
  {
    var row = String.Format("| {0,6} | {1,8}GB | {2,8}GB | {3,17}% | {4,8}GB | {5,17}% |",
      path, 
      diskSpace[0], 
      diskSpace[1], 
      diskPercentage[0], 
      diskSpace[2], 
      diskPercentage[1]
    );

    Console.WriteLine($"{tableView.Lines}\n{row}");
  }

  private static void RetrieveDisk(string args)
  {
    var driveInfo = new DriveInfo(args);
    
    var volume = new Volume<double>
    {
      TotalSize = driveInfo.TotalSize,
      UsedSpace = driveInfo.AvailableFreeSpace,
      FreeSpace = driveInfo.TotalSize - driveInfo.AvailableFreeSpace
    };
    
    var space = new Space<double>();
    
    List<double> diskSpace = new()
    {
      space.DiskSpace(volume.TotalSize),
      space.DiskSpace(volume.UsedSpace),
      space.DiskSpace(volume.FreeSpace)
    };
      
    List<double> diskPercentage = new()
    {
      space.DiskPercentage(volume.TotalSize, volume.UsedSpace),
      space.DiskPercentage(volume.TotalSize, volume.FreeSpace)
    };

    PrintDrive(args, diskSpace, diskPercentage);
  }

  private static void RetrievePath()
  {
    var path = new Path();

    foreach (var args in path.FilterPath())
    {
      RetrieveDisk(args);
    }
  }
  
  public static void Main()
  {
    tableView.WriteColumn();
    RetrievePath();
    Console.WriteLine(tableView.Lines);
  }
}
