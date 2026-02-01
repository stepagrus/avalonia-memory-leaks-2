namespace AvaloniaApplication3;

public class ItemViewModel
{
  public ItemViewModel(string name)
  {
    Name = name;
    Rows = Enumerable.Range(1, 10)
      .Select(x => new RowViewModel($"name {x}"))
      .ToList();
  }
  public string Name { get; set; }


  public List<RowViewModel> Rows { get; }
}

