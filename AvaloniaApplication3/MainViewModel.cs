using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaApplication3;

internal partial class MainViewModel : ObservableObject
{
  public MainViewModel()
  {
    Items = [new ItemViewModel("A"), new ItemViewModel("B")];
  }

  public List<ItemViewModel> Items { get; }

  [ObservableProperty]
  public partial ItemViewModel? SelectedItem { get; set; }

  [RelayCommand]
  private void StartStop()
  {
    if (_cts == null)
    {
      _cts = new CancellationTokenSource();
      Task.Run(async () =>
      {
        while (_cts?.IsCancellationRequested != true)
        {
          const int Delay = 20;
          await Task.Delay(Delay);
          SelectedItem = Items[0];

          await Task.Delay(Delay);
          SelectedItem = Items[1];
        }

        SelectedItem = null;
        _cts = null;
      });
    }
    else
    {
      _cts?.Cancel();
    }
  }

  private CancellationTokenSource? _cts;
}

public class ItemViewModel : ObservableObject
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

public partial class RowViewModel : ObservableObject
{
  public RowViewModel(string name)
  {
    Name = name;
  }
  public string Name { get; }
}

