﻿using System;
using AltV.NativesDb.Reader.Models.NativeDb;

namespace Durty.AltV.NativesTypingsGenerator.Converters
{
    public class NativeReturnTypeToCSharpCApiTypingConverter
    {
        public string Convert(Native native, NativeType nativeType)
        {
            return nativeType switch
            {
                NativeType.Any => "ResultInt",
                NativeType.Boolean => "ResultBool",
                NativeType.Float => "ResultFloat",
                NativeType.Int => "ResultInt",
                NativeType.String => "ResultString",
                NativeType.Vector3 => "ResultVector3",
                NativeType.Void => "void",
                NativeType.ScrHandle => "ResultInt",
                NativeType.MemoryBuffer => "ResultInt",
                NativeType.Interior => "ResultInt",
                NativeType.Object => "ResultInt",
                NativeType.Hash => "ResultInt",
                NativeType.Entity => "ResultInt",
                NativeType.Ped => "ResultInt",
                NativeType.Vehicle => "ResultInt",
                NativeType.Cam => "ResultInt",
                NativeType.FireId => "ResultInt",
                NativeType.Blip => "ResultInt",
                NativeType.Pickup => "ResultInt",
                NativeType.Player => "ResultInt",
                NativeType.CarGenerator => "ResultInt",
                NativeType.Group => "ResultInt",
                NativeType.Train => "ResultInt",
                NativeType.Weapon => "ResultInt",
                NativeType.Texture => "ResultInt",
                NativeType.TextureDict => "ResultInt",
                NativeType.CoverPoint => "ResultInt",
                NativeType.Camera => "ResultInt",
                NativeType.TaskSequence => "ResultInt",
                NativeType.ColourIndex => "ResultInt",
                NativeType.Sphere => "ResultInt",
                _ => throw new ArgumentOutOfRangeException(nameof(nativeType), nativeType, null)
            };
        }
    }
}