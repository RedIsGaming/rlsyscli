namespace rlsyscli.DiskVolume;

public readonly struct Volume<T, U>
{
  public IEnumerable<T> Path { get; init; }
  public U TotalSize { get; init; }
  public U UsedSpace { get; init; }
  public U FreeSpace { get; init; }
}
