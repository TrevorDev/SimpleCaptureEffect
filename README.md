# SimpleCaptureEffect
Wrapper around camera effect winRT api's to provide a simple interface

### Useful links
- [MediaCapture How to's](https://msdn.microsoft.com/en-us/library/windows/apps/mt244352.aspx)
- [MediaCapture API](https://msdn.microsoft.com/library/windows/apps/br241124)
- [Win2D docs](https://github.com/Microsoft/Win2D)
- [Win2D Effects](http://microsoft.github.io/Win2D/html/N_Microsoft_Graphics_Canvas_Effects.htm)
- [Lumia SDK effects](https://msdn.microsoft.com/en-us/library/dn859593.aspx)


### Installing Nuget Package and Dependencies
- This requires your project to be a Windows 10 Universal App project
- Right click project references -> Manage Nuget Packages
- Search and install SimpleCameraEffect and Win2D.uwp nuget packages

### Basic Example
```
async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mc = new MediaCapture();
            await mc.InitializeAsync();
            //captureElement comes from <CaptureElement Name="captureElement"> in xaml file
            captureElement.Source = mc;
            await mc.StartPreviewAsync();

            var effect = SimpleCaptureEffect.CustomVideoEffect.CreateDefinition((context, canvasDevice) => {
                using (CanvasBitmap input = CanvasBitmap.CreateFromDirect3D11Surface(canvasDevice, context.InputFrame.Direct3DSurface))
                using (CanvasRenderTarget output = CanvasRenderTarget.CreateFromDirect3D11Surface(canvasDevice, context.OutputFrame.Direct3DSurface))
                using (CanvasDrawingSession ds = output.CreateDrawingSession())
                {
                    TimeSpan time = context.InputFrame.RelativeTime.HasValue ? context.InputFrame.RelativeTime.Value : new TimeSpan();
                    float dispX = (float)Math.Cos(time.TotalSeconds) * 75f;
                    ds.Clear(Colors.Black);
                    var morph = new MorphologyEffect()
                    {
                        Source = input,
                        Mode = MorphologyEffectMode.Dilate,
                        Width = (int)Math.Abs(Math.Round(dispX)) + 5,
                        Height = 8
                    };
                    ds.DrawImage(morph, 0f, 0f);
                }
            });
            await mc.AddVideoEffectAsync(effect, MediaStreamType.Photo);
        }
```
