namespace rlsyscli;

public class Program
{
  public static void Main()
  {
    //Testing stuff, will be deleted later
    var driveInfo = new DriveInfo("/");
    float exact = (driveInfo.TotalSize + 0.0f) / 1024 / 1024 / 1024;
    var x = double.Round(exact, 2, MidpointRounding.AwayFromZero);
    Console.WriteLine(x + "GB");
    Console.WriteLine(driveInfo.TotalSize);
  }
}
