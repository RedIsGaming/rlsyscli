namespace rlsyscli.DiskVolume;

public struct Volume<T>
{
  public IEnumerable<T> Path { get; init; }
}
