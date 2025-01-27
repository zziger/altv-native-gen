﻿using System;
using AltV.NativesDb.Reader.Models.NativeDb;

namespace Durty.AltV.NativesTypingsGenerator.Converters
{
    public class NativeTypeToCSharpCApiTypingConverter
    {
        public string Convert(Native native, NativeType nativeType, bool isReference)
        {
            string referenceType = isReference ? "&" : "";
            return nativeType switch
            {
                NativeType.Any => "int32_t" + referenceType,
                NativeType.Boolean => "uint8_t" + referenceType,
                NativeType.Float => "float" + referenceType,
                NativeType.Int => "int32_t" + referenceType,
                NativeType.String => "const char*" + referenceType,
                NativeType.Vector3 => "vector3_t" + referenceType,
                NativeType.Void => "void" + referenceType,
                NativeType.ScrHandle => "uint32_t" + referenceType,
                NativeType.MemoryBuffer => "int32_t" + referenceType,
                NativeType.Interior => "int32_t" + referenceType,
                NativeType.Object => "uint32_t" + referenceType,
                NativeType.Hash => "uint32_t" + referenceType,
                NativeType.Entity => "uint32_t" + referenceType,
                NativeType.Ped => "uint32_t" + referenceType,
                NativeType.Vehicle => "uint32_t" + referenceType,
                NativeType.Cam => "int32_t" + referenceType,
                NativeType.FireId => "int32_t" + referenceType,
                NativeType.Blip => "int32_t" + referenceType,
                NativeType.Pickup => "int32_t" + referenceType,
                NativeType.Player => "uint32_t" + referenceType,
                NativeType.CarGenerator => "int32_t" + referenceType,
                NativeType.Group => "int32_t" + referenceType,
                NativeType.Train => "uint32_t" + referenceType,
                NativeType.Weapon => "int32_t" + referenceType,
                NativeType.Texture => "int32_t" + referenceType,
                NativeType.TextureDict => "int32_t" + referenceType,
                NativeType.CoverPoint => "int32_t" + referenceType,
                NativeType.Camera => "int32_t" + referenceType,
                NativeType.TaskSequence => "int32_t" + referenceType,
                NativeType.ColourIndex => "int32_t" + referenceType,
                NativeType.Sphere => "int32_t" + referenceType,
                _ => throw new ArgumentOutOfRangeException(nameof(nativeType), nativeType, null)
            };
        }
    }
}
