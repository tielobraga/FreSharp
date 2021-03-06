﻿#region License

// Copyright 2017 Tua Rua Ltd.
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// 
//  Additional Terms
//  No part, or derivative of this Air Native Extensions's code is permitted 
//  to be sold as the basis of a commercially packaged Air Native Extension which 
//  undertakes the same purpose as this software. That is, a WebView for Windows, 
//  OSX and/or iOS and/or Android.
//  All Rights Reserved. Tua Rua Ltd.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using FreSharp.Geom;
using TuaRua.FreSharp.Display;
using TuaRua.FreSharp.Geom;
using FREObject = System.IntPtr;
using Point = System.Windows.Point;

namespace TuaRua.FreSharp {
    /// <summary>
    /// 
    /// </summary>
    public static class FreSharpExtensions {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Converts a C# string to a FREObject
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this string str) => new FreObjectSharp(str).RawValue;

        /// <summary>
        /// Converts a FREObject to a C# string
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static string AsString(this FREObject inFre) => FreSharpHelper.GetAsString(inFre);

        /// <summary>
        /// Converts a FREObject to a DateTime
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static DateTime AsDateTime(this FREObject inFre) => FreSharpHelper.GetAsDateTime(inFre);

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Converts a C# DateTime to a FREObject
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this DateTime value) => new FreObjectSharp(value).RawValue;

        /// <summary>
        /// Converts a FREObject to a C# bool
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static bool AsBool(this FREObject inFre) => FreSharpHelper.GetAsBool(inFre);

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Converts a C# bool to a FREObject
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this bool b) => new FreObjectSharp(b).RawValue;

        /// <summary>
        /// Gets the property of a FREObject
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="name">Name of the property</param>
        /// <returns></returns>
        public static FREObject GetProp(this FREObject inFre, string name) {
            //throws? //TODO
            return FreSharpHelper.GetProperty(inFre, name);
        }

        /// <summary>
        /// Sets the property of a FREObject
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        public static void SetProp(this FREObject inFre, string name, object value) {
            FreSharpHelper.SetProperty(inFre, name, value);
        }

        /// <summary>
        /// Sets the property of a FREObject
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="name">Name of the property</param>
        /// <param name="value">Value of the property</param>
        public static void SetProp(this FREObject inFre, string name, FREObject value) {
            FreSharpHelper.SetProperty(inFre, name, value);
        }

        /// <summary>
        /// Returns the type of the FREObject
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static FreObjectTypeSharp Type(this FREObject inFre) => FreSharpHelper.GetType(inFre);

        /// <summary>
        /// Calls a method on a FREObject
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="method">The method name</param>
        /// <param name="args">Arguments to pass to the method</param>
        /// <returns></returns>
        public static FREObject Call(this FREObject inFre, string method, params object[] args) {
            uint resultPtr = 0;
            var argsArr = new ArrayList();
            if (args != null) {
                for (var i = 0; i < args.Length; i++) {
                    argsArr.Add(args.ElementAt(i));
                }
            }

            var ret = FreSharpHelper.Core.callMethod(inFre, method, FreSharpHelper.ArgsToArgv(argsArr),
                FreSharpHelper.GetArgsC(argsArr), ref resultPtr);

            var status = (FreResultSharp) resultPtr;

            if (status == FreResultSharp.Ok) {
                return ret;
            }

            FreSharpHelper.ThrowFreException(status, "cannot call method " + method, ret);
            return FREObject.Zero;
        }

        /// <summary>
        /// Calls a method on a FREArray
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="methodName">The method name</param>
        /// <param name="args">Arguments to pass to the method</param>
        /// <returns></returns>
        public static FREArray Call(this FREObject inFre, string methodName, ArrayList args) {
            uint resultPtr = 0;
            var ret = new FREArray(FreSharpHelper.Core.callMethod(inFre, methodName,
                FreSharpHelper.ArgsToArgv(args), FreSharpHelper.GetArgsC(args), ref resultPtr));
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) {
                return ret;
            }

            FreSharpHelper.ThrowFreException(status, "cannot call method " + methodName, ret.RawValue);
            return null;
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Converts a C# int[] to a FREObject
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this int[] arr) => new FREArray(arr).RawValue;

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Converts a C# bool[] to a FREObject
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this bool[] arr) => new FREArray(arr).RawValue;

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Converts a C# double[] to a FREObject
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this double[] arr) => new FREArray(arr).RawValue;

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Converts a C# string[] to a FREObject
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this string[] arr) => new FREArray(arr).RawValue;

        /// <summary>
        /// Converts a FREObject to an ArrayList
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static ArrayList ToArrayList(this FREObject inFre) => new FREArray(inFre).AsArrayList();

        /// <summary>
        /// Converts a FREArray to an ArrayList
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static ArrayList ToArrayList(this FREArray inFre) => inFre.AsArrayList();

        /// <summary>
        /// Converts a FREObject to a C# int
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static int AsInt(this FREObject inFre) => FreSharpHelper.GetAsInt(inFre);

        /// <summary>
        /// Initialise a System.Drawing.Color from a FREObject.
        /// </summary>
        /// <param name="inFre"></param>
        /// <param name="hasAlpha">Set to true when the AS3 uint is in ARGB format</param>
        /// <returns></returns>
        public static Color AsColor(this FREObject inFre, bool hasAlpha = false) {
            var rgb = FreSharpHelper.GetAsUInt(new FreObjectSharp(inFre).RawValue);
            if (hasAlpha) {
                return Color.FromArgb(
                    Convert.ToByte((rgb >> 24) & 0xff),
                    Convert.ToByte((rgb >> 16) & 0xff),
                    Convert.ToByte((rgb >> 8) & 0xff),
                    Convert.ToByte((rgb >> 0) & 0xff));
            }
            return Color.FromArgb(
                Convert.ToByte((rgb >> 16) & 0xff),
                Convert.ToByte((rgb >> 8) & 0xff),
                Convert.ToByte((rgb >> 0) & 0xff));
        }


        /// <summary>
        /// Converts a C# System.Drawing.Color to a FREObject
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static FREObject ToFREObject(this Color c) => new FreObjectSharp((uint) ((c.A << 24)
                                                                                        | (c.R << 16)
                                                                                        | (c.G << 8)
                                                                                        | (c.B << 0))).RawValue;

        /// <summary>
        /// Converts a FREObject to a C# Dictionary&lt;string, object&gt;
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Dictionary<string, object> AsDictionary(this FREObject inFre) =>
            FreSharpHelper.GetAsDictionary(inFre);

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Converts a C# int to a FREObject
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this int i) => new FreObjectSharp(i).RawValue;

        /// <summary>
        /// Converts a FREObject to a C# uint
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static uint AsUInt(this FREObject inFre) => FreSharpHelper.GetAsUInt(inFre);

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Converts a C# uint to a FREObject
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this uint i) => new FreObjectSharp(i).RawValue;

        /// <summary>
        /// Converts a FREObject to a C# double
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static double AsDouble(this FREObject inFre) => FreSharpHelper.GetAsDouble(inFre);

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Converts a C# double to a FREObject
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this double i) => new FreObjectSharp(i).RawValue;

        /// <summary>
        /// Converts a FREObject to a C# Rect
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Rect AsRect(this FREObject inFre) => new FreRectangleSharp(inFre).Value;

        /// <summary>
        /// Converts a FREObject to a C# Bitmap
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Bitmap AsBitmap(this FREObject inFre) => new FreBitmapDataSharp(inFre).AsBitmap();

        /// <summary>
        /// Converts a C# Bitmap to a FREObject
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static FREObject ToFREObject(this Bitmap bitmap) => new FreBitmapDataSharp(bitmap).RawValue;

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Converts a C# Rect to a FREObject
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this Rect rect) => new FreRectangleSharp(rect).RawValue;


        /// <summary>
        /// Converts a FREObject to a C# Point
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Point AsPoint(this FREObject inFre) => new FrePointSharp(inFre).Value;

        /// <summary>
        /// Converts a FrePointSharp to a C# Point
        /// </summary>
        /// <param name="inFre"></param>
        /// <returns></returns>
        public static Point AsPoint(this FrePointSharp inFre) => inFre.Value;

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Converts a C# Point to a FREObject
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public static FREObject ToFREObject(this Point point) => new FrePointSharp(point).RawValue;

        /// <summary>
        /// Creates a FREObject
        /// </summary>
        /// <param name="value"></param>
        /// <param name="className">Name of the Class</param>
        /// <param name="args">Arguments to pass to the Class</param>
        /// <returns></returns>
        public static FREObject Init(this FREObject value, string className, params object[] args) {
            uint resultPtr = 0;
            var argsArr = new ArrayList();
            if (args != null) {
                for (var i = 0; i < args.Length; i++) {
                    argsArr.Add(args.ElementAt(i));
                }
            }

            var rawValue = FreSharpHelper.Core.getFREObject(className, FreSharpHelper.ArgsToArgv(argsArr),
                FreSharpHelper.GetArgsC(argsArr), ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) {
                return rawValue;
            }

            FreSharpHelper.ThrowFreException(status, "cannot create class " + className, rawValue);
            return FREObject.Zero;
        }


        /// <summary>
        /// Creates a FREObject
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name">Name of the Class</param>
        /// <returns></returns>
        public static FREObject Init(this FREObject value, string name) {
            uint resultPtr = 0;
            var argsArr = new ArrayList();
            var rawValue = FreSharpHelper.Core.getFREObject(name, FreSharpHelper.ArgsToArgv(argsArr),
                FreSharpHelper.GetArgsC(argsArr), ref resultPtr);
            var status = (FreResultSharp) resultPtr;
            if (status == FreResultSharp.Ok) {
                return rawValue;
            }

            FreSharpHelper.ThrowFreException(status, "cannot create class " + name, rawValue);
            return FREObject.Zero;
        }
    }
}