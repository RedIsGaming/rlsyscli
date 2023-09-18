using rlsyscli_test.Data;
namespace rlsyscli_test.Test;

[Trait("Category", "Unit")]
[Trait("Priority", "High")]
[Trait("Environment", "Development")]
public record DiskLimitTest
{
  //Checks if the DriveName is in the list. It returns true if the given volume is in there.
  //The options to from are: /dev, /run, /, /dev/shm, /dev/sda1, /tmp and /run/user/1000.
  [Fact]
  private void CheckVolumeTest()
  {
    var diskLimit = new DiskLimit<float>
    {
      DriveName = "/Tmp"
    };

    var result = diskLimit.CheckVolume();
    Assert.Contains(diskLimit.DriveName, result, StringComparer.OrdinalIgnoreCase);
  }
  
  //Calculates and returns true if the given TotalStorage is lower then the disk storage.
  //TotalStorage can't be lower then 0 and is measured in Gigabytes.
  [Fact]
  private void CalculateStorageTest()
  {
    var diskLimit = new DiskLimit<float>
    {
      DriveName = "/",
      TotalStorage = 50f
    };
    
    var result = diskLimit.CalculateStorage(diskLimit.DriveName, diskLimit.TotalStorage);
    Assert.True(result);
  }
  
  //Compares and returns true if the given AllocateStorage isn't lower then the disk storage.
  //It's combined and AllocateStorage can't be lower then 0. This is measured in Gigabytes.
  [Fact]
  private void CompareStorageTest()
  {
    var diskLimit = new DiskLimit<float>
    {
      DriveName = "/",
      AllocateStorage = 50f,
    };
    
    var result = diskLimit.CompareStorage(diskLimit.DriveName, diskLimit.AllocateStorage);
    Assert.True(result);
  }
  
  //This test works, but it's still a bit wacky.
  //Problem with driveName path still occuring in all tests, except the first.
  [Fact]
  private void AvailableStorageTest()
  {
    var diskLimit = new DiskLimit<float>
    {
      DriveName = "/",
      TotalStorage = 29.28f,
      AllocateStorage = 29.28f,
    };

    var result = diskLimit.AvailableStorage(
      diskLimit.DriveName, diskLimit.TotalStorage, diskLimit.AllocateStorage
    );
    Assert.True(result);
  }
}
