using Microsoft.AspNetCore.Components;
using static Microsoft.AspNetCore.Components.Web.RenderMode;

namespace Intranet.Test.WebUI.Server.Components;
public partial class App
{
  [CascadingParameter]
  private HttpContext HttpContext { get; set; } = default!;

  private IComponentRenderMode? RenderModeForPage =>
      HttpContext.Request.Path.StartsWithSegments("/Account")
          ? null
          : InteractiveServer;

  protected override void OnInitialized()
  {
  }
}
