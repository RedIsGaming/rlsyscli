using System.Data;
namespace rlsyscli.Table;

public readonly struct TableView<T>
where T : IConvertible
{
  //2 Properties that is used for the Table.
  public DataTable DataTables { get; init; }
  public T Lines { get; init; }
  
  //A method that set the row with the correct table information.
  private List<string> TableColumn()
  {
    var columns = new List<string>
    {
      "All Volume path",
      "Total",
      "Used",
      "Free",
      "% used",
      "% free"
    };

    return columns;
  }
  
  //A method that filters through the List and adds them to the datatable column list.
  private void FilterColumn()
  {
    foreach (var args in TableColumn())
    {
      DataTables.Columns.Add(args);
    }
  }
  
  //A method used for printing it to the Console.
  public void WriteColumn()
  {
    Console.WriteLine(Lines);
    FilterColumn();
    
    foreach (var args in DataTables.Columns)
    {
      var column = String.Format("| {0,7} ", args);
      Console.Write(column);
      
      if (args == DataTables.Columns[^1])
      {
        Console.WriteLine("|");
      }
    }
  }
}
