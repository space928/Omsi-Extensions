# Performance Tips
OmsiHook makes accessing internal Omsi data very easy and abstracts away many of the complexities 
of marshalling the data to and from C#, but with this can come performance issues when abused.

## 1. Cache properties you need to reuse frequently
In OmsiHook all data is accessed through C# properties:
```cs
var year = omsi.Globals.Time.Year;

// Internally Year is a property of Time which is a property of Globals:
    public int Year
    {
        get => Memory.ReadMemory<int>(0x00861790);
        set => Memory.WriteMemory(0x00861790, value);
    }
```
C# does not do any caching of the value of these properties, so every time you access 
`omsi.Globals.Time.Year`, a new `OmsiTime` object is constructed and two separate remote memory 
reads need to occur. The trick to save performance here is given that the `OmsiTime` object is 
not going to change (this is a global variable in Omsi), we can cache the reference to our `OmsiTime` 
object and save an extra object construction and memory read every time we want to access the year. 
As long as `OmsiTime` remains valid, the `Year` property will always return the latest value when 
accessed.
```cs
// Bad
public void Loop() 
{
    var year = omsi.Globals.Time.Year;
    // do something with year...
}

// Good
private OmsiTime time;

public void Init()
{
    // Only need to get OmsiTime once!
    time = omsi.Globals.Time;
}

public void Loop() 
{
    var year = time.Year;
    // do something with year...
}
```

### 1.1. MemArrays
Further to this `MemArray<>` and related classes implement a cached wrapper over Omsi arrays, hence 
you can store a reference to a `MemArray` and keep it for as long as the array exists in Omsi. You 
can check if a given `MemArray` is using automatic caching with the `memArray.Cached` property (for 
practical reasons some MemArrays in OmsiHook might not be cached). 

Cached `MemArray`s have the advantage that they are super fast to read (as the contents is cached),
but this comes at the disavantage that they take longer to create (as the entire array is read at 
that point) and can go out of sync with the array in Omsi (you need to call `UpdateFromHook` as 
needed to refresh the `MemArray`. Writing to `MemArray`s is the same regardless of whether it's 
cached or not.

## 2. Strings & Remote method calls incur latency
OmsiHook allows you to call remote methods in Omsi, this is a fairly complicated process and as such 
isn't always very fast. For native Omsi plugins using OmsiHook you don't need to worry much about the 
performance of remote calls, but for external applications using OmsiHook, these calls are especially 
expensive. This is because in this case OmsiHook needs to rely on OmsiHookRPCPlugin, a native Omsi 
plugin shipped with OmsiHook which executes remote calls on the behalf of OmsiHook in batches, once 
per frame. As such, for external applications, remote calls will *always* take at least 1 frame to 
complete.

While reading and writing memory is fast, *allocating* new memory can take a long time. This is 
*especially* for projects using OmsiHook in an external application (ie not a native Omsi plugin);
in this case, when memory needs to be allocated OmsiHook needs to send a message to OmsiHookRPCPlugin
(as memory allocation is a remote method call) and wait until the next frame renders in Omsi for it 
to be allocated. Certain array writing methods and notably **string** writing methods require memory 
allocation and as such can be slow. OmsiHook doesn't yet support asynchronous writing so updating 
multiple strings at once is very slow, as each call to update a single string needs (unless running as 
a native Omsi plugin) to wait for Omsi to render a frame to execute before it can return.

```cs
var map = omsi.Globals.Map;
// Slow!
public void Update()
{
    map.FriendlyName = "OmsiHook";
    map.Name = "is very cool";
    map.Description = $"{DateTime.Now}";
}
```

For now, a potential solution is to do performance intensive string writing in another thread. 
OmsiHookRPCPlugin can listen to up to 8 instances of OmsiHook at once and will execute all remote 
calls it receives during one frame within the next frame.
