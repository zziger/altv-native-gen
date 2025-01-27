﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace AltV.NativesDb.Reader.Models.NativeDb
{
    public class NativeDb
    {
        public string VersionHash { get; set; }

        [JsonProperty("SYSTEM")]
        public Dictionary<string, Native> System { get; set; }

        [JsonProperty("APP")]
        public Dictionary<string, Native> App { get; set; }

        [JsonProperty("AUDIO")]
        public Dictionary<string, Native> Audio { get; set; }

        [JsonProperty("BRAIN")]
        public Dictionary<string, Native> Brain { get; set; }

        [JsonProperty("CAM")]
        public Dictionary<string, Native> Cam { get; set; }

        [JsonProperty("CLOCK")]
        public Dictionary<string, Native> Clock { get; set; }

        [JsonProperty("CUTSCENE")]
        public Dictionary<string, Native> Cutscene { get; set; }

        [JsonProperty("DATAFILE")]
        public Dictionary<string, Native> Datafile { get; set; }

        [JsonProperty("DECORATOR")]
        public Dictionary<string, Native> Decorator { get; set; }

        [JsonProperty("DLC")]
        public Dictionary<string, Native> Dlc { get; set; }

        [JsonProperty("ENTITY")]
        public Dictionary<string, Native> Entity { get; set; }

        [JsonProperty("EVENT")]
        public Dictionary<string, Native> Event { get; set; }

        [JsonProperty("FILES")]
        public Dictionary<string, Native> Files { get; set; }

        [JsonProperty("FIRE")]
        public Dictionary<string, Native> Fire { get; set; }

        [JsonProperty("GRAPHICS")]
        public Dictionary<string, Native> Graphics { get; set; }

        [JsonProperty("HUD")]
        public Dictionary<string, Native> Hud { get; set; }

        [JsonProperty("INTERIOR")]
        public Dictionary<string, Native> Interior { get; set; }

        [JsonProperty("ITEMSET")]
        public Dictionary<string, Native> Itemset { get; set; }

        [JsonProperty("LOADINGSCREEN")]
        public Dictionary<string, Native> Loadingscreen { get; set; }

        [JsonProperty("LOCALIZATION")]
        public Dictionary<string, Native> Localization { get; set; }

        [JsonProperty("MISC")]
        public Dictionary<string, Native> Misc { get; set; }

        [JsonProperty("MOBILE")]
        public Dictionary<string, Native> Mobile { get; set; }

        [JsonProperty("MONEY")]
        public Dictionary<string, Native> Money { get; set; }

        [JsonProperty("NETSHOPPING")]
        public Dictionary<string, Native> Netshopping { get; set; }

        [JsonProperty("NETWORK")]
        public Dictionary<string, Native> Network { get; set; }

        [JsonProperty("OBJECT")]
        public Dictionary<string, Native> Object { get; set; }

        [JsonProperty("PAD")]
        public Dictionary<string, Native> Pad { get; set; }

        [JsonProperty("PATHFIND")]
        public Dictionary<string, Native> Pathfind { get; set; }

        [JsonProperty("PED")]
        public Dictionary<string, Native> Ped { get; set; }

        [JsonProperty("PHYSICS")]
        public Dictionary<string, Native> Physics { get; set; }

        [JsonProperty("PLAYER")]
        public Dictionary<string, Native> Player { get; set; }

        [JsonProperty("RECORDING")]
        public Dictionary<string, Native> Recording { get; set; }

        [JsonProperty("REPLAY")]
        public Dictionary<string, Native> Replay { get; set; }

        [JsonProperty("SCRIPT")]
        public Dictionary<string, Native> Script { get; set; }

        [JsonProperty("SHAPETEST")]
        public Dictionary<string, Native> Shapetest { get; set; }

        [JsonProperty("SOCIALCLUB")]
        public Dictionary<string, Native> Socialclub { get; set; }

        [JsonProperty("STATS")]
        public Dictionary<string, Native> Stats { get; set; }

        [JsonProperty("STREAMING")]
        public Dictionary<string, Native> Streaming { get; set; }

        [JsonProperty("TASK")]
        public Dictionary<string, Native> Task { get; set; }

        [JsonProperty("VEHICLE")]
        public Dictionary<string, Native> Vehicle { get; set; }

        [JsonProperty("WATER")]
        public Dictionary<string, Native> Water { get; set; }

        [JsonProperty("WEAPON")]
        public Dictionary<string, Native> Weapon { get; set; }

        [JsonProperty("ZONE")]
        public Dictionary<string, Native> Zone { get; set; }

        public List<Native> AllNatives
        {
            get
            {
                List<Native> natives = new List<Native>();
                natives.AddRange(System.Values);
                natives.AddRange(App.Values);
                natives.AddRange(Graphics.Values);
                natives.AddRange(Audio.Values);
                natives.AddRange(Brain.Values);
                natives.AddRange(Cam.Values);
                natives.AddRange(Clock.Values);
                natives.AddRange(Cutscene.Values);
                natives.AddRange(Datafile.Values);
                natives.AddRange(Decorator.Values);
                natives.AddRange(Dlc.Values);
                natives.AddRange(Entity.Values);
                natives.AddRange(Event.Values);
                natives.AddRange(Files.Values);
                natives.AddRange(Fire.Values);
                natives.AddRange(Hud.Values);
                natives.AddRange(Interior.Values);
                natives.AddRange(Itemset.Values);
                natives.AddRange(Loadingscreen.Values);
                natives.AddRange(Localization.Values);
                natives.AddRange(Misc.Values);
                natives.AddRange(Mobile.Values);
                natives.AddRange(Money.Values);
                natives.AddRange(Netshopping.Values);
                natives.AddRange(Network.Values);
                natives.AddRange(Object.Values);
                natives.AddRange(Pad.Values);
                natives.AddRange(Pathfind.Values);
                natives.AddRange(Ped.Values);
                natives.AddRange(Physics.Values);
                natives.AddRange(Player.Values);
                natives.AddRange(Recording.Values);
                natives.AddRange(Replay.Values);
                natives.AddRange(Script.Values);
                natives.AddRange(Shapetest.Values);
                natives.AddRange(Socialclub.Values);
                natives.AddRange(Stats.Values);
                natives.AddRange(Streaming.Values);
                natives.AddRange(Task.Values);
                natives.AddRange(Vehicle.Values);
                natives.AddRange(Water.Values);
                natives.AddRange(Weapon.Values);
                natives.AddRange(Zone.Values);
                return natives;
            }
        }
    }
}