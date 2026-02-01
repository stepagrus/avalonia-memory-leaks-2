using System.Reflection;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaApplication3;

internal partial class MainViewModel : ObservableObject
{
  public MainViewModel()
  {
    Items = [new ItemViewModel("A"), new ItemViewModel("B")];

    Title = GetVersion();
  }

  public string Title { get; }

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

  private string GetVersion()
  {
    var informationalVersionAttribute = typeof(Avalonia.Controls.DataGrid).Assembly
          .GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)
          .FirstOrDefault() as AssemblyInformationalVersionAttribute;

    return informationalVersionAttribute?.InformationalVersion.Split("+").FirstOrDefault() ?? "-";
  }

  private CancellationTokenSource? _cts;
}