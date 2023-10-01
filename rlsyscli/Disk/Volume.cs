namespace rlsyscli.Disk;

public readonly struct Volume<T>
where T : IConvertible
{
  public T TotalSize { get; init; }
  public T UsedSpace { get; init; }
  public T FreeSpace { get; init; }
}
