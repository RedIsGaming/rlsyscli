namespace rlsyscli.Disk;

public readonly struct Volume<T>
where T : IConvertible
{
  public IEnumerable<string> Path { get; init; }
  public T TotalSize { get; init; }
  public T UsedSpace { get; init; }
  public T FreeSpace { get; init; }
}
