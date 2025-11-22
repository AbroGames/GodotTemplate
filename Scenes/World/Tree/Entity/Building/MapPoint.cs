using System;
using System.Collections.Generic;
using GodotTemplate.Scenes.World.Data;
using GodotTemplate.Scenes.World.Data.MapPoint;
using GodotTemplate.Scenes.World.Services;
using GodotTemplate.Scenes.World.Services.StartStop;
using Godot;
using MessagePack;

namespace GodotTemplate.Scenes.World.Tree.Entity.Building;

public partial class MapPoint : Node2D
{
    
    public MapPointData Data { get; private set; } //TODO Как связать их на клиенте? Через Export + Sync по id?

    public void UpdatePosition(Vector2 position)
    {
        Position = position;
        Data.PositionX = Position.X;
        Data.PositionY = Position.Y;
    }

    public MapPoint Init(MapPointData data)
    {
        Data = data;
        Position = Vec2(data.PositionX, data.PositionY);
        return this;
    }

    public static MapPoint Create(PackedScene scene, MapPointDataStorage storage, Action<MapPointData> init = null)
    {
        MapPointData mapPointData = new MapPointData();
        init?.Invoke(mapPointData);
        storage.AddMapPoint(mapPointData);
        
        MapPoint mapPoint = scene.Instantiate<MapPoint>();
        mapPoint.Init(mapPointData);
        return mapPoint;
        
        //TODO return scene.Instantiate<MapPoint>().Init(mapPointData);
    }

    public class Loader : IWorldTreeLoader
    {
        public const string Name = "MapPoint";
        public string GetName() => Name;
        
        private readonly Dictionary<long, MapPoint> _mapPointById = new();

        public void Create(World world)
        {
            foreach (MapPointData mapPointData in world.Data.MapPoint.MapPointById.Values)
            {
                MapPoint mapPoint = world.PackedScenes.MapPoint.Instantiate<MapPoint>();
                world.Tree.MapSurface.AddChildWithUniqueName(mapPoint, "MapPoint");
                _mapPointById.Add(mapPointData.Id, mapPoint);
            }
        }

        public void Init(World world)
        {
            foreach (MapPointData mapPointData in world.Data.MapPoint.MapPointById.Values)
            {
                MapPoint mapPoint = _mapPointById[mapPointData.Id];
                mapPoint.Init(mapPointData);
            }
        }
    }
}