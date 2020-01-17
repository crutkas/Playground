using System;
using System.Collections.Generic;
using Windows.Foundation;

namespace VirtualDesktopManager
{
    // I assume this means the window changed desktops?
    public class WindowChangedDesktopsEventArgs : EventArgs
    {
        public WindowReference Window;
    }

    public class VirtualDesktopRemovedEventArgs : EventArgs
    {
        public VirtualDesktop RemovedDesktop;

        // I assume this is where all the windows from the old desktop go or is this which is shown next?
        public VirtualDesktop FallbackDesktop;
    }

    public class VirtualDesktopRemovedFailedEventArgs : VirtualDesktopRemovedEventArgs
    {
        public Exception exception;
    }

    public class VirtualDesktopChangedEventArgs : EventArgs
    {
        public VirtualDesktop OriginalDesktop;
        public VirtualDesktop TargetDesktop;
    }

    public class VirtualDesktopAddedEventArgs : EventArgs
    {
        public VirtualDesktop NewDesktop;
    }

    public class VirtualDesktopRenamedEventArgs : EventArgs
    {
        public string OriginalName;
        public string NewName;
    }


    public class VirtualDesktopManager
    {
        // Think through the SKU quesiton.  When should a dev check this?  At start-up, some other time?
        public static VirtualDesktopManager Current { get { return new VirtualDesktopManager(); } }

        // I assume this is for switching desktops?
        public event TypedEventHandler<VirtualDesktopManager, VirtualDesktopChangedEventArgs> CurrentDesktopChanged;

        // Created a virtual destktop, maps to VirtualDesktopCreated
        public event TypedEventHandler<VirtualDesktopManager, VirtualDesktopAddedEventArgs> VirtualDesktopAdded;

        // destroy succeeded, maps to VirtualDesktopDestroyed
        public event TypedEventHandler<VirtualDesktopManager, VirtualDesktopRemovedEventArgs> VirtualDesktopRemoved;
        // begin destroying, VirtualDesktopDestroyBegin
        public event TypedEventHandler<VirtualDesktopManager, VirtualDesktopRemovedFailedEventArgs> VirtualDesktopFailed;
        // begin destroying, VirtualDesktopDestroyBegin

        // Should there be a 'Added' / 'Removed' pair on VirtualDesktop instead? The Desktop has the list of
        // windows on it, but then you need to listen over here to know when the list changes. And there doesn't
        // appear to be an event for "a new window was created and it landed on this desktop"
        public event TypedEventHandler<VirtualDesktopManager, WindowChangedDesktopsEventArgs> WindowChangedDesktops;
        // Window collection changed events

        public IList<VirtualDesktop> VirtualDesktops { get; }

        public VirtualDesktop CurrentDesktop { get { return new VirtualDesktop(); } }

        public VirtualDesktop GetAdjacentDesktop(VirtualDesktopAdjacentDirection direction) { return GetAdjacentDesktop(direction, CurrentDesktop); }
        public VirtualDesktop GetAdjacentDesktop(VirtualDesktopAdjacentDirection direction, VirtualDesktop desktopReference) { return new VirtualDesktop(); }

        // Added 'Try' since I assume maybe it can fail? Does it need an Async version? 
        // (note current guidance is that if you think you need async, add both sync and async)
        // Is this actually a synchronous operation under the covers?
        public bool SwitchToDesktop(VirtualDesktop newDesktop) { return true; }

        // This is a forward looking reordering changed event since re-ordering is a top requested feature.  Will no-op initially
        public event TypedEventHandler<VirtualDesktopManager, VirtualDesktopRenamedEventArgs> VirtualDesktopOrderChanged;

        // Adding this since there will be a name in Vb
        public event TypedEventHandler<VirtualDesktopManager, VirtualDesktopRenamedEventArgs> VirtualDesktopNameChanged;

        // Do these need to be a 'Try'? Can they fail? Async?
        public VirtualDesktop AddDesktop(VirtualDesktopAdjacentDirection desiredDirection) { return AddDesktop(desiredDirection, CurrentDesktop); }
        public VirtualDesktop AddDesktop(VirtualDesktopAdjacentDirection desiredDirection, VirtualDesktop desktopReference) { return new VirtualDesktop(); }

        // What are the cases when this fails?  (only the last one?)
        public bool CanRemoveDesktop(VirtualDesktop desktop) { return true; }

        public void RemoveDesktop(VirtualDesktop desktop, VirtualDesktop fallbackDesktop) { }

        public VirtualDesktop GetById(string virtualDesktopId) { return new VirtualDesktop(); }

        // These are Pinning / Unpinning apps and windows
        public bool ShowApplicationOnAllDesktops(ApplicationReference app)
        { return true; }

        public bool ShowWindowOnAllDesktops(WindowReference window)
        { return true; }

        public bool RemoveApplicationFromAllDesktops(ApplicationReference app)
        { return true; }

        public bool RemoveWindowFromAllDesktops(WindowReference window)
        { return true; } 

        public bool IsApplicationOnAllDesktops(ApplicationReference app)
        { return true; }

        public bool IsWindowShownOnAllDesktops(WindowReference window)
        { return true; }
    }

    public class VirtualDesktop
    {
        // How persistent are these Ids?  How do these relate to the new name property?
        // It would only be meaningful if Desktop Ids persisted across reboots (otherwise
        // you would just use the object).
        public Guid Id { get; }

        // Name property, since we're adding this in Vb
        public string Name { get; }

        public void MoveWindowToDesktop(WindowReference window) { }
        public bool IsWindowOnDesktop(WindowReference window) { return true; }
    }

    public interface WindowReference
    {
        // Win32 / WinRT factories.
        // This would have to be ported from Mn work that Roberth is working on so it is downlevel.
    }

    // Mike to start a thread with Raymond Chen on this
    public interface ApplicationReference
    {
        // Wrapper of AUMID or PFN or something else? 
        // Maybe just a string?
    }

    public enum VirtualDesktopAdjacentDirection
    {
        Previous = 3,
        Next = 4
    };
}
