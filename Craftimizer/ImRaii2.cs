using Dalamud.Bindings.ImGui;
using Dalamud.Bindings.ImPlot;
using System;
using System.Numerics;

namespace Craftimizer.Plugin;

public static class ImRaii2
{
    public struct Disposable : IDisposable
    {
        private readonly Action? _endAction;
        public bool Success { get; }
        private bool _disposed;

        public Disposable(Action? endAction, bool success)
        {
            _endAction = endAction;
            Success = success;
            _disposed = false;
        }

        public static implicit operator bool(Disposable d) => d.Success;

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _endAction?.Invoke();
            }
        }
    }

    public static Disposable GroupPanel(string name, float width, out float internalWidth)
    {
        internalWidth = ImGuiUtils.BeginGroupPanel(name, width);
        return new Disposable(ImGuiUtils.EndGroupPanel, true);
    }

    public static Disposable Plot(string title_id, Vector2 size, ImPlotFlags flags)
    {
        var success = ImPlot.BeginPlot(title_id, size, flags);
        return new Disposable(success ? new Action(ImPlot.EndPlot) : null, success);
    }

    public static Disposable PushStyle(ImPlotStyleVar idx, Vector2 val)
    {
        ImPlot.PushStyleVar(idx, val);
        return new Disposable(() => ImPlot.PopStyleVar(), true);
    }

    public static Disposable PushStyle(ImPlotStyleVar idx, float val)
    {
        ImPlot.PushStyleVar(idx, val);
        return new Disposable(() => ImPlot.PopStyleVar(), true);
    }

    public static Disposable PushColor(ImPlotCol idx, Vector4 col)
    {
        ImPlot.PushStyleColor(idx, col);
        return new Disposable(() => ImPlot.PopStyleColor(), true);
    }

    public static Disposable TextWrapPos(float wrap_local_pos_x)
    {
        ImGui.PushTextWrapPos(wrap_local_pos_x);
        return new Disposable(ImGui.PopTextWrapPos, true);
    }
}
