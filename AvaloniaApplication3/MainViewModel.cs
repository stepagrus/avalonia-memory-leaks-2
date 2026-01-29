using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
      const int Delay = 20;
      Task.Run(async () =>
      {
        while (_cts?.IsCancellationRequested != true)
        {
          await Task.Delay(Delay);
          await Dispatcher.UIThread.InvokeAsync(() =>
          {
            SelectedItem = Items[0];
          });

          await Task.Delay(Delay);
          await Dispatcher.UIThread.InvokeAsync(() =>
          {
            SelectedItem = Items[1];
          });
        }

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
          return SelectedItem = null;
        });
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

public partial class RowViewModel
{
  public RowViewModel(string name)
  {
    Name = name;
  }
  public string Name { get; }
}

