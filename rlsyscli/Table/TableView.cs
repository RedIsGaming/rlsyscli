using System.Data;
namespace rlsyscli.Table;

public readonly struct TableView<T>
where T : IConvertible
{
  public DataTable DataTables { get; init; }
  public T Lines { get; init; }
  
  private List<string> TableColumn()
  {
    var columns = new List<string>
    {
      "Volume",
      "Total size",
      "Used space",
      "Percent used space",
      "Free space",
      "Percent free space"
    };

    return columns;
  }

  private void FilterColumn()
  {
    foreach (var args in TableColumn())
    {
      DataTables.Columns.Add(args);
    }
  }

  public void WriteColumn()
  {
    Console.WriteLine(Lines);
    FilterColumn();
    
    foreach (var args in DataTables.Columns)
    {
      var column = String.Format("| {0,6} ", args);
      Console.Write(column);
      
      if (args == DataTables.Columns[^1])
      {
        Console.WriteLine("|");
      }
    }
  }
}
