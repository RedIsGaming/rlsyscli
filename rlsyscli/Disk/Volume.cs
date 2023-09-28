namespace rlsyscli.Disk;

public readonly struct Volume<T>
where T : IConvertible
{
  //3 Properties to get the space of all the available disks.
  public T TotalSize { get; init; }
  public T UsedSpace { get; init; }
  public T FreeSpace { get; init; }
}
