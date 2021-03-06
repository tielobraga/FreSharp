

# FreSharp

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=5UR2T52J633RC)

### Features
 - Build Adobe Air Native Extensions using C#

The package is hosted on NuGet at https://www.nuget.org/packages/TuaRua.FreSharp/

----------

### Getting Started

A basic Hello World [starter project](/starter_project) is included 


### How to use
###### Converting from FREObject args into C# types, returning FREObjects
The following table shows the primitive as3 types which can easily be converted to/from C# types


| AS3 type | C# type | AS3 param->C# | return C#->AS3 |
|:--------:|:--------:|:--------------|:-----------|
| String | string | `var str = argv[0].AsString()` | `return str.ToFREObject()`|
| int | int | `var i = argv[0].AsInt()` | `return i.ToFREObject()`|
| Boolean | bool | `var b = argv[0].AsBool()` | `return b.ToFREObject()`|
| Number | double | `var dbl = argv[0].AsDouble()` | `return dbl.ToFREObject()`|
| uint ARGB | Color | `var clr = argv[0].AsColor()` | `return clr.ToFREObject()`|
| Date | DateTime | `var dt = argv[0].AsDateTime()` | `return dt.ToFREObject()`|
| Rectangle | Rect | `var rect = argv[0].AsRect()` | `return rect.ToFREObject()` |
| Point | Point | `var pnt = argv[0].AsPoint()` | `return pnt.ToFREObject()` |
| BitmapData | Bitmap | `var bmp = argv[0].AsBitmap()` | `return bmp.ToFREObject()` |
| Array | string[] | `var arr = argv[0].AsStringArray()` | `return arr.ToFREObject()`|
| Array | int[] | `var arr = argv[0].AsIntArray()` | `return arr.ToFREObject()`|
| Array | double[] | `var arr = argv[0].AsDoubleArray()` | `return arr.ToFREObject()`|
| Array | bool[] | `var arr = argv[0].AsBoolArray()` | `return arr.ToFREObject()`|
| Object | Dictionary | `var dct = argv[0].AsDictionary()` | N/A |



Example - Convert a FREObject into a String, and String into FREObject

````C#
try {
   var airString = argv[0].AsString();
   Trace("String passed from AIR:" + airString);
}
catch (Exception e) {
    Console.WriteLine(@"caught in C#: type: {0} message: {1}", e.GetType(), e.Message);
}
const string sharpString = "I am a string from C#";
return sharpString.ToFREObject();
`````

Example - Call a method on an FREObject
````C#
var person = argv[0];
var addition = person.Call("add", 100, 33);
Trace("result is: ", addition.AsInt());
`````

Example - Get a property of a FREObject
````C#
var oldAge = person.GetProp("age").AsInt();
Trace("result is: ", oldAge);
`````

Example - Convert a FREObject Object into a Dictionary
````C#
var dictionary = person.AsDictionary();
var name = dictionary["name"];
`````

Example - Create a new FREObject
````C#
var newPerson = new FREObject().Init("com.tuarua.Person");
Trace("We created a new person. type =", newPerson.Type());
`````

Example - Error handling
````C#
var testString = argv[0];
try {
    testString.Call("noStringFunc"); //call method on a string
}
catch (Exception e) {
    return new FreException(e).RawValue; //return as3 error and throw in swc
}
`````

Advanced: Extending FreObjectSharp. Creating a C# version of flash.geom.point

````C#
using System.Collections;
using System.Windows;
using TuaRua.FreSharp;
using FREObject = System.IntPtr;

namespace FreSharp.Geom {
    public class FrePointSharp {
        public FrePointSharp() { }

        public FREObject RawValue { get; set; } = FREObject.Zero;

        public FrePointSharp(FREObject freObject) {
            RawValue = freObject;
        }

        public FrePointSharp(Point value) {
            uint resultPtr = 0;
            var args = new ArrayList {
                value.X,
                value.Y
            };

            RawValue = FreSharpHelper.Core.getFREObject("flash.geom.Point", FreSharpHelper.ArgsToArgv(args),
                FreSharpHelper.GetArgsC(args), ref resultPtr);
            var status = (FreResultSharp) resultPtr;

            if (status == FreResultSharp.Ok) {
                return;
            }
            FreSharpHelper.ThrowFreException(status, "cannot create point ", RawValue);
        }

        
        public Point Value => new Point(
            RawValue.GetProp("x").AsDouble(), 
            RawValue.GetProp("y").AsDouble());
    }
}
`````

### Tech

Uses .NET 4.6

### Prerequisites

You will need
 
 - Visual Studio 2017
 - AIR 19+ SDK
