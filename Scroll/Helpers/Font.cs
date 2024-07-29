namespace Scroll.Helpers;

using System;

using Dalamud.Interface.ManagedFontAtlas;

internal class Font : IDisposable
{
    internal Font(IFontHandle handle, string name, float size)
    {
        this.Handle = handle;
        this.Name = name;
        this.Size = size;
    }

    public void Dispose()
    {
        this.Handle?.Pop();
    }

    internal Font Push()
    {
        this.Handle?.Push();
        return this;
    }

    internal IFontHandle Handle { get; set; }
    internal string Name { get; set; }
    internal float Size { get; set; }
}